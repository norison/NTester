import {apiSlice} from "../../app/api/apiSlice";
import LoginRequest from "./models/LoginRequest";
import AuthResponse from "./models/AuthResponse";
import RegisterRequest from "./models/RegisterRequest";
import AccessTokenService from "./services/AccessTokenService";

export const authApiSlice = apiSlice.injectEndpoints({
    endpoints: builder => ({
        login: builder.mutation<AuthResponse, LoginRequest>({
            query: request => ({
                url: "/auth/login",
                method: "POST",
                body: request
            }),
            transformResponse: (response: AuthResponse) => {
                AccessTokenService.setAccessToken(response.accessToken);
                return response;
            }
        }),
        register: builder.mutation<AuthResponse, RegisterRequest>({
            query: request => ({
                url: "/auth/register",
                method: "POST",
                body: request
            }),
            transformResponse: (response: AuthResponse) => {
                AccessTokenService.setAccessToken(response.accessToken);
                return response;
            }
        }),
        logout: builder.mutation<void, void>({
            query: () => ({
                url: "/auth/logout",
                method: "POST"
            }),
            transformResponse: () => {
                AccessTokenService.removeAccessToken();
            }
        })
    })
});

export const {
    useLoginMutation,
    useRegisterMutation,
    useLogoutMutation
} = authApiSlice;