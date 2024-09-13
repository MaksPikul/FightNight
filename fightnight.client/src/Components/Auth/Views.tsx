


import { useAuth } from "../../Context/UseAuth";

import { Outlet, Navigate } from "react-router-dom";
import { HomeHeader } from "../Header/HomeHeader";

export const PrivateViews = () => {
    const { isLoggedIn } = useAuth();
    return isLoggedIn() ?
        <div>
            <HomeHeader />
            <Outlet />
        </div>
        :
        <Navigate to="login" />
}

export const PublicViews = () => {
    const { isLoggedIn } = useAuth();
    return isLoggedIn() ?
        <Navigate to="home" />
        :
        <Outlet />
}
