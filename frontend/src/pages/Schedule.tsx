import { useEffect, useState } from "react";
import type { Schedule } from "../types/schedule";

import {
    getSchedules,
    createSchedule,
    deleteSchedule
} from "../services/scheduleService";


function Schedules() {

    const [schedules, setSchedules] = useState<Schedule[]>([]);

    const [newSchedule, setNewSchedule] = useState<Schedule>({
        id: 0,
        travelDate: "",
        departureTime: "",
        arrivalTime: ""
    });

    useEffect(() => {
        loadSchedules();
    }, []);

    const loadSchedules = async () => {
        const data = await getSchedules();

        setSchedules(data);
    };

    const handleCreate = async () => {
        await createSchedule(newSchedule);

        setNewSchedule({
            id: 0,
            travelDate: "",
            departureTime: "",
            arrivalTime: ""
        });

        loadSchedules();
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

            <div className="
                bg-white
                shadow
                rounded-xl
                p-6
                mb-8
            ">
                <h2 className="text-xl font-semibold mb-4">
                    Add Schedule
                </h2>

                <div className="grid gap-4">

                    <input
                        type="date"
                        className="border p-2 rounded"
                        value={newSchedule.travelDate}
                        onChange={(e) =>
                            setNewSchedule({
                                ...newSchedule,
                                travelDate: e.target.value
                            })
                        }
                    />

                    <input
                        type="time"
                        className="border p-2 rounded"
                        value={newSchedule.departureTime}
                        onChange={(e) =>
                            setNewSchedule({
                                ...newSchedule,
                                departureTime: e.target.value
                            })
                        }
                    />

                    <input
                        type="time"
                        className="border p-2 rounded"
                        value={newSchedule.arrivalTime}
                        onChange={(e) =>
                            setNewSchedule({
                                ...newSchedule,
                                arrivalTime: e.target.value
                            })
                        }
                    />

                    <button
                        onClick={handleCreate}
                        className="
                            bg-blue-600
                            text-white
                            px-4
                            py-2
                            rounded-lg
                        "
                    >
                        Add Schedule
                    </button>

                </div>
            </div>

            <div className="grid gap-4">

                {schedules.map((schedule) => (

                    <div
                        key={schedule.id}
                        className="
                            bg-white
                            shadow
                            rounded-xl
                            p-6
                            flex
                            justify-between
                            items-center
                        "
                    >

                        <div>

                            <h3 className="font-semibold">
                                {new Date(schedule.travelDate).toLocaleDateString()}
                            </h3>

                            <p>
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
                            "
                        >
                            Delete
                        </button>

                    </div>

                ))}

            </div>

        </div>
    );
}


export default Schedules;