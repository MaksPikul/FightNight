
import { useEffect, useState } from "react"
import { SignOutModal } from "../Components/Modals/signout-modal";
import { CreateEventModal } from "../Components/Modals/CreateEventModal";
import { ConfirmDeleteEventModal } from "../Components/Modals/ConfirmDeleteEvent";
import { UploadBannerModal } from "../Components/Modals/UploadBannerModal";
import { ConfirmDeleteMessageModal } from "../Components/Modals/ConfirmDeleteMessage";
import { InviteMemberModal } from "../Components/Modals/InviteMemberModal";

export const ModalProvider = () => {
    const [isMounted, setIsMounted] = useState(false);

    useEffect(() => {
        setIsMounted(true)
    }, [])

    if (!isMounted) {
        return null;
    }

    return (
        <>
            <InviteMemberModal />
            <SignOutModal />
            <CreateEventModal />
            <UploadBannerModal />
            <ConfirmDeleteEventModal />
            <ConfirmDeleteMessageModal />
        </>
    )
}