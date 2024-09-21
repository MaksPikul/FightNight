import { MessageCircleReply, Pin, Smile, Trash2, UserRound } from "lucide-react"
import { Message } from "../../../Models/Message"
import { Avatar, AvatarFallback, AvatarImage } from "../../ui/avatar"
import { Button } from "../../ui/button"
import { Card } from "../../ui/card"
import { format } from "date-fns"
import { useState } from "react"
import { useAuth } from "../../../Context/UseAuth"
import { useModal } from "../../../Hooks/use-modal-store"
import {
    ContextMenu,
    ContextMenuItem,
    ContextMenuTrigger,
    ContextMenuContent,
    ContextMenuSub,
    ContextMenuSubTrigger,
    ContextMenuSubContent
} from "../../ui/context-menu"

interface MessageBoxProps {
    msg: Message
}

export const MessageBox = ({
    msg
}: MessageBoxProps) => {
    const [isHovered, setIsHovered] = useState(false);
    const { user } = useAuth()
    const { onOpen } = useModal()

    const isAdmin = true;
    const isOwner = user?.userId === msg.userId;

    const outputDate = (input: any) => {
        const newDate = new Date(input)
        const time = newDate.toTimeString().split(":")
        const daysBetween = Math.abs((newDate.getTime() - new Date().getTime()) / (1000 * 3600 *24))
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

        <ContextMenu>
            <ContextMenuTrigger>
                <Card
                    onMouseEnter={() => setIsHovered(true)}
                    onMouseLeave={() => setIsHovered(false)}
                    className="flex ml-auto px-3 gap-x-3 rounded-none hover:bg-red-500 transition-all flex-row items-center py-1 ">
                    <Avatar className="h-8 w-8 mr-2">
                        <AvatarImage src={""} />
                        <AvatarFallback> <UserRound /> </AvatarFallback>
                    </Avatar>

                    <div className="flex flex-col">
                        <header className="flex flex-row gap-x-3 items-center">
                            <p className={`${msg.userId === user?.userId && "font-bold"}`}>{msg.username}</p>
                            <p className="text-xs">{outputDate(msg.timeStamp)}</p>

                            {isHovered &&
                            <div className="flex flex-row">
                                <button> <MessageCircleReply /> </button>
                                <button> <Pin /> </button>
                                <button> <Smile /> </button>
                                {isAdmin && <button onClick={() => onOpen("ConfirmDeleteMessage", { message: msg })}> <Trash2 className="text-red-500"/> </button>}
                            </div>}
                        </header>

                        <p>{msg.message}</p>
                        {msg.isEdited && <p className="text-sm">(Edited)</p> }
                    </div>
                </Card>
            </ContextMenuTrigger>

            <ContextMenuContent>
                {isOwner && <ContextMenuItem
                    onClick={() => null}>Edit Message</ContextMenuItem>}

                <ContextMenuSub>
                    <ContextMenuSubTrigger inset>Add Reaction</ContextMenuSubTrigger>
                    <ContextMenuSubContent className="w-48">
                        <ContextMenuItem>List of reactions</ContextMenuItem>
                    </ContextMenuSubContent>
                </ContextMenuSub>

                <ContextMenuItem
                    onClick={() => null}>Reply</ContextMenuItem>

                <ContextMenuItem
                    onClick={() => null}>Pin</ContextMenuItem>

                {isAdmin && <ContextMenuItem
                    onClick={() => null}>Delete Message</ContextMenuItem>}
            </ContextMenuContent>
        </ContextMenu>
    )
}