import {apiSlice} from "../../app/api/apiSlice";
import User from "./models/User";

export const accountApiSlice = apiSlice.injectEndpoints({
    endpoints: builder => ({
        getUser: builder.query<User, void>({
            query: () => "/account",
            keepUnusedDataFor: 0
        })
    })
});

export const {useGetUserQuery, useLazyGetUserQuery} = accountApiSlice;