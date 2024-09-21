import { Message } from "../../../Models/Message"
import { ScrollArea } from "../../ui/scroll-area"
import { MessageBox } from "./MessageBox"

interface ChatMessagesProps {
    messages: Message[]
}

export const ChatMessages = ({
    messages
}:ChatMessagesProps) => {

    return (
        <ScrollArea className="flex-1 flex ">
            {messages.map((msg) => {
                return (
                <div key={msg.id}>
                    <MessageBox msg={msg} />
                </div>
                )
            })}
        </ScrollArea>
    )
}