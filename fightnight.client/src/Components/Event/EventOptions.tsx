"use client"

import { useState, useTransition } from "react"

//import { useRouter } from "next/navigation"
import { ToolTippedButton } from "../Misc/ttButton"
import { Plus, TicketCheck, UserRoundPen } from "lucide-react"
import { useAuth } from "../../Context/UseAuth"
import { useNavigate } from "react-router-dom"
import { useModal } from "../../Hooks/use-modal-store"

export const EventOptions = () => {

    
    const { onOpen } = useModal()

    //protect this route for paying users only in future
    
    
    return (
        <div
        className="flex flex-row justify-center gap-x-6">

            <ToolTippedButton 
            trigger={<Plus />}
                content="Create Event"
                disabled={false}
            onClick={()=>onOpen("CreateEvent")}/>

            <ToolTippedButton 
            trigger={<UserRoundPen />}
                content="Register to Event"
                disabled={false }
                onClick={()=>null}/>

            <ToolTippedButton 
            trigger={<TicketCheck />}
                content="Purchase Tickets"
                disabled={false}
            onClick={()=>null }/>

        </div>
    )
}