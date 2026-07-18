import { useEffect, useState } from "react";
import type { Booking } from "../types/booking";
import BookingForm from "../components/booking/BookingForm";
import BookingList from "../components/booking/BookingList";
import { getBookings, createBooking, updateBooking, deleteBooking, searchBookings } from "../services/bookingService";

function Bookings() {
    const [bookings, setBookings] = useState<Booking[]>([]);
    const [editingBooking, setEditingBooking] = useState<Booking | null>(null);
    const [search, setSearch] = useState("");

    useEffect(() => {
        loadBookings();
    }, []);

    const loadBookings = async () => {
        const data = await getBookings();
        setBookings(data);
    };

    const handleSearch = async () => {
        const result = await searchBookings(undefined, search, undefined);
        setBookings(result);
    };

    const handleSave = async (booking: Booking) => {
        if (editingBooking) {
            await updateBooking(booking.id, booking);
        } else {
            await createBooking(booking);
        }
        setEditingBooking(null);
        loadBookings();
    };

    const handleDelete = async (id: number) => {
        await deleteBooking(id);
        loadBookings();
    };

    return (
        <div className="min-h-screen bg-gray-100 p-8">
            <div className="max-w-7xl mx-auto">
                {/* Two-column layout: form (left) and list (right) */}
                <div className="grid grid-cols-1 lg:grid-cols-5 gap-8">
                    {/* Left column: BookingForm */}
                    <div className="lg:col-span-3">
                        <BookingForm onSubmit={handleSave} editingBooking={editingBooking} />
                    </div>

                    {/* Right column: Search + BookingList */}
                    <div className="lg:col-span-2 space-y-6">
                        {/* Search bar – stays near the list */}
                        <div className="flex gap-3">
                            <input
                                className="border rounded-xl p-3 flex-1 shadow-sm focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none"
                                placeholder="Search by route (e.g. London → Paris)..."
                                value={search}
                                onChange={(e) => setSearch(e.target.value)}
                            />
                            <button
                                onClick={handleSearch}
                                className="bg-blue-600 hover:bg-blue-700 text-white px-6 rounded-xl shadow-sm transition duration-200"
                            >
                                Search
                            </button>
                        </div>

                        <BookingList bookings={bookings} onEdit={setEditingBooking} onDelete={handleDelete} />
                    </div>
                </div>
            </div>
        </div>
    );
}

export default Bookings;