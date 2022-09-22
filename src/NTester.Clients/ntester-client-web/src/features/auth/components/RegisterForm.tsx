import { Controller, useForm } from "react-hook-form";
import { Box, Button, Grid, TextField } from "@mui/material";
import * as yup from "yup";
import { yupResolver } from "@hookform/resolvers/yup";
import RegisterRequest from "../models/RegisterRequest";

interface RegisterFormProps {
	onSubmit: (registerRequest: RegisterRequest) => void;
	disabled: boolean;
}

interface FormInputs {
	email: string;
	userName: string;
	password: string;
	name: string;
	surname: string;
}

const schema = yup
	.object({
		email: yup.string().email().required(),
		userName: yup.string().min(2).required().label("Username"),
		password: yup.string().min(2).required().label("Password"),
		name: yup.string().min(2).required(),
		surname: yup.string().min(2).required(),
	})
	.required();

function RegisterForm({ onSubmit, disabled }: RegisterFormProps) {
	const {
		handleSubmit,
		control,
		formState: { errors },
	} = useForm<FormInputs>({
		resolver: yupResolver(schema),
		defaultValues: {
			email: "",
			userName: "",
			password: "",
			name: "",
			surname: "",
		},
	});

	return (
		<Box component="form" noValidate onSubmit={handleSubmit((data) => onSubmit(data))} sx={{ mt: 2 }}>
			<Grid container spacing={2}>
				<Grid item xs={12} sm={6}>
					<Controller
						name="name"
						control={control}
						render={({ field }) => (
							<TextField
								{...field}
								required
								fullWidth
								label="Name"
								autoFocus
								error={Boolean(errors.name)}
								helperText={errors.name?.message}
							/>
						)}
					/>
				</Grid>
				<Grid item xs={12} sm={6}>
					<Controller
						name="surname"
						control={control}
						render={({ field }) => (
							<TextField
								{...field}
								required
								fullWidth
								label="Surname"
								error={Boolean(errors.surname)}
								helperText={errors.surname?.message}
							/>
						)}
					/>
				</Grid>
				<Grid item xs={12}>
					<Controller
						name="email"
						control={control}
						render={({ field }) => (
							<TextField
								{...field}
								required
								fullWidth
								label="Email Address"
								error={Boolean(errors.email)}
								helperText={errors.email?.message}
							/>
						)}
					/>
				</Grid>
				<Grid item xs={12}>
					<Controller
						name="userName"
						control={control}
						render={({ field }) => (
							<TextField
								{...field}
								required
								fullWidth
								label="Username"
								error={Boolean(errors.userName)}
								helperText={errors.userName?.message}
							/>
						)}
					/>
				</Grid>
				<Grid item xs={12}>
					<Controller
						name="password"
						control={control}
						render={({ field }) => (
							<TextField
								{...field}
								type="password"
								required
								fullWidth
								label="Password"
								error={Boolean(errors.password)}
								helperText={errors.password?.message}
							/>
						)}
					/>
				</Grid>
			</Grid>

			<Button type="submit" disabled={disabled} fullWidth variant="contained" sx={{ mt: 3, mb: 2 }}>
				Sign Up
			</Button>
		</Box>
	);
}

export default RegisterForm;
