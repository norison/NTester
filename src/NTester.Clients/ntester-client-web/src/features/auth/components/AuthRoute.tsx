import {Navigate, Outlet} from "react-router-dom";
import {selectCurrentUser} from "../../account/accountSlice";
import {useAppSelector} from "../../../app/hooks";

interface AuthRouteProps {
    redirectPath?: string;
    children?: JSX.Element;
}

function AuthRoute({redirectPath = "/", children}: AuthRouteProps) {
    const user = useAppSelector(selectCurrentUser);

    if (!user) {
        return children ?? <Outlet/>;
    }

    return <Navigate to={redirectPath} replace/>;
}

export default AuthRoute;