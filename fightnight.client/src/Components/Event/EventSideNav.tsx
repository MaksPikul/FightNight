import { Plus } from "lucide-react"
import { Button } from "../ui/button"
import { Separator } from "../ui/separator"
import { UserCard } from "./UserCard";
import { UserEventProfile } from "../../Models/User";
import { UserGroup } from "./UserGroup";

export const EventMembers = () => {
    const users = [
        { id: 1, username: 'Alice', type: 'admin', role: 3 },
        { id: 2, username: 'Bob', type: 'guest', role: 3 },
        { id: 3, username: 'Charlie', type: 'member', role: 3 },
        { id: 5, username: 'Eve', type: 'guest', role: 3 },
        { id: 5, username: 'Eve', type: 'guest', role: 3 },
        { id: 5, username: 'Eve', type: 'guest', role: 3 },
        { id: 5, username: 'Eve', type: 'guest', role: 3 },
        { id: 6, username: 'Frank', type: 'member',role:3 },
    ];

    const groupedUsers = users.reduce((acc, user) => {
        if (!acc[user.type]) {
            acc[user.type] = [];
        }
        acc[user.type].push(user);
        return acc;
    }, {});

    console.log(groupedUsers.guest)
    

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