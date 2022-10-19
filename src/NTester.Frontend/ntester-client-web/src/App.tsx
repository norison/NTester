import { useGetUserQuery } from "features/account/accountApiSlice";
import LoginView from "features/auth/views/LoginView";
import RegisterView from "features/auth/views/RegisterView";
import PrivateRoute from "features/auth/components/PrivateRoute";
import AuthRoute from "features/auth/components/AuthRoute";
import DashboardView from "common/views/DashboardView";
import { Navigate, Route, Routes } from "react-router-dom";
import TopBarProgress from "react-topbar-progress-indicator";
import { CssBaseline, ThemeProvider } from "@mui/material";
import { theme } from "themes/mainTheme";
import { ToastContainer } from "react-toastify";

function App() {
	const { isLoading } = useGetUserQuery();

	if (isLoading) {
		return <TopBarProgress />;
	}

	return (
		<ThemeProvider theme={theme}>
			<CssBaseline />
			<ToastContainer position="bottom-right" />
			<main>
				<Routes>
					<Route path="/">
						<Route element={<PrivateRoute />}>
							<Route index element={<DashboardView />} />
						</Route>
						<Route element={<AuthRoute />}>
							<Route path="login" element={<LoginView />} />
							<Route path="register" element={<RegisterView />} />
						</Route>
						<Route path="*" element={<Navigate to="/" replace />} />
					</Route>
				</Routes>
			</main>
		</ThemeProvider>
	);
}

export default App;
