import { useAppSelector } from "app/hooks";
import { selectCurrentUser } from "features/account/accountSlice";
import { useLogoutMutation } from "features/auth/authApiSlice";
import { useNavigate } from "react-router-dom";
import { isFetchBaseQueryError } from "utils/errorHelpers";
import { toast } from "react-toastify";
import Header from "common/components/Header";

function DashboardView() {
	const navigate = useNavigate();
	const user = useAppSelector(selectCurrentUser)!;
	const [logout] = useLogoutMutation();

	const handleLogout = async () => {
		try {
			await logout().unwrap();
			navigate("/login");
		} catch (e) {
			if (isFetchBaseQueryError(e)) {
				toast.error(e.data.message);
			}
		}
	};

	const headerActionItems = [
		{
			text: "Profile",
			handler: () => {
				toast.success("Profile");
			},
		},
		{ text: "Logout", handler: handleLogout },
	];

	return <Header user={user} actionItems={headerActionItems} />;
}

export default DashboardView;
