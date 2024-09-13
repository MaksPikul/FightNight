import { useAuth } from "../../Context/UseAuth";

export const SigninGoogle = () => {

    const queryParameters = new URLSearchParams(window.location.search)
    const code = queryParameters.get("code")
    const { GoogleLogin } = useAuth();


    GoogleLogin(code)
    
    return (
        <div> loading</div>
    )
}