

import { EventHeader } from "./EventHeader"
import { EventMembers } from "./EventSideNav"
import { EventChat } from "./NewFolder/EventChat"
import { EventTeamOptions } from "./NewFolder/EventTeamOptions"


export const EventTeamPage = () => {

    

    

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
                <EventChat />
            </div>
        </div>
    )
}