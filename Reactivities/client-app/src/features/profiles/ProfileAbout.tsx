import { Button, Grid, Header, TabPane } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import { useState } from "react";
import ProfileEditForm from "./ProfileEditForm";
import { ProfileFormValues } from "../../app/models/profile";
import { observer } from "mobx-react-lite";

export default observer(function ProfilePhoto() {
    const { profileStore: { isCurrentUser, editProfile, profile }, userStore: { user, setDisplayName } } = useStore();
    const [editProfileMode, setEditProfileMode] = useState(false);

    function handleEditProfile(creds: ProfileFormValues) {
        editProfile(creds).then(() => {
            setEditProfileMode(false);
            setDisplayName(profile!.displayName)
        });
    }

    return (
        <TabPane>
            <Grid>
                <Grid.Column width={16}>
                    <Header floated="left" icon={'user'} content={`About ${user?.displayName}`} />
                    {
                        isCurrentUser && (
                            <Button
                                floated="right"
                                basic
                                content={editProfileMode ? 'Cancel' : 'Edit Profile'}
                                onClick={() => setEditProfileMode(!editProfileMode)}
                            />
                        )
                    }
                </Grid.Column>
                <Grid.Column width={16}>
                    {
                        editProfileMode ? (
                            <ProfileEditForm editProfile={handleEditProfile} />
                        ) : (
                            <p>{profile?.bio}</p>
                        )
                    }
                </Grid.Column>
            </Grid>
        </TabPane>
    )
}
)