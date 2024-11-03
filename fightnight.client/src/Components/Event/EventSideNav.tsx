import { Plus } from "lucide-react"
import { Button } from "../ui/button"
import { Separator } from "../ui/separator"
import { UserCard } from "./UserCard";
import { UserEventProfile } from "../../Models/User";
import { UserGroup } from "./UserGroup";
import { useQuery } from "@tanstack/react-query";
import { GetEventMembers } from "../../Services/MemberService";
import { useParams } from "react-router-dom";




export const EventMembers = () => {

    const { eventId } = useParams()

    
    const GetMembers = useQuery({
        queryKey: ["Members", eventId],
        queryFn: () => GetEventMembers(eventId)
    })

    if (!GetMembers.data) return "...loading"

    console.log(GetMembers.data)
    const groupedUsers = GetMembers.data.reduce((acc, user) => {
        if (!acc[user.role]) {
            acc["role"+user.role] = [];
        }
        acc["role"+user.role].push(user);
        return acc;
    }, {});
    
    

    return (
        <nav className="">
            
            <UserGroup title="Admin" userList={groupedUsers.role3 || []} />
            <Separator />
            <UserGroup title="Moderators" userList={groupedUsers.member || []} />
            <Separator />
            <UserGroup title="Fighters" userList={groupedUsers.guest || []} />
            
        </nav>
    )
}