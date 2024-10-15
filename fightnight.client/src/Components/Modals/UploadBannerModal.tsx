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
import { useState } from "react";
import { FormError } from "../Misc/formError";
import { useNavigate } from "react-router-dom";
import { useToast } from "../ui/use-toast";
import Croppie  from "croppie"
import { useAuth } from "../../Context/UseAuth";
import { UploadPfpApi } from "../../Services/UserService";










export const UploadBannerModal = () => {
    const [error, setError] = useState<string | undefined>("");
    const { isOpen, onClose, type} = useModal();

    const [file, setFile] = useState();
    const [image, setImage] = useState();
    //const navigate = useNavigate()
    
    
    const { user } = useAuth()
    const isModalOpen = isOpen && type === "UploadBanner";
    
    
    

    const UploadFile = async () => {

        console.log(file)

        if (file) {
            const formData = new FormData()
            formData.append("file", file);
            const res = await UploadPfpApi(formData)
            console.log(res)

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
                        Choose a pfp to upload
                    </DialogTitle>
                    <DialogDescription
                        className="text-secondary p-2  text-center">

                    </DialogDescription>
                </DialogHeader>

                <img src={image} className="w-[800px] h-[120px]" />

                
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