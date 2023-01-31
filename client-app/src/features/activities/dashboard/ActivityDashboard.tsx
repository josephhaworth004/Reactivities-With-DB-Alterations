import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { Grid } from "semantic-ui-react";
import LoadingComponent from "../../../app/layout/LoadingComponent";
import { useStore } from "../../../app/stores/store";
import ActivityFilters from "./ActivityFilters";
import ActivityList from "./ActivityList";

// Destructure the Activity[] from props from above to give type-safety 
// No need to prefix things with props.
export default observer(function ActivityDashboard() {
    const {activityStore} = useStore();
    const {loadActivities, activityRegistry} = activityStore

  useEffect(() => {
    if (activityRegistry.size <= 1) loadActivities();
  }, [loadActivities]); // These dependencies ensure the axios.get will  be called once. Would be infinite loop otherwise

  if (activityStore.loadingInitial) return <LoadingComponent content="Loading activities..." />;

    return (
        // Grid is part of semantic UI styling
        // width = '10' is the number of columns in the grid
        // Max grid size 16
        // the grid under is 6 to make 16
        // Only display an activity if we have one (not null)
        <Grid>
            <Grid.Column width='10'>
                <ActivityList />
            </Grid.Column>
            <Grid.Column width='6'>
                <ActivityFilters />
            </Grid.Column>
        </Grid>
    )
})