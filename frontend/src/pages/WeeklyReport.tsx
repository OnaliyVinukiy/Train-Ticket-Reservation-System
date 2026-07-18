import { useEffect, useState } from "react";
import type { WeeklyReport } from "../types/weeklyReport";
import { getWeeklyReport } from "../services/reportService";

function WeeklyReport() {
    const [report, setReport] = useState<WeeklyReport[]>([]);

    useEffect(() => {
        loadReport();
    }, []);

    const loadReport = async () => {
        const data = await getWeeklyReport();
        setReport(data);
    };

    return (
        <div className="p-8">
            <h1 className="text-3xl font-bold mb-6">Weekly Booking Report</h1>
            <div className="grid gap-4">
                {report.map((day) => (
                    <div key={day.day} className="bg-white rounded-xl shadow p-6">
                        <h2 className="text-xl font-semibold mb-2">{day.day}</h2>
                        <p className="text-gray-600">Total Bookings: {day.bookingCount}</p>
                        <div className="mt-4">
                            <h3 className="font-medium mb-2">Special Requests</h3>
                            {day.specialRequests.length === 0 ? (
                                <p className="text-gray-400">No requests</p>
                            ) : (
                                <ul className="list-disc ml-5">
                                    {day.specialRequests.map((request, index) => (
                                        <li key={index}>{request}</li>
                                    ))}
                                </ul>
                            )}
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
}

export default WeeklyReport;