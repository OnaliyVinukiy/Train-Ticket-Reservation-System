import React from "react";
import { Document, Page, View, Text, StyleSheet } from "@react-pdf/renderer";

interface Booking {
    id: string | number;
    route: {
        departureStation: string;
        destinationStation: string;
    };
    schedule: {
        travelDate: string;
    };
    seatNumber: string | number;
    ticketPrice: number;
    specialRequests: any[];
}

interface RouteFrequency {
    route: string;
    count: number;
}

interface ReportPDFProps {
    bookings: Booking[];
    routeFrequency: RouteFrequency[];
    fromDate: string;
    toDate: string;
}

const styles = StyleSheet.create({
    page: {
        padding: 35,
        fontSize: 10,
        color: "#1f2937",
        backgroundColor: "#ffffff",
    },
    header: {
        marginBottom: 25,
    },
    title: {
        fontSize: 24,
        fontWeight: "bold",
        color: "#111827",
        marginBottom: 6,
    },
    subtitle: {
        fontSize: 11,
        color: "#6b7280",
    },
    sectionTitle: {
        fontSize: 14,
        fontWeight: "bold",
        marginBottom: 10,
        color: "#111827",
    },
    summaryRow: {
        flexDirection: "row",
        justifyContent: "space-between",
        marginBottom: 25,
    },
    card: {
        width: "23%",
        padding: 12,
        borderRadius: 8,
        backgroundColor: "#f3f4f6",
    },
    cardTitle: {
        fontSize: 9,
        color: "#6b7280",
        marginBottom: 6,
    },
    cardValue: {
        fontSize: 18,
        fontWeight: "bold",
        color: "#111827",
    },
    chartBox: {
        marginBottom: 25,
    },
    chartRow: {
        flexDirection: "row",
        alignItems: "center",
        marginBottom: 8,
    },
    chartLabel: {
        width: "35%",
        fontSize: 9,
    },
    chartBackground: {
        width: "50%",
        height: 10,
        backgroundColor: "#e5e7eb",
        borderRadius: 5,
    },
    chartFill: {
        height: 10,
        backgroundColor: "#2563eb",
        borderRadius: 5,
    },
    chartValue: {
        marginLeft: 8,
        fontSize: 9,
        fontWeight: "bold",
    },
    tableHeader: {
        flexDirection: "row",
        backgroundColor: "#111827",
        color: "#ffffff",
        padding: 8,
        borderRadius: 4,
    },
    tableRow: {
        flexDirection: "row",
        padding: 8,
        borderBottomWidth: 1,
        borderBottomColor: "#e5e7eb",
    },
    colRoute: {
        width: "35%",
    },
    colDate: {
        width: "25%",
    },
    colSeat: {
        width: "15%",
    },
    colPrice: {
        width: "25%",
    },
    footer: {
        marginTop: 20,
        fontSize: 8,
        color: "#9ca3af",
        textAlign: "center",
    }
});

const formatDate = (date: string) => {
    return new Date(date).toLocaleDateString("en-GB", {
        day: "2-digit",
        month: "short",
        year: "numeric",
    });
};

const ReportPDF: React.FC<ReportPDFProps> = ({
    bookings,
    routeFrequency,
    fromDate,
    toDate,
}) => {
    const totalBookings = bookings.length;
    const totalCost = bookings.reduce((sum, b) => sum + b.ticketPrice, 0);
    const specialRequests = bookings.reduce((sum, b) => sum + b.specialRequests.length, 0);
    const sortedFrequency = [...routeFrequency].sort((a, b) => b.count - a.count);
    const popularRoute = sortedFrequency.length ? sortedFrequency[0].route : "No Data";
    const maxCount = Math.max(...routeFrequency.map(r => r.count), 1);

    return (
        <Document>
            <Page size="A4" style={styles.page}>
                <View style={styles.header}>
                    <Text style={styles.title}>Train Booking Analytics</Text>
                    <Text style={styles.subtitle}>Report Period: {fromDate} - {toDate}</Text>
                </View>
                <View style={styles.summaryRow}>
                    <View style={styles.card}>
                        <Text style={styles.cardTitle}>Total Bookings</Text>
                        <Text style={styles.cardValue}>{totalBookings}</Text>
                    </View>
                    <View style={styles.card}>
                        <Text style={styles.cardTitle}>Revenue</Text>
                        <Text style={styles.cardValue}>Rs. {totalCost}</Text>
                    </View>
                    <View style={styles.card}>
                        <Text style={styles.cardTitle}>Popular Route</Text>
                        <Text style={{ fontSize: 11, fontWeight: "bold" }}>{popularRoute}</Text>
                    </View>
                    <View style={styles.card}>
                        <Text style={styles.cardTitle}>Requests</Text>
                        <Text style={styles.cardValue}>{specialRequests}</Text>
                    </View>
                </View>
                {routeFrequency.length > 0 && (
                    <View style={styles.chartBox}>
                        <Text style={styles.sectionTitle}>Route Popularity</Text>
                        {routeFrequency.map(item => (
                            <View key={item.route} style={styles.chartRow}>
                                <Text style={styles.chartLabel}>{item.route}</Text>
                                <View style={styles.chartBackground}>
                                    <View style={{ ...styles.chartFill, width: `${(item.count / maxCount) * 100}%` }} />
                                </View>
                                <Text style={styles.chartValue}>{item.count}</Text>
                            </View>
                        ))}
                    </View>
                )}
                <Text style={styles.sectionTitle}>Booking Details</Text>
                <View style={styles.tableHeader}>
                    <Text style={styles.colRoute}>Route</Text>
                    <Text style={styles.colDate}>Date</Text>
                    <Text style={styles.colSeat}>Seat</Text>
                    <Text style={styles.colPrice}>Price</Text>
                </View>
                {bookings.map(b => (
                    <View key={b.id} style={styles.tableRow}>
                        <Text style={styles.colRoute}>
                            {b.route.departureStation} → {b.route.destinationStation}
                        </Text>
                        <Text style={styles.colDate}>{formatDate(b.schedule.travelDate)}</Text>
                        <Text style={styles.colSeat}>{b.seatNumber}</Text>
                        <Text style={styles.colPrice}>Rs. {b.ticketPrice}</Text>
                    </View>
                ))}
                <Text style={styles.footer}>Generated by Train Ticket Reservation System</Text>
            </Page>
        </Document>
    );
};

export default ReportPDF;