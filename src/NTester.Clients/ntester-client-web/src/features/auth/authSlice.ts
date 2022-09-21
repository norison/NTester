import {createSlice, PayloadAction} from "@reduxjs/toolkit";
import {RootState} from "../../app/store";

interface AuthSliceState {
    accessToken: string | null;
}

const initialState: AuthSliceState = {
    accessToken: null
};

const authSlice = createSlice({
    name: "auth",
    initialState,
    reducers: {
        setAccessToken(state, action: PayloadAction<string>) {
            state.accessToken = action.payload;
        },
        removeAccessToken(state) {
            state = initialState;
        }
    }
});

export const selectAccessToken = (state: RootState) => state.auth.accessToken;

export const {setAccessToken, removeAccessToken} = authSlice.actions;
export default authSlice.reducer;