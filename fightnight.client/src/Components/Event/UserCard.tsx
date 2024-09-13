import { UserEventProfile } from "../../Models/User"
import {
    Avatar,
    AvatarFallback,
    AvatarImage,
} from "@/components/ui/avatar"

interface UserCardProps {
    user: UserEventProfile
}

export const UserCard = ({
    user 
}:UserCardProps) => {

    return (
        <div className="hover:bg-blue-600 rounded transition-all flex flex-row  w-56 px-3 py-1 gap-x-3">
            <Avatar className="size-9">
                <AvatarImage  src="https://github.com/shadcn.png" alt="@shadcn" />
                <AvatarFallback>CN</AvatarFallback>
            </Avatar>
            {user.username}
        </div>
    )
}