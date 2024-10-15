import { useEffect, useRef, useState } from "react";
import { Message } from "../../../../Models/Message"
import { ScrollArea, ScrollBar } from "../../../ui/scroll-area"
import { MessageBox } from "./MessageBox"
import { InfiniteData, QueryObserverResult, RefetchOptions, UseInfiniteQueryResult } from "@tanstack/react-query";

interface ChatMessagesProps {
    getMsgs: UseInfiniteQueryResult<InfiniteData<any, unknown>, Error>
    addMsgMutation: any,
}

export const ChatMessages = ({
    getMsgs,
    addMsgMutation,
}: ChatMessagesProps) => {

    const messages = getMsgs.data || []

    const [offset, setOffset] = useState<number>(0);

    let ownsAbove = true;
    const bottomRef = useRef<HTMLDivElement>(null)
    const scrollRef = useRef(null)

    useEffect(() => {
        if (bottomRef.current) {
            bottomRef.current.scrollIntoView();
        }
    }, [messages])


    /*
    const getMessages = () => {
        if (scrollRef.current.scrollTop === 0 && getMsgs.hasNextPage && !getMsgs.isFetchingNextPage) {
            getMsgs.refetch()
        }
        console.log(getMsgs.data)
    }
    */

    

    return (
        <ScrollArea
            //onScroll={getMessages}
            className="flex-1 flex ">

            {getMsgs.isLoading && "getting more messages"}

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
            <div ref={bottomRef}></div>
        </ScrollArea>
    )
}