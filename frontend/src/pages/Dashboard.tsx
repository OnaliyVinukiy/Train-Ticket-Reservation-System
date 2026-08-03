import { useEffect, useState } from "react";
import type { Booking } from "../types/booking";
import { getBookings } from "../services/bookingService";

function Dashboard() {
    const [bookings, setBookings] = useState<Booking[]>([]);

    useEffect(() => {
        loadDashboard();
    }, []);

    const loadDashboard = async () => {
        const data = await getBookings();
        setBookings(data);
    };

    const totalBookings = bookings.length;
    const recurringBookings = bookings.filter(booking => booking.bookingType === "Recurring").length;
    const totalCost = bookings.reduce((sum, booking) => sum + booking.ticketPrice, 0);
    const today = new Date().toISOString().split("T")[0];
    const todayBookings = bookings.filter(booking => booking.schedule.travelDate.split("T")[0] === today).length;

    return (
        <div className="min-h-screen bg-gray-100 p-8">
            <div className="max-w-7xl mx-auto space-y-8">
                <div>
                    <h1 className="text-4xl font-bold text-gray-800">Dashboard</h1>
                    <p className="text-gray-500 mt-2">Welcome back! Here is your reservation overview.</p>
                </div>
                <div className="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-4 gap-6">
                    <div className="bg-white rounded-2xl shadow-md p-6 border border-gray-100">
                        <p className="text-gray-500 text-sm">Total Bookings</p>
                        <h2 className="text-3xl font-bold text-blue-600 mt-3">{totalBookings}</h2>
                    </div>
                    <div className="bg-white rounded-2xl shadow-md p-6 border border-gray-100">
                        <p className="text-gray-500 text-sm">Today's Trips</p>
                        <h2 className="text-3xl font-bold text-green-600 mt-3">{todayBookings}</h2>
                    </div>
                    <div className="bg-white rounded-2xl shadow-md p-6 border border-gray-100">
                        <p className="text-gray-500 text-sm">Recurring Bookings</p>
                        <h2 className="text-3xl font-bold text-purple-600 mt-3">{recurringBookings}</h2>
                    </div>
                    <div className="bg-white rounded-2xl shadow-md p-6 border border-gray-100">
                        <p className="text-gray-500 text-sm">Total Cost</p>
                        <h2 className="text-3xl font-bold text-orange-600 mt-3">Rs. {totalCost.toLocaleString()}</h2>
                    </div>
                </div>
                <div className="grid grid-cols-1 gap-6">
                    <div className="bg-white rounded-2xl shadow-md p-6">
                        <div className="flex justify-between items-center mb-5">
                            <h2 className="text-xl font-bold text-gray-800">Recent Bookings</h2>
                        </div>
                        <div className="space-y-4">
                            {bookings.slice(-5).reverse().map(booking => (
                                <div key={booking.id} className="flex justify-between items-center bg-gray-50 rounded-xl p-4">
                                    <div>
                                        <p className="font-semibold text-gray-800">
                                            {booking.route.departureStation} → {booking.route.destinationStation}
                                        </p>
                                        <p className="text-sm text-gray-500">{booking.schedule.travelDate.split("T")[0]}</p>
                                    </div>
                                    <div className="text-right">
                                        <p className="font-bold text-blue-600">Rs. {booking.ticketPrice.toLocaleString()}</p>
                                        <p className="text-xs text-gray-500">{booking.bookingReference}</p>
                                    </div>
                                </div>
                            ))}
                            {bookings.length === 0 && (
                                <p className="text-gray-500 text-center py-8">No bookings available</p>
                            )}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default Dashboard;