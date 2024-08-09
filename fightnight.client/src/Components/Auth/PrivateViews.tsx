


import { useAuth } from "../../Context/UseAuth";

import { Outlet, Navigate } from "react-router-dom";

const PrivateViews = () => {
    const { isLoggedIn } = useAuth();
    return isLoggedIn() ? <Outlet /> : <Navigate to="/" />
}

export default PrivateViews;