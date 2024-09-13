import { FcGoogle} from "react-icons/fc"
import { FaApple } from "react-icons/fa";
import { DEFAULT_LOGIN_REDIRECT } from "../../routes";
import { Button } from "../ui/button";
import { Link } from "react-router-dom";
import { getGoogleOAuthURL } from "../../Services/AuthService";

export const Social = () => {

    const onClick = (provider:"google") => {
        //google sign in   
    }

    return (
        <div 
            className="flex flex-col justify-center">
            <a href={getGoogleOAuthURL()}>
            <Button className="w-64 flex"> 
                <a href={getGoogleOAuthURL()}>Sign In with Google</a>
                <FcGoogle className="h-5 w-5"/>
                    </Button>
            </a>
        
            <a >
                <Button className="w-64 flex">
                    <a href={getGoogleOAuthURL()}>Sign In with Apple</a>
                    <FaApple className="h-5 w-5" />
                </Button>
            </a>
        </div>
    )
}