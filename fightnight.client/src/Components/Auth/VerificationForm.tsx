

import { useCallback, useEffect, useState } from "react"
import { Card, CardContent, CardHeader } from "../ui/card"
import { CircleLoader } from "react-spinners"
import { FormError } from "../Misc/formError"
import { FormSuccess } from "../Misc/formSuccess"

export const VerificationForm = () => {
    
    const [error, setError] = useState<string | undefined>("");
    const [success, setSuccess] = useState<string | undefined>("");

    //const searchParams = useSearchParams()
    //const token = searchParams.get("token")
    
    const onLoad = useCallback(()=> {
        const postData = async () => {
            const response = await fetch(`/api/auth/verification/${token}`,{
                method: "POST"
            })
            console.log(response)
            return response
        }
        setSuccess("")
        setError("")

        postData()
        .catch(err => console.log(err))
        .then((data)=>{
            setSuccess(data?.success)
            setError(data?.error)
        })

    }, [token])

    useEffect(() => {
        onLoad();
    },[onLoad])



    return (
        <Card>
            <CardHeader>

            </CardHeader>

            <CardContent>
                <div className="flex items-center w-full justify-center">
                    {!success && !error && (
                        <CircleLoader />
                    )}
                    <FormError message={error}/>
                    <FormSuccess message={success}/>
                </div>
            </CardContent>
        </Card>
    )
}