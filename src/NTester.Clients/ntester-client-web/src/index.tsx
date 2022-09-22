import "@fontsource/roboto/300.css";
import "@fontsource/roboto/400.css";
import "@fontsource/roboto/500.css";
import "@fontsource/roboto/700.css";

import "react-toastify/dist/ReactToastify.css";

import { Provider } from "react-redux";
import { BrowserRouter } from "react-router-dom";
import { store } from "app/store";
import App from "App";
import React from "react";
import ReactDOM from "react-dom";

ReactDOM.render(
	<React.StrictMode>
		<Provider store={store}>
			<BrowserRouter>
				<App />
			</BrowserRouter>
		</Provider>
	</React.StrictMode>,
	document.getElementById("root")!
);
