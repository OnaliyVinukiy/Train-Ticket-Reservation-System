import { useEffect, useState } from "react";
import type { Schedule } from "../types/schedule";
import {
    getSchedules,
    deleteSchedule
} from "../services/scheduleService";


function Schedules() {

    const [schedules, setSchedules] = useState<Schedule[]>([]);

    useEffect(() => {
        loadSchedules();
    }, []);


    const loadSchedules = async () => {

        const data = await getSchedules();
        setSchedules(data);

    };


    const handleDelete = async (id: number) => {

        await deleteSchedule(id);
        loadSchedules();

    };


    return (

        <div className="p-8">

            <h1 className="text-3xl font-bold mb-6">
                Schedules
            </h1>


            <div className="grid gap-4">

                {
                    schedules.map((schedule) => (

                        <div
                            key={schedule.id}
                            className="
                                bg-white
                                rounded-xl
                                shadow
                                p-6
                                flex
                                justify-between
                                items-center
                            "
                        >

                            <div>

                                <h2 className="font-semibold text-lg">
                                    {new Date(schedule.travelDate).toLocaleDateString()}
                                </h2>


                                <p className="text-gray-600">
                                    {schedule.departureTime}
                                    {" → "}
                                    {schedule.arrivalTime}
                                </p>

                            </div>


                            <button
                                onClick={() => handleDelete(schedule.id)}
                                className="
                                    bg-red-500
                                    text-white
                                    px-4
                                    py-2
                                    rounded-lg
                                    hover:bg-red-600
                                "
                            >
                                Delete
                            </button>

                        </div>

                    ))
                }

            </div>

        </div>

    );
}


export default Schedules;