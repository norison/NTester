import {useLoginMutation} from "../authApiSlice";
import {useDispatch} from "react-redux";
import LoginForm from "../components/LoginForm";
import TopBarProgress from "react-topbar-progress-indicator";
import {Avatar, Box, Container, Link, Typography} from "@mui/material";
import {useLazyGetUserQuery} from "../../account/accountApiSlice";
import {setAccessToken} from "../authSlice";
import {setUser} from "../../account/accountSlice";
import {toast} from "react-toastify";
import {isErrorWithMessage, isFetchBaseQueryError} from "../../../utils/errorHelpers";
import {ErrorResponse} from "../../../common/models/ErrorResponse";
import {useNavigate} from "react-router-dom";
import {LockOutlined} from "@mui/icons-material";

function LoginView() {
    const dispatch = useDispatch();
    const navigate = useNavigate();
    const [login, authResult] = useLoginMutation();
    const [trigger, accountResult] = useLazyGetUserQuery();

    const onSubmit = async (userName: string, password: string) => {
        try {
            const authResponse = await login({userName, password}).unwrap();
            dispatch(setAccessToken(authResponse.accessToken));

            const user = await trigger().unwrap();
            dispatch(setUser(user));
        } catch (e) {
            if (isFetchBaseQueryError(e)) {
                toast.error((e.data as ErrorResponse).message);
            } else if (isErrorWithMessage(e)) {
                toast.error(e.message);
            }
        }
    };

    const isLoading = authResult.isLoading || accountResult.isLoading;

    return (
        <Container maxWidth="xs">
            {isLoading ? <TopBarProgress/> : null}

            <Box sx={{marginTop: 8, display: "flex", flexDirection: "column", alignItems: "center"}}>

                <Avatar sx={{margin: 1, backgroundColor: 'secondary.main'}}>
                    <LockOutlined/>
                </Avatar>

                <Typography component="h1" variant="h5">Sign in</Typography>

                <LoginForm onSubmit={onSubmit} disabled={isLoading}/>

                <Link href="#" display="block" align="center" variant="body2">
                    {"Don't have an account? Sign Up"}
                </Link>

            </Box>

            <Typography variant="body2" color="text.secondary" align="center" sx={{marginTop: 8}}>
                {'Copyright © '}
                <Link color="inherit" href="#">
                    NTester
                </Link>{' '}
                {new Date().getFullYear()}
                {'.'}
            </Typography>

        </Container>
    );
}

export default LoginView;