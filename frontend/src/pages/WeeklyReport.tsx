import { useEffect, useState } from "react";
import type { WeeklyReport } from "../types/weeklyReport";
import { getWeeklyReport } from "../services/reportService";

function WeeklyReport() {
    const today = new Date().toISOString().split("T")[0];
    const [report, setReport] = useState<WeeklyReport[]>([]);

    useEffect(() => {
        loadReport();
    }, []);

    const loadReport = async () => {
        const data = await getWeeklyReport(today);
        setReport(data);
    };

    return (
        <div className="p-8">
            <h1 className="text-3xl font-bold mb-6">Weekly Booking Report</h1>
            <div className="grid gap-4">
                {report.map((day) => (
                    <div key={day.date} className="bg-white rounded-xl shadow p-6">
                        <h2 className="text-xl font-semibold">{day.day}</h2>
                        <p className="text-gray-600 mb-4">Date: {day.date.split("T")[0]}</p>
                        <p className="font-medium">Total Bookings: {day.bookings.length}</p>
                        {day.bookings.length === 0 ? (
                            <p className="text-gray-400 mt-3">No bookings</p>
                        ) : (
                            <div className="mt-4 space-y-3">
                                {day.bookings.map((booking) => (
                                    <div key={booking.bookingId} className="border rounded-lg p-4">
                                        <p>Route: {booking.route}</p>
                                        <p>Seat: {booking.seatNumber}</p>
                                        <p>Price: Rs. {booking.ticketPrice}</p>
                                        <p className="mt-2 font-medium">Special Requests:</p>
                                        {booking.specialRequests.length === 0 ? (
                                            <span className="text-gray-400">None</span>
                                        ) : (
                                            <ul className="list-disc ml-5">
                                                {booking.specialRequests.map((request: string, index: number) => (
                                                    <li key={index}>{request}</li>
                                                ))}
                                            </ul>
                                        )}
                                    </div>
                                ))}
                            </div>
                        )}
                    </div>
                ))}
            </div>
        </div>
    );
}

export default WeeklyReport;