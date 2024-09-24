import { Message } from "../../../Models/Message"
import { ScrollArea } from "../../ui/scroll-area"
import { MessageBox } from "./MessageBox"

interface ChatMessagesProps {
    messages: Message[],
    addMsgMutation: any
}

export const ChatMessages = ({
    messages,
    addMsgMutation
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
            {addMsgMutation.isPending && <MessageBox msg={addMsgMutation.variables} />}
            {addMsgMutation.isError && <button onClick={() => addMsgMutation.mutate(addMsgMutation.variables)}>Retry</button> }
        </ScrollArea>
    )
}