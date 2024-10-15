import { createContext, useState, useEffect, useContext } from "react";
import { UserProfile } from "../Models/User"
import { fetchGoogleUserInfo, LoginApi, LogoutApi, PingApi, RegisterApi } from "../Services/AuthService";
import { useNavigate } from "react-router-dom";

type UserContextType = {
    user: UserProfile | null,
    registerUser: (
        username: string,
        email: string,
        password: string,
        inviteId: string | undefined
    ) => void,
    loginUser: (
        email: string,
        password: string,
        rememberMe: boolean,
        inviteId: string | undefined
    ) => void,
    logout: () => void,
    isLoggedIn: () => boolean | undefined
};

interface Props {
    children: React.ReactNode,
};

const UserContext =
    createContext<UserContextType>({} as UserContextType)

export const UserProvider = ({ children }: Props) => {
    const navigate = useNavigate();
    const [user, setUser] = useState<UserProfile | null | boolean>(null);
    const [isReady, setIsReady] = useState(false);

    useEffect(() => {
        //const user = localStorage.getItem("user");
        //const token = localStorage.getItem("token");
        PingApi()
            .catch()
            .then(res => {
                //.log(res.data.isAuthenticated)
                setUser(res.data)
            })
        
        setIsReady(true);
    }, [])

    const registerUser = async (
        email: string,
        username: string,
        password: string,
        inviteId: string | undefined
     ) => {
        const res = await RegisterApi(email, username, password, inviteId)
        if (res?.data) {
            // remove later with email verification
            //This should be in verify user
            //setUser(res.data)
            //navigate("/home")
            return { success: res.data }
        }
        else if (res?.response) {
            return { error: res.response.data }
        }
        else {
            return { error: "Whole app BUGGIN rn, try again later doe stilll" }
        }
    }

    const loginUser = async (
        email: string,
        password: string,
        rememberMe: boolean,
        inviteId: string | undefined
    ) => {
        const res = await LoginApi(email, password, rememberMe, inviteId)
        if (res?.data) {
            setUser(res?.data)
            //navigate("/home");
        }
        else if(res?.response){
            return res.response.data
        }
        else {
            return "Whole app BUGGIN rn, try again later doe stilll"
        }
    }

    

    const isLoggedIn = () => {
        return user?.isAuthed
    }

    const logout = async () => {
        await LogoutApi()
        setUser({ isAuthed: false })
        navigate("/")
    }

    return (
        <UserContext.Provider
            value={{ user, logout, isLoggedIn, registerUser, loginUser }}> 
            {isReady ? children : null}
        </UserContext.Provider>
    )
}

export const useAuth = () => useContext(UserContext);