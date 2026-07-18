import type { Booking } from "../../types/booking";

interface BookingListProps {
    bookings: Booking[];
    onEdit: (booking: Booking) => void;
    onDelete: (id: number) => void;
}

function BookingList({ bookings, onEdit, onDelete }: BookingListProps) {
    return (
        <div className="space-y-6">
            <h2 className="text-3xl font-bold text-gray-800 border-b border-gray-200 pb-4">
                Bookings
            </h2>

            {bookings.length === 0 && (
                <div className="bg-gray-50 rounded-2xl p-12 text-center border-2 border-dashed border-gray-300">
                    <p className="text-gray-500 text-lg">No bookings found.</p>
                </div>
            )}

            {bookings.map((booking) => (
                <div
                    key={booking.id}
                    className="bg-white rounded-2xl shadow-md hover:shadow-2xl transition-all duration-300 p-6 border border-gray-100"
                >
                    <div className="flex flex-col md:flex-row md:justify-between md:items-start gap-4">
                        <div>
                            <h3 className="text-2xl font-bold text-gray-800">
                                {booking.bookingReference}
                            </h3>
                            <p className="text-gray-600 text-lg">
                                {booking.route.departureStation}{" "}
                                <span className="mx-2">→</span>{" "}
                                {booking.route.destinationStation}
                            </p>
                        </div>
                        <span className="inline-block bg-blue-100 text-blue-800 text-sm font-semibold px-4 py-1.5 rounded-full">
                            {booking.bookingType}
                        </span>
                    </div>

                    <div className="mt-6 grid grid-cols-2 md:grid-cols-4 gap-4 text-sm text-gray-700">
                        <p className="flex items-center gap-2">
                            <svg
                                xmlns="http://www.w3.org/2000/svg"
                                fill="none"
                                viewBox="0 0 24 24"
                                strokeWidth={1.5}
                                stroke="currentColor"
                                className="w-5 h-5 text-blue-500"
                            >
                                <path
                                    strokeLinecap="round"
                                    strokeLinejoin="round"
                                    d="M6.75 3v2.25M17.25 3v2.25M3 18.75V7.5a2.25 2.25 0 012.25-2.25h13.5A2.25 2.25 0 0121 7.5v11.25m-18 0A2.25 2.25 0 005.25 21h13.5A2.25 2.25 0 0021 18.75m-18 0v-7.5A2.25 2.25 0 015.25 9h13.5A2.25 2.25 0 0121 11.25v7.5"
                                />
                            </svg>
                            {new Date(booking.schedule.travelDate).toLocaleDateString()}
                        </p>
                        <p className="flex items-center gap-2">
                            <svg
                                xmlns="http://www.w3.org/2000/svg"
                                fill="none"
                                viewBox="0 0 24 24"
                                strokeWidth={1.5}
                                stroke="currentColor"
                                className="w-5 h-5 text-green-500"
                            >
                                <path
                                    strokeLinecap="round"
                                    strokeLinejoin="round"
                                    d="M12 6v6h4.5m4.5 0a9 9 0 11-18 0 9 9 0 0118 0z"
                                />
                            </svg>
                            {booking.schedule.departureTime} – {booking.schedule.arrivalTime}
                        </p>
                        <p className="flex items-center gap-2">
                            <svg
                                xmlns="http://www.w3.org/2000/svg"
                                fill="none"
                                viewBox="0 0 24 24"
                                strokeWidth={1.5}
                                stroke="currentColor"
                                className="w-5 h-5 text-purple-500"
                            >
                                <path
                                    strokeLinecap="round"
                                    strokeLinejoin="round"
                                    d="M15.75 6a3.75 3.75 0 11-7.5 0 3.75 3.75 0 017.5 0zM4.501 20.118a7.5 7.5 0 0114.998 0A17.933 17.933 0 0112 21.75c-2.676 0-5.216-.584-7.499-1.632z"
                                />
                            </svg>
                            Seat: {booking.seatNumber}
                        </p>
                        <p className="flex items-center gap-2">
                            <svg
                                xmlns="http://www.w3.org/2000/svg"
                                fill="none"
                                viewBox="0 0 24 24"
                                strokeWidth={1.5}
                                stroke="currentColor"
                                className="w-5 h-5 text-yellow-500"
                            >
                                <path
                                    strokeLinecap="round"
                                    strokeLinejoin="round"
                                    d="M12 6v12m-3-2.818l.879.659c1.171.879 3.07.879 4.242 0 1.172-.879 1.172-2.303 0-3.182C13.536 12.219 12.768 12 12 12c-.725 0-1.45-.22-2.003-.659-1.106-.879-1.106-2.303 0-3.182s2.9-.879 4.006 0l.415.33M21 12a9 9 0 11-18 0 9 9 0 0118 0z"
                                />
                            </svg>
                            Rs. {booking.ticketPrice}
                        </p>
                    </div>

                    {booking.specialRequests.length > 0 && (
                        <div className="mt-6 pt-4 border-t border-gray-200">
                            <h4 className="font-semibold text-gray-700 mb-2">
                                Special Requests
                            </h4>
                            <ul className="list-disc ml-6 text-gray-600 space-y-1">
                                {booking.specialRequests.map((request) => (
                                    <li key={request.id}>{request.description}</li>
                                ))}
                            </ul>
                        </div>
                    )}

                    <div className="mt-6 flex flex-wrap gap-3">
                        <button
                            onClick={() => onEdit(booking)}
                            className="px-5 py-2.5 bg-gradient-to-r from-yellow-400 to-yellow-500 text-white font-medium rounded-xl shadow-sm hover:shadow-md hover:scale-105 transition-all duration-200"
                        >
                            Edit
                        </button>
                        <button
                            onClick={() => onDelete(booking.id)}
                            className="px-5 py-2.5 bg-gradient-to-r from-red-500 to-red-600 text-white font-medium rounded-xl shadow-sm hover:shadow-md hover:scale-105 transition-all duration-200"
                        >
                            Delete
                        </button>
                    </div>
                </div>
            ))}
        </div>
    );
}

export default BookingList;