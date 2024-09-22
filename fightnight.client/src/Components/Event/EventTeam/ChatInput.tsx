import { useForm } from "react-hook-form"
import { Form, FormControl, FormDescription, FormField, FormMessage, FormItem, FormLabel } from "../../ui/form"
import { Input } from "../../ui/input"
import * as z from "zod"
import { zodResolver } from "@hookform/resolvers/zod"
import { MessageSchema } from "../../../Schemas"
import { Paperclip, Plus } from "lucide-react"
import { Button } from "../../ui/button"
import { HubConnection } from "@microsoft/signalr"
import { Message } from "../../../Models/Message"
import { useMessage } from "../../../Context/UseMessage"


interface ChatInputProps {
    eventId: string
}

export const ChatInput = ({
    eventId
}:ChatInputProps) => {
    const { SendMessage } = useMessage()
    
    const form = useForm<z.infer<typeof MessageSchema>>({
        resolver: zodResolver(MessageSchema),
        mode:"onChange",
        defaultValues: {
            message:""
        }
    })
    const onSubmit = async (value: z.infer<typeof MessageSchema>) => {

        if (!value.message.trim()) {
            return; 
        }
        SendMessage(value.message, eventId);
        form.reset()
    }





    return (
        <Form {...form}>
            <form
            className="flex flex-row px-3 py-1  rounded-lg bg-red-500 gap-x-2"
            onSubmit={form.handleSubmit(onSubmit)}>

                
                
                <FormField 
                    control={form.control}
                    name="message"
                    render={({ field }) => (
                        <FormItem>
                            <FormControl>
                                <Input 
                                className="w-96 border-none "
                                {...field}
                                //disabled={isPending}
                                    placeholder="Message.."
                                type="text"
                                />
                            </FormControl>
                            <FormMessage/>
                        </FormItem>
                    )} />
                <Button
                variant="ghost"
                    disabled={!form.formState.isDirty}
                type="submit">
                    Send
                </Button>
            </form>
        </Form>
    )
}