import { Card } from "../ui/card"
import { EventHeader } from "./EventHeader"
import { EventSideNav } from "./EventSideNav"

export const EventTeamPage = () => {
    return (
        <div>
            <EventHeader
                title={"Team Window"}
                desc={"View the Team and Chat with them!"} />

            <div
            className="flex flex-row">
                
                <Card className="w-[400px]">
                    Chat place
                </Card>
                <EventSideNav />
            </div>
        </div>
    )
}