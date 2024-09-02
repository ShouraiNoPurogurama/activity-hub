import { useField } from "formik";
import { Form, Label } from "semantic-ui-react";
import DatePicker, {DatePickerProps} from "react-datepicker";

type Props = {
    label: string;
    type?: string;
    showLabel?: boolean;
}   &  Partial<DatePickerProps> 
 

export default function MyDateInput(props: Props) {
    const [field, meta, helpers] = useField(props.name!);
    return (
        <Form.Field error={meta.touched && !!meta.error}>
            <DatePicker 
                {...field}
                {...props}                
                selected={field.value }
                placeholderText={props.placeholderText}
                onChange={(date) => helpers.setValue(date)}
            />
            {meta.touched && meta.error ? (
                <Label basic color="red">{meta.error}</Label>
            ) : null}
        </Form.Field>
    )
}