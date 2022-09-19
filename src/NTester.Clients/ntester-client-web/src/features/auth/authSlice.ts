import {createAsyncThunk, createSlice} from "@reduxjs/toolkit";
import LoginRequest from "../../services/auth/models/LoginRequest";
import authService from "../../services/auth/AuthService";
import {fetchUserAsync} from "../account/accountSlice";

interface AuthState {
    accessToken: string | null,
    loadingStatus: "idle" | "loading" | "error";
    error: string | null;
}

const initialState: AuthState = {
    accessToken: null,
    loadingStatus: "idle",
    error: null
}

export const loginAsync = createAsyncThunk("login", async (loginRequest: LoginRequest, {dispatch}) => {
    const response = await authService.login(loginRequest);
    localStorage.setItem("AccessToken", response.accessToken);
    await dispatch(fetchUserAsync());
    return response.accessToken;
});

const authSlice = createSlice({
    name: "auth",
    initialState,
    reducers: {},
    extraReducers: builder => builder
        .addCase(loginAsync.pending, state => {
            state.loadingStatus = "loading";
        })
        .addCase(loginAsync.fulfilled, (state, action) => {
            state.loadingStatus = "idle";
            state.accessToken = action.payload;
        })
        .addCase(loginAsync.rejected, state => {
            state.loadingStatus = "error";
        })
})

export default authSlice.reducer;