
//import { useSession } from "next-auth/react";
import { Card, CardContent } from "../ui/card"
import { EventsBox } from "../Event/EventsBox"
//import { ProfileBox } from "./ProfileBox"
//import { auth } from "@/auth";
//import { getUserEvents } from "@/data/events";

export const HomePage = async () => {
    // session = await auth()
    //const userId = session?.user?.id
    //const userEvents = await getUserEvents(userId)
    
    //eventMembers table, get all rows with userId
    //with these rows, get each event using event id
    // figure out user role in the event

    return(
    <Card>
        <CardContent
        className="flex flex-row">
            <EventsBox events={null}/>
                {/*<ProfileBox />*/}
        </CardContent>
    </Card>
    )
}