import { Link, Typography } from "@mui/material";

function Copyright() {
	return (
		<Typography variant="body2" color="text.secondary" align="center" sx={{ marginTop: 8 }}>
			{"Copyright © "}
			<Link href="#">NTester</Link> {new Date().getFullYear()}
			{"."}
		</Typography>
	);
}

export default Copyright;
