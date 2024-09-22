

import { useParams } from "react-router-dom"
import { EventHeader } from "./EventHeader"
import { EventMembers } from "./EventSideNav"
import { EventChat } from "./EventTeam/EventChat"
import { EventTeamOptions } from "./EventTeam/EventTeamOptions"


export const EventTeamPage = () => {

    

    const { eventId } = useParams();
    

    return (
        <div>
            <EventHeader
            title={"Team Window"}
            desc={"View the Team and Chat with them!"} />

            <div className="flex flex-row ">
                <nav className="px-3 py-2 space-y-2">
                    <EventTeamOptions />
                    <EventMembers />
                </nav>
                <EventChat eventId={eventId} />
            </div>
        </div>
    )
}