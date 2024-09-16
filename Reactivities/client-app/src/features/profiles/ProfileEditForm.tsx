import { Form, Formik } from "formik";
import { Button, Header } from "semantic-ui-react";
import MyTextInput from "../../app/common/form/MyTextInput";
import { ProfileFormValues } from "../../app/models/profile";
import * as Yup from 'yup';


interface Props {
    editProfile: (creds: ProfileFormValues) => void;
}

export default function ProfileEditForm({ editProfile }: Props) {
    return (
        <Formik
            initialValues={{ displayName: '', bio: '', error: null }}
            validationSchema={Yup.object({
                displayName: Yup.string().required()
            })}
            onSubmit={(values) => editProfile(values)}
        >
            {({ handleSubmit, isSubmitting, isValid, dirty }) => (
                <Form className="ui form" onSubmit={handleSubmit} autoComplete="off">
                    <Header as={'h2'} content='Edit profile' color="teal" textAlign="center" />
                    <MyTextInput placeholder="Display Name" name='displayName' />
                    <MyTextInput placeholder="Bio" name='bio' />
                    <Button
                        loading={isSubmitting}
                        positive
                        content='Update profile'
                        type="submit" fluid
                        disabled={!isValid || !dirty}
                    />
                </Form>
            )}
        </Formik>
    )
}