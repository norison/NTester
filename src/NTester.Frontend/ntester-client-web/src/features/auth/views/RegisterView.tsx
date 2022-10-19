import { LockOutlined } from "@mui/icons-material";
import { Container, Box, Avatar, Typography, Link } from "@mui/material";
import Copyright from "common/components/Copyright";
import { useLazyGetUserQuery } from "features/account/accountApiSlice";
import RegisterForm from "features/auth/components/RegisterForm";
import RegisterRequest from "features/auth/models/RegisterRequest";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import TopBarProgress from "react-topbar-progress-indicator";
import { isErrorWithMessage, isFetchBaseQueryError } from "utils/errorHelpers";
import { useRegisterMutation } from "../authApiSlice";

function RegisterView() {
	const navigate = useNavigate();
	const [register, authResult] = useRegisterMutation();
	const [getUser, accountResult] = useLazyGetUserQuery();

	const onSubmit = async (registerRequest: RegisterRequest) => {
		try {
			await register(registerRequest).unwrap();
			await getUser().unwrap();

			navigate("/");
		} catch (e) {
			if (isFetchBaseQueryError(e)) {
				toast.error(e.data.message);
			} else if (isErrorWithMessage(e)) {
				toast.error(e.message);
			}
		}
	};

	const isLoading = authResult.isLoading || accountResult.isLoading || accountResult.isFetching;

	return (
		<Container maxWidth="xs">
			{isLoading ? <TopBarProgress /> : null}

			<Box sx={{ marginTop: 8, display: "flex", flexDirection: "column", alignItems: "center" }}>
				<Avatar sx={{ margin: 1, backgroundColor: "secondary.main" }}>
					<LockOutlined />
				</Avatar>

				<Typography component="h1" variant="h5">
					Sign up
				</Typography>

				<RegisterForm onSubmit={onSubmit} disabled={isLoading} />

				<Link href="/login">
					<Typography>Already have an account? Sign in</Typography>
				</Link>
			</Box>
			<Copyright />
		</Container>
	);
}

export default RegisterView;
