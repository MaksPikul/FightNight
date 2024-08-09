import { createContext, useState, useEffect, useContext } from "react";
import { UserProfile } from "../Models.ts/User"
import { LoginApi, RegisterApi } from "../Services/AuthService";
import { useNavigate } from "react-router-dom";
import axios from "axios";

type UserContextType = {
    user: UserProfile | null,
    token: string | null,
    registerUser: (
        username: string,
        email: string,
        password: string
    ) => void,
    loginUser: (
        email: string,
        password: string
    ) => void,
    logout: () => void,
    isLoggedIn: () => boolean
};

interface Props {
    children: React.ReactNode,
};

const UserContext =
    createContext<UserContextType>({} as UserContextType)

export const UserProvider = ({ children }: Props) => {
    const navigate = useNavigate();
    const [user, setUser] = useState<UserProfile | null>(null);
    const [isReady, setIsReady] = useState(false);
    const [token, setToken] = useState<string | null>(null);

    useEffect(() => {
        const user = localStorage.getItem("user");
        const token = localStorage.getItem("token");

        if (user && token) {
            setUser(JSON.parse(user))
            setToken(token)
            axios.defaults.headers.common["Authorization"] = "Bearer " + token;
        }
        setIsReady(true);
    }, [])

    const registerUser = async (
        email: string,
        username: string,
        password: string,
     ) => {
        await RegisterApi(email, username, password)
            .then(res => {
                if (res) {
                    localStorage.setItem("token", res?.data.token)
                    const userObj = {
                        email: res?.data.email,
                        username: res?.data.username
                    }
                    localStorage.setItem("user", JSON.stringify(userObj))
                    setToken(res?.data.token);
                    setUser(userObj)
                    //toast success
                    navigate("/home");
                }
            })
            .catch (e => console.log(e))//add error toast
    }

    const loginUser = async (
        email: string,
        password: string,
    ) => {
        await LoginApi(email, password)
            .then(res => {
                if (res) {
                    localStorage.setItem("token", res?.data.token)
                    const userObj = {
                        email: res?.data.email,
                        username: res?.data.username
                    }
                    localStorage.setItem("user", JSON.stringify(userObj))
                    setToken(res?.data.token);
                    setUser(userObj)
                    //toast success
                    navigate("/home");
                }
            })
            .catch(e => console.log(e))//add error toast
    }

    const isLoggedIn = () => {
        return !!user;
    }

    const logout = () => {
        localStorage.removeItem("token")
        localStorage.removeItem("user")
        setToken("")
        setUser(null)
        navigate("/")
    }

    return (
        <UserContext.Provider
            value={{ loginUser, user, token, logout, isLoggedIn, registerUser, loginUser }}>
            {isReady ? children : null}
        </UserContext.Provider>
    )
}

export const useAuth = () => useContext(UserContext);