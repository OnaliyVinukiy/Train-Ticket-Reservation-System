import { Outlet } from "react-router-dom";
import Sidebar from "../components/common/Sidebar";


function MainLayout() {

    return (
        <div className="flex">

            <Sidebar />

            <main className="flex-1 bg-gray-50 min-h-screen">
                <Outlet />
            </main>

        </div>
    );
}


export default MainLayout;