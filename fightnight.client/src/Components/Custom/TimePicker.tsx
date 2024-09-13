import { ClockIcon } from "lucide-react"
import { Button } from "../ui/button"
import { FormControl } from "../ui/form"
import { Label } from "../ui/label"
import { ScrollArea } from "../ui/scroll-area"
import { Separator } from "../ui/separator"
import {
    Popover,
    PopoverContent,
    PopoverTrigger,
} from "@/components/ui/popover"
import { useState } from "react"
import { cn } from "../../lib/utils"

interface TimePickerProps {
    value: string,
    onChange: any,
    disabled: boolean
}

export const TimePicker = ({
    value,
    onChange,
    disabled
}: TimePickerProps) => {

    const [hour, setHour] = useState(0)
    const [minute, setMinute] = useState(0)
    const [pickerOpen, setPickerOpen] = useState(false);

    return (
        <Popover open={pickerOpen} onOpenChange={setPickerOpen}>
            <PopoverTrigger asChild>
                <FormControl>
                    <Button
                        disabled={disabled }
                        variant={"outline"}
                        className={cn(
                            "flex w-[240px] pl-3 text-left font-normal",
                            !value && "text-muted-foreground"
                        )}
                    >
                        {value ? (
                            value
                        ) : (
                            <span>Pick a Time</span>
                        )}
                        <ClockIcon className="ml-auto h-4 w-4 opacity-50" />
                    </Button>
                </FormControl>
            </PopoverTrigger>
            <PopoverContent className="w-auto p-0" align="start">
                                       
            <div
                className="flex flex-col justify-center">
                <div
                    className="flex flex-row gap-x-1">
                    <div
                        className="flex flex-col w-16 items-center">
                        <Label className="py-2 px-1">Hour</Label>
                        <Separator />
                        <ScrollArea >
                            <div
                                className="flex flex-col h-52">
                                {Array.from({ length: 25 }, (_, i) => (
                                    <Button
                                        className={i === hour ? "bg-red-500" : "" }
                                        onClick={()=>setHour(i) }
                                        key={i}
                                        value={i}
                                        variant="ghost">
                                        {i}
                                    </Button>
                                ))}
                            </div>
                        </ScrollArea>
                    </div >

                    <div
                        className="flex flex-col w-16 items-center">
                        <Label className="py-2 px-1">Minute</Label>
                        <Separator />
                            <ScrollArea>
                            <div
                                className="flex flex-col h-52">
                                {Array.from({ length: 61 }, (_, i) => (
                                    <Button
                                        className={i === minute ? "bg-red-500" : ""}
                                        onClick={() => setMinute(i)}
                                        value={i}
                                        key={i}
                                        variant="ghost">
                                        {i}
                                    </Button>
                                ))}
                            </div>
                        </ScrollArea>
                    </div>
            </div>

            
                <Button
                variant="ghost"
                onClick={() => {
                    onChange(hour.toString() + ":" + minute.toString().padStart(2, "0"))
                    setPickerOpen(false)
                }}>
                Ok
                </Button>
            </div>

            </PopoverContent>
        </Popover>
    )
}