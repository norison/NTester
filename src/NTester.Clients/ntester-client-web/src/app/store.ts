import {configureStore, ThunkAction, Action} from '@reduxjs/toolkit';
import authReducer from "../features/auth/authSlice";
import accountReducer from "../features/account/accountSlice";

export const store = configureStore({
    reducer: {
        authReducer,
        accountReducer
    },
});

export type AppDispatch = typeof store.dispatch;
export type RootState = ReturnType<typeof store.getState>;
export type AppThunk<ReturnType = void> = ThunkAction<ReturnType,
    RootState,
    unknown,
    Action<string>>;
