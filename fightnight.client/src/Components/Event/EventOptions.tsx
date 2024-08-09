"use client"

import { useTransition } from "react"
//import { redirect } from "next/dist/server/api-utils"
//import { CreateEvent, getUserEvents } from '@/data/events'
//import { useSession } from "next-auth/react"
//import { useRouter } from "next/navigation"
import { ToolTippedButton } from "../ttButton"
import { Plus, TicketCheck, UserRoundPen } from "lucide-react"

export const EventOptions = () => {

    //const { data: session} = useSession()
    //const router = useRouter()

    const [isPending, startTransition] = useTransition()

    //protect this route for paying users only in future
    const submitCreateEvent = () => {
        startTransition(()=> {
            CreateEvent(session?.user?.id)
            .then((res)=>{
                if (res.error) {
                    //toast error
                }
                else {
                    //router.push(`/event/${res}`)
                }
            })
        })
    }

    const submitRegisterToEvent = () => {
        startTransition(()=> {
            console.log(session?.user?.id)
            getUserEvents(session?.user?.id)
            .then((res)=>{
                console.log(res)
                if (res.error) {
                    //toast error
                }
                else {
                    //router.push(`/event/${res.id}`)
                }
            })
        })
    }

    return (
        <div
        className="flex flex-row justify-center">


            <ToolTippedButton 
            trigger={<Plus />}
            content="Create Event"
            disabled={isPending}
            onClick={submitCreateEvent}/>

            <ToolTippedButton 
            trigger={<UserRoundPen />}
            content="Register to Event"
            disabled={isPending}
            onClick={submitRegisterToEvent}/>

            <ToolTippedButton 
            trigger={<TicketCheck />}
            content="Purchase Tickets"
            disabled={isPending}
            onClick={submitRegisterToEvent}/>


            {/*
            <Button
            disabled={isPending}
            onClick={submitCreateEvent}
            title="Create Event"/>
            <Button
            onClick={submitRegisterToEvent}
            title="Register to Event" >
                reg
            </Button>
            <Button
            title="Buy tickets for event" />
            */}
        </div>
    )
}