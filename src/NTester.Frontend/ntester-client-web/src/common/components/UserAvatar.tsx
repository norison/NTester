import { IconButton, Avatar } from "@mui/material";
import { MouseEventHandler } from "react";

interface UserAvatarProps {
	name: string;
	surname: string;
	onClick: MouseEventHandler<HTMLButtonElement>;
}

function UserAvatar({ name, surname, onClick }: UserAvatarProps) {
	const stringToColor = (str: string) => {
		let hash = 0;
		let i;

		for (i = 0; i < str.length; i += 1) {
			hash = str.charCodeAt(i) + ((hash << 5) - hash);
		}

		let color = "#";

		for (i = 0; i < 3; i += 1) {
			const value = (hash >> (i * 8)) & 0xff;
			color += `00${value.toString(16)}`.slice(-2);
		}

		return color;
	};

	const stringAvatar = (name: string) => {
		return {
			sx: {
				bgcolor: stringToColor(name),
			},
			children: `${name.split(" ")[0][0]}${name.split(" ")[1][0]}`,
		};
	};

	return (
		<IconButton onClick={onClick}>
			<Avatar {...stringAvatar(`${name} ${surname}`)} />
		</IconButton>
	);
}

export default UserAvatar;
