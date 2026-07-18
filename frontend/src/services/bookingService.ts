import api from "./api";
import type { Booking } from "../types/booking";

export const getBookings = async () => {
    const response = await api.get<Booking[]>("/booking");
    return response.data;
};

export const createBooking = async (booking: Booking) => {
    const response = await api.post("/booking", booking);
    return response.data;
};

export const updateBooking = async (id: number, booking: Booking) => {
    await api.put(`/booking/${id}`, booking);
};

export const deleteBooking = async (id: number) => {
    await api.delete(`/booking/${id}`);
};

export const searchBookings = async (
    date?: string,
    route?: string,
    reference?: string
) => {

    const response = await api.get(
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