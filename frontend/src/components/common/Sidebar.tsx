import { NavLink } from "react-router-dom";

function Sidebar() {

    const menuItems = [
        {
            name: "Dashboard",
            path: "/"
        },
        {
            name: "Bookings",
            path: "/bookings"
        },
        {
            name: "Schedules",
            path: "/schedules"
        },
        {
            name: "Requests",
            path: "/requests"
        },
        {
            name: "Reports",
            path: "/reports"
        },
        {
            name: "Chatbot",
            path: "/chatbot"
        }
    ];


    return (

        <aside
            className="
                w-64
                min-h-screen
                bg-slate-900
                text-white
                p-6
            "
        >

            <h1 className="text-xl font-bold mb-8">
                Train Manager
            </h1>


            <nav className="space-y-2">

                {
                    menuItems.map((item) => (

                        <NavLink
                            key={item.path}
                            to={item.path}
                            className={({ isActive }) =>
                                `
                                    block
                                    px-4
                                    py-3
                                    rounded-lg
                                    transition

                                    ${isActive
                                    ? "bg-blue-600"
                                    : "hover:bg-slate-800"
                                }
                                `
                            }
                        >
                            {item.name}
                        </NavLink>

                    ))
                }

            </nav>

        </aside>

    );
}


export default Sidebar;