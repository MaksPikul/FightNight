
import {
        Dialog,
        DialogContent,
        DialogDescription,
        DialogHeader,
        DialogTitle,
    } from "@/components/ui/dialog"

import { Button } from "../ui/button";

import { Input } from "../ui/input";



import { Label } from "../ui/label";
import { useModal } from "../../Hooks/use-modal-store";
import { useRef, useState } from "react";
import { DeleteEventApi } from "../../Services/EventsService";
import { FormError } from "../Misc/formError";
import { useNavigate } from "react-router-dom";
import { useToast } from "../ui/use-toast";
import { useMutation, useQueryClient } from "@tanstack/react-query";




export const ConfirmDeleteEventModal = () => {
    const [error, setError] = useState<string | undefined>("");
    const [inputValue, setInputValue] = useState('');
    const { isOpen, onClose, type, data } = useModal();
    const navigate = useNavigate()

    const { eventTitle, eventId } = data
    const isModalOpen = isOpen && type === "ConfirmDeleteEvent";
    const { toast } = useToast()

    const queryClient = useQueryClient()

    const deleteEventMutation = useMutation({
        mutationFn: () => DeleteEventApi(eventId),
        onSuccess: () => {
            //navigate("/home")
            handleClose()
            toast({
                title: "Event Deleted and Closed Successfully",
                description: "An email has been sent out to notify everyone involved.",
            })
        },
        onError: (err) => {
            setError(err.message)
        },
        onSettled: async () => {
            return await queryClient.invalidateQueries({ queryKey: ['userEvents'] })
        },
    })

    const onSubmit = async () => {
        if (eventTitle === inputValue) deleteEventMutation.mutate()

        else setError("The title you've entered is wrong")
    }

    const handleClose = () => {
        setInputValue('')
        setError("")
        onClose()
    }

    return (
        <Dialog open={isModalOpen} onOpenChange={() => {handleClose()}
        }>
            <DialogContent className=" p-0 overflow-hidden justify-center">
                <DialogHeader className="pt-8 px-6">
                    <DialogTitle className="text-center mb-3">
                        Deleting Event
                    </DialogTitle>
                    <DialogDescription
                        className="text-secondary p-2  rounded-md bg-yellow-500 text-center">
                        <p>Are you sure you want to Close <b><u>{eventTitle}</u></b> ?</p>

                        <p>This action cannot be undone.</p>
                    </DialogDescription>
                </DialogHeader>

                
                <Label>Enter Event Title</Label>
                <Input
                    value={inputValue}
                    onChange={(e) => setInputValue(e.target.value)} />

                    <FormError message={error} />

                        <div className="flex flex-row gap-2 justify-center pb-6">
                        <Button
                            className="bg-red-600 hover:bg-red-700 "
                            onClick={onSubmit}>
                            Delete</Button>
                        <Button onClick={() => handleClose()}>Cancel</Button>
                        </div>

                        
            </DialogContent>
        </Dialog>
    )
}