import { useMutation, useQueryClient } from "@tanstack/react-query";
import { DeleteMessageApi } from "../../Services/MessageService";
import { useModal } from "../../Hooks/use-modal-store";
import { useState } from "react";
import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogHeader,
    DialogTitle,
} from "@/components/ui/dialog"
import { MessageBoxContent } from "../Event/EventTeam/Messages/MessageBoxContent";
import { Button } from "../ui/button";
import { useToast } from "../ui/use-toast";




export const ConfirmDeleteMessageModal = () => {

    const [error, setError] = useState<string | undefined>("");
    const { toast } = useToast()

    const { isOpen, onClose, type, data } = useModal();
    const isModalOpen = isOpen && type === "ConfirmDeleteMessage";

    const { message } = data

    const queryClient = useQueryClient()

    const deleteMessageMutation = useMutation({
        mutationFn: () => DeleteMessageApi(message?.id, message?.eventId),
        onSuccess: () => {
            toast({
                title: "Message Deleted Successfully",
            })
        },
        onError: (err) => {
            toast({
                title: "Error Deleting Message",
                description: err.message 
            })
        },
        onSettled: async () => {
            return await queryClient.invalidateQueries({ queryKey: ["Messages", message?.eventId] })
        },
    })


    return (
        <Dialog open={isModalOpen} onOpenChange={() => onClose()}>
            <DialogContent className=" p-0 overflow-hidden justify-center">
                <DialogHeader className="pt-8 px-6">
                    <DialogTitle className="text-center mb-3">
                        Deleting Message
                    </DialogTitle>
                    <DialogDescription
                        className="text-secondary p-2  rounded-md bg-yellow-500 text-center">
                        <p>Are you sure you want to delete this message?</p>
                        <p>This action cannot be undone.</p>
                    </DialogDescription>
                </DialogHeader>

                <MessageBoxContent
                    msg={message}
                    showOptions={false} />

                <div className="flex flex-row">
                    <Button
                        onClick={() => onClose()}> Cancel </Button>
                    <Button
                        className="bg-red-600"
                        onClick={() => {
                            deleteMessageMutation.mutate()
                            onClose()
                        }}> Delete </Button>
                </div>

            </DialogContent>
        </Dialog>
    )
}