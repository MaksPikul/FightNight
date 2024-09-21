import { Ellipsis} from "lucide-react"
import { UserEventProfile } from "../../Models/User"
import { Button } from "../ui/button"
import { Separator } from "../ui/separator"
import { UserCard } from "./UserCard"

interface UserGroupProps {
    title: string,
    userList: UserEventProfile[]
}

export const UserGroup = ({
    title,
    userList
}: UserGroupProps) => {

    return (
        <div>
            <div className="flex flex-row">
                <p className="font-bold" >{title}</p>
                {/*title !== "Admin" && <Button variant="ghost" className=""> <Ellipsis /> </Button> */}
            </div>

            {(userList.map((user: UserEventProfile, index) => {
                return <UserCard keys={index.toString()} user={user} />
            }))}
        </div>
    )
}