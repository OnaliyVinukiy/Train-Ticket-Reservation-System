import { useEffect, useState } from "react";
import { exportCSV, getRouteFrequency, getWeeklyReport, getWeeklySummary } from "../services/reportService";
import type { WeeklyReport, WeeklySummary } from "../types/weeklyReport";
import RouteFrequencyChart from "../components/report/RouteFrequencyChart";
import {
    ChartBarIcon,
    CalendarDaysIcon,
    CurrencyRupeeIcon,
    MapPinIcon,
    StarIcon,
    ArrowDownTrayIcon,
} from "@heroicons/react/24/outline";

function Reports() {
    const today = new Date();
    const defaultDate = today.toISOString().split("T")[0];
    const [startDate, setStartDate] = useState(defaultDate);
    const [report, setReport] = useState<WeeklyReport[]>([]);
    const [summary, setSummary] = useState<WeeklySummary>();
    const [routeFrequency, setRouteFrequency] = useState<any[]>([]);

    useEffect(() => {
        loadReport();
    }, []);

    const loadReport = async () => {
        const weekly = await getWeeklyReport(startDate);
        setReport(weekly);

        const summary = await getWeeklySummary(startDate);
        setSummary(summary);

        const endDate = new Date(new Date(startDate).setDate(new Date(startDate).getDate() + 6))
            .toISOString()
            .split("T")[0];

        const routes = await getRouteFrequency(startDate, endDate);
        const chartData = Object.entries(routes).map(([route, count]) => ({
            route,
            count,
        }));
        setRouteFrequency(chartData);
    };

    const handleExport = async () => {
        await exportCSV(
            startDate,
            new Date(new Date(startDate).setDate(new Date(startDate).getDate() + 6))
                .toISOString()
                .split("T")[0]
        );
    };

    return (
        <div className="min-h-screen bg-gradient-to-br from-slate-50 to-slate-100 p-6 md:p-10">
            <div className="max-w-7xl mx-auto">
                {/* Header Card */}
                <div className="bg-white/80 backdrop-blur-sm rounded-3xl shadow-xl p-6 md:p-8 mb-10 border border-white/20">
                    <div className="flex flex-col md:flex-row md:items-center md:justify-between gap-4">
                        <div className="flex items-center gap-3">
                            <ChartBarIcon className="w-8 h-8 text-indigo-600" />
                            <h1 className="text-3xl md:text-4xl font-bold">
                                Weekly Booking Report
                            </h1>
                        </div>
                        <div className="flex flex-wrap items-center gap-3">
                            <input
                                type="date"
                                value={startDate}
                                onChange={(e) => setStartDate(e.target.value)}
                                className="border-2 border-gray-200 rounded-xl px-4 py-2.5 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all duration-200 bg-gray-50/50"
                            />
                            <button
                                onClick={loadReport}
                                className="bg-indigo-600 hover:bg-indigo-700 active:scale-95 text-white px-6 py-2.5 rounded-xl font-medium transition-all duration-200 shadow-md hover:shadow-lg"
                            >
                                Generate
                            </button>
                            <button
                                onClick={handleExport}
                                className="bg-emerald-600 hover:bg-emerald-700 active:scale-95 text-white px-6 py-2.5 rounded-xl font-medium transition-all duration-200 shadow-md hover:shadow-lg flex items-center gap-2"
                            >
                                <ArrowDownTrayIcon className="w-5 h-5" />
                                Export CSV
                            </button>
                        </div>
                    </div>
                </div>

                {/* Summary Cards */}
                {summary && (
                    <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6 mb-10">
                        <div className="bg-white rounded-2xl shadow-lg p-6 border-l-4 border-indigo-500 hover:shadow-xl transition-shadow duration-200">
                            <div className="flex items-center justify-between">
                                <div>
                                    <p className="text-gray-500 text-sm font-medium uppercase tracking-wider">
                                        Total Bookings
                                    </p>
                                    <p className="text-3xl font-bold mt-1 text-gray-800">
                                        {summary.totalBookings}
                                    </p>
                                </div>
                                <CalendarDaysIcon className="w-8 h-8 text-indigo-500" />
                            </div>
                        </div>
                        <div className="bg-white rounded-2xl shadow-lg p-6 border-l-4 border-emerald-500 hover:shadow-xl transition-shadow duration-200">
                            <div className="flex items-center justify-between">
                                <div>
                                    <p className="text-gray-500 text-sm font-medium uppercase tracking-wider">
                                        Total Ticket Cost
                                    </p>
                                    <p className="text-3xl font-bold mt-1 text-gray-800">
                                        Rs. {summary.totalTicketCost}
                                    </p>
                                </div>
                                <CurrencyRupeeIcon className="w-8 h-8 text-emerald-500" />
                            </div>
                        </div>
                        <div className="bg-white rounded-2xl shadow-lg p-6 border-l-4 border-amber-500 hover:shadow-xl transition-shadow duration-200">
                            <div className="flex items-center justify-between">
                                <div>
                                    <p className="text-gray-500 text-sm font-medium uppercase tracking-wider">
                                        Most Popular Route
                                    </p>
                                    <p className="text-lg font-semibold mt-1 text-gray-800 truncate">
                                        {summary.mostPopularRoute}
                                    </p>
                                </div>
                                <MapPinIcon className="w-8 h-8 text-amber-500" />
                            </div>
                        </div>
                        <div className="bg-white rounded-2xl shadow-lg p-6 border-l-4 border-rose-500 hover:shadow-xl transition-shadow duration-200">
                            <div className="flex items-center justify-between">
                                <div>
                                    <p className="text-gray-500 text-sm font-medium uppercase tracking-wider">
                                        Special Requests
                                    </p>
                                    <p className="text-3xl font-bold mt-1 text-gray-800">
                                        {summary.totalSpecialRequests}
                                    </p>
                                </div>
                                <StarIcon className="w-8 h-8 text-rose-500" />
                            </div>
                        </div>
                    </div>
                )}

                {/* Chart */}
                <RouteFrequencyChart data={routeFrequency} />

                {/* Daily Report Cards */}
                <div className="grid md:grid-cols-2 lg:grid-cols-3 gap-6 mt-10">
                    {report.map((day) => (
                        <div
                            key={day.date}
                            className="bg-white rounded-2xl shadow-lg overflow-hidden hover:shadow-xl transition-shadow duration-200"
                        >
                            <div className="bg-gradient-to-r from-indigo-50 to-purple-50 px-5 py-4 border-b border-gray-100">
                                <h2 className="text-xl font-bold text-gray-800">{day.day}</h2>
                                <p className="text-sm text-gray-500">
                                    {new Date(day.date).toLocaleDateString("en-US", {
                                        weekday: "long",
                                        year: "numeric",
                                        month: "short",
                                        day: "numeric",
                                    })}
                                </p>
                            </div>
                            <div className="p-5 space-y-3 max-h-[400px] overflow-y-auto">
                                {day.bookings.length === 0 && (
                                    <div className="text-gray-400 text-center py-6 italic">
                                        No bookings
                                    </div>
                                )}
                                {day.bookings.map((booking) => (
                                    <div
                                        key={booking.bookingId}
                                        className="bg-gray-50 rounded-xl p-4 border border-gray-100 hover:border-indigo-200 transition-colors duration-150"
                                    >
                                        <div className="flex justify-between items-start">
                                            <div>
                                                <p className="text-xs text-gray-400 font-mono">
                                                    #{booking.bookingId}
                                                </p>
                                                <p className="font-semibold text-gray-800 mt-0.5">
                                                    {booking.route}
                                                </p>
                                            </div>
                                            <span className="text-sm font-bold text-indigo-600">
                                                Rs. {booking.ticketPrice}
                                            </span>
                                        </div>
                                        <div className="mt-2 flex items-center gap-3 text-sm text-gray-600">
                                            <span className="bg-indigo-100 text-indigo-800 px-2.5 py-0.5 rounded-full text-xs font-medium">
                                                Seat {booking.seatNumber}
                                            </span>
                                        </div>
                                        {booking.specialRequests.length > 0 && (
                                            <div className="mt-3 pt-2 border-t border-gray-200">
                                                <p className="text-xs font-semibold text-gray-500 uppercase tracking-wider">
                                                    Requests
                                                </p>
                                                <ul className="list-disc list-inside text-sm text-gray-700 mt-1 space-y-0.5">
                                                    {booking.specialRequests.map((request) => (
                                                        <li key={request} className="truncate">
                                                            {request}
                                                        </li>
                                                    ))}
                                                </ul>
                                            </div>
                                        )}
                                    </div>
                                ))}
                            </div>
                        </div>
                    ))}
                </div>
            </div>
        </div>
    );
}

export default Reports;