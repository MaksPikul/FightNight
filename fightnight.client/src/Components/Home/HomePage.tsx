
//import { useSession } from "next-auth/react";
import { Card, CardContent } from "../ui/card"
import { EventsBox } from "../Event/EventsBox"
import { useEffect, useState } from "react";
import { useAuth } from "../../Context/UseAuth";
import { GetUserEvents } from "../../Services/EventsService";
import { Event } from "../../Models/Event";
import { HomeHeader } from "../Header/HomeHeader";
//import { ProfileBox } from "./ProfileBox"
//import { auth } from "@/auth";
//import { getUserEvents } from "@/data/events";

export const HomePage = () => {
    
    
    
    //const userId = session?.user?.id
    //const userEvents = await getUserEvents(userId)

    //eventMembers table, get all rows with userId
    //with these rows, get each event using event id
    // figure out user role in the event


    // <ProfileBox />
    return (
       /* <Card
        className="flex justify-center">
        <CardContent>*/
        <div
        className=" p-5">
            <EventsBox />
        </div>
        /*</CardContent>
    </Card >*/
    )
}