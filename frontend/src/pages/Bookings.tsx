import { useEffect, useState } from "react";
import type { Booking } from "../types/booking";
import BookingForm from "../components/booking/BookingForm";
import BookingList from "../components/booking/BookingList";
import { getBookings, createBooking, updateBooking, deleteBooking, searchBookings } from "../services/bookingService";

function Bookings() {
  const [bookings, setBookings] = useState<Booking[]>([]);
  const [editingBooking, setEditingBooking] = useState<Booking | null>(null);
  const [travelDate, setTravelDate] = useState("");
  const [route, setRoute] = useState("");
  const [reference, setReference] = useState("");

  useEffect(() => {
    loadBookings();
  }, []);

  const loadBookings = async () => {
    const data = await getBookings();
    setBookings(data);
  };

  const handleSearch = async () => {
    const result = await searchBookings(travelDate || undefined, route || undefined, reference || undefined);
    setBookings(result);
  };

  const clearSearch = () => {
    setTravelDate("");
    setRoute("");
    setReference("");
    loadBookings();
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
        <div className="grid grid-cols-1 lg:grid-cols-5 gap-8">
          <div className="lg:col-span-3">
            <BookingForm onSubmit={handleSave} editingBooking={editingBooking} />
          </div>
          <div className="lg:col-span-2 space-y-6">
            <div className="bg-white rounded-2xl shadow-md p-5 space-y-4">
              <h2 className="text-xl font-bold">Search Bookings</h2>
              <input
                type="date"
                value={travelDate}
                onChange={(e) => setTravelDate(e.target.value)}
                className="w-full border rounded-xl p-3"
              />
              <input
                placeholder="Route"
                value={route}
                onChange={(e) => setRoute(e.target.value)}
                className="w-full border rounded-xl p-3"
              />
              <input
                placeholder="Booking Reference"
                value={reference}
                onChange={(e) => setReference(e.target.value)}
                className="w-full border rounded-xl p-3"
              />
              <div className="flex gap-3">
                <button onClick={handleSearch} className="flex-1 bg-blue-600 text-white rounded-xl py-3 hover:bg-blue-700">
                  Search
                </button>
                <button onClick={clearSearch} className="flex-1 bg-gray-500 text-white rounded-xl py-3 hover:bg-gray-600">
                  Clear
                </button>
              </div>
            </div>
            <BookingList bookings={bookings} onEdit={setEditingBooking} onDelete={handleDelete} />
          </div>
        </div>
      </div>
    </div>
  );
}

export default Bookings;