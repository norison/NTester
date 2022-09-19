import {FormEvent, useState} from "react";
import {useAppDispatch} from "../../app/hooks";
import {loginAsync} from "./authSlice";
import LoginRequest from "../../services/auth/models/LoginRequest";
import {useNavigate} from "react-router-dom";

export default function LoginView() {
    const dispatch = useAppDispatch();
    const navigate = useNavigate();
    const [userName, setUserName] = useState<string>("");
    const [password, setPassword] = useState<string>("");

    const handleSubmit = (event: FormEvent) => {
        event.preventDefault();

        const loginRequest: LoginRequest = {
            userName,
            password,
            clientName: process.env.REACT_APP_CLIENT_NAME!
        }

        console.log(loginRequest);

        dispatch(loginAsync(loginRequest)).then(() => navigate("/")).catch(e => console.log(e));
    }

    return (
        <form onSubmit={handleSubmit}>
            <label htmlFor="userName">User Name: </label>
            <input type="text" value={userName} onChange={e => setUserName(e.target.value)}/>
            <label htmlFor="password">Password: </label>
            <input type="password" value={password} onChange={e => setPassword(e.target.value)}/>
            <input type="submit" value="Login"/>
        </form>
    );
}
