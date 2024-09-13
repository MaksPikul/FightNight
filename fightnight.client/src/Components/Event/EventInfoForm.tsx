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
import { Event } from "../../Models/Event";

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
import { useEffect } from "react";
import { UpdateEvent } from "../../Services/EventsService";
import { ScrollArea } from "../ui/scroll-area";
import { Label } from "../ui/label";
import { Separator } from "../ui/separator";
import { TimePicker } from "../Custom/TimePicker";

//required because otherwise ill have to do event.event.title
interface EventInfoFormProps {
    event: Event
}

export const EventInfoForm = ({ event }: EventInfoFormProps) => {


    const form = useForm<z.infer<typeof EventSchema>>({
        resolver: zodResolver(EventSchema),
        defaultValues: {
            title: event.title,
            date: event.date,
            time: event.time,
            venueAddress: event.venueAddress,
            numMatches: "3", //event.numMatches, //add this
            numRounds: "3", //event.numMatches.toString(),
            roundDur: "3",//event.roundDur.toString(),
        }
    })

    const onSubmit = (values: z.infer<typeof EventSchema>) => {
        UpdateEvent(event.id, values)
        //console.log({id: event.id, ...values })
    }
    
    //Event Time


    return (
        <>

            <EventHeader
                title={"Event Information"}
                desc={"Adjust Information about the event itself"} />
            <Form {...form}>
                <form
                onSubmit={form.handleSubmit(onSubmit)} 
                className="space-y-4 m-3">


                    <FormField
                    control={form.control}
                    name="title"
                    render={({ field }) => ( 
                        <FormItem>
                            <FormLabel>Event Title</FormLabel>
                            <FormControl>
                                <Input
                                type="text"
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
                            //Show Are you sure modal
                        }}>

                        Delete Event
                        </Button>
                    </div>
                </form>
            </Form>
        </>
    )

}