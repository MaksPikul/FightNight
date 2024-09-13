import { Plus } from "lucide-react"
import { Button } from "../ui/button"
import { Separator } from "../ui/separator"
import { UserCard } from "./UserCard";
import { UserEventProfile } from "../../Models/User";
import { UserGroup } from "./UserGroup";

export const EventSideNav = () => {
    const users = [
        { id: 1, username: 'Alice', type: 'admin' },
        { id: 2, username: 'Bob', type: 'guest' },
        { id: 3, username: 'Charlie', type: 'member' },
        { id: 5, username: 'Eve', type: 'guest' },
        { id: 5, username: 'Eve', type: 'guest' },
        { id: 5, username: 'Eve', type: 'guest' },
        { id: 5, username: 'Eve', type: 'guest' },
        { id: 6, username: 'Frank', type: 'member' },
    ];

    const groupedUsers = users.reduce((acc, user) => {
        if (!acc[user.type]) {
            acc[user.type] = [];
        }
        acc[user.type].push(user);
        return acc;
    }, {});

    
    

    return (
        <nav className="px-3 py-2 space-y-2">
            <UserGroup title="Admin" userList={groupedUsers.admin} />
            <Separator />
            <UserGroup title="Moderators" userList={groupedUsers.member} />
            <Separator />
            <UserGroup title="Fighters" userList={groupedUsers.guest} />
        </nav>
    )
}