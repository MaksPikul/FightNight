import axios from "axios";
import { UserProfile, UserProfileToken } from "../Models/User";


const api = "https://localhost:7161/api/"

export const VerifyEmailApi = async (
    token: string
) => {
    try {
        const data = await axios.patch(api + "account/verify", {
            token: token
        });
        return data;
    }
    catch (err) {
        return err
    }
}

export const ChangeUserPassword = async (
    newPassword: string,
    token : string
) => {
    try {
        const data = await axios.patch(api + "account/change-password", {
            newPassword,
            token
        });
        return data;
    }
    catch (err) {
        return err
    }
}

export const LoginApi = async (
    email: string,
    password: string,
    rememberMe: boolean
) => {
    try {
        const data = await
            axios.post<UserProfileToken>(api + "account/login", {
                email,
                password,
                rememberMe
            }, {
                withCredentials: true
            });
           
        return data
    }
    catch (err) {
        return err
    }
}

export const LogoutApi = async () => {
    
    await axios.post<UserProfile>(api + "account/logout", {}, {
            withCredentials: true
        });
}

export const PingApi = async () => {
    try {
        const data = await axios.get<UserProfile>(api + "account/ping", {
                withCredentials: true
            });
        return data
    }
    catch (err) {
        console.log(err)
    }
}

export const RegisterApi = async (
    email: string,
    username: string,
    password: string
) => {
    try {
        const data = await
            axios.post<UserProfile>(api + "account/register", {
                username:username,
                email:email,
                password:password
            });
        return data
    }
    catch (err) {
        return err
    }
}

export function getGoogleOAuthURL() {

    const rootUrl = 'https://accounts.google.com/o/oauth2/v2/auth'

    const options = {
        redirect_uri: 'https://localhost:7161/api/account/oauth/google',
        client_id: import.meta.env.VITE_GOOGLE_CLIENT_ID,
        access_type: "offline",
        response_type: "code",
        prompt: "consent",
        scope: [
            'https://www.googleapis.com/auth/userinfo.profile',
            'https://www.googleapis.com/auth/userinfo.email'
        ].join(" ")
    }

    const qs = new URLSearchParams(options)

    return `${rootUrl}?${qs.toString()}`

}