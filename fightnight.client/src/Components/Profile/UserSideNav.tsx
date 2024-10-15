import { Settings, UserRound } from "lucide-react"
import { Button } from "../ui/button"
import { Sheet, SheetContent, SheetHeader, SheetTitle, SheetTrigger } from "../ui/sheet"
import { useModal } from "../../Hooks/use-modal-store"
import { useAuth } from "../../Context/UseAuth"
import { Avatar, AvatarFallback, AvatarImage } from "../ui/avatar"

export const UserSideNav = () => {

    const { onOpen } = useModal()
    const { user } = useAuth()

    return (
        <Sheet>
            <SheetTrigger
                asChild>
                <Button
                    onClick={() => null}
                    variant="ghost">
                    <Settings />
                </Button>
            </SheetTrigger>
            <SheetContent>
                <SheetHeader>
                    <SheetTitle>Account & Settings</SheetTitle>
                </SheetHeader>

                <Avatar className="h-8 w-8 mr-2">
                    <AvatarImage src={user?.picture} />
                    <AvatarFallback> <UserRound /> </AvatarFallback>
                </Avatar>

                <Button
                    type="button"
                    onClick={() => {
                        onOpen("UploadBanner", { eventId: user?.userId })
                    }}>
                    Upload a new Profile Picture
                </Button>


            </SheetContent>
        </Sheet>
    )
}