import { useGetUserQuery } from "features/account/accountApiSlice";
import LoginView from "features/auth/views/LoginView";
import RegisterView from "features/auth/views/RegisterView";
import PrivateRoute from "features/auth/components/PrivateRoute";
import AuthRoute from "features/auth/components/AuthRoute";
import DashboardView from "common/views/DashboardView";
import { Route, Routes } from "react-router-dom";
import TopBarProgress from "react-topbar-progress-indicator";

function App() {
	const { isLoading } = useGetUserQuery();

	if (isLoading) {
		return <TopBarProgress />;
	}

	return (
		<Routes>
			<Route
				path="/"
				element={
					<PrivateRoute>
						<DashboardView />
					</PrivateRoute>
				}
			/>
			<Route element={<AuthRoute />}>
				<Route path="/login" element={<LoginView />} />
				<Route path="/register" element={<RegisterView />} />
			</Route>
		</Routes>
	);
}

export default App;
