import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogHeader,
    DialogTitle,
} from "@/components/ui/dialog"

import { Button } from "../ui/button";

import { Input } from "../ui/input";



import { Label } from "../ui/label";
import { useModal } from "../../Hooks/use-modal-store";
import { useEffect, useRef, useState } from "react";
import { DeleteEvent } from "../../Services/EventsService";
import { FormError } from "../Misc/formError";
import { useNavigate } from "react-router-dom";
import { useToast } from "../ui/use-toast";
import Croppie  from "croppie"




const croppieOptions = {
    showZoomer: true,
    //enableOrientation: true,
    //mouseWheelZoom: "ctrl",
    viewport: {
        width: 800,
        height: 20,
        type: "square"
    },
    boundary: {
        width: "50vw",
        height: "50vh"
    }
};





export const UploadBannerModal = () => {
    const [error, setError] = useState<string | undefined>("");
    const { isOpen, onClose, type, data } = useModal();

    const [isUploaded, setIsUploaded] = useState(false);
    const [file, setFile] = useState();
    const [image, setImage] = useState();
    //const navigate = useNavigate()
    
    
    const { eventId } = data
    const isModalOpen = isOpen && type === "UploadBanner";
    

    /*
    const onSubmit = async () => {
        if (eventTitle === inputValue) {
            const res = await DeleteEvent(eventId)
            if (res?.data) {
                navigate("/home")
                handleClose()
                toast({
                    title: "Event Deleted Successfully!",
                    description: "An email has been sent out to notify everyone involved.",
                })
            }
            else if (res?.response) {
                setError(res?.response?.data)
            }
            else {
                setError("System Error")
            }
        }
        else {
            setError("Incorrect Input")
        }
    }
    
    
    const handleClose = () => {
        setInputValue('')
        setError("")
        onClose()
    }
    */
    

    const UploadFile = async () => {

        console.log(file)

        if (file) {
            const formData = new FormData()
            formData.append("file", file);
            //const res = await UploadEventBanner(formData, event?.id)
            console.log(formData)

        }
    }

    const onChangeFile = (e: any) => {
        setFile(e.target.files[0])
        
        setImage(URL.createObjectURL(e.target.files[0]))

    }
    
    

    return (
        <Dialog className="" open={isModalOpen} onOpenChange={() => onClose()}>
            <DialogContent className=" p-0 overflow-hidden justify-center">
                <DialogHeader className="pt-8 px-6">
                    <DialogTitle className="text-center mb-3">
                        Choose a banner to upload
                    </DialogTitle>
                    <DialogDescription
                        className="text-secondary p-2  text-center">

                    </DialogDescription>
                </DialogHeader>

                <img src={image} className="w-[800px] h-[120px]" />









                <Label >Upload Event Banner</Label>
                <Input
                    type="file"
                    onChange={(e) => onChangeFile(e)} />
                <Button
                    onClick={() => UploadFile()}>
                    Upload Image
                </Button>
                


            </DialogContent>
        </Dialog>
    )
}