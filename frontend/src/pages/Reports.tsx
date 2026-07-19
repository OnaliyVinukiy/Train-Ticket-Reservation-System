import { useState } from "react";
import { pdf } from "@react-pdf/renderer";
import { getBookingReport, getRouteFrequency, exportCSV } from "../services/reportService";
import RouteFrequencyChart from "../components/report/RouteFrequencyChart";
import ReportPDF from "../components/report/ReportPdf";

function Reports() {
    const today = new Date().toISOString().split("T")[0];
    const [fromDate, setFromDate] = useState(today);
    const [toDate, setToDate] = useState(today);
    const [route, setRoute] = useState("");
    const [bookingType, setBookingType] = useState("");
    const [bookings, setBookings] = useState<any[]>([]);
    const [routeFrequency, setRouteFrequency] = useState<any[]>([]);

    const generateReport = async () => {
        const data = await getBookingReport(fromDate, toDate, route, bookingType);
        setBookings(data);

        const frequency = await getRouteFrequency(fromDate, toDate);
        const chart = Object.entries(frequency).map(([route, count]) => ({ route, count }));
        setRouteFrequency(chart);
    };

    const exportPDF = async () => {
        const doc = <ReportPDF
            bookings={bookings}
            routeFrequency={routeFrequency}
            fromDate={fromDate}
            toDate={toDate}
        />;
        const blob = await pdf(doc).toBlob();
        const link = document.createElement("a");
        link.href = URL.createObjectURL(blob);
        link.download = `report_${fromDate}_${toDate}.pdf`;
        link.click();
        URL.revokeObjectURL(link.href);
    };

    return (
        <div className="min-h-screen bg-gray-100 p-8">
            <div className="max-w-7xl mx-auto">
                <h1 className="text-4xl font-bold mb-8">Booking Analytics Dashboard</h1>

                <div className="bg-white rounded-2xl shadow p-6 mb-8">
                    <div className="grid md:grid-cols-4 gap-4">
                        <div>
                            <label>From Date</label>
                            <input
                                type="date"
                                value={fromDate}
                                onChange={e => setFromDate(e.target.value)}
                                className="border rounded-xl p-3 w-full"
                            />
                        </div>
                        <div>
                            <label>To Date</label>
                            <input
                                type="date"
                                value={toDate}
                                onChange={e => setToDate(e.target.value)}
                                className="border rounded-xl p-3 w-full"
                            />
                        </div>
                        <div>
                            <label>Route</label>
                            <input
                                placeholder="Colombo → Kandy"
                                value={route}
                                onChange={e => setRoute(e.target.value)}
                                className="border rounded-xl p-3 w-full"
                            />
                        </div>
                        <div>
                            <label>Booking Type</label>
                            <select
                                value={bookingType}
                                onChange={e => setBookingType(e.target.value)}
                                className="border rounded-xl p-3 w-full"
                            >
                                <option value="">All</option>
                                <option value="OneOff">One Off</option>
                                <option value="Recurring">Recurring</option>
                            </select>
                        </div>
                    </div>
                    <div className="flex gap-4 mt-6">
                        <button onClick={generateReport} className="bg-blue-600 text-white px-6 py-3 rounded-xl">
                            Generate Report
                        </button>
                        <button onClick={() => exportCSV(fromDate, toDate)} className="bg-green-600 text-white px-6 py-3 rounded-xl">
                            Export CSV
                        </button>
                        <button
                            onClick={exportPDF}
                            className="bg-red-600 text-white px-6 py-3 rounded-xl"
                            disabled={bookings.length === 0}
                        >
                            Export PDF
                        </button>
                    </div>
                </div>

                <div className="grid md:grid-cols-4 gap-6 mb-8">
                    <div className="bg-white p-6 rounded-xl shadow">
                        <h3>Total Bookings</h3>
                        <p className="text-3xl font-bold">{bookings.length}</p>
                    </div>
                    <div className="bg-white p-6 rounded-xl shadow">
                        <h3>Total Cost</h3>
                        <p className="text-3xl font-bold">Rs. {bookings.reduce((sum, b) => sum + b.ticketPrice, 0)}</p>
                    </div>
                    <div className="bg-white p-6 rounded-xl shadow">
                        <h3>Popular Route</h3>
                        <p className="font-bold">
                            {routeFrequency.length > 0 ? routeFrequency.sort((a, b) => b.count - a.count)[0].route : "No data"}
                        </p>
                    </div>
                    <div className="bg-white p-6 rounded-xl shadow">
                        <h3>Special Requests</h3>
                        <p className="text-3xl font-bold">{bookings.reduce((sum, b) => sum + b.specialRequests.length, 0)}</p>
                    </div>
                </div>

                <RouteFrequencyChart data={routeFrequency} />

                <div className="mt-10 space-y-4">
                    {bookings.map(b => (
                        <div key={b.id} className="bg-white rounded-xl shadow p-5">
                            <h2 className="font-bold text-xl">
                                {b.route.departureStation} → {b.route.destinationStation}
                            </h2>
                            <p>Date: {b.schedule.travelDate}</p>
                            <p>Seat: {b.seatNumber}</p>
                            <p>Price: Rs. {b.ticketPrice}</p>
                        </div>
                    ))}
                </div>
            </div>
        </div>
    );
}

export default Reports;