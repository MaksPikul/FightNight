import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogHeader,
    DialogTitle,

} from "@/Components/ui/dialog"
import { useModal } from "@/Hooks/use-modal-store";
import { format } from "date-fns"
import {
    Form,
    FormControl,
    FormField,
    FormItem,
    FormLabel,
    FormMessage,
} from "../ui/form"
import { cn } from "@/lib/utils"
import { Button } from "@/components/ui/button"
import { Calendar } from "@/components/ui/calendar"
import {
    Popover,
    PopoverContent,
    PopoverTrigger,
} from "@/components/ui/popover"

import { useForm } from "react-hook-form"
import * as z from "zod"
import { zodResolver } from "@hookform/resolvers/zod"
import { Input } from "../ui/input"
import { CreateEventSchema } from "../../Schemas";
import { CalendarIcon } from "lucide-react";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { CreateEventApi } from "../../Services/EventsService";
import { FormError } from "../Misc/formError";


export const CreateEventModal = () => {
    
    const { isOpen, onClose, type } = useModal();
    const isModalOpen = isOpen && type === "CreateEvent";
    const [isLoading, setIsLoading] = useState(false)
    const navigate = useNavigate()
    const [error, setError] = useState("")

    const onSubmit = async (values: z.infer<typeof CreateEventSchema>) => {
            setIsLoading(true)
            const res = await CreateEventApi(values.title, values.date)
            setIsLoading(false)
        if (res?.data) {
            form.reset()
            console.log(res.data)
            navigate(`/event/${res?.data}`)
            onClose()
        }
        else if (res?.response) {
            setError(res.response.data)
        }
        else {
            setError("Whole app BUGGIN rn, try again later doe stilll");
        }
    }

    const form = useForm({
        resolver: zodResolver(CreateEventSchema),
        defaultValues: {
            title: "",
            date: Date.now(),
        }
    });

    return (
        <Dialog open={isModalOpen} onOpenChange={() => onClose()}>
            <DialogContent className=" p-0 overflow-hidden">
                <DialogHeader className="pt-8 px-6">
                    <DialogTitle className="text-center">
                        Create Event
                    </DialogTitle>
                    <DialogDescription className=" text-center">
                        Enter Title and Start Date of Event
                    </DialogDescription>
                </DialogHeader>

                <Form {...form}>
                    <form
                        onSubmit={form.handleSubmit(onSubmit)}
                        className="space-y-4 items-center">
                        <div className="space-y-4 px-6 ">
                            

                            <FormError message={error}/>
                            <FormField
                                control={form.control}
                                name="title"
                                render={({ field }) => (
                                    <FormItem>
                                        <FormLabel className="">
                                            Event Title
                                        </FormLabel>
                                        <FormControl>
                                            <Input
                                                disabled={isLoading}
                                                placeholder="Name"
                                                {...field}
                                            />
                                        </FormControl>
                                        <FormMessage />
                                    </FormItem>
                                )} />

                            <FormField
                                control={form.control}
                                name="date"
                                render={({ field }) => (
                                    <FormItem
                                        className="flex flex-col">
                                        <FormLabel>Event Date</FormLabel>
                                        <FormControl>
                                            <Popover>
                                                <PopoverTrigger asChild>
                                                    <FormControl>
                                                        <Button
                                                            disabled={isLoading}
                                                            variant={"outline"}
                                                            className={cn(
                                                                "w-[240px] pl-3 text-left font-normal",
                                                                !field.value && "text-muted-foreground"
                                                            )}
                                                        >
                                                            {field.value ? (
                                                                format(field.value, "PPP")
                                                            ) : (
                                                                <span>Pick a date</span>
                                                            )}
                                                            <CalendarIcon className="ml-auto h-4 w-4 opacity-50" />
                                                        </Button>
                                                    </FormControl>
                                                </PopoverTrigger>
                                                <PopoverContent className="w-auto p-0" align="start">
                                                    <Calendar
                                                        mode="single"
                                                        selected={field.value}
                                                        onSelect={field.onChange}
                                                        disabled={(date) =>
                                                            date < new Date() || date < new Date("1900-01-01")
                                                        }
                                                        initialFocus
                                                    />
                                                </PopoverContent>
                                            </Popover>

                                        </FormControl>
                                        <FormMessage />
                                    </FormItem>
                                )} />




                        </div>
                        <div className="flex justify-center pb-6">
                            <Button>
                                Create
                            </Button>
                        </div>
                        
                    </form>
                </Form>
                

            </DialogContent>
        </Dialog>
    )
}