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
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { AddMessageApi, DeleteMessageApi, GetEventMessagesApi, UpdateMessageApi } from "../../../Services/MessageService"




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

    const { isPending, error, data: messages, status } = useQuery({
        queryKey: ["Messages", eventId],
        queryFn: () => GetEventMessagesApi(eventId)
    })

    const addMsgMutation = useMutation({
        mutationFn: (
            msg: string,
        ) => AddMessageApi(msg, eventId, user?.userName, "user?.picture"),
        onSettled: async () => {
            
            return await queryClient.invalidateQueries({ queryKey: ["Messages", eventId] })
        },
    })

    useEffect(() => {
        const conn = new HubConnectionBuilder()
            .withUrl("https://localhost:7161/chathub")
            .withAutomaticReconnect()
            .configureLogging(LogLevel.Information)
            .build();
        setConnection(conn);
    }, [])

    useEffect(() => {
        console.log("re-runs")
        if (connection) {
            connection.on("ConnectionRes", (message) => {

                console.log(message)
            });
            connection.on("SendMsgRes", (message) => {

                // user sends api, ticked
                // msg gets added to db, ticked
                // msg obj get send over websockets to here,
                // simply append message onto list
                console.log(message)

            });
            connection.on("EditMsgRes", (message) => {

                console.log(message)
                //find msg by id in list,
                //edit msg and flag

            });
            connection.on("DeleteMsgRes", (message) => {
                console.log(message)
                //find msg by id in list,
                //remove from list

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




    if (isPending) return 'Loading...'

    if (error) return error.message
    
    if (status === "success") 
        return (
            <Card className="flex flex-col bg-red-800 h-[700px] w-[700px] p-2 gap-y-2">
                {/*<ChatNav />*/}
                <ChatMessages
                    addMsgMutation={addMsgMutation}
                    messages={messages}/>
                <ChatInput
                    mutate={addMsgMutation.mutate}/>
            </Card>
        )
}