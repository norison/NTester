import {createSlice, PayloadAction} from "@reduxjs/toolkit";
import User from "./models/User";
import {RootState} from "../../app/store";
import {accountApiSlice} from "./accountApiSlice";
import {authApiSlice} from "../auth/authApiSlice";

interface AccountSliceState {
    user: User | null
}

const initialState: AccountSliceState = {
    user: null
};

const accountSlice = createSlice({
    name: "account",
    initialState,
    reducers: {
        setUser(state, action: PayloadAction<User>) {
            state.user = action.payload;
        },
        removeUser(state) {
            state = initialState;
        }
    },
    extraReducers: builder => {
        builder
            .addMatcher(accountApiSlice.endpoints.getUser.matchFulfilled, (state, action) => {
                state.user = action.payload;
            })
            .addMatcher(authApiSlice.endpoints.logout.matchFulfilled, (state) => {
                state.user = null;
            });
    }
});

export const selectCurrentUser = (state: RootState) => state.account.user;
export default accountSlice.reducer;