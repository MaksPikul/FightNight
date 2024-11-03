import { useAuth } from "../../../Context/UseAuth"
import { useMessage } from "../../../Context/UseMessage"
import { Event } from "../../../Models/Event"
import { Message } from "../../../Models/Message"
import { Button } from "../../ui/button"
import { Card } from "../../ui/card"
import { ChatInput } from "./Messages/ChatInput"
import { ChatMessages } from "./Messages/ChatMessages"
import { ChatNav } from "./ChatNav"
import { HubConnection, HubConnectionBuilder , LogLevel} from '@microsoft/signalr'
import { useEffect, useState } from "react"
import { useInfiniteQuery, useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { AddMessageApi, DeleteMessageApi, GetEventMessagesApi, UpdateMessageApi } from "../../../Services/MessageService"

import { useInView } from "react-intersection-observer"



interface EventChatProps {
    eventId: string
}


export const EventChat = ({
    eventId
}: EventChatProps) => {

    const [connection, setConnection] = useState<HubConnection>()
    //const [messages, setMessages] = useState<Message[]>([]);
    //const [event, setEvent] = useState<Event>();
    const { user } = useAuth()
    const queryClient = useQueryClient()

    /*
    const getMsgs = useQuery({
        queryKey: ["Messages", eventId],
        queryFn: ( ) => GetEventMessagesApi(eventId, 0),
        
    })*/
    


    
    const getMsgs = useInfiniteQuery({
        queryKey: ["Messages", eventId],
        queryFn: ({ pageParam }) => GetEventMessagesApi(eventId, pageParam),
        initialPageParam: 0,
        getNextPageParam: (lastPage) => lastPage?.nextCursor ,
    })

    
    
    

    const addMsgMutation = useMutation({
        mutationFn: (
            msg: string,
        ) => AddMessageApi(msg, eventId, user?.userName, user?.picture),
        onSettled: async () => {
            
            //return await queryClient.invalidateQueries({ queryKey: ["Messages", eventId] })
        },
    })

    //addMsgMutation.

    useEffect(() => {
        const conn = new HubConnectionBuilder()
            .withUrl("https://localhost:7161/chathub")
            .withAutomaticReconnect()
            .configureLogging(LogLevel.Information)
            .build();
        setConnection(conn);
    }, [])

    useEffect(() => {
        if (connection) {
            connection.on("ConnectionRes", (message) => {

                console.log(message)
            });
            connection.on("SendMsgRes", (newMsg) => {

                
                queryClient.setQueryData(
                    ["Messages", eventId],
                    (data) => {
                        data.pages.map((page, index) => {
                            if (index == data.pages.length - 1) {
                                page.data = [...page.data, newMsg]
                            }
                        })
                        return data
                })
                
            });
            connection.on("EditMsgRes", (message) => {

                queryClient.setQueryData(
                    ["Messages", eventId],
                    (oldMsgs: Message[]) => {
                        
                       //return oldMsgs.map((oldMsg) => {
                       //     return oldMsg.id === message.id ? message : oldMsg
                        //})
                    })
            });
            connection.on("DeleteMsgRes", (messageId) => {

                
                queryClient.setQueryData(
                    ["Messages", eventId],
                    (oldMsgs: Message[]) => {
                       // return oldMsgs.filter((msg) => msg.id !== messageId)
                    })
            });
            connection.start()
                .then(() => {
                    connection.invoke("ConnectionReq",
                        {
                            userId: user?.userId,
                            eventId: eventId
                        })
                })
            
            }

            
        return () => {
            //connection?.invoke("DisconnectReq", roomName);
            connection?.stop();
        }
        
    }, [connection])

    

    if (getMsgs.isPending) return 'Loading...'

    if (getMsgs.error) return getMsgs.error.message
    
    if (getMsgs.status === "success") 
        return (
            <Card className="flex flex-col bg-red-800 h-[700px] w-[700px] p-2 gap-y-2">
                {/*<ChatNav />*/}
                <ChatMessages
                    test={getMsgs}
                    addMsgMutation={addMsgMutation}/>
                <ChatInput
                    mutate={addMsgMutation.mutate}/>
            </Card>
        )
}