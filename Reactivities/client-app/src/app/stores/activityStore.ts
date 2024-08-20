import { makeAutoObservable, runInAction } from "mobx";
import { Activity } from "../models/activity";
import agent from "../api/agent";
import { v4 as uuid } from 'uuid'

export default class ActivityStore {
    activities: Activity[] = [];
    selectedActivity: Activity | undefined = undefined;
    editMode = false;
    loading = false;
    loadingInitial = false;

    constructor() {
        makeAutoObservable(this)
    }

    //use arrow function to bind the function with the class automatically
    loadActivities = async () => {
        this.setLoadingInitial(true);
        try {
            const activities = await agent.Activities.list();
            this.activities = [];
            activities.forEach(activity => {
                activity.date = activity.date.split("T")[0];
                this.activities.push(activity)
            })
            this.setLoadingInitial(false);
            this.activities.forEach(a => console.log(a.title))
        } catch (error) {
            console.log(error)
            this.setLoadingInitial(false);
        }
    }

    setLoadingInitial = (state: boolean) => {
        this.loadingInitial = state;
    }
 
    selectActivity = (id: string) => {
        this.selectedActivity = this.activities.find(a => a.id === id);
    }

    cancelSelectActivity = () => {
        this.selectedActivity = undefined;
    }

    openForm = (id?: string) => {
        id ? this.selectActivity(id) : this.cancelSelectActivity();
        this.editMode = true;
    }

    closeForm = () => {
        this.editMode = false;
    }

    createActivity = async (activity: Activity) => {
        this.loading = true;
        activity.id = uuid();
        try {
            await agent.Activities.create(activity);
            runInAction(() => {
                this.activities.push(activity);
                this.selectedActivity = activity;
                this.editMode = false;
                this.loading = false;
            })
        } catch (error) {
            console.log(error)
            runInAction(() => {
                this.loading= false;
            })
        }
    }

    updateActivity = async (activity: Activity) => {
        this.loading = true;
        try {
            await agent.Activities.update(activity);
            runInAction(() => {
                this.activities = this.activities.filter(a => a.id !== activity.id)
                this.activities.push(activity)
                this.selectedActivity = activity;
                this.editMode = false;
                this.loading = false
            })
        } catch (error) {
            console.log(error)
            runInAction(() => {
                this.loading = false;
            })
        }
    }

    deleteActivity = async (id: string) => {
        this.loading = true;
        try {
            await agent.Activities.delete(id);
            runInAction(() => {
                this.activities = this.activities.filter(a => a.id !== id);
                if(this.selectedActivity?.id === id) this.cancelSelectActivity();
                this.loading = false;
            })
        } catch (error) {
            runInAction(() => {
                this.loading = false;
            })
        }
    }
}