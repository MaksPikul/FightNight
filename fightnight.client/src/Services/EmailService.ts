import axios from "axios";
const api = "https://localhost:7161/api/"

export const SendForgotPasswordEmail = async (
    token: string
) => {
    try {
        const data = await axios.post(api + "emails/forgot-password", {
            token: token
        });
        return data;
    }
    catch (err) {
        return err
    }
}
