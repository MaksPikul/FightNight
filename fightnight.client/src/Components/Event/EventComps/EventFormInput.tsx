import { ElementRef, useRef, useState } from "react"
import { Input } from "../../ui/input"
import { useOnClickOutside } from "usehooks-ts";

export const EventInputField = (field: any) => {
    const [isEditing, setIsEditing] = useState(false)

    const inputRef = useRef<ElementRef<"input">>(null);

    const enableEditing = () => {
        setIsEditing(true)
        setTimeout(() => {
            inputRef.current?.focus();
            inputRef.current?.select();
        })
    }

    const disableEditing = () => {
        setIsEditing(false)
        
    }

    //addEventListener("keydown", onKeyDown)
    useOnClickOutside(inputRef, disableEditing)

    return (
        <Input
            ref={inputRef}
            onClick={enableEditing} 

            className={isEditing ? "bg-green-500 " : "" }
        {...field}/>
    )
}