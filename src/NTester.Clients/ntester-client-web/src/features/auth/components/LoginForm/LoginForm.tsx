import {Controller, useForm} from "react-hook-form";
import {Box, Button, TextField} from "@mui/material";
import * as yup from "yup";
import {yupResolver} from "@hookform/resolvers/yup";

interface LoginFormProps {
    onSubmit: (userName: string, password: string) => void;
}

interface FormInputs {
    userName: string;
    password: string;
}

const schema = yup.object({
    userName: yup.string().min(2).required().label("Username"),
    password: yup.string().min(2).required().label("Password")
}).required();


function LoginForm({onSubmit}: LoginFormProps) {
    const {register, handleSubmit, control, formState: {errors}} = useForm<FormInputs>({
        resolver: yupResolver(schema),
        defaultValues: {
            userName: "",
            password: ""
        }
    });

    return (
        <Box component="form" onSubmit={handleSubmit(data => onSubmit(data.userName, data.password))} sx={{mt: 1}}>
            <Controller name="userName"
                        control={control}
                        render={({field}) => <TextField 
                            {...field} 
                            margin="normal"
                            fullWidth 
                            label="Username" 
                            error={Boolean(errors.userName)}
                            helperText={errors.userName?.message}/>}/>

            <Controller name="password"
                        control={control}
                        render={({field}) => <TextField 
                            {...field} 
                            type="password"
                            margin="normal"
                            fullWidth
                            label="Password" 
                            error={Boolean(errors.password)}
                            helperText={errors.password?.message}/>}/>

            <Button type="submit" fullWidth variant="contained" sx={{mt: 3, mb: 2}}>Sign In</Button>
        </Box>
    );
}

export default LoginForm;