import {
    BaseQueryFn
} from "@reduxjs/toolkit/dist/query/baseQueryTypes";
import {
    FetchArgs,
    fetchBaseQuery,
    FetchBaseQueryError,
} from "@reduxjs/toolkit/query/react";
import RefreshRequest from "../../features/auth/models/RefreshRequest";
import AuthResponse from "../../features/auth/models/AuthResponse";
import {ACCESS_TOKEN_LOCAL_STORAGE_NAME} from "../../common/constants/localStorageConstants";
import {setAccessToken} from "../../features/auth/authSlice";

const baseQuery = fetchBaseQuery({
    baseUrl: `${process.env.REACT_APP_BASE_URL}/api`,
    credentials: "include",
    prepareHeaders: (headers, {getState}) => {
        const accessToken = localStorage.getItem(ACCESS_TOKEN_LOCAL_STORAGE_NAME);
        if (accessToken) {
            headers.set("Authorization", `Bearer ${accessToken}`);
            headers.set("Content-Type", "application/json");
            headers.set("X-Client", process.env.REACT_APP_CLIENT_NAME ?? "");
        }
        return headers;
    },
});

const baseQueryWithReAuth: BaseQueryFn<string | FetchArgs, unknown, FetchBaseQueryError> = async (args, api, extraOptions) => {
    let result = await baseQuery(args, api, extraOptions)

    if (result.error && result.error.status === 401) {
        const accessToken = localStorage.getItem(ACCESS_TOKEN_LOCAL_STORAGE_NAME);

        if (!accessToken) {
            return result;
        }

        const fetchArgs = args as FetchArgs;
        const headers = fetchArgs.headers as Headers;

        if (!headers.get("WWW-Authenticate")?.includes("The token expired")) {
            return result;
        }

        const request: RefreshRequest = {accessToken};
        const refreshResult = await baseQuery({url: 'auth/refresh', method: 'POST', body: request}, api, extraOptions);

        const response = refreshResult.data as AuthResponse;

        if (response) {
            api.dispatch(setAccessToken(response.accessToken));
            result = await baseQuery(args, api, extraOptions)
        } else {
            api.dispatch()
        }
    }
    return result
}
