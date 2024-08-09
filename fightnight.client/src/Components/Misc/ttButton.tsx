import {
    Tooltip,
    TooltipContent,
    TooltipTrigger,
} from "../ui/tooltip"
import { Button } from "../ui/button";

interface ttButtonProps {
    trigger:React.ReactNode;
    content:string;
    disabled: boolean;
    onClick: () => void;
}

export const ToolTippedButton = ({
    trigger,
    content,
    disabled,
    onClick
}:ttButtonProps) => {

    return(
    <Tooltip delayDuration={75}>
        <TooltipTrigger
        asChild>
            <Button
            className="text-black"
            variant="outline"
            disabled={disabled}
            onClick={onClick}>
                {trigger}
            </Button>
        </TooltipTrigger>
        <TooltipContent>
            {content}
        </TooltipContent>
    </Tooltip>
    )
}