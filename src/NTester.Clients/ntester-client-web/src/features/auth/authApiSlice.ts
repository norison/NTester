import {apiSlice} from "../../app/api/apiSlice";
import LoginRequest from "./models/LoginRequest";
import AuthResponse from "./models/AuthResponse";
import RegisterRequest from "./models/RegisterRequest";
import {ACCESS_TOKEN_LOCAL_STORAGE_NAME} from "../../common/constants/localStorageConstants";

export const authApiSlice = apiSlice.injectEndpoints({
    endpoints: builder => ({
        login: builder.mutation<AuthResponse, LoginRequest>({
            query: request => ({
                url: "/auth/login",
                method: "POST",
                body: request
            }),
            transformResponse: (response: AuthResponse) => {
                localStorage.setItem(ACCESS_TOKEN_LOCAL_STORAGE_NAME, response.accessToken);
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
                localStorage.setItem(ACCESS_TOKEN_LOCAL_STORAGE_NAME, response.accessToken);
                return response;
            }
        }),
        logout: builder.mutation<void, void>({
            query: () => ({
                url: "/auth/logout",
                method: "POST"
            }),
            transformResponse: () => {
                localStorage.removeItem(ACCESS_TOKEN_LOCAL_STORAGE_NAME);
            }
        })
    })
});

export const {
    useLoginMutation,
    useRegisterMutation,
    useLogoutMutation
} = authApiSlice;