import {apiSlice} from "app/api/apiSlice";
import LoginRequest from "features/auth/models/LoginRequest";
import AuthResponse from "features/auth/models/AuthResponse";
import RegisterRequest from "features/auth/models/RegisterRequest";
import AccessTokenService from "features/auth/services/AccessTokenService";

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