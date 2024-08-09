"use client"


import { 
    Card,
    CardContent,
    CardFooter,
    CardHeader,
    CardTitle
 } from "../ui/card";
import { Social } from "./social";

 //import { Header } from "./header";
 //import { Social } from "./social";
 //import { BackButton } from "./back-button";

interface CardWrapperProps {
    children: React.ReactNode;
    headerLabel: string;
    backButtonLabel: string;
    backButtonHrefLabel: string;
    backButtonHref: string;
    showSocial?: boolean;
}

export const CardWrapper = ({
    children,
    headerLabel,
    backButtonLabel,
    backButtonHrefLabel,
    backButtonHref,
    showSocial,
}: CardWrapperProps) => {

    return(
        <Card className="w-[500px] shadow-md">
            <CardHeader
            className="flex text-center">
                <CardTitle>{headerLabel}</CardTitle>
            </CardHeader>

            <CardContent>
            {children}

            {showSocial &&(
                //<> <separator />
                <Social />
            )}
            </CardContent>

            <CardFooter 
            className="justify-center flex-col flex">
            <p>{backButtonLabel}</p>
                <a
                href={backButtonHref}
                className="text-sm font-bold underline">
                    {backButtonHrefLabel}
                </a>
            </CardFooter>   
            
        </Card>
    )
}