import {useGetUserQuery} from "features/account/accountApiSlice";
import LoginView from "features/auth/views/LoginView";
import PrivateRoute from "features/auth/components/PrivateRoute";
import AuthRoute from "features/auth/components/AuthRoute";
import DashboardView from "common/views/DashboardView";
import {Route, Routes} from "react-router-dom";
import TopBarProgress from "react-topbar-progress-indicator";

function App() {
    const {isLoading} = useGetUserQuery();

    if (isLoading) {
        return <TopBarProgress/>;
    }

    return (
        <Routes>
            <Route path="/" element={<PrivateRoute><DashboardView/></PrivateRoute>}/>
            <Route path="/login" element={<AuthRoute><LoginView/></AuthRoute>}/>
        </Routes>
    );
}

export default App;
