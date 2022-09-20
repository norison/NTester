import LoginRequest from "./models/LoginRequest";
import AuthResponse from "./models/AuthResponse";
import RegisterRequest from "./models/RegisterRequest";
import api from "../../app/api";
import {ACCESS_TOKEN_LOCAL_STORAGE_NAME} from "../../common/constants/localStorageConstants";

class AuthService {
    public async login(request: LoginRequest): Promise<void> {
        const response = await api.post<AuthResponse>("auth/login", request);
        localStorage.setItem(ACCESS_TOKEN_LOCAL_STORAGE_NAME, response.data.accessToken);
    }

    public async register(request: RegisterRequest): Promise<void> {
        const response = await api.post<AuthResponse>("auth/register", request);
        localStorage.setItem(ACCESS_TOKEN_LOCAL_STORAGE_NAME, response.data.accessToken);
    }

    public async logout(): Promise<void> {
        await api.post("auth/logout");
        localStorage.removeItem(ACCESS_TOKEN_LOCAL_STORAGE_NAME);
    }
}

export default new AuthService();
