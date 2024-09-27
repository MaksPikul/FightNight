import { useModal } from "../../../../Hooks/use-modal-store"
import { MessageCircleReply, Pin, Smile, Trash2 } from "lucide-react"
import { Message } from "../../../../Models/Message"

interface MessageBoxOptionsProps {
    msg: Message
}

export const MessageBoxOptions = ({
    msg
}: MessageBoxOptionsProps) => {

    const { onOpen } = useModal()
    const isAdmin = true;

    return (
        <div className="flex flex-row">
            <button> <MessageCircleReply /> </button>
            <button> <Pin className="rotate-45" /> </button>
            <button> <Smile /> </button>
            {isAdmin && <button onClick={() => onOpen("ConfirmDeleteMessage", { message: msg })}> <Trash2 className="text-red-500" /> </button>}
        </div>
    )
}