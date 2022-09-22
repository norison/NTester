import {Navigate, Outlet} from "react-router-dom";
import {selectCurrentUser} from "../../account/accountSlice";
import {useAppSelector} from "../../../app/hooks";

interface PrivateRouteProps {
    redirectPath?: string;
    children?: JSX.Element;
}

function PrivateRoute({redirectPath = "/login", children}: PrivateRouteProps) {
    const user = useAppSelector(selectCurrentUser);
    
    if(user) {
        return children ?? <Outlet/>;
    }

    return <Navigate to={redirectPath} replace/>;
}

export default PrivateRoute;