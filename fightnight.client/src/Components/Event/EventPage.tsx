import { useEffect, useState } from "react"
import { Card } from "../ui/card"
import { EventInfoForm } from "./EventInfoForm"
import { GetEventAndUsers, UploadEventBanner } from "../../Services/EventsService"
import { useNavigate, useParams } from "react-router-dom"
import { EventNav } from "./EventNav"
import { useForm } from "react-hook-form"
import * as z from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import {
    Form,
    FormControl,
    FormField,
    FormItem,
    FormLabel,
    FormMessage,
    FormDescription
} from "../ui/form"
import { EventHeader } from "./EventHeader"
import { EventSchema } from "../../Schemas";
import { Input } from "../ui/input";
import { EventInputField } from "./EventComps/EventFormInput";
import { Event, EventRole } from "../../Models/Event";

import { cn } from "@/lib/utils"
import { Button } from "@/components/ui/button"
import { Calendar } from "@/components/ui/calendar"
import {
    Popover,
    PopoverContent,
    PopoverTrigger,
} from "@/components/ui/popover"
import { CalendarIcon, ClockIcon } from "lucide-react";
import { format } from "date-fns";
import { UpdateEvent } from "../../Services/EventsService";
import { ScrollArea } from "../ui/scroll-area";
import { Label } from "../ui/label";
import { Separator } from "../ui/separator";
import { TimePicker } from "../Custom/TimePicker";
import { useModal } from "../../Hooks/use-modal-store"
import { useToast } from "../ui/use-toast"
import { Textarea } from "../ui/textarea"

export const EventPage = () => {
    const [event, setEvent] = useState<Event>();
    const [mounted, setMounted] = useState(false);
    const navigate = useNavigate();

    
    const { eventId } = useParams();
    const { onOpen } = useModal();
    const { toast } = useToast()

    const form = useForm<z.infer<typeof EventSchema>>({
        resolver: zodResolver(EventSchema),
    })

    useEffect(() => {
        console.log(eventId)
        GetEventAndUsers(eventId)
            .then(res => {
                if (res.data) {
                    setEvent(res?.data)
                    setMounted(true)
                    form.reset({
                        ...res.data,
                        date: new Date(res.data.date),
                        numMatches: res.data.numMatches.toString(),
                        numRounds: res.data.numRounds.toString(),
                        roundDur: res.data.roundDur.toString()
                    });
                }
                else if (res.response) {
                    console.log(res.response)
                    //navigate("/home")
                }
            }
            )
    },[])

    const onSubmit = async (values: z.infer<typeof EventSchema>) => {
        const res = await UpdateEvent(event.id, values)
        if (res?.data) {
            setEvent(response)
            location.reload();
        }
        else if (res?.response) {
            toast({
                title: "Error",
                description: res.response.data,
            })
        }
        else {
            console.log("mega err")
        }
    }
    
    

    
    

    return (
        <div>
        {mounted? 
            <>
            <EventHeader
            title={"Event Information"}
            //image={file && URL.createObjectURL(file)}
            desc={"Adjust Information about the event itself"}/>

                    
                    
                    

            
                    
            <Form {...form}>
                <form
                onSubmit={form.handleSubmit(onSubmit)} 
                className="space-y-4 m-3">


                <div className="flex flex-row justify-between items-end">
                    <FormField
                    control={form.control}
                    name="title"
                    render={({ field }) => ( 
                        <FormItem>
                            <FormLabel>Event Title</FormLabel>
                            <FormControl>
                                <Input
                                className="w-96"
                                type=""
                                //ref={inputRef}
                                //onClick={enableEditing}
                                    //className={isEditing ? "bg-green-500 " : ""}
                                {...field} />
                            </FormControl>
                            <FormMessage />
                        </FormItem>
                    )} />

                    {/* 
                    <Button
                    type="button"
                    onClick={() => {
                        onOpen("UploadBanner", { eventTitle: event?.title, eventId: event?.id })
                    }}>
                    Upload use pfp (to be reused)
                    </Button>
                    */}
                </div>
                    
                    

                    <FormField
                        control={form.control}
                        name="desc"
                        render={({ field }) => (
                            <FormItem>
                                <FormLabel>Description / Extra Information</FormLabel>
                                <FormControl>
                                    <Textarea
                                        //ref={inputRef}
                                        //onClick={enableEditing}
                                        //className={isEditing ? "bg-green-500 " : ""}
                                        {...field} />
                                </FormControl>
                                <FormMessage />
                            </FormItem>
                        )} />


                    <div
                    className=" flex flex-row gap-x-2">
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
                                                    variant={"outline"}
                                                    className={cn(
                                                        "flex w-[240px] pl-3 text-left font-normal",
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
                                                disabled={(date: Date) =>
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



                    <FormField
                        control={form.control}
                        name="time"
                        render={({ field }) => (
                            <FormItem
                                className="flex flex-col">
                                <FormLabel>Event Time</FormLabel>
                                <FormControl>

                                    <TimePicker
                                        value={field.value}
                                        onChange={field.onChange}
                                        disabled={false} />

                                </FormControl>
                                <FormMessage />
                            </FormItem>
                            )} />
                     </div>







                        <FormField
                        control={form.control}
                        name="venueAddress"
                        render={({ field }) => (
                            <FormItem>
                                <FormLabel>Venue Address</FormLabel>
                                <FormControl>
                                    <Input
                                        //ref={inputRef}
                                        //onClick={enableEditing}
                                        //className={isEditing ? "bg-green-500 " : ""}
                                        {...field} />
                                </FormControl>

                                <FormMessage />
                            </FormItem>
                        )} />

                    <Separator />

                    {/*<div
                    className="flex flex-row">*/}
                        <FormField
                            control={form.control}
                            name="numMatches"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>Number of Matches</FormLabel>
                                    <FormControl>
                                        <Input
                                            className="w-16"
                                            type="number"
                                            min={1}
                                            //ref={inputRef}
                                            //onClick={enableEditing}
                                            //className={isEditing ? "bg-green-500 " : ""}
                                            {...field} />
                                    </FormControl>
                                    <FormMessage />
                                </FormItem>
                            )} />

                        <FormField
                            control={form.control}
                            name="numRounds"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>Rounds per Match</FormLabel>
                                    <FormControl>
                                        <Input
                                            className="w-16"
                                            type="number"
                                            min={1 }
                                            //ref={inputRef}
                                            //onClick={enableEditing}
                                            //className={isEditing ? "bg-green-500 " : ""}
                                            {...field} />
                                    </FormControl>
                                    <FormMessage />
                                </FormItem>
                            )} />
                        <FormField
                            control={form.control}
                            name="roundDur"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>Round Duration (Minutes)</FormLabel>
                                    <FormControl>
                                        <Input
                                            className="w-16"
                                            type="number"
                                            min={1}
                                            //ref={inputRef}
                                            //onClick={enableEditing}
                                            //className={isEditing ? "bg-green-500 " : ""}
                                            {...field} />
                                    </FormControl>
                                    <FormMessage />
                                </FormItem>
                            )} />

                    {/*</div>*/}
                    <Separator />
                    {event?.role === EventRole.Admin &&
                        <div
                        className="flex flex-row justify-between">
                            <Button
                                type="submit"
                                disabled={!form.formState.isDirty}>
                                Save Changes
                            </Button>

                            <Button
                                type="button"
                                className="bg-red-600"
                                onClick={() => {
                                    onOpen("ConfirmDeleteEvent", { eventTitle: event?.title, eventId: event?.id })
                                }}>

                                Delete Event
                            </Button>
                        </div>
                    }
                </form>
            </Form>

                </>
        :
        <div>loading</div>
        }
        </div>

    )
}