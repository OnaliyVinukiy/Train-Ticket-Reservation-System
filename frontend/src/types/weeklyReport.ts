export interface WeeklyBooking {

    bookingId: number;
    route: string;
    seatNumber: string;
    ticketPrice: number;
    specialRequests: string[];

}

export interface WeeklyReport {

    date: string;
    day: string;
    bookings: WeeklyBooking[];

}

export interface WeeklySummary {

    totalBookings: number;
    totalTicketCost: number;
    totalSpecialRequests: number;
    mostPopularRoute: string;

}