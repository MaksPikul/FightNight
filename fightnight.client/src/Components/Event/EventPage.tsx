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
import { useQuery, useQueryClient } from "@tanstack/react-query"
import { EventPageForm } from "./EventPageForm"

export const EventPage = () => {
    //const [event, setEvent] = useState<Event>();
    const [mounted, setMounted] = useState(false);

    const navigate = useNavigate();

    const { eventId } = useParams();

    const {
        isPending,
        status,
        error,
        data: event,
    } = useQuery({
        queryFn: () => GetEventAndUsers(eventId),
        queryKey: ['Event', eventId],
    })

    

    //use mutate to update the data

    if (isPending) return "loading..."

    if (error) {
        navigate("/home")
        return
    }

    if (status === "success")
        return (
            <div>
                <EventHeader
                    title={"Event Information"}
                    //image={file && URL.createObjectURL(file)}
                    desc={"Adjust Information about the event itself"} />
                <EventPageForm event={event} />
            </div>
        )
}
    

   
    
    
    

    
    
   
