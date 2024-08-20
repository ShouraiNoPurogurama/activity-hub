import {
    CardMeta,
    CardHeader,
    CardDescription,
    CardContent,
    Card,
    Image,
    Button,
} from 'semantic-ui-react'
import { useStore } from "../../../app/stores/store";
import LoadingComponent from '../../../app/layout/LoadingComponent';

export default function ActivityDetails() {
    const {activityStore} = useStore();
    const {selectedActivity: activity, openForm, cancelSelectActivity} = activityStore;

    if(!activity) return <LoadingComponent/>; 

    return (
        <Card fluid>
            <Image src={`/assets/categoryImages/${activity.category}.jpg`}/>
            <CardContent>
                <CardHeader>{activity.title}</CardHeader>
                <CardMeta>
                    <span className='date'>{activity.date}</span>
                </CardMeta>
                <CardDescription>
                    {activity.description}
                </CardDescription>
            </CardContent>
            <CardContent extra>
                <Button.Group widths='2'>
                    <Button onClick={() => openForm(activity.id)} basic color="blue" content="Edit"></Button>
                    <Button onClick={cancelSelectActivity} basic color="grey" content="Cancel"></Button>
                </Button.Group>
            </CardContent>
        </Card>
    )
}