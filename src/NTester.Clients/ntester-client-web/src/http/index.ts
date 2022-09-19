import axios from "axios";
import AuthResponse from "../services/auth/models/AuthResponse";
import RefreshRequest from "../services/auth/models/RefreshRequest";

const BASE_URL = process.env.REACT_APP_BASE_URL;
const ACCESS_TOKEN_NAME = "AccessToken";

const api = axios.create({
    baseURL: BASE_URL,
    withCredentials: true,
    headers: {
        "Content-Type": "application/json",
        "Accept": "application/json"
    }
})

api.interceptors.request.use(config => {
    const accessToken = localStorage.getItem(ACCESS_TOKEN_NAME);

    if (accessToken) {
        config.headers = {
            "Authorization": `Bearer ${accessToken}`
        }
    }

    return config;
});

api.interceptors.response.use(response => {
    return response;
}, async error => {
    const accessToken = localStorage.getItem(ACCESS_TOKEN_NAME);

    if (!accessToken) {
        throw error;
    }

    if (error.response.status === 401 && error.config && !error.config._isRetry) {
        error.config._isRetry = true;

        try {
            const body: RefreshRequest = {accessToken};
            const response = await axios.post<AuthResponse>(`${BASE_URL}/auth/refresh`, body, {withCredentials: true});
            localStorage.setItem(ACCESS_TOKEN_NAME, response.data.accessToken);
            return api.request(error.config);
        } catch (e) {
            console.log(e);
        }
    }

    throw error;
});

export default api;