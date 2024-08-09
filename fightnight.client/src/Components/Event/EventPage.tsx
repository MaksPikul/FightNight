import { Card } from "../ui/card"
import { EventSideNav } from "./EventSideNav"

export const EventPage = () => {

    return (
        <Card
        className="flex flex-row">
            <EventSideNav />
            

            <Card
            className="flex flex-col">
                <div> general info </div>
                <div> fight info </div>
            </Card>
        </Card>
    )
}