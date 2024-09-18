import { useField } from "formik";
import ReactDatePicker, { ReactDatePickerProps } from "react-datepicker";
import { Form, Label } from "semantic-ui-react";

type Props = {
    label: string;
    type?: string;
    showLabel?: boolean;
}   &  ReactDatePickerProps
 
export default function MyDateInput(props: Props) {
    const [field, meta, helpers] = useField(props.name!);
    return (
        <Form.Field error={meta.touched && !!meta.error}>
            <ReactDatePicker
                {...field}
                {...props}                
                selected={field.value }
                placeholderText={props.placeholderText}
                onChange={(date: Date | null) => helpers.setValue(date)}
            />
            {meta.touched && meta.error ? (
                <Label basic color="red">{meta.error}</Label>
            ) : null}
        </Form.Field>
    )
}