import reportApi from "./reportApi";


export const getWeeklyReport = async (
    startDate: string
) => {
    const response = await reportApi.get(
        "/report/weekly",
        {
            params: { startDate }
        }
    );

    return response.data;
};


export const getBookingReport = async (
    fromDate: string,
    toDate: string,
    route?: string,
    bookingType?: string
) => {

    const response = await reportApi.get(
        "/report/bookings",
        {
            params:
            {
                fromDate,
                toDate,
                route,
                bookingType
            }
        }
    );

    return response.data;
};


export const getRouteFrequency = async (
    fromDate: string,
    toDate: string
) => {

    const response = await reportApi.get(
        "/report/route-frequency",
        {
            params:
            {
                fromDate,
                toDate
            }
        }
    );

    return response.data;
};


export const getWeeklySummary = async (
    startDate: string
) => {

    const response = await reportApi.get(
        "/report/summary",
        {
            params:
            {
                startDate
            }
        }
    );

    return response.data;
};


export const startExport = async () => {

    const response =
        await reportApi.post(
            "/report/export/start"
        );


    return response.data;
};


export const getExportStatus = async (
    jobId: string
) => {

    const response =
        await reportApi.get(
            `/report/export/status/${jobId}`
        );


    return response.data;
};


export const downloadExport = async (
    jobId: string
) => {

    const response = await reportApi.get(
        `/report/export/download/${jobId}`,
        {
            responseType: "blob"
        }
    );


    const blob = new Blob(
        [response.data],
        {
            type: "text/csv"
        }
    );


    const url =
        window.URL.createObjectURL(blob);


    const link =
        document.createElement("a");


    link.href = url;

    link.setAttribute(
        "download",
        "booking-report.csv"
    );


    document.body.appendChild(link);

    link.click();


    document.body.removeChild(link);


    setTimeout(() => {
        window.URL.revokeObjectURL(url);
    }, 100);

};