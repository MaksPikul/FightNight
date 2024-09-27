import { UseMutationResult } from "@tanstack/react-query";
import { useModal } from "../../../../Hooks/use-modal-store";
import { Message } from "../../../../Models/Message";
import { ContextMenuContent, ContextMenuItem, ContextMenuSub, ContextMenuSubContent, ContextMenuSubTrigger } from "../../../ui/context-menu";


interface MsgBoxContextMenuProps {
    msg: Message;
    isAdmin: boolean;
    isOwner: boolean;
    editMessageMutation: UseMutationResult<any, Error, string, unkown>
}

export const MsgBoxContextMenu = ({
    msg,
    isAdmin,
    isOwner,
    editMessageMutation
}: MsgBoxContextMenuProps) => {
    const { onOpen } = useModal()

    return (
        <ContextMenuContent>
            {isOwner && <ContextMenuItem
                onClick={() => null}>Edit Message</ContextMenuItem>}W

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

            {isAdmin && <ContextMenuItem className="text-red-600"
                onClick={() => onOpen("ConfirmDeleteMessage", { message: msg })}> Delete Message </ContextMenuItem>}
        </ContextMenuContent>
    )
}