import {Navigate} from "react-router-dom";
import {useAppSelector} from "../app/hooks";
import {LoadingStatus} from "../common/enums/LoadingStatus";

function DashboardPage() {
    const {user, loadingStatus} = useAppSelector(state => state.account);

    if (loadingStatus === LoadingStatus.Loading) {
        return <div>Loading...</div>;
    }

    if (!user) {
        return <Navigate to="/login"/>;
    }

    return (
        <div>Dashboard</div>
    );
}

export default DashboardPage;