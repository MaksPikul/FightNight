import { Plus } from "lucide-react"
import { Button } from "../ui/button"
import { Separator } from "../ui/separator"
import { UserCard } from "./UserCard";
import { UserEventProfile } from "../../Models/User";
import { UserGroup } from "./UserGroup";
import { useQuery } from "@tanstack/react-query";
import { GetEventMembers } from "../../Services/MemberService";


interface EventMembersProps {
    eventId: string | undefined
}

export const EventMembers = ({
    eventId
}:EventMembersProps) => {


    const GetMembers = useQuery({
        queryKey: ["Members", eventId],
        queryFn: ()=> GetEventMembers(eventId)
    })

    const groupedUsers = GetMembers.data.reduce((acc, user) => {
        if (!acc[user.Role]) {
            acc[user.Role] = [];
        }
        acc[user.Role].push(user);
        return acc;
    }, {});
    

    return (
        <nav className="">
            <UserGroup title="Admin" userList={groupedUsers.admin} />
            <Separator />
            <UserGroup title="Moderators" userList={groupedUsers.member} />
            <Separator />
            <UserGroup title="Fighters" userList={groupedUsers.guest} />
        </nav>
    )
}