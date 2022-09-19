import {Navigate} from "react-router-dom";
import {useAppSelector} from "../app/hooks";
import {Box, CircularProgress} from "@mui/material";

interface ProtectedRouteProps {
    element: JSX.Element
}

export default function ProtectedRoute({element}: ProtectedRouteProps) {
    const {user, loadingStatus} = useAppSelector(state => state.accountReducer);

    if (loadingStatus === "loading") {
        return (
            <Box sx={{display: 'flex'}}>
                <CircularProgress/>
            </Box>
        );
    }

    if (user) {
        return element;
    }

    return <Navigate to="/login"/>
}