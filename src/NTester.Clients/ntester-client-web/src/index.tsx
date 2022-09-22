import "react-toastify/dist/ReactToastify.css";

import { createRoot } from "react-dom/client";
import { Provider } from "react-redux";
import { BrowserRouter } from "react-router-dom";
import { store } from "app/store";
import { ToastContainer } from "react-toastify";
import { ThemeProvider } from "@mui/material";
import { theme } from "themes/mainTheme";
import App from "App";

const container = document.getElementById("root")!;
const root = createRoot(container);

root.render(
	<Provider store={store}>
		<ThemeProvider theme={theme}>
			<BrowserRouter>
				<App />
			</BrowserRouter>
			<ToastContainer />
		</ThemeProvider>
	</Provider>
);
