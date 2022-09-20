import {createAsyncThunk, createSlice} from "@reduxjs/toolkit";
import User from "./models/User";
import accountService from "./accountService";
import {LoadingStatus} from "../../common/enums/LoadingStatus";

interface AccountSlice {
    user: User | undefined;
    loadingStatus: LoadingStatus,
    error: string | undefined;
}

const initialState: AccountSlice = {
    user: undefined,
    loadingStatus: LoadingStatus.Idle,
    error: undefined
}

export const fetchUserAsync = createAsyncThunk("account/fetchUser", async () => {
    return await accountService.getUser();
});

const accountSlice = createSlice({
    name: "account",
    initialState,
    reducers: {},
    extraReducers: builder => builder
        .addCase(fetchUserAsync.pending, (state) => {
            state.loadingStatus = LoadingStatus.Loading;
        })
        .addCase(fetchUserAsync.fulfilled, (state, action) => {
            state.loadingStatus = LoadingStatus.Idle;
            state.user = action.payload;
        })
        .addCase(fetchUserAsync.rejected, (state, action) => {
            state.loadingStatus = LoadingStatus.Error;
            state.error = action.error.message;
        })
})

export default accountSlice.reducer;