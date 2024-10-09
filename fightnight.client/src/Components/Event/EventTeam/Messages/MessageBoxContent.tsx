import { MessageCircleReply, Pin, Smile, UserRound } from "lucide-react"
import { Message } from "../../../../Models/Message"
import { Avatar, AvatarFallback, AvatarImage } from "../../../ui/avatar"
import { Card } from "../../../ui/card"
import { useState } from "react"
import { MessageBoxOptions } from "./MessageBoxOptions"
import { UseMutationResult } from "@tanstack/react-query"
import { useAuth } from "../../../../Context/UseAuth"

interface MessageBoxContentProps {
    msg: Message
    showOptions?: boolean
    ownsAbove: boolean
    editMessageMutation: UseMutationResult<any, Error, string, unkown>
}

export const MessageBoxContent = ({
    msg,
    showOptions = true,
    ownsAbove,
    editMessageMutation
}: MessageBoxContentProps) => {
    const [isHovered, setIsHovered] = useState(false);
    const { user } = useAuth()
    

    const outputDate = (input: any) => {
        const newDate = new Date(input)
        const time = newDate.toTimeString().split(":")
        const daysBetween = Math.abs((newDate.getTime() - new Date().getTime()) / (1000 * 3600 * 24))
        const date = Math.floor(daysBetween)
        let s = ""
        if (date === 0) {
            s = "Today at "
        }
        else if (date === 1) {
            s = "Yesterday at "
        }
        else {
            s = format(date, "dd-MM-yyyy") + " "
        }
        return s + time[0] + ":" + time[1]
    }

    return (
        <Card
        onMouseEnter={() => setIsHovered(true)}
        onMouseLeave={() => setIsHovered(false)}
            className={`border-none flex ml-auto px-3 gap-x-3 rounded-none hover:bg-red-500 transition-all flex-row items-center py-1 `}>
            
            
             <Avatar className="h-8 w-8 mr-2">
                {!ownsAbove && 
                <>
                    <AvatarImage src={""} />
                    <AvatarFallback> <UserRound /> </AvatarFallback>
                </>}
            </Avatar>
            

            <div className="flex flex-col">
                <div className="flex flex-row">
                {!ownsAbove &&
                <header className="flex flex-row gap-x-3 items-center">
                    <p className={`${msg.userId === user?.userId && "font-bold"}`}>{msg.username}</p>
                    <p className="text-xs">{/*outputDate(msg.timeStamp)*/"date"}</p>

                    </header>}
                {isHovered && showOptions && <MessageBoxOptions msg={msg} />}
                </div>

                <p>{msg.message}</p>
                {msg.isEdited && <p className="text-sm">(Edited)</p>}
            </div>
        </Card>
    )
}