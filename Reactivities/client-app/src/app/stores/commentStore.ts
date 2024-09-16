import { ChatComment } from './../models/comment';
import { HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { makeAutoObservable, runInAction, values } from "mobx";
import { store } from "./store";

export default class CommentStore {
    comments: ChatComment[] = [];
    hubConnection: HubConnection | null = null;

    /**
     *
     */
    constructor() {
        makeAutoObservable(this);
    }

    createHubConnection = (activityId: string) => {
        this.hubConnection = new HubConnectionBuilder()
            .withUrl('http://localhost:5000/chat?activityId=' + activityId, {
                accessTokenFactory: () => store.userStore.user?.token as string
            })
            .withAutomaticReconnect()
            .configureLogging(LogLevel.Information)
            .build();
        
        this.hubConnection.start().catch(error => console.log("Error establishing the connection"))

        this.hubConnection.on('LoadComments', (comments: ChatComment[]) => {
            runInAction(() => {
                comments.forEach(comment => {
                    // Comments create time lost the 'Z' character in UTC format, must append it
                    // manually.
                    comment.createdAt = new Date(comment.createdAt + 'Z');
                })
                this.comments = comments
            });
        });

        //This method get the comment from SignalR hub, not the db 
        // so don't need to manually append the 'Z' at the end of the create time
        this.hubConnection.on('ReceiveComment', (comment: ChatComment) => {
            runInAction(() => {
                comment.createdAt = new Date(comment.createdAt)
                this.comments.unshift(comment)
            });
        })
    }

    stopHubConnection = () => {
        this.hubConnection?.stop().catch(error => console.log('Error stopping connection: ' + error));
    }

    clearComments = () => {
        this.comments = [];
        this.stopHubConnection();
    }

    addComment = async (values: any) => {
        values.activityId = store.activityStore.selectedActivity?.id;
        try {
            await this.hubConnection?.invoke('SendComment', values);
        } catch (error) {
            console.log(error);
        }
    }
}