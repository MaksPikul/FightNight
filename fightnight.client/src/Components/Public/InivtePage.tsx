import { useMutation, useQuery } from "@tanstack/react-query"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "../ui/card"
import { Button } from "../ui/button"
import { useNavigate, useParams } from "react-router-dom"
import { FormError } from "../Misc/formError"
import { useState } from "react"
import { AcceptInvite } from "../../Services/MemberService"

export const InvitePage = () => {

    const  [error, setError] = useState()

    const { inviteId } = useParams()

    const inviteQuery = useQuery({
        queryFn: () => null, //GetInviteInfo(),//(Invite),
        queryKey: ['Invite', inviteId],
    })
    const navigate = useNavigate()

    const sendInvite = useMutation({
        mutationFn: () => AcceptInvite(inviteId),
        onSuccess: (data) => {
            
        },
        onError: (err) => {
            setError(err)
        }
    })



    return (
        <Card>
            <CardHeader>
                <CardTitle>
                    You Have been Invited to "EVENT"
                </CardTitle>
                <CardDescription>
                    Do you wish to accept Invite?
                </CardDescription>
            </CardHeader>

            <CardContent>

                <Button
                    onClick={()=> sendInvite.mutate()}>
                    Yes, Take me to the event
                </Button>

                <Button
                    onClick={() => navigate("/")}>
                    No, Take me back to home
                </Button>
                
                <FormError message={error}/>

            </CardContent>
        </Card>
    )
}