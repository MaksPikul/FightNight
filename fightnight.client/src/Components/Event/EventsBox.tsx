

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
import { useQuery, useQueryClient } from "@tanstack/react-query"




export const EventsBox = () => {

    //"sort / filter / search / completed"
    const queryClient = useQueryClient();
    //const [events, setEvents] = useState<Event[]>([]);

    //const { getUserEvents } = useEvent();

    const {
        isPending,
        error,
        data: events,
        isFetching
    } = useQuery({
        queryFn: () => GetUserEvents(),
            queryKey: ['userEvents'],
        });

    if (isPending) return <p>"loading..."</p>

    if (error) return <p>{error.message}</p>

    //const x = queryClient.getQueryData(['userEvents', user?.userId]);
    //console.log(x[0])

    if (events) return(
    <Card
    className="p-5 w-[500px] ">
            <Carousel>
                
                    <CarouselContent>
                    {events.length !== 0 ?
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