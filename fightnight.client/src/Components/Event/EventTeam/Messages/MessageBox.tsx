import { MessageCircleReply, Pin, Smile, Trash2, UserRound } from "lucide-react"
import { Message } from "../../../Models/Message"
import { Avatar, AvatarFallback, AvatarImage } from "../../ui/avatar"
import { Button } from "../../ui/button"
import { Card } from "../../ui/card"
import { format } from "date-fns"
import { useState } from "react"
import { useAuth } from "../../../../Context/UseAuth"
import {
    ContextMenu,
    ContextMenuItem,
    ContextMenuTrigger,
    ContextMenuContent,
    ContextMenuSub,
    ContextMenuSubTrigger,
    ContextMenuSubContent
} from "../../../ui/context-menu"
import { MessageBoxContent } from "./MessageBoxContent"
import { MsgBoxContextMenu } from "./MsgBoxContextMenu"
import { useMutation, useQueryClient } from "@tanstack/react-query"
import { UpdateMessageApi } from "../../../../Services/MessageService"

interface MessageBoxProps {
    msg: Message,
    ownsAbove: boolean
}

export const MessageBox = ({
    msg,
    ownsAbove
}: MessageBoxProps) => {
    const { user } = useAuth()
    const [isEditing, setIsEditing] = useState()

    const isAdmin = true;
    const isOwner = user?.userId === msg.userId;


    const queryClient = useQueryClient()

    const editMessageMutation = useMutation({
        mutationFn: (newMsg: string ) => UpdateMessageApi(msg.id, msg.eventId, newMsg),
        onSettled: async () => {
            return await queryClient.invalidateQueries({ queryKey: ["Messages", msg.eventId] })
        },
    })

    return (
        <ContextMenu>
            <ContextMenuTrigger>
                <MessageBoxContent
                    msg={msg}
                    ownsAbove={ownsAbove}
                    editMessageMutation={editMessageMutation} />
            </ContextMenuTrigger>

            <MsgBoxContextMenu
                msg={msg}
                isAdmin={isAdmin}
                isOwner={isOwner}
                editMessageMutation={editMessageMutation} />
        </ContextMenu>
    )
}