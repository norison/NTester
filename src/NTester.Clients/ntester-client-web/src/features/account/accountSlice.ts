import {createAsyncThunk, createSlice} from "@reduxjs/toolkit";
import User from "../../services/account/models/User";
import accountService from "../../services/account/AccountService";

interface AccountSlice {
    user: User | null;
    loadingStatus: "idle" | "loading" | "error",
    error: string | null;
}

const initialState: AccountSlice = {
    user: null,
    loadingStatus: "idle",
    error: null
}

export const fetchUserAsync = createAsyncThunk("fetchUser", async () => {
    return await accountService.getUser();
});

const accountSlice = createSlice({
    name: "account",
    initialState,
    reducers: {},
    extraReducers: builder => builder
        .addCase(fetchUserAsync.pending, (state) => {
            state.loadingStatus = "loading";
        })
        .addCase(fetchUserAsync.fulfilled, (state, action) => {
            state.loadingStatus = "idle";
            state.user = action.payload;
        })
        .addCase(fetchUserAsync.rejected, (state, action) => {
            console.log(action);
            state.loadingStatus = "error";
        })
})

export default accountSlice.reducer;