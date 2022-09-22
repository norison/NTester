import {useAppSelector} from "app/hooks";
import {selectCurrentUser} from "features/account/accountSlice";
import {Box, Button, Container, Typography} from "@mui/material";
import {useLogoutMutation} from "features/auth/authApiSlice";
import {useNavigate} from "react-router-dom";
import TopBarProgress from "react-topbar-progress-indicator";

function DashboardView() {
    const navigate = useNavigate();
    const user = useAppSelector(selectCurrentUser)!;
    const [logout, {isLoading}] = useLogoutMutation();

    const logoutHandler = async () => {
        await logout().unwrap();
        navigate("/");
    };

    if (isLoading) {
        return <TopBarProgress/>;
    }

    return (
        <Container component="main" maxWidth="xs" sx={{justifyContent: "center", alignItems: "center"}}>
            <Typography align="center">{user.name} {user.surname}</Typography>
            <Typography align="center">{user.userName}</Typography>
            <Typography align="center">{user.email}</Typography>
            <Typography align="center">{user.id}</Typography>

            <Box sx={{display: "flex", justifyContent: "center", alignItems: "center", marginTop: "10px"}}>
                <Button onClick={logoutHandler} variant="outlined" size="small">Logout</Button>
            </Box>
        </Container>
    );
}

export default DashboardView;