import { ContextMenuItem, ContextMenuContent, ContextMenuSub, ContextMenuSubTrigger, ContextMenuSubContent } from "../../ui/context-menu"

interface ContextMenuItemsProps {
    isOwner: boolean
    isAdmin: boolean
}

export const ContextMenuItems = ({
    isOwner,
    isAdmin
}: ContextMenuItemsProps) => {

    return (


        //DELETE MAYBE



        <ContextMenuContent>
            {isOwner && <ContextMenuItem
            onClick={()=>null}>Edit Message</ContextMenuItem>}

            <ContextMenuSub>
                <ContextMenuSubTrigger inset>Add Reaction</ContextMenuSubTrigger>
                <ContextMenuSubContent className="w-48">
                    <ContextMenuItem>List of reactions</ContextMenuItem>
                </ContextMenuSubContent>
            </ContextMenuSub>

            <ContextMenuItem
            onClick={() => null}>Reply</ContextMenuItem>

            <ContextMenuItem
            onClick={() => null}>Pin</ContextMenuItem>

            {isAdmin && <ContextMenuItem
            onClick={() => null}>Delete Message</ContextMenuItem>}
        </ContextMenuContent>
    )
}