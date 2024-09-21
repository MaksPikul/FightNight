import { useAuth } from "../../../Context/UseAuth"
import { Event } from "../../../Models/Event"
import { Message } from "../../../Models/Message"
import { Button } from "../../ui/button"
import { Card } from "../../ui/card"
import { ChatInput } from "./ChatInput"
import { ChatMessages } from "./ChatMessages"
import { ChatNav } from "./ChatNav"
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr'
import { useEffect, useState } from "react"

export const EventChat = () => {

    const [connection, setConnection] = useState<HubConnection>()
    const [messages, setMessages] = useState<Message[]>([]);
    const [event, setEvent] = useState<Event>();
    const { user } = useAuth()

    useEffect(() => {
        const conn = new HubConnectionBuilder()
            .withUrl("https://localhost:7161/chathub")
            .withAutomaticReconnect()
            //.configureLogging(LogLevel.Information)
            .build();
        setConnection(conn);
    }, [])

    useEffect(() => {
        if (connection) {
            connection.on("ConnectionRes", (message) => {

                console.log(message)
            });
            connection.on("SendMsgRes", (message) => {
                //if success
                console.log(message)
                setMessages(prevMessages => [...prevMessages, message]);

                //if failed
            });

            connection.start()
                .then(() => { 
            connection.invoke("ConnectionReq",
                {
                    userId: user.userId,
                    username: "maks",
                    eventId: "849847a3-6342-4b37-b02f-720d5fc16d79"
                })
            })
        }

        return () => {
            //connection?.invoke("DisconnectReq", roomName);
            connection?.stop();
        }
    }, [connection])

    async function DeleteMessage(msgId: string) {
        //take away from useState()
        //await connection.invoke("DeleteMsgReq", msgId);
    }

    async function SendMessage(msg: string) {
        if (connection != null) {
            const newMsg: Message = {
                eventId: "849847a3-6342-4b37-b02f-720d5fc16d79",//event.id,
                userId: user.userId,
                username: "maks",//user.username,
                message: msg,
                isLoading: true
                // Create in backend
                //timeStamp: new Date(), 
                //isEdited: false,
            }

            //setMessages(prevMessages => [...prevMessages, message]);

            await connection.invoke("SendMsgReq", newMsg);
        }
    }

    return (
        <Card
            className="flex flex-col bg-red-800 h-[700px] w-[700px] p-2 gap-y-2">
            
            <ChatNav />
            <ChatMessages messages={messages} />
            <ChatInput SendMessage={SendMessage} />
        </Card>
    )
}