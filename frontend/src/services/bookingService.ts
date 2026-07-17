import api from "./api";
import type { Booking } from "../types/booking";


export const getBookings = async (): Promise<Booking[]> => {

    const response = await api.get("/Booking");

    return response.data;
};