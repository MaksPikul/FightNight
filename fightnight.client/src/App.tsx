//import './App.css';
import { useAuth} from './Context/UseAuth';
import { Route, Routes, Navigate } from "react-router-dom";
import { PrivateViews, PublicViews } from './Components/Auth/Views';
import { LoginForm } from './Components/Auth/login-form';
import { RegisterForm } from './Components/Auth/RegisterForm';
import { HomePage } from './Components/Home/HomePage';
import { EventPage } from './Components/Event/EventPage';
import { EventTeamPage } from './Components/Event/EventTeamPage';
import { EventLayout } from './Components/Event/EventLayout';
import { VerifyEmail } from './Components/Auth/VerifyEmail';
import { ResetPassword } from './Components/Auth/ResetPassword';
import { ForgotPassword } from './Components/Auth/ForgotPassword';

function App() {

    const { isLoggedIn } = useAuth();

    // Check if event exists and if user is able to access it.


   
    return  (
        isLoggedIn() === undefined ? <div>loading</div>
        :
        <Routes>
            <Route element={<PublicViews />}>
                <Route path="/" element={<div>landing page</div>} /> 
                <Route path="/login" element={<LoginForm />} /> 
                <Route path="/register" element={<RegisterForm />} /> 
                <Route path="/verify-email/:token" element={<VerifyEmail />} />
                <Route path="/forgot-password/" element={<ForgotPassword />} />
                <Route path="/reset-password/:token" element={<ResetPassword />} />
            </Route>
                    
            <Route element={<PrivateViews />}>
                <Route path="/home" element={<HomePage />} />
                <Route element={<EventLayout />} >
                <Route path="/event/:eventId" element={<EventPage />} />
                <Route path="/event/:eventId/team" element={<EventTeamPage />} />
                </Route>
            </Route>
            
            <Route path="*" element={<Navigate to="/" />} />
        </Routes>
    )
}

export default App;