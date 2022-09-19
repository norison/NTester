import {useAppSelector} from "./app/hooks";

function App() {
    const user = useAppSelector(state => state.accountReducer.user);
    
    return (
        <div className="App">
           <div>{user?.userName}</div>
           <div>{user?.email}</div>
           <div>{user?.name}</div>
           <div>{user?.surname}</div>
        </div>
    );
}

export default App;
