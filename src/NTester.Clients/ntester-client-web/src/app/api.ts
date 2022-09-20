import axios from "axios";
import authService from "../features/auth/authService";
import {ACCESS_TOKEN_LOCAL_STORAGE_NAME} from "../common/constants/localStorageConstants";
import {ErrorResponse} from "../common/models/ErrorResponse";
import RefreshRequest from "../features/auth/models/RefreshRequest";
import AuthResponse from "../features/auth/models/AuthResponse";

const api = axios.create({
    baseURL: process.env.REACT_APP_BASE_URL,
    withCredentials: true,
    headers: {
        "Content-Type": "application/json",
        "Accept": "application/json",
        "X-Version": 1.0,
        "X-Client": process.env.REACT_APP_CLIENT_NAME || ""
    },
});

api.interceptors.request.use((config) => {
    const accessToken = localStorage.getItem(ACCESS_TOKEN_LOCAL_STORAGE_NAME);

    if (accessToken) {
        config.headers = {
            ...config.headers,
            "Authorization": `Bearer ${accessToken}`,
        };
    }

    return config;
});

api.interceptors.response.use(
    (response) => {
        return response;
    },
    async (error) => {
        if (error.response.status === 401 && error.config && !error.config._isRetry) {
            error.config._isRetry = true;

            try {
                const accessToken = localStorage.getItem(ACCESS_TOKEN_LOCAL_STORAGE_NAME) || "";
                const request: RefreshRequest = {accessToken};
                const response = await axios.post<AuthResponse>("auth/refresh", request);
                localStorage.setItem(ACCESS_TOKEN_LOCAL_STORAGE_NAME, response.data.accessToken);
                return api.request(error.config);
            } catch (e) {
                console.log(e);
            }
        }

        throw new Error(error.response.data.message);
    }
);

export default api;
