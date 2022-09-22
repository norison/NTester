import { Link, Typography } from "@mui/material";

interface LogoProps {
	text: string;
}

function Logo({ text }: LogoProps) {
	return (
		<Typography
			variant="h6"
			component={Link}
			href="/"
			sx={{
				display: { xs: "none", md: "block" },
				fontFamily: "monospace",
				fontWeight: 700,
				letterSpacing: ".3rem",
				color: "inherit",
				textDecoration: "none",
			}}
		>
			{text}
		</Typography>
	);
}

export default Logo;
