import { makeAutoObservable} from "mobx";

export default class ActivityStore {
    title = 'Hello from Mobx!';
    
    constructor() {
        makeAutoObservable(this)
    }

    setTitle = () => {
        this.title = this.title + '!'
    }
    
}