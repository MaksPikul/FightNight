import { Card } from "../ui/card";
import { Separator } from "../ui/separator";

interface EventHeaderProps {
    title: string;
    desc: string;
    image?: string
}

export const EventHeader = ({
    title,
    desc,
    image
}: EventHeaderProps) => {

    const myDiv = document.getElementById('myDiv');
    //750 x 100

    return (
        <Card id="myDiv" className={`flex items-center p-3 h-[120px]`}>
            <div className="flex flex-row" >
                <div className="">
                    <p className="font-bold text-xl"> {title}</p>
                    <p> {desc} </p>
                </div>
                <img src={image } />
            </div>
            {/*<Separator className="mt-3" />*/}
            
        </Card>
    )
}