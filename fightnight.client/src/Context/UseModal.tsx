
import { useEffect, useState } from "react"
import { SignOutModal } from "../Components/Modals/signout-modal";
import { CreateEventModal } from "../Components/Modals/CreateEventModal";
import { ConfirmDeleteEventModal } from "../Components/Modals/ConfirmDeleteEvent";
import { UploadBannerModal } from "../Components/Modals/UploadBannerModal";

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
            <SignOutModal />
            <CreateEventModal />
            <ConfirmDeleteEventModal />
            <UploadBannerModal />
        </>
    )
}