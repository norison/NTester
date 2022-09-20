import {PropsWithChildren} from "react";
import User from "../../features/account/models/User";
import {Navigate, Outlet} from "react-router-dom";

interface PublicRouteProps extends PropsWithChildren<any> {
    user: User | undefined;
    redirectUrl?: string;
}

function PublicRoute({user, redirectUrl = "/", children}: PublicRouteProps) {
    if (user) {
        return <Navigate to={redirectUrl} replace/>;
    }

    return children ? children : <Outlet/>;
}

export default PublicRoute;