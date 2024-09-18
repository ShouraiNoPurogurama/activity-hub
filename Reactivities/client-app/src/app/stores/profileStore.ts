import { Photo, Profile, ProfileFormValues, UserActivity } from './../models/profile';
import { makeAutoObservable, reaction, runInAction } from "mobx";
import agent from "../api/agent";
import { store } from './store';

export default class ProfileStore {
    profile: Profile | null = null;
    loadingProfile = false;
    uploading = false;
    loading = false;
    followings: Profile[] = [];
    loadingFollowing = false;
    activeTab = 0;
    userActivities: UserActivity[] = [];
    loadingActivities = false;

    /**
     *
     */
    constructor() {
        makeAutoObservable(this);

        //handle distinct the Followers and Following tab in Profile Content TabPane
        reaction(
            () => this.activeTab,
            activeTab => {
                if (activeTab === 3 || activeTab === 4) {
                    const predicate = activeTab === 3 ? 'followers' : "following";
                    this.loadFollowings(predicate);
                } else {
                    this.followings = [];
                }
            }
        )
    }

    setActiveTab = (activeTab: number) => {
        this.activeTab = activeTab;
    }

    get isCurrentUser() {
        if (store.userStore.user && this.profile) {
            return store.userStore.user.username === this.profile.username;
        }
        return false;
    }

    loadProfile = async (username: string) => {
        this.loadingProfile = true;
        try {
            const profile = await agent.Profiles.get(username);
            runInAction(() => {
                this.profile = profile;
                this.loadingProfile = false
            })
        } catch (error) {
            console.log(error)
            runInAction(() => this.loadingProfile = false)
        }
    }

    uploadPhoto = async (file: Blob) => {
        this.uploading = true;
        try {
            const response = await agent.Profiles.uploadPhoto(file);
            const photo = response.data;
            runInAction(() => {
                if (this.profile) {
                    this.profile.photos?.push(photo);
                    if (photo.isMain && store.userStore.user) {
                        store.userStore.setImage(photo.url);
                        this.profile.image = photo.url;
                    }
                }
                this.uploading = false;
            })
        } catch (error) {
            console.log(error);
            runInAction(() => this.uploading = false)
        }
    }

    setMainPhoto = async (photo: Photo) => {
        this.loading = true;
        try {
            await agent.Profiles.setMainPhoto(photo.id);
            store.userStore.setImage(photo.url);
            runInAction(() => {
                if (this.profile && this.profile.photos) {
                    this.profile.photos.find(p => p.isMain)!.isMain = false;
                    this.profile.photos.find(p => p.id === photo.id)!.isMain = true;
                    this.profile.image = photo.url;
                    this.loading = false
                }
            })
        } catch (error) {
            console.log(error)
            runInAction(() => this.loading = false);
        }
    }

    deletePhoto = async (photo: Photo) => {
        this.loading = true;
        try {
            //delete from server through API
            await agent.Profiles.deletePhoto(photo.id);
            runInAction(() => {
                if (this.profile) {
                    //delete photo from store
                    this.profile.photos = this.profile.photos?.filter(p => p.id !== photo.id);
                    this.loading = false;
                }
            })
        } catch (error) {
            console.log(error);
            runInAction(() => this.loading = false);
        }
    }

    editProfile = async (creds: ProfileFormValues) => {
        this.loading = true;
        try {
            await agent.Profiles.editProfile(creds);
            runInAction(() => {
                this.profile!.bio = creds?.bio ?? this.profile?.bio;
                this.profile!.displayName = creds.displayName ?? this.profile!.displayName;
                this.loading = false;
            })
        } catch (error) {
            runInAction(() => this.loading = false)
        }
    }

    updateFollowing = async (username: string, following: boolean) => {
        this.loading = true;
        try {
            await agent.Profiles.updateFollowing(username);
            store.activityStore.updateAttendeeFollowing(username);
            runInAction(() => {
                //if the profile to be follow/unfollow not the current logged-in user
                //this.profile: target profile
                //this step increase/decrease the target profile followers count
                if (this.profile && this.profile.username !== store.userStore.user?.username
                    && this.profile.username !== username
                ) {
                    following ? this.profile.followersCount++ : this.profile.followersCount--;
                    this.profile.following = !this.profile.following;
                }
                console.log('this.profile equals to ' + this.profile?.displayName)
                //after adjust the target profile followers count, we need to update its followings collection
                //if A is watching B and B unfollow/follow someone, update the status here
                if(this.profile && this.profile.username === store.userStore.user?.username) {
                    following ? this.profile.followingCount++ : this.profile.followingCount--;
                }
                this.followings.forEach(profile => {
                    //if a profile in the followings collection found, then unfollow, else, follow
                    if (profile.username === username) {
                        //existing status
                        profile.following ? profile.followersCount-- : profile.followersCount++;
                        profile.following = !profile.following;
                    }
                })
                this.loading = false;
            })
        } catch (error) {
            console.log(error);
            runInAction(() => {
                this.loading = false;
            })
        }
    }

    loadFollowings = async (predicate: string) => {
        this.loadingFollowing = true;
        try {
            const followings = await agent.Profiles.listFollowing(this.profile!.username, predicate);
            runInAction(() => {
                this.followings = followings;
                this.loadingFollowing = false;
            })

        } catch (error) {
            console.log(error);
            runInAction(() => this.loadingFollowing = false)
        }
    }

    loadUserActivities = async (username: string, predicate?: string) => {
        this.loadingActivities = true;
        try {
            const activities = await agent.Profiles.listActivities(username, predicate!);
            runInAction(() => {
                this.userActivities = activities;
                this.loadingActivities = false;
            })
        } catch (error) {
            console.log(error);
            this.loadingActivities = false;
        }
    }

    clearEvents = () => {
        this.userActivities = [];
    }
}