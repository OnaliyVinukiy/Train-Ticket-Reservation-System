import { useEffect, useState } from "react";
import type { Booking } from "../types/booking";
import { getBookings } from "../services/bookingService";

function Bookings() {
    const [bookings, setBookings] = useState<Booking[]>([]);

    useEffect(() => {
        loadBookings();
    }, []);

    const loadBookings = async () => {
        const data = await getBookings();
        setBookings(data);
    };

    return (
        <div className="min-h-screen bg-gray-50 p-8">
            <div className="max-w-7xl mx-auto">

                {/* Header */}
                <div className="mb-8">
                    <h1 className="text-3xl font-bold text-gray-900">
                        Train Bookings
                    </h1>

                    <p className="text-gray-500 mt-2">
                        Manage your personal train reservations
                    </p>
                </div>


                {/* Booking Cards */}
                <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
                    {bookings.map((booking) => (
                        <div
                            key={booking.id}
                            className="
                                bg-white
                                rounded-2xl
                                shadow-sm
                                border
                                border-gray-200
                                p-6
                                hover:shadow-lg
                                transition
                            "
                        >

                            {/* Booking Reference */}
                            <div className="flex justify-between items-start mb-5">
                                <div>
                                    <p className="text-xs text-gray-400 uppercase">
                                        Booking Reference
                                    </p>

                                    <h2 className="text-lg font-semibold text-gray-900">
                                        {booking.bookingReference}
                                    </h2>
                                </div>

                                <span
                                    className="
                                        px-3
                                        py-1
                                        rounded-full
                                        text-xs
                                        font-medium
                                        bg-blue-100
                                        text-blue-700
                                    "
                                >
                                    {booking.bookingType}
                                </span>
                            </div>


                            {/* Route */}
                            <div className="mb-5">
                                <p className="text-sm text-gray-500">
                                    Route
                                </p>

                                <p className="text-lg font-medium text-gray-800">
                                    {booking.route.departureStation}

                                    <span className="mx-2 text-blue-500">
                                        →
                                    </span>

                                    {booking.route.destinationStation}
                                </p>
                            </div>


                            {/* Booking Details */}
                            <div className="space-y-3 text-sm">

                                <div className="flex justify-between">
                                    <span className="text-gray-500">
                                        Seat
                                    </span>

                                    <span className="font-medium">
                                        {booking.seatNumber}
                                    </span>
                                </div>


                                <div className="flex justify-between">
                                    <span className="text-gray-500">
                                        Travel Date
                                    </span>

                                    <span className="font-medium">
                                        {new Date(
                                            booking.schedule.travelDate
                                        ).toLocaleDateString()}
                                    </span>
                                </div>


                                <div className="flex justify-between">
                                    <span className="text-gray-500">
                                        Departure
                                    </span>

                                    <span className="font-medium">
                                        {booking.schedule.departureTime}
                                    </span>
                                </div>


                                <div className="flex justify-between">
                                    <span className="text-gray-500">
                                        Price
                                    </span>

                                    <span className="font-semibold text-green-600">
                                        Rs. {booking.ticketPrice}
                                    </span>
                                </div>

                            </div>


                            {/* Special Requests */}
                            {booking.specialRequests.length > 0 && (
                                <div className="mt-5 pt-4 border-t">

                                    <p className="text-sm font-medium text-gray-700 mb-2">
                                        Special Requests
                                    </p>

                                    <div className="flex flex-wrap gap-2">
                                        {booking.specialRequests.map(
                                            (request) => (
                                                <span
                                                    key={request.id}
                                                    className="
                                                        bg-gray-100
                                                        text-gray-700
                                                        px-3
                                                        py-1
                                                        rounded-full
                                                        text-xs
                                                    "
                                                >
                                                    {request.description}
                                                </span>
                                            )
                                        )}
                                    </div>

                                </div>
                            )}

                        </div>
                    ))}
                </div>

            </div>
        </div>
    );
}

export default Bookings;