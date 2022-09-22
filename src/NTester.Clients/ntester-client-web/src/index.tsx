import '@fontsource/roboto/300.css';
import '@fontsource/roboto/400.css';
import '@fontsource/roboto/500.css';
import '@fontsource/roboto/700.css';

import "react-toastify/dist/ReactToastify.css";

import {createRoot} from 'react-dom/client';
import {Provider} from 'react-redux';
import {BrowserRouter} from "react-router-dom";
import {store} from 'app/store';
import reportWebVitals from 'reportWebVitals';
import App from "./App";
import {ToastContainer} from "react-toastify";

const container = document.getElementById('root')!;
const root = createRoot(container);

root.render(
    <Provider store={store}>
        <BrowserRouter>
            <App/>
        </BrowserRouter>
        <ToastContainer/>
    </Provider>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
