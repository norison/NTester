import {createSlice, PayloadAction} from "@reduxjs/toolkit";
import User from "../account/models/User";
import {RootState} from "../../app/store";

interface AuthSliceState {
    user: User | null,
    accessToken: string | null;
}

const initialState: AuthSliceState = {
    user: null,
    accessToken: null
};

const authSlice = createSlice({
    name: "auth",
    initialState,
    reducers: {
        setAccessToken(state, action: PayloadAction<string>) {
            state.accessToken = action.payload;
        },
        setUser(state, action: PayloadAction<User>) {
            state.user = action.payload;
        },
        reset(state) {
            state = initialState;
        }
    }
});

export const selectCurrentUser = (state: RootState) => state.auth.user;
export const selectAccessToken = (state: RootState) => state.auth.accessToken;

export const {setAccessToken, setUser, reset} = authSlice.actions;
export default authSlice.reducer;