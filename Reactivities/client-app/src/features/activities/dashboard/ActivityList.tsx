import { Button, Item, Label, Segment } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";
import { observer } from "mobx-react-lite";
import { Link } from "react-router-dom";
import ActivityListItem from "./ActivityListItem";

export default observer(function ActivityList() {
    const { activityStore } = useStore();
    const {activitiesByDate } = activityStore;


    return (
        <Segment>
            <Item.Group >
                {activitiesByDate.map(activity => (
                    <ActivityListItem activity={activity}/>
                ))}
            </Item.Group>
        </Segment>
    )
}
)