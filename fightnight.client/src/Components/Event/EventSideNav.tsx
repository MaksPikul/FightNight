import { Button } from "../ui/button"
import { Separator } from "../ui/separator"

export const EventSideNav = () => {

    return (
        <nav>
            <header>
                Participants
            </header>
            <Separator />
            <div> 
                <p>Admin</p>
                {"Maks"}
            </div>
            <Separator />
            <div>
                <p>Moderators</p>
                <p>{"some guy"}</p>
                <Button>add mods</Button>
            </div>
            <Separator />
            <div>
                <p>Fighters</p>
                <p>{"some girl"}</p>
                <Button>Invite Fighters after launch</Button>
            </div>
        </nav>
    )
}