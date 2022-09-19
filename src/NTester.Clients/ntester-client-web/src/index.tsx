import '@fontsource/roboto/300.css';
import '@fontsource/roboto/400.css';
import '@fontsource/roboto/500.css';
import '@fontsource/roboto/700.css';

import React from 'react';
import {createRoot} from 'react-dom/client';
import {Provider} from 'react-redux';
import {store} from './app/store';
import reportWebVitals from './reportWebVitals';
import {BrowserRouter, Route, Routes} from "react-router-dom";
import App from "./App";
import LoginView from "./features/auth/LoginView";
import ProtectedRoute from "./common/ProtectedRoute";
import {fetchUserAsync} from "./features/account/accountSlice";

const container = document.getElementById('root')!;
const root = createRoot(container);

store.dispatch(fetchUserAsync());

root.render(
    <Provider store={store}>
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<ProtectedRoute element={<App/>}/>}/>
                <Route path="/login" element={<LoginView/>}/>
            </Routes>
        </BrowserRouter>
    </Provider>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
