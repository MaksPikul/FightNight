import { UserRoundPlus } from "lucide-react"
import { Button } from "../../ui/button"
import { useModal } from "../../../Hooks/use-modal-store";

interface EventTeamOptionsProps {
    eventId: string
}

export const EventTeamOptions = ({
    eventId
}: EventTeamOptionsProps) => {
    const { onOpen } = useModal();

    return (
        <Button className="gap-x-2" onClick={() => onOpen("InviteMember", {eventId, eventTitle:"MISSING"})}>
            <UserRoundPlus />
            add member
        </Button>
    )
}