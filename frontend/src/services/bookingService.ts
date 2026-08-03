import bookingApi from "./bookingApi";
import type { Booking } from "../types/booking";

export const getBookings = async () => {
    const response = await bookingApi.get<Booking[]>("/booking");
    return response.data;
};

export const createBooking = async (booking: Booking) => {
    const response = await bookingApi.post("/booking", booking);
    return response.data;
};

export const updateBooking = async (id: number, booking: Booking) => {
    await bookingApi.put(`/booking/${id}`, booking);
};

export const deleteBooking = async (id: number) => {
    await bookingApi.delete(`/booking/${id}`);
};

export const searchBookings = async (
    date?: string,
    route?: string,
    reference?: string
) => {

    const response = await bookingApi.get(
        "/booking/search",
        {
            params: {
                date,
                route,
                reference
            }
        }
    );


    return response.data;

};