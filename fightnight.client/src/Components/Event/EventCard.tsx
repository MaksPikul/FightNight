
//import { Database } from "@/database.types"
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
    //event : Database["public"]["Tables"]["events"]
    //role : Database["public"]["Tables"]["eventMembers"]["Row"]["role"]
}

export const EventCard = ({
    event,
    role
}:EventCardProps) => {
    //const router = useRouter()

    //protect this action for admins only
    function directToEvent ()  {
        //router.push(`/event/${event.id}`)
    }
    
    return (
        <Card>
            
            <CardHeader>
                <CardTitle>title{event.title}</CardTitle>
                <CardDescription>
                    date{event.dateTime}
                </CardDescription>
                <CardDescription>
                    loc{event.location}
                </CardDescription>
                <Separator />
            </CardHeader>
            
            <CardContent
            className="flex flex-col">
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

                { role === "admin" &&
                <Button
                onClick={directToEvent}
                className="">
                    Manage Event
                </Button>
                }
            </CardContent>
        </Card>
    )
}