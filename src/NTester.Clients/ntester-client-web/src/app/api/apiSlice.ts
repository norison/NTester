import {
    BaseQueryFn
} from "@reduxjs/toolkit/dist/query/baseQueryTypes";
import {
    createApi,
    FetchArgs,
    fetchBaseQuery,
    FetchBaseQueryError,
} from "@reduxjs/toolkit/query/react";
import RefreshRequest from "../../features/auth/models/RefreshRequest";
import AuthResponse from "../../features/auth/models/AuthResponse";
import {RootState} from "../store";
import {removeAccessToken, setAccessToken} from "../../features/auth/authSlice";

const baseQuery = fetchBaseQuery({
    baseUrl: `${process.env.REACT_APP_BASE_URL}/api`,
    credentials: "include",
    prepareHeaders: (headers, {getState}) => {
        headers.set("Content-Type", "application/json");
        headers.set("X-Client", process.env.REACT_APP_CLIENT_NAME ?? "");

        const accessToken = (getState() as RootState).auth.accessToken;
        if (accessToken) {
            headers.set("Authorization", `Bearer ${accessToken}`);
        }

        return headers;
    },
});

const baseQueryWithReAuth: BaseQueryFn<string | FetchArgs, unknown, FetchBaseQueryError> = async (args, api, extraOptions) => {
    let result = await baseQuery(args, api, extraOptions);

    if (result.error && result.error.status === 401) {
        const state = api.getState() as RootState;
        const accessToken = state.auth.accessToken;

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
            result = await baseQuery(args, api, extraOptions);
        } else {
            api.dispatch(removeAccessToken());
        }
    }
    return result;
}

export const apiSlice = createApi({
    baseQuery: baseQueryWithReAuth,
    endpoints: builder => ({})
});
