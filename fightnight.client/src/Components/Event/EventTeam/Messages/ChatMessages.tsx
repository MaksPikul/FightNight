import { useEffect, useRef, useState } from "react";
import { Message } from "../../../../Models/Message"
import { ScrollArea, ScrollBar } from "../../../ui/scroll-area"
import { MessageBox } from "./MessageBox"
import { InfiniteData, QueryObserverResult, RefetchOptions, UseInfiniteQueryResult } from "@tanstack/react-query";
import { useInView } from "react-intersection-observer";

interface ChatMessagesProps {
    addMsgMutation: any,
    test: any
}

export const ChatMessages = ({
    addMsgMutation,
    test
}: ChatMessagesProps) => {
    const [isInitialLoad, setIsInitialLoad] = useState(true);

    const latestPageRef = useRef()
    const { ref, inView } = useInView()
    const bottomRef = useRef<HTMLDivElement>(null)
    const topRef = useRef<HTMLDivElement>(null)
    let ownsAbove = true;

    if (!test.data) return <div>"loading"</div>

    const x = test.data.pages
    const messages = [...x].reverse()

    


    /*
    I want to scroll to bottom, 
    - When page loads
    - When i submit a message

    when a new page fetcheds
    i want to go to the bottom of the new page
    */



    
    useEffect(() => {
        if (bottomRef.current && isInitialLoad ) {
            bottomRef.current.scrollIntoView();
            setIsInitialLoad(false)
        }
    }, [messages])
    

    useEffect(() => {
        if (inView && test.hasNextPage && latestPageRef.current) {
            test.fetchNextPage()

            latestPageRef.current.scrollIntoView();
            console.log(latestPageRef.current)
        }
    }, [messages])

    const setLatestPageRef = (element) => {
        if (element) {
            latestPageRef.current = element; // Set the latest page ref to the last page
            // Scroll the latest page into view
            element.scrollIntoView({ behavior: 'smooth' });
        }
    };
    

    /*
    useEffect(() => {
        if (inView && test.hasNextPage) {
            test.fetchNextPage()
            //const latestPage = test.data.pages.length - 1;
            const listNode = messageRef.current.querySelectorAll('li');
            console.log(listNode.length - 1)
            console.log(listNode[0])
            
            if (listNode[0]) {
                const x = listNode[0]
                x.scrollIntoView({
                    block: 'nearest',
                    inline: 'center'
                });
            }
        }
    }, [test.fetchNextPage, inView])
    */
    return (
        <ScrollArea
            ref={topRef }
            className="flex-1 flex ">
            <div ref={ref}></div>

            <ul>
                {messages.map((page, index) => { 
                return (
                    <>
                            {page.data.map((msg) => {
                            return <MessageBox
                                ownsAbove={false }
                                    msg={msg} />
                            })}
                        {console.log(index, test.data.pages.length - 1, page) }
                        <div key={index} className="my-2" ref={index === test.data.pages.length - 1 ? setLatestPageRef : null}/> 
                    </>
                    )
            })}
            </ul>
            


            {addMsgMutation.isPending && <MessageBox ownsAbove={ownsAbove} msg={addMsgMutation.variables} />}
            {addMsgMutation.isError && <button onClick={() => addMsgMutation.mutate(addMsgMutation.variables)}>Retry</button>}
            <div ref={bottomRef}></div>
        </ScrollArea>
    )
    

    

    
}