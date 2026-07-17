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
    id: number;
    description: string;
}


export type BookingType = "OneOff" | "Recurring";


export interface Booking {

    id: number;

    bookingReference: string;

    seatNumber: string;

    ticketPrice: number;

    bookingType: BookingType;

    route: Route;

    schedule: Schedule;

    specialRequests: SpecialRequest[];
}