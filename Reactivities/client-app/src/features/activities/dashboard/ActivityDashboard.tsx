import { Grid } from "semantic-ui-react";
import ActivityList from "./ActivityList";
import ActivityDetails from "../details/ActivityDetails";
import ActivityForm from "../form/ActivityForm";
import { useStore } from "../../../app/stores/store";
import { observer } from "mobx-react-lite";


export default observer (function ActivityDashboard() {
    const { activityStore } = useStore();
    const { selectedActivity, editMode } = activityStore;
    const {loading} = activityStore;

    return (
        <Grid>
            <Grid.Column width='10'>
                <ActivityList
                ></ActivityList>
            </Grid.Column>
            <Grid.Column width='6'>
                {
                    selectedActivity &&
                    <ActivityDetails />
                }
                {
                    editMode &&
                    <ActivityForm
                        submitting={loading}
                    />
                }
            </Grid.Column>
        </Grid>
    )
}
)