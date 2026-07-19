import { useEffect, useState } from "react";
import type { Schedule } from "../types/schedule";
import {
    getSchedules,
    createSchedule,
    deleteSchedule,
    updateSchedule
} from "../services/scheduleService";

function Schedules() {
    const [schedules, setSchedules] = useState<Schedule[]>([]);
    const [editingId, setEditingId] = useState<number | null>(null);

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

    const handleUpdate = async (schedule: Schedule) => {
        await updateSchedule(schedule);
        setEditingId(null);
        loadSchedules();
    };


    const startEditing = (schedule: Schedule) => {
        setEditingId(schedule.id);
    };

    const cancelEditing = () => {
        setEditingId(null);
        loadSchedules();
    };

    const handleEditChange = (id: number, field: keyof Schedule, value: string) => {
        setSchedules(prev =>
            prev.map(s =>
                s.id === id ? { ...s, [field]: value } : s
            )
        );
    };

    return (
        <div className="p-8">
            <h1 className="text-3xl font-bold mb-6">Schedules</h1>

            {/* Add Schedule Form */}
            <div className="bg-white shadow rounded-xl p-6 mb-8">
                <h2 className="text-xl font-semibold mb-4">Add Schedule</h2>
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
                        className="bg-blue-600 text-white px-4 py-2 rounded-lg"
                    >
                        Add Schedule
                    </button>
                </div>
            </div>

            {/* Schedule List */}
            <div className="grid gap-4">
                {schedules.map((schedule) => {
                    const isEditing = editingId === schedule.id;

                    return (
                        <div
                            key={schedule.id}
                            className="bg-white shadow rounded-xl p-6 flex justify-between items-center"
                        >
                            <div className="flex-1">
                                {isEditing ? (
                                    // Edit mode: show inputs
                                    <div className="grid grid-cols-3 gap-4">
                                        <input
                                            type="date"
                                            className="border p-2 rounded"
                                            value={schedule.travelDate}
                                            onChange={(e) =>
                                                handleEditChange(schedule.id, "travelDate", e.target.value)
                                            }
                                        />
                                        <input
                                            type="time"
                                            className="border p-2 rounded"
                                            value={schedule.departureTime}
                                            onChange={(e) =>
                                                handleEditChange(schedule.id, "departureTime", e.target.value)
                                            }
                                        />
                                        <input
                                            type="time"
                                            className="border p-2 rounded"
                                            value={schedule.arrivalTime}
                                            onChange={(e) =>
                                                handleEditChange(schedule.id, "arrivalTime", e.target.value)
                                            }
                                        />
                                    </div>
                                ) : (
                                    // Display mode
                                    <div>
                                        <h3 className="font-semibold">
                                            {new Date(schedule.travelDate).toLocaleDateString()}
                                        </h3>
                                        <p>
                                            {schedule.departureTime} → {schedule.arrivalTime}
                                        </p>
                                    </div>
                                )}
                            </div>

                            <div className="flex gap-2">
                                {isEditing ? (
                                    <>
                                        <button
                                            onClick={() => handleUpdate(schedule)}
                                            className="bg-green-600 text-white px-4 py-2 rounded-lg"
                                        >
                                            Save
                                        </button>
                                        <button
                                            onClick={cancelEditing}
                                            className="bg-gray-400 text-white px-4 py-2 rounded-lg"
                                        >
                                            Cancel
                                        </button>
                                    </>
                                ) : (
                                    <>
                                        <button
                                            onClick={() => startEditing(schedule)}
                                            className="bg-blue-600 text-white px-4 py-2 rounded-lg"
                                        >
                                            Update
                                        </button>
                                        <button
                                            onClick={() => handleDelete(schedule.id)}
                                            className="bg-red-600 text-white px-4 py-2 rounded-lg"
                                        >
                                            Delete
                                        </button>
                                    </>
                                )}
                            </div>
                        </div>
                    );
                })}
            </div>
        </div>
    );
}

export default Schedules;