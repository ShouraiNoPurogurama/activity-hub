import { observer } from "mobx-react-lite";
import { Profile } from "../../app/models/profile";
import { Reveal, RevealContent, Button } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import { SyntheticEvent } from "react";

interface Props {
    profile: Profile;
}

export default observer(function FollowButton({ profile }: Props) {
    const { profileStore, userStore } = useStore();
    const { updateFollowing, loading, setFollowings, followings } = profileStore;

    if (userStore.user?.username === profile.username) return null;

    function handleFollow(e: SyntheticEvent, username: string) {
        e.preventDefault();

        if (username === profileStore.profile!.username) {
            if (profile.following) {
                updateFollowing(username, false)
                setFollowings([...followings.filter(x => x.username !== userStore.user?.username)])
            } else if (!profile.following) {
                updateFollowing(username, true)
                setFollowings([...followings], userStore.user?.username)
            }
        } else {
            profile.following ? updateFollowing(username, false) : updateFollowing(username, true);
        }
    }

    return (<Reveal animated="move">
        <RevealContent visible style={{ width: '100%' }}>
            <Button fluid color="teal" content={profile.following ? 'Following' : 'Not following'} />
        </RevealContent>
        <RevealContent hidden style={{ width: '100%' }}>
            <Button
                fluid
                basic
                color={profile.following ? 'red' : 'green'}
                content={profile.following ? 'Unfollow' : 'Follow'}
                loading={loading}
                onClick={(e) => handleFollow(e, profile.username)}
            />
        </RevealContent>
    </Reveal>)
}

)