import { ContextMenu, ContextMenuContent, ContextMenuItem, ContextMenuTrigger } from "../ui/context-menu"
import { UserEventProfile } from "../../Models/User"
import {
    Avatar,
    AvatarFallback,
    AvatarImage,
} from "@/components/ui/avatar"
import { Badge } from "../ui/badge"
import { useAuth } from "../../Context/UseAuth"
import { EventRole } from "../../Models/Event"

interface UserCardProps {
    userProfile: any,
    keys: string
}

export const UserCard = ({
    userProfile,
    keys
}: UserCardProps) => {
    const { user } = useAuth()
    
    const isAdmin = userProfile.role === EventRole.Admin
    const ownProfile = userProfile.userId === user?.userId;
    

    return (
        <ContextMenu>
            <ContextMenuTrigger key={keys} className="hover:bg-blue-600 rounded transition-all flex flex-row  w-48 px-3 py-1 gap-x-3">
                    <Avatar className="size-9">
                        <AvatarImage  src="https://github.com/shadcn.png" alt="@shadcn" />
                        <AvatarFallback>CN</AvatarFallback>
                    </Avatar>
                    {user?.username}
            </ContextMenuTrigger>

            <ContextMenuContent>
                <ContextMenuItem> <Badge> {userProfile.role } </Badge> </ContextMenuItem>
                {(isAdmin && !ownProfile) && <ContextMenuItem className="text-red-600"> Remove From Event </ContextMenuItem>}
            </ContextMenuContent>
        </ContextMenu>
    )
}