import { BaseQueryFn } from "@reduxjs/toolkit/dist/query/baseQueryTypes";
import { createApi, FetchArgs, fetchBaseQuery, FetchBaseQueryError } from "@reduxjs/toolkit/query/react";
import RefreshRequest from "features/auth/models/RefreshRequest";
import AuthResponse from "features/auth/models/AuthResponse";
import AccessTokenService from "features/auth/services/AccessTokenService";

const baseQuery = fetchBaseQuery({
	baseUrl: `${process.env.REACT_APP_BASE_URL}/api`,
	credentials: "include",
	prepareHeaders: (headers) => {
		headers.set("Content-Type", "application/json");
		headers.set("X-Client", process.env.REACT_APP_CLIENT_NAME ?? "");

		const accessToken = AccessTokenService.getAccessToken();
		if (accessToken) {
			headers.set("Authorization", `Bearer ${accessToken}`);
		}

		return headers;
	},
});

const baseQueryWithReAuth: BaseQueryFn<string | FetchArgs, unknown, FetchBaseQueryError> = async (
	args,
	api,
	extraOptions
) => {
	let result = await baseQuery(args, api, extraOptions);

	if (result.error && result.error.status === 401) {
		const accessToken = AccessTokenService.getAccessToken();

		if (!accessToken) {
			return result;
		}

		const authHeaderValue = result.meta?.response?.headers.get("WWW-Authenticate");

		if (!authHeaderValue?.includes("The token expired")) {
			return result;
		}

		AccessTokenService.removeAccessToken();

		const request: RefreshRequest = { accessToken };
		const refreshResult = await baseQuery(
			{ url: "auth/refresh", method: "POST", body: request },
			api,
			extraOptions
		);

		const response = refreshResult.data as AuthResponse;

		if (response) {
			AccessTokenService.setAccessToken(response.accessToken);
			result = await baseQuery(args, api, extraOptions);
		}
	}
	return result;
};

export const apiSlice = createApi({
	baseQuery: baseQueryWithReAuth,
	endpoints: () => ({}),
});
