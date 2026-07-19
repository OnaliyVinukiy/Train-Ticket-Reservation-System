import { useEffect, useState } from "react";
import type { Booking } from "../../types/booking";
import { BOOKING_TYPES } from "../../types/booking";
import { RECURRENCE_PATTERNS } from "../../types/recurrencePattern";

interface BookingFormProps {
    onSubmit: (booking: Booking) => void;
    editingBooking?: Booking | null;
}

const emptyBooking: Booking = {
    id: 0,
    bookingReference: "",
    seatNumber: "",
    ticketPrice: 0,
    bookingType: BOOKING_TYPES.OneOff,
    recurrencePattern: RECURRENCE_PATTERNS.None,
    recurrenceEndDate: "",
    route: {
        departureStation: "",
        destinationStation: ""
    },
    schedule: {
        travelDate: "",
        departureTime: "",
        arrivalTime: ""
    },
    specialRequests: []
};

function BookingForm({ onSubmit, editingBooking }: BookingFormProps) {
    const [booking, setBooking] = useState<Booking>(emptyBooking);
    const [requests, setRequests] = useState<string[]>([]);
    const [newRequest, setNewRequest] = useState("");
    const [errors, setErrors] = useState<Record<string, string>>({});

    useEffect(() => {
        if (editingBooking) {
            setBooking(editingBooking);
            setRequests(editingBooking.specialRequests.map(x => x.description));
        }
    }, [editingBooking]);

    const toggleRequest = (value: string) => {
        setRequests(previous =>
            previous.includes(value)
                ? previous.filter(x => x !== value)
                : [...previous, value]
        );
    };

    const validateForm = () => {
        const newErrors: Record<string, string> = {};

        if (!booking.route.departureStation.trim()) {
            newErrors.departureStation = "Departure station is required";
        }

        if (!booking.route.destinationStation.trim()) {
            newErrors.destinationStation = "Destination station is required";
        }

        if (
            booking.route.departureStation.trim() &&
            booking.route.destinationStation.trim() &&
            booking.route.departureStation === booking.route.destinationStation
        ) {
            newErrors.destinationStation =
                "Destination cannot be the same as departure station";
        }


        if (!booking.schedule.travelDate) {
            newErrors.travelDate = "Travel date is required";
        }
        else if (
            new Date(booking.schedule.travelDate) < new Date()
        ) {
            newErrors.travelDate =
                "Travel date cannot be in the past";
        }


        if (!booking.schedule.departureTime) {
            newErrors.departureTime = "Departure time is required";
        }


        if (!booking.schedule.arrivalTime) {
            newErrors.arrivalTime = "Arrival time is required";
        }


        if (
            booking.schedule.departureTime &&
            booking.schedule.arrivalTime &&
            booking.schedule.departureTime >= booking.schedule.arrivalTime
        ) {
            newErrors.arrivalTime =
                "Arrival time must be after departure time";
        }


        if (!booking.seatNumber.trim()) {
            newErrors.seatNumber = "Seat number is required";
        }


        if (booking.ticketPrice <= 0) {
            newErrors.ticketPrice =
                "Ticket price must be greater than zero";
        }


        if (
            booking.bookingType === BOOKING_TYPES.Recurring &&
            !booking.recurrenceEndDate
        ) {
            newErrors.recurrenceEndDate =
                "Recurrence end date is required";
        }


        setErrors(newErrors);

        return Object.keys(newErrors).length === 0;
    };

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();

        if (!validateForm()) {
            return;
        }


        const updatedBooking: Booking = {
            ...booking,
            specialRequests: requests.map(request => ({
                description: request
            }))
        };


        onSubmit(updatedBooking);

        setBooking(emptyBooking);
        setRequests([]);
        setErrors({});
    };

    return (
        <form
            onSubmit={handleSubmit}
            className="bg-white rounded-2xl shadow-xl p-6 space-y-6 transition-all duration-300"
        >
            <h2 className="text-2xl font-bold text-gray-800 border-b border-gray-200 pb-3">
                {editingBooking ? "Edit Booking" : "New Booking"}
            </h2>

            {/* Route */}
            <section>
                <h3 className="text-sm font-semibold text-gray-500 uppercase tracking-wider mb-3">Route</h3>
                <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                    <div className="space-y-1">
                        <label htmlFor="departure" className="text-sm font-medium text-gray-700">
                            Departure Station
                        </label>
                        <input
                            id="departure"
                            className={`w-full px-4 py-2.5 border rounded-xl outline-none transition ${errors.departureStation
                                ? "border-red-500 focus:ring-red-500"
                                : "border-gray-300 focus:ring-blue-500"
                                }`}
                            placeholder="e.g. London"
                            value={booking.route.departureStation}
                            onChange={(e) =>
                                setBooking({
                                    ...booking,
                                    route: { ...booking.route, departureStation: e.target.value }
                                })
                            }
                        />
                        {errors.departureStation && (
                            <p className="text-sm text-red-600">
                                {errors.departureStation}
                            </p>
                        )}
                    </div>
                    <div className="space-y-1">
                        <label htmlFor="destination" className="text-sm font-medium text-gray-700">
                            Destination Station
                        </label>
                        <input
                            id="destination"
                            className={`w-full px-4 py-2.5 border rounded-xl outline-none transition ${errors.destinationStation
                                ? "border-red-500 focus:ring-red-500"
                                : "border-gray-300 focus:ring-blue-500"
                                }`}
                            placeholder="e.g. Paris"
                            value={booking.route.destinationStation}
                            onChange={(e) =>
                                setBooking({
                                    ...booking,
                                    route: { ...booking.route, destinationStation: e.target.value }
                                })
                            }
                        />
                        {errors.destinationStation && (
                            <p className="text-sm text-red-600">
                                {errors.destinationStation}
                            </p>
                        )}
                    </div>
                </div>
            </section>

            {/* Schedule */}
            <section>
                <h3 className="text-sm font-semibold text-gray-500 uppercase tracking-wider mb-3">Schedule</h3>
                <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
                    <div className="space-y-1">
                        <label htmlFor="travelDate" className="text-sm font-medium text-gray-700">
                            Travel Date
                        </label>
                        <input
                            id="travelDate"
                            className={`w-full px-4 py-2.5 border rounded-xl outline-none transition ${errors.travelDate
                                ? "border-red-500 focus:ring-red-500"
                                : "border-gray-300 focus:ring-blue-500"
                                }`}
                            type="date"
                            value={booking.schedule.travelDate}
                            onChange={(e) =>
                                setBooking({
                                    ...booking,
                                    schedule: { ...booking.schedule, travelDate: e.target.value }
                                })
                            }
                        />
                        {errors.travelDate && (
                            <p className="text-sm text-red-600">
                                {errors.travelDate}
                            </p>
                        )}
                    </div>
                    <div className="space-y-1">
                        <label htmlFor="departureTime" className="text-sm font-medium text-gray-700">
                            Departure Time
                        </label>
                        <input
                            id="departureTime"
                            className={`w-full px-4 py-2.5 border rounded-xl outline-none transition ${errors.departureTime
                                ? "border-red-500 focus:ring-red-500"
                                : "border-gray-300 focus:ring-blue-500"
                                }`}
                            type="time"
                            value={booking.schedule.departureTime}
                            onChange={(e) =>
                                setBooking({
                                    ...booking,
                                    schedule: { ...booking.schedule, departureTime: e.target.value }
                                })
                            }
                        />
                        {errors.departureTime && (
                            <p className="text-sm text-red-600">
                                {errors.departureTime}
                            </p>
                        )}
                    </div>
                    <div className="space-y-1">
                        <label htmlFor="arrivalTime" className="text-sm font-medium text-gray-700">
                            Arrival Time
                        </label>
                        <input
                            id="arrivalTime"
                            className={`w-full px-4 py-2.5 border rounded-xl outline-none transition ${errors.arrivalTime
                                ? "border-red-500 focus:ring-red-500"
                                : "border-gray-300 focus:ring-blue-500"
                                }`}
                            type="time"
                            value={booking.schedule.arrivalTime}
                            onChange={(e) =>
                                setBooking({
                                    ...booking,
                                    schedule: { ...booking.schedule, arrivalTime: e.target.value }
                                })
                            }
                        />
                        {errors.arrivalTime && (
                            <p className="text-sm text-red-600">
                                {errors.arrivalTime}
                            </p>
                        )}
                    </div>
                </div>
            </section>

            {/* Seat & Price */}
            <section>
                <h3 className="text-sm font-semibold text-gray-500 uppercase tracking-wider mb-3">Seat &amp; Price</h3>
                <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                    <div className="space-y-1">
                        <label htmlFor="seatNumber" className="text-sm font-medium text-gray-700">
                            Seat Number
                        </label>
                        <input
                            id="seatNumber"
                            className={`w-full px-4 py-2.5 border rounded-xl outline-none transition ${errors.seatNumber
                                ? "border-red-500 focus:ring-red-500"
                                : "border-gray-300 focus:ring-blue-500"
                                }`}
                            placeholder="e.g. 12A"
                            value={booking.seatNumber}
                            onChange={(e) =>
                                setBooking({
                                    ...booking,
                                    seatNumber: e.target.value
                                })
                            }
                        />{errors.seatNumber && (
                            <p className="text-sm text-red-600">
                                {errors.seatNumber}
                            </p>
                        )}
                    </div>
                    <div className="space-y-1">
                        <label htmlFor="ticketPrice" className="text-sm font-medium text-gray-700">
                            Ticket Price (Rs.)
                        </label>
                        <input
                            id="ticketPrice"
                            className={`w-full px-4 py-2.5 border rounded-xl outline-none transition ${errors.ticketPrice
                                ? "border-red-500 focus:ring-red-500"
                                : "border-gray-300 focus:ring-blue-500"
                                }`}
                            type="number"
                            placeholder="0.00"
                            value={booking.ticketPrice}
                            onChange={(e) =>
                                setBooking({
                                    ...booking,
                                    ticketPrice: Number(e.target.value)
                                })
                            }
                        />
                        {errors.ticketPrice && (
                            <p className="text-sm text-red-600">
                                {errors.ticketPrice}
                            </p>
                        )}
                    </div>
                </div>
            </section>

            {/* Booking Type */}
            <section>
                <h3 className="text-sm font-semibold text-gray-500 uppercase tracking-wider mb-3">Booking Type</h3>
                <div className="space-y-4">
                    <div className="max-w-xs">
                        <select
                            id="bookingType"
                            className="w-full px-4 py-2.5 border border-gray-300 rounded-xl focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition duration-200 bg-white"
                            value={booking.bookingType}
                            onChange={(e) =>
                                setBooking({
                                    ...booking,
                                    bookingType: e.target.value as Booking["bookingType"]
                                })
                            }
                        >
                            <option value={BOOKING_TYPES.OneOff}>One Off</option>
                            <option value={BOOKING_TYPES.Recurring}>Recurring</option>
                        </select>
                    </div>

                    {booking.bookingType === BOOKING_TYPES.Recurring && (
                        <div className="grid grid-cols-1 sm:grid-cols-2 gap-4 bg-gray-50 p-4 rounded-xl border border-gray-200">
                            <div className="space-y-1">
                                <label htmlFor="recurrencePattern" className="text-sm font-medium text-gray-700">
                                    Recurrence Pattern
                                </label>
                                <select
                                    id="recurrencePattern"
                                    className="w-full px-4 py-2.5 border border-gray-300 rounded-xl focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition duration-200 bg-white"
                                    value={booking.recurrencePattern}
                                    onChange={(e) =>
                                        setBooking({
                                            ...booking,
                                            recurrencePattern: e.target.value as Booking["recurrencePattern"]
                                        })
                                    }
                                >
                                    <option value={RECURRENCE_PATTERNS.Daily}>Daily</option>
                                    <option value={RECURRENCE_PATTERNS.Weekly}>Weekly</option>
                                    <option value={RECURRENCE_PATTERNS.Monthly}>Monthly</option>
                                </select>
                            </div>
                            <div className="space-y-1">
                                <label htmlFor="recurrenceEndDate" className="text-sm font-medium text-gray-700">
                                    Recurrence End Date
                                </label>
                                <input
                                    id="recurrenceEndDate"
                                    className="w-full px-4 py-2.5 border border-gray-300 rounded-xl focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition duration-200"
                                    type="date"
                                    value={booking.recurrenceEndDate}
                                    onChange={(e) =>
                                        setBooking({
                                            ...booking,
                                            recurrenceEndDate: e.target.value
                                        })
                                    }
                                />
                            </div>
                        </div>
                    )}
                </div>
            </section>

            {/* Special Requests */}
            <section className="border-t border-gray-200 pt-6">
                <h3 className="text-sm font-semibold text-gray-500 uppercase tracking-wider mb-4">Special Requests</h3>
                <div className="space-y-3">
                    <div className="flex flex-wrap gap-6">
                        {["Window Seat", "Wheelchair Assistance", "Extra Luggage"].map(request => (
                            <label key={request} className="flex items-center space-x-2 text-sm">
                                <input
                                    type="checkbox"
                                    checked={requests.includes(request)}
                                    onChange={() => toggleRequest(request)}
                                    className="w-4 h-4 text-blue-600 rounded border-gray-300 focus:ring-blue-500"
                                />
                                <span>{request}</span>
                            </label>
                        ))}
                    </div>

                    <div className="flex gap-3 items-center">
                        <label className="text-sm font-medium text-gray-700 whitespace-nowrap">
                            Custom:
                        </label>
                        <input
                            className="flex-1 px-4 py-2 border border-gray-300 rounded-xl focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition duration-200"
                            placeholder="e.g. Extra luggage"
                            value={newRequest}
                            onChange={(e) => setNewRequest(e.target.value)}
                        />
                        <button
                            type="button"
                            className="bg-green-600 hover:bg-green-700 text-white px-5 py-2 rounded-xl transition duration-200"
                            onClick={() => {
                                if (newRequest.trim()) {
                                    setRequests([...requests, newRequest]);
                                    setNewRequest("");
                                }
                            }}
                        >
                            Add
                        </button>
                    </div>

                    {requests.length > 0 && (
                        <div className="flex flex-wrap gap-2 mt-2">
                            {requests.map((req, idx) => (
                                <span
                                    key={idx}
                                    className="inline-flex items-center bg-blue-100 text-blue-800 text-sm px-3 py-1 rounded-full"
                                >
                                    {req}
                                    <button
                                        type="button"
                                        className="ml-2 text-blue-600 hover:text-blue-800"
                                        onClick={() => setRequests(requests.filter((_, i) => i !== idx))}
                                    >
                                        ×
                                    </button>
                                </span>
                            ))}
                        </div>
                    )}
                </div>
            </section>

            <button
                type="submit"
                className="w-full py-3 px-6 bg-gradient-to-r from-blue-500 to-indigo-600 text-white font-semibold rounded-xl shadow-md hover:shadow-xl hover:scale-[1.02] transition-all duration-200"
            >
                Save Booking
            </button>
        </form>
    );
}

export default BookingForm;