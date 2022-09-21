import {createSlice, PayloadAction} from "@reduxjs/toolkit";
import User from "./models/User";
import {RootState} from "../../app/store";

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
    }
});

export const selectCurrentUser = (state: RootState) => state.account.user;

export const {setUser, removeUser} = accountSlice.actions;
export default accountSlice.reducer;