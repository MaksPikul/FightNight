import { create } from "zustand"
import { Message } from "../Models/Message";

export type ModalType = "signOut" | "CreateEvent" | "ConfirmDeleteEvent"
    | "UploadBanner" | "ConfirmDeleteMessage" | "InviteMember"

interface ModalData {
    eventTitle?: string
    eventId?: string
    message?: Message
}

interface ModalStore {
    type: ModalType | null;
    isOpen: boolean;
    data: ModalData
    onOpen: (type: ModalType, data?: ModalData) => void;
    onClose: () => void;
}

export const useModal = create<ModalStore>((set) => ({
    type: null,
    data: {},
    isOpen: false,
    onOpen: (type, data = {}) => set({ isOpen: true, type, data }),
    onClose: () => set({ type: null, isOpen: false })
}));