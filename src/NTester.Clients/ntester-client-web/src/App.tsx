import {BrowserRouter, Route, Routes} from "react-router-dom";
import DashboardPage from "./pages/DashboardPage";
import LoginPage from "./pages/LoginPage";
import RegisterPage from "./pages/RegisterPage";
import {ToastContainer} from "react-toastify";
import 'react-toastify/dist/ReactToastify.css';
import {useAppDispatch, useAppSelector} from "./app/hooks";
import {useEffect} from "react";
import {LoadingStatus} from "./common/enums/LoadingStatus";
import {fetchUserAsync} from "./features/account/accountSlice";
import TopBarProgress from "react-topbar-progress-indicator";
import PublicRoute from "./pages/routes/PublicRoute";
import PrivateRoute from "./pages/routes/PrivateRoute";

function App() {
    const dispatch = useAppDispatch();
    const {user, loadingStatus} = useAppSelector(state => state.account);

    useEffect(() => {
        if (!user) {
            dispatch(fetchUserAsync());
        }
    }, []);

    return (
        <>
            {loadingStatus === LoadingStatus.Loading ? <TopBarProgress/> : null}
            
            <BrowserRouter>
                <Routes>
                    <Route path="/" element={<PrivateRoute user={user}/>}>
                        <Route index element={<DashboardPage/>}/>
                    </Route>
                    <Route element={<PublicRoute user={user}/>}>
                        <Route path="/login" element={<LoginPage/>}/>
                        <Route path="/register" element={<RegisterPage/>}/>
                    </Route>
                </Routes>
            </BrowserRouter>
            
            <ToastContainer
                position="bottom-right"
                autoClose={5000}
                hideProgressBar={false}
                newestOnTop={false}
                closeOnClick
                rtl={false}
                pauseOnFocusLoss
                draggable
                pauseOnHover
            />
        </>
    )
}

export default App;
