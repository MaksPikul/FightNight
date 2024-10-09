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
import { Tabs, TabsContent, TabsList, TabsTrigger } from "../ui/tabs";
import * as z from "zod";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { EventRole } from "../../Models/Event";
import { InviteSchema } from "../../Schemas";
import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage } from "../ui/form";
import { AddMemberToEvent, SendInvite } from "../../Services/MemberService";




export const InviteMemberModal = () => {
    const { isOpen, onClose, type, data } = useModal();
    const isModalOpen = isOpen && type === "InviteMember";

    const { eventId, eventTitle } = data;
    
    const form = useForm({
        resolver: zodResolver(InviteSchema),
        defaultValues: {
            email: "",
            role: 1,
            //If applicable for moderator, add permission list
        }
    });
    
    const handleClose = () => {
        form.reset()
        onClose()
    }

    const onSubmit = async (values: z.infer<typeof InviteSchema>) => {
        console.log(values)
        const result = await SendInvite(values.email, values.role, eventId)
        handleClose()
    }

    return (
        <Dialog open={isModalOpen} onOpenChang ={() => { handleClose() }
        }>
            <DialogContent className=" p-0 overflow-hidden justify-center">
                <DialogHeader className="pt-8 px-6">
                    <DialogTitle className="text-center mb-3">
                        Invite to { eventTitle }
                    </DialogTitle>
                    <DialogDescription>
                        Invite is still valid if account doesn't exist
                        Invite Lasts 30 Days
                    </DialogDescription>
                </DialogHeader>

                <Form {...form}>
                    <form
                        onSubmit={form.handleSubmit(onSubmit)}
                        className="space-y-4 m-3">


                        <FormField
                        control={form.control}
                        name="email"
                        render={({ field }) => (
                            <FormItem>
                                <FormLabel> Email of Account to Invite </FormLabel>
                                <FormControl>
                                    <Input
                                        type="email" 
                                        {...field} />
                                </FormControl>
                                <FormMessage />
                            </FormItem>
                        )} />


                        <Button
                        type="submit"
                        onClick={() => {
                            form.setValue('role', 1);
                        }}
                        > Invite as Moderator </Button>
                        <Button
                        type="submit"
                        onClick={() => {
                            form.setValue('role', 2);
                        }}
                        > Invite as Competitor </Button>


                        <Button
                            className="bg-red-600"
                            onClick={ ()=>handleClose() }
                        > Close </Button>

                    </form>
                </Form>


                
            </DialogContent>
        </Dialog>
    )
}