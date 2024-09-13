

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
import { Card } from "../ui/card"
import { Event } from "../../Models/Event"
import { useEffect, useState } from "react"
import { GetUserEvents } from "../../Services/EventsService"
import { useAuth } from "../../Context/UseAuth"


export const EventsBox = () => {

    //"sort / filter / search / completed"


    const { user } = useAuth();
    const [events, setEvents] = useState<Event[]>([]);
    const [mounted, setMounted] = useState(false);

    //const { getUserEvents } = useEvent();

    useEffect(() => {
        GetUserEvents(user?.userId)
            .catch(
            //Toast Error
            )
            .then(res => {
                setEvents(res?.data)
            })
        setMounted(true)
    }, [])
    console.log(events)
    return(
    <Card
    className="p-5 w-[500px] ">
            <Carousel>
                
                    <CarouselContent>
                    {events.length !== 0 && mounted ?
                    <>
                        {events.map((event, index) => (
                            <CarouselItem key={index}>
                                <EventCard event={event} />
                            </CarouselItem>
                        ))}
                    </>
                    :
                            <div>No events to display</div>}
                </CarouselContent>

                    
                    <CarouselPrevious />
                    <CarouselNext />

                    <EventOptions />
                    
        </Carousel>
    </Card>
    )
}