import {useAppDispatch, useAppSelector} from "../app/hooks";
import {toast} from "react-toastify";
import {fetchUserAsync} from "../features/account/accountSlice";
import {useNavigate} from "react-router-dom";
import LoginForm from "../features/auth/components/LoginForm/LoginForm";
import authService from "../features/auth/authService";
import {Avatar, Box, Container, CssBaseline, Link, Typography} from "@mui/material";
import LockOutlinedIcon from "@mui/icons-material/LockOutlined";

function LoginPage() {
    const navigate = useNavigate();
    const dispatch = useAppDispatch();

    const onSubmit = async (userName: string, password: string) => {
        try {
            await authService.login({userName, password});
            await dispatch(fetchUserAsync());
            navigate("/");
        } catch (e: any) {
            toast.error(e.message);
        }
    };

    return (
        <Container maxWidth="xs">
            <Box sx={{marginTop: 8, display: 'flex', flexDirection: 'column', alignItems: 'center'}}>
                <Avatar sx={{m: 1, bgcolor: 'secondary.main'}}><LockOutlinedIcon/></Avatar>
                <Typography component="h1" variant="h5">Sign in</Typography>
                <LoginForm onSubmit={onSubmit}/>
                <Link href="#" variant="body2">{"Don't have an account? Sign Up"}</Link>
            </Box>
        </Container>
    );
}

export default LoginPage;