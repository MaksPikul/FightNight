
//import { Database } from "@/database.types"
import { useNavigate } from "react-router-dom"
import { Event, EventRole } from "../../Models/Event"
import { Button } from "../ui/button"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "../ui/card"
import { ScrollArea } from "../ui/scroll-area"
import { Separator } from "../ui/separator"
import {
    Tabs,
    TabsContent,
    TabsList,
    TabsTrigger,
  } from "../ui/tabs"
//import { useRouter } from "next/navigation"

interface EventCardProps {
    event: Event,
    //role:any
}

export const EventCard = ({
    event,
    //role
}:EventCardProps) => {
    const navigate = useNavigate()

    //protect this action for admins only
    function directToEvent () {
        navigate(`/event/${event.id}`)
    }
    
    return (
        <div>
            <CardHeader>
                <CardTitle>{event.title}</CardTitle>
                <CardDescription>
                    <p>Date: {event.date.toString()}</p>
                    <p>Start Time: {event.time} </p>
                </CardDescription>
                <CardDescription>
                    <p>Venue: {event.venue}</p>
                    <p>Venue Address: {event.venueAddress}</p>
                </CardDescription>
                <Separator />
            </CardHeader>
            
            <CardContent
            className="flex flex-col items-center">
            <Tabs 
            defaultValue="fightCard" 
            className="w-[400px]">
                <TabsList className="grid w-full grid-cols-2">
                    <TabsTrigger value="fightCard">Fight Card</TabsTrigger>
                    <TabsTrigger value="moreInfo">More Info</TabsTrigger>
                </TabsList>

                <TabsContent 
                value="fightCard"
                className="h-48">
                    <ScrollArea>
                        <p>Fight card</p>
                    </ScrollArea>
                </TabsContent>

                <TabsContent 
                value="moreInfo"
                className="h-48">
                    <p>type{event.type}</p>
                    <p>Organiser{event.organizer}</p>
                    <p>...</p>
                </TabsContent>

            </Tabs>

            <div>
                { event.role === EventRole.Admin &&
                <Button
                onClick={directToEvent}
                className="w-64">
                    Manage Event
                </Button>
                    }
            </div>
            </CardContent>
        </div>
    )
}