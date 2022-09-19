import LoginRequest from "./models/LoginRequest";
import AuthResponse from "./models/AuthResponse";
import api from "../../http";

class AuthService {
    public async login(request: LoginRequest): Promise<AuthResponse> {
        const response = await api.post<AuthResponse>("auth/login", request);
        return response.data;
    }
}

export default new AuthService();