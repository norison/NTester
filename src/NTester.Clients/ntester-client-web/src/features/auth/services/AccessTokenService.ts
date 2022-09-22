const ACCESS_TOKEN_STORAGE_NAME = "AccessToken";

class AccessTokenService {
    public setAccessToken(token: string): void {
        localStorage.setItem(ACCESS_TOKEN_STORAGE_NAME, token);
    }

    public removeAccessToken(): void {
        localStorage.removeItem(ACCESS_TOKEN_STORAGE_NAME);
    }

    public getAccessToken(): string | null {
        return localStorage.getItem(ACCESS_TOKEN_STORAGE_NAME);
    }
}

export default new AccessTokenService();