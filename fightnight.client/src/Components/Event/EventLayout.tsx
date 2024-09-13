import { Card } from '../ui/card';
import { EventNav } from './EventNav';
import { Outlet} from "react-router-dom";
export const EventLayout = () => {
    return (
        <Card
            className="flex flex-col items-center">
            <div>
                <EventNav />
                <Outlet />
            </div>
        </Card>
    );
};