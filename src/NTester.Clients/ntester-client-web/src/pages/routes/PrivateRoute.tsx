import User from "../../features/account/models/User";
import {Navigate, Outlet} from "react-router-dom";
import {PropsWithChildren} from "react";

interface PrivateRouteProps extends PropsWithChildren<any> {
    user: User | undefined;
    redirectUrl?: string;
}

function PrivateRoute({user, redirectUrl = "/login", children}: PrivateRouteProps) {
    if (!user) {
        return <Navigate to={redirectUrl} replace/>;
    }

    return children ? children : <Outlet/>;
}

export default PrivateRoute;