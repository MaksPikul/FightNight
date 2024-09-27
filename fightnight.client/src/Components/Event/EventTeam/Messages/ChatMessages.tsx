import { useEffect, useRef } from "react";
import { Message } from "../../../../Models/Message"
import { ScrollArea, ScrollBar } from "../../../ui/scroll-area"
import { MessageBox } from "./MessageBox"

interface ChatMessagesProps {
    messages: Message[],
    addMsgMutation: any,
}

export const ChatMessages = ({
    messages,
    addMsgMutation,
}: ChatMessagesProps) => {

    let ownsAbove = true;

    const bottom = useRef<HTMLDivElement>(null)

    useEffect(() => {
        if (bottom.current) {
            bottom.current.scrollIntoView();
        }
    }, [messages])

   // function updateScroll() {
        //element.scrollTop = element.scrollHeight;
    //}
    

    return (
        <ScrollArea
            className="flex-1 flex ">
        
            {messages.map((msg, index) => {
                let prevUserId;

                if (index === 0) {
                    prevUserId = "-1"
                }
                else {
                    prevUserId = messages[index - 1].userId;
                }
                ownsAbove = prevUserId === msg.userId

                return (
                <div key={msg.id}>
                        <MessageBox
                            ownsAbove={ownsAbove}
                            msg={msg} />
                </div>
                )
            })}
            {addMsgMutation.isPending && <MessageBox ownsAbove={ownsAbove}  msg={addMsgMutation.variables} />}
            {addMsgMutation.isError && <button onClick={() => addMsgMutation.mutate(addMsgMutation.variables)}>Retry</button>}
            <div ref={bottom}></div>
        </ScrollArea>
    )
}