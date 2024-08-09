

import {
  Carousel,
  CarouselContent,
  CarouselItem,
  CarouselNext,
  CarouselPrevious,
} from "../ui/carousel"
//import { Button } from "../ui/button"
import { EventCard } from "./EventCard"
import { EventOptions } from "./EventOptions"
//import { useSession } from "next-auth/react"
///import { Database } from "@/database.types"
import { Card } from "../ui/card"

interface EventsBoxProps {
    events: any
}

export const EventsBox = ({
    events
}:EventsBoxProps) => {

    
    //"sort / filter / search / completed"
    console.log(events)

    

    return(
    <Card
    className="p-5 w-[500px]">
        
        <Carousel>
            <CarouselContent>
                {events.length !== 0 ?
                <>
                {events.map((event, index)=> (
                    <CarouselItem>
                        <EventCard event={event.events} role={event.role}/>
                    </CarouselItem>
                ))}
                </>
                :
                <div>No events to display</div>
                }
            </CarouselContent>

            <CarouselPrevious />
            <CarouselNext />

            <EventOptions />
        </Carousel>
        
    </Card>
    )
}