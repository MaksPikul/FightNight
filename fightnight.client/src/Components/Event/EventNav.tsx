import { useEffect, useState } from "react";
import { Badge } from "../ui/badge"
import { Button } from "../ui/button"
import {
    DropdownMenu,
    DropdownMenuContent,
    DropdownMenuItem,
    DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu"
import { ChevronDown, X } from "lucide-react";
import {useNavigate} from "react-router-dom";


export const EventNav = () => {
    const [isOpened, setIsOpened] = useState(false)
    const navigate = useNavigate()

    const current = window.location.pathname.split("/")

    

    const [windowSize, setWindowSize] = useState({
        width: window.innerWidth,
        height: window.innerHeight,
    });

    useEffect(() => {
        const handleResize = () => {
            setWindowSize({
                width: window.innerWidth,
                height: window.innerHeight,
            });
        };

        window.addEventListener('resize', handleResize);
        return () => window.removeEventListener('resize', handleResize);

    }, []);
    console.log(current)

    const navigations = [
        {
            title: "Event Info",
            redirect: "",
            onPath: current[3] === undefined ,
            isDisabled: false
        },
        {
            title: "Team Panel",
            redirect: "/team",
            onPath: current[3] === "team",
            isDisabled: false

        },
        {
            title: "Fight Card",
            redirect: "/card",
            onPath: current[3] === "card",
            isDisabled: true

        },
        {
            title: "Ticket Management",
            redirect: "/ticket",
            onPath: current[3] === "ticket",
            isDisabled: true

        },
        
    ]

    

    return (
        <div className="flex space-x-1 ">
            
                <Button
                    onClick={() => navigate("/home")}>
                    Home </Button>
            <Badge
                className="flex bg-green-600 text-md justify-center w-40 h-10 rounded-lg ">
                Launch Event
            </Badge>
            {window.innerWidth > 700 ?
                <>
                    {navigations.map((nav, index) => {
                        return (
                            <Button
                                disabled={nav.isDisabled}
                                onClick={() => navigate(`/event/${current[2]}` +nav.redirect)}
                                className={nav.onPath&& "bg-red-500"}
                                key={index }
                                variant="ghost"
                            >{nav.title}
                            </Button>
                        )
                    }) }
                </>
                :
                <DropdownMenu onOpenChange={() => setIsOpened(!isOpened)} >
                    <DropdownMenuTrigger
                        asChild>
                        <Button
                            variant="ghost"> Navigation
                            {isOpened ? <X /> : <ChevronDown />} </Button>
                    </DropdownMenuTrigger>
                    <DropdownMenuContent>
                        {navigations.map((nav, index) => {
                            return (
                                <DropdownMenuItem
                                    disabled={nav.isDisabled }
                                    onClick={() => navigate(`/event/${current[2]}` + nav.redirect)}
                                    className={nav.onPath && "bg-red-500"}
                                    key={index}
                                    variant="ghost"
                                >{nav.title}
                                </DropdownMenuItem>
                            )
                        })}
                    </DropdownMenuContent>
                </DropdownMenu>
            }
        </div>
    )
}