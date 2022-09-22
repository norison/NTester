import { forwardRef } from "react";
import { Link as RouterLink, LinkProps as RouterLinkProps } from "react-router-dom";
import { createTheme } from "@mui/material";
import { LinkProps } from "@mui/material/Link";

const LinkBehavior = forwardRef<HTMLAnchorElement, Omit<RouterLinkProps, "to"> & { href: RouterLinkProps["to"] }>(
	(props, ref) => {
		const { href, ...other } = props;
		// Map href (MUI) -> to (react-router)
		return <RouterLink ref={ref} to={href} {...other} />;
	}
);

export const theme = createTheme({
	components: {
		MuiLink: {
			defaultProps: {
				component: LinkBehavior,
			} as LinkProps,
		},
		MuiButtonBase: {
			defaultProps: {
				LinkComponent: LinkBehavior,
			},
		},
	},
});
