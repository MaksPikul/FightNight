import { FcGoogle} from "react-icons/fc"
import { FaApple } from "react-icons/fa";
import { DEFAULT_LOGIN_REDIRECT } from "../../routes";
import { Button } from "../ui/button";

export const Social = () => {

    const onClick = (provider:"google") => {
        //google sign in   
    }

    return (
        <div 
        className="flex flex-col justify-center">
            <Button
            className=" flex"
            onClick={()=>onClick("google")}> 
                <p>Log in with Google</p>
                <FcGoogle className="h-5 w-5"/>
            </Button>
        
            <Button
            className=" flex"
            onClick={()=>onClick("google")}> 
                <p>Log in with Apple</p>
                <FaApple className="h-5 w-5"/>
            </Button>
        </div>
    )
}