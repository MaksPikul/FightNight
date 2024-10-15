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
import { Message } from "../../Models/Message";
import { RemoveMemberFromEvent } from "../../Services/MemberService";




export const ConfirmRemoveMemberModal = () => {

    const [error, setError] = useState<string | undefined>("");
    const { toast } = useToast()

    const { isOpen, onClose, type, data } = useModal();
    const isModalOpen = isOpen && type === "ConfirmRemoveMember";

    const { } = data

    const queryClient = useQueryClient()

    const deleteMemberMutation = useMutation({
        mutationFn: () => RemoveMemberFromEvent(userId, eventId),
        onSuccess: () => {
            toast({
                title: "Member Deleted Successfully",
            })
        },
        onError: (err) => {
            toast({
                title: err || "Error Removing User",
                description: err.message 
            })
        },
        onSettled: async () => {
            //return await queryClient.invalidateQueries({ queryKey: ["Messages", message?.eventId] })
            
        },
    })

    return (
        <Dialog open={isModalOpen} onOpenChange={() => onClose()}>
            <DialogContent className=" p-0 overflow-hidden justify-center">
                <DialogHeader className="pt-8 px-6">
                    <DialogTitle className="text-center mb-3">
                        Removing User From event
                    </DialogTitle>
                    <DialogDescription
                        className="text-secondary p-2  rounded-md bg-yellow-500 text-center">
                        <p>Are you sure you want to remove this member?</p>
                    </DialogDescription>
                </DialogHeader>

                

                <div className="flex flex-row">
                    <Button
                        className="bg-red-600"
                        onClick={() => {
                            deleteMemberMutation.mutate()
                            onClose()
                        }}> Delete </Button>

                    <Button
                        onClick={() => onClose()}> Cancel </Button>
                </div>

            </DialogContent>
        </Dialog>
    )
}