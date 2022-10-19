import { AppBar, Box, Menu, MenuItem, Toolbar } from "@mui/material";
import { useState } from "react";
import User from "features/account/models/User";
import Logo from "./Logo";
import UserAvatar from "./UserAvatar";

interface HeaderActionItem {
	text: string;
	handler: () => void;
}

interface HeaderProps {
	user: User;
	actionItems: HeaderActionItem[];
}

function Header({ user, actionItems }: HeaderProps) {
	const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);

	const handleClick = (event: React.MouseEvent<HTMLButtonElement>) => {
		setAnchorEl(event.currentTarget);
	};
	const handleClose = () => {
		setAnchorEl(null);
	};

	return (
		<AppBar position="static">
			<Toolbar sx={{ display: "flex", justifyContent: "space-between" }}>
				<Logo text="NTester" />
				<Box>
					<UserAvatar name={user.name} surname={user.surname} onClick={handleClick} />
					<Menu
						sx={{ mt: 5 }}
						anchorEl={anchorEl}
						anchorOrigin={{
							vertical: "top",
							horizontal: "right",
						}}
						keepMounted
						transformOrigin={{
							vertical: "top",
							horizontal: "right",
						}}
						open={Boolean(anchorEl)}
						onClose={handleClose}
					>
						{actionItems.map((x) => (
							<MenuItem onClick={x.handler}>{x.text}</MenuItem>
						))}
					</Menu>
				</Box>
			</Toolbar>
		</AppBar>
	);
}

export default Header;
