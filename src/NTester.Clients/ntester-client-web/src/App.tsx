import {Route, Routes} from "react-router-dom";
import LoginView from "./features/auth/views/LoginView";
import {useAppDispatch} from "./app/hooks";
import {useGetUserQuery} from "./features/account/accountApiSlice";
import {useEffect, useState} from "react";
import {setUser} from "./features/account/accountSlice";
import TopBarProgress from "react-topbar-progress-indicator";
import PrivateRoute from "./features/auth/components/PrivateRoute";
import AuthRoute from "./features/auth/components/AuthRoute";

function App() {
    const dispatch = useAppDispatch();
    const [userLoaded, setUserLoaded] = useState<boolean>(false);
    const {data: user, isLoading, isSuccess} = useGetUserQuery();

    useEffect(() => {
        if (isLoading) {
            return;
        }

        if (user && isSuccess) {
            dispatch(setUser(user));
        }

        setUserLoaded(true);
    }, [user, isLoading, isSuccess]);

    if (!userLoaded) {
        return <TopBarProgress/>;
    }

    return (
        <Routes>

            <Route path="/" element={<PrivateRoute/>}>
                <Route index element={<p>Default</p>}/>
            </Route>
            <Route element={<AuthRoute/>}>
                <Route path="/login" element={<LoginView/>}/>
            </Route>

        </Routes>
    );
}

export default App;
