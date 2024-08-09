import axios from "axios";
import { UserProfileToken } from "../Models.ts/User";


const api = "https://localhost:5173/api"

export const LoginApi = async (
    email: string,
    password: string
) => {
    try {
        const data = await
            axios.post<UserProfileToken>(api + "account/login", {
                email,
                password
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
            axios.post<UserProfileToken>(api + "account/register", {
                username,
                email,
                password
            });
        return data
    }
    catch (err) {
        console.log(err)
    }
}