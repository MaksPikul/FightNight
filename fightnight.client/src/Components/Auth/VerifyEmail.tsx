import { CircleLoader } from "react-spinners";
import { CardWrapper } from "./card-wrapper"
import { FormError } from "../Misc/formError";
import { FormSuccess } from "../Misc/formSuccess";
import { useCallback, useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import { VerifyEmailApi } from "../../Services/AuthService";

export const VerifyEmail = () => {

    const [error, setError] = useState("");
    const [success, setSuccess] = useState("");

    const { token } = useParams();

    const onSubmit = useCallback( async() => {
        if (!token) {
            setError("Missing token")
            return
        }
        setError("what")
        const res = await VerifyEmailApi(token)
        if (res?.data) {
            setSuccess(res?.data)
        }
        else if (res?.response) {
            //Show Resend Button
            setError(res?.response.data+"here until api made")
        }
        else {
            setError("Whole app BUGGIN rn, try again later doe stilll")
        }
    }, [token])


    useEffect(() => {
        onSubmit();
    }, [onSubmit])

    return (
        <CardWrapper
            headerLabel="Email Verification"
            backButtonLabel=""
            backButtonHref="/login"
            backButtonHrefLabel="To login"
            showSocial={false }>

            <div className="flex items-center w-full justify-center">
                {!success && !error && (
                    <CircleLoader />
                )}
                <FormError message={error} />
                <FormSuccess message={success} />
            </div>
        </CardWrapper>
    )
}