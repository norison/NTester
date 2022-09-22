import {useLoginMutation} from "features/auth/authApiSlice";
import {useLazyGetUserQuery} from "features/account/accountApiSlice";
import {Avatar, Box, Button, Container, Link, Typography} from "@mui/material";
import {toast} from "react-toastify";
import LoginForm from "features/auth/components/LoginForm";
import TopBarProgress from "react-topbar-progress-indicator";
import {isErrorWithMessage, isFetchBaseQueryError} from "utils/errorHelpers";
import {useNavigate} from "react-router-dom";
import {LockOutlined} from "@mui/icons-material";

function LoginView() {
    const navigate = useNavigate();
    const [login, authResult] = useLoginMutation();
    const [getUser, accountResult] = useLazyGetUserQuery();

    const onSubmit = async (userName: string, password: string) => {
        try {
            await login({userName, password}).unwrap();
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

    const isLoading = authResult.isLoading ||
        accountResult.isLoading ||
        accountResult.isFetching;

    return (
        <Container maxWidth="xs">
            {isLoading ? <TopBarProgress/> : null}

            <Box sx={{marginTop: 8, display: "flex", flexDirection: "column", alignItems: "center"}}>

                <Avatar sx={{margin: 1, backgroundColor: 'secondary.main'}}>
                    <LockOutlined/>
                </Avatar>

                <Typography component="h1" variant="h5">Sign in</Typography>

                <LoginForm onSubmit={onSubmit} disabled={isLoading}/>

                <Link href="#"><Typography>Don't have an account? Sign Up</Typography></Link>

            </Box>

            <Typography variant="body2" color="text.secondary" align="center" sx={{marginTop: 8}}>
                {'Copyright © '}
                <Button href="#">NTester</Button>{" "}
                {new Date().getFullYear()}
                {'.'}
            </Typography>

        </Container>
    );
}

export default LoginView;