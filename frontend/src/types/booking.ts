import type { RecurrencePattern } from "./recurrencePattern";

export interface Route {
    departureStation: string;
    destinationStation: string;
}

export interface Schedule {
    travelDate: string;
    departureTime: string;
    arrivalTime: string;
}

export interface SpecialRequest {
    id?: number;
    description: string;
}

export const BOOKING_TYPES = {
    OneOff: "OneOff",
    Recurring: "Recurring"
} as const;

export type BookingType = typeof BOOKING_TYPES[keyof typeof BOOKING_TYPES];

export interface Booking {
    id: number;
    bookingReference: string;
    seatNumber: string;
    ticketPrice: number;
    bookingType: BookingType;
    recurrencePattern: RecurrencePattern;
    recurrenceEndDate: string;
    route: Route;
    schedule: Schedule;
    specialRequests: SpecialRequest[];
}