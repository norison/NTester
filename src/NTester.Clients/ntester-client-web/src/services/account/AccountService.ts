import User from "./models/User";
import api from "../../http";

class AccountService {
    public async getUser(): Promise<User> {
        const response = await api.get<User>("account");
        return response.data;
    }
}

export default new AccountService();