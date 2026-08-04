import { useState } from "react";
import { pdf } from "@react-pdf/renderer";
import {
    getBookingReport,
    getRouteFrequency,
    startExport,
    getExportStatus,
    downloadExport,
} from "../services/reportService";
import RouteFrequencyChart from "../components/report/RouteFrequencyChart";
import ReportPDF from "../components/report/ReportPdf";

// Simple spinner using Tailwind CSS
const Spinner = () => (
    <div className="inline-block h-5 w-5 animate-spin rounded-full border-2 border-solid border-current border-r-transparent motion-reduce:animate-[spin_1.5s_linear_infinite]" />
);

function Reports() {
    const today = new Date().toISOString().split("T")[0];
    const [fromDate, setFromDate] = useState(today);
    const [toDate, setToDate] = useState(today);
    const [route, setRoute] = useState("");
    const [bookingType, setBookingType] = useState("");
    const [bookings, setBookings] = useState<any[]>([]);
    const [routeFrequency, setRouteFrequency] = useState<any[]>([]);

    // Loading states
    const [isGenerating, setIsGenerating] = useState(false);
    const [isPdfExporting, setIsPdfExporting] = useState(false);
    const [isCsvExporting, setIsCsvExporting] = useState(false);
    const [statusMessage, setStatusMessage] = useState("");

    const generateReport = async () => {
        setIsGenerating(true);
        setStatusMessage("Generating report...");
        try {
            const data = await getBookingReport(fromDate, toDate, route, bookingType);
            setBookings(data ?? []);
            const frequency = await getRouteFrequency(fromDate, toDate);
            const chart = Object.entries(frequency ?? {}).map(([route, count]) => ({ route, count }));
            setRouteFrequency(chart);
            setStatusMessage("Report generated successfully.");
        } catch (error) {
            console.error(error);
            setStatusMessage("Failed to generate report.");
        } finally {
            setIsGenerating(false);
        }
    };

    const exportPDF = async () => {
        if (bookings.length === 0) {
            setStatusMessage("No data to export. Please generate a report first.");
            return;
        }
        setIsPdfExporting(true);
        setStatusMessage("Preparing PDF...");
        try {
            const doc = (
                <ReportPDF
                    bookings={bookings}
                    routeFrequency={routeFrequency}
                    fromDate={fromDate}
                    toDate={toDate}
                />
            );
            const blob = await pdf(doc).toBlob();
            const url = URL.createObjectURL(blob);
            const link = document.createElement("a");
            link.href = url;
            link.download = `report_${fromDate}_${toDate}.pdf`;
            link.style.display = "none";
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
            setTimeout(() => URL.revokeObjectURL(url), 100);
            setStatusMessage("PDF downloaded successfully.");
        } catch (error) {
            console.error(error);
            setStatusMessage("Failed to export PDF.");
        } finally {
            setIsPdfExporting(false);
        }
    };

    const exportCSV = async () => {
        setIsCsvExporting(true);
        setStatusMessage("Starting CSV export...");
        try {
            const response = await startExport();
            const jobId = response.jobId;
            setStatusMessage("Generating CSV, please wait...");

            const checkStatus = async () => {
                try {
                    const status = await getExportStatus(jobId);
                    if (status.status === "Completed") {
                        setStatusMessage("CSV ready. Downloading...");
                        await downloadExport(jobId);
                        setStatusMessage("CSV export completed.");
                        setIsCsvExporting(false);
                        return;
                    }
                    if (status.status.startsWith("Failed")) {
                        setStatusMessage(`Export failed: ${status.status}`);
                        setIsCsvExporting(false);
                        return;
                    }
                    // Still processing, check again after delay
                    setTimeout(checkStatus, 3000);
                } catch (error) {
                    console.error(error);
                    setStatusMessage("Error checking export status.");
                    setIsCsvExporting(false);
                }
            };
            checkStatus();
        } catch (error) {
            console.error(error);
            setStatusMessage("Failed to start CSV export.");
            setIsCsvExporting(false);
        }
    };

    const popularRoute =
        routeFrequency.length > 0
            ? [...routeFrequency].sort((a, b) => b.count - a.count)[0].route
            : "No data";
    const totalCost = bookings.reduce((sum, b) => sum + (b.ticketPrice ?? 0), 0);
    const totalRequests = bookings.reduce(
        (sum, b) => sum + (b.specialRequests?.length ?? 0),
        0
    );

    return (
        <div className="min-h-screen bg-gray-100 p-8">
            <div className="max-w-7xl mx-auto">
                <h1 className="text-4xl font-bold mb-8">Booking Analytics Dashboard</h1>

                {/* Filters & Actions */}
                <div className="bg-white rounded-2xl shadow p-6 mb-8">
                    <div className="grid md:grid-cols-4 gap-4">
                        <div>
                            <label>From Date</label>
                            <input
                                type="date"
                                value={fromDate}
                                onChange={(e) => setFromDate(e.target.value)}
                                className="border rounded-xl p-3 w-full"
                            />
                        </div>
                        <div>
                            <label>To Date</label>
                            <input
                                type="date"
                                value={toDate}
                                onChange={(e) => setToDate(e.target.value)}
                                className="border rounded-xl p-3 w-full"
                            />
                        </div>
                        <div>
                            <label>Route</label>
                            <input
                                placeholder="Colombo → Kandy"
                                value={route}
                                onChange={(e) => setRoute(e.target.value)}
                                className="border rounded-xl p-3 w-full"
                            />
                        </div>
                        <div>
                            <label>Booking Type</label>
                            <select
                                value={bookingType}
                                onChange={(e) => setBookingType(e.target.value)}
                                className="border rounded-xl p-3 w-full"
                            >
                                <option value="">All</option>
                                <option value="OneOff">One Off</option>
                                <option value="Recurring">Recurring</option>
                            </select>
                        </div>
                    </div>

                    <div className="flex flex-wrap items-center gap-4 mt-6">
                        <button
                            onClick={generateReport}
                            disabled={isGenerating || isCsvExporting || isPdfExporting}
                            className="bg-blue-600 text-white px-6 py-3 rounded-xl disabled:opacity-50 flex items-center gap-2"
                        >
                            {isGenerating && <Spinner />}
                            {isGenerating ? "Generating..." : "Generate Report"}
                        </button>

                        <button
                            onClick={exportCSV}
                            disabled={isCsvExporting || isGenerating || isPdfExporting}
                            className="bg-green-600 text-white px-6 py-3 rounded-xl disabled:opacity-50 flex items-center gap-2"
                        >
                            {isCsvExporting && <Spinner />}
                            {isCsvExporting ? "Exporting CSV..." : "Export CSV"}
                        </button>

                        <button
                            onClick={exportPDF}
                            disabled={bookings.length === 0 || isPdfExporting || isGenerating || isCsvExporting}
                            className="bg-red-600 text-white px-6 py-3 rounded-xl disabled:opacity-50 flex items-center gap-2"
                        >
                            {isPdfExporting && <Spinner />}
                            {isPdfExporting ? "Generating PDF..." : "Export PDF"}
                        </button>

                        {statusMessage && (
                            <p className="text-sm text-gray-700 bg-gray-200 px-4 py-2 rounded-full">
                                {statusMessage}
                            </p>
                        )}
                    </div>
                </div>

                {/* Summary Cards */}
                <div className="grid md:grid-cols-4 gap-6 mb-8">
                    <div className="bg-white p-6 rounded-xl shadow">
                        <h3>Total Bookings</h3>
                        <p className="text-3xl font-bold">{bookings.length}</p>
                    </div>
                    <div className="bg-white p-6 rounded-xl shadow">
                        <h3>Total Cost</h3>
                        <p className="text-3xl font-bold">Rs. {totalCost}</p>
                    </div>
                    <div className="bg-white p-6 rounded-xl shadow">
                        <h3>Popular Route</h3>
                        <p className="font-bold">{popularRoute}</p>
                    </div>
                    <div className="bg-white p-6 rounded-xl shadow">
                        <h3>Special Requests</h3>
                        <p className="text-3xl font-bold">{totalRequests}</p>
                    </div>
                </div>

                {/* Chart */}
                <RouteFrequencyChart data={routeFrequency} />

                {/* Booking List */}
                <div className="mt-10 space-y-4">
                    {bookings.map((b) => (
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