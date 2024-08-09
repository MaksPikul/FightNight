import './App.css';
import { useAuth, UserProvider } from './Context/UseAuth';
import { ModalProvider } from './Context/UseModal';
import { Route, Routes, BrowserRouter } from "react-router-dom";
import PrivateViews from './Components/Auth/PrivateViews';
import { LoginForm } from './Components/Auth/login-form';
import { RegisterForm } from './Components/Auth/RegisterForm';
import { TooltipProvider } from './Components/ui/tooltip';



function App() {

    const { isLoggedIn } = useAuth()

    //once i add routing middle ware,, i can remove the * path
    return  (


        <BrowserRouter >
    <UserProvider>
            <TooltipProvider>
                <ModalProvider / >
     
        <Routes>
            <Route path="/" element={<LoginForm />}> </Route>
            <Route path="/register" element={<RegisterForm />}> </Route>
                <Route element={<PrivateViews />}>

                <Route path="/home" element={<div> at home lolers</div>} />
            </Route>
            <Route path="*" element={<LoginForm />} />
            </Routes>
        
            </TooltipProvider>
            </UserProvider >
        </BrowserRouter>
    
    )
}

export default App;