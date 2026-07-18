import api from "./api";


export const getWeeklyReport = async (startDate: string) => {

    const response = await api.get("/Report/weekly", {
        params: {
            startDate
        }
    });

    return response.data;

};

export const getWeeklySummary = async (
    startDate: string
) => {

    const response = await api.get(
        "/report/summary",
        {
            params: {
                startDate
            }
        }
    );

    return response.data;

};

export const exportCSV = async (fromDate: string, toDate: string) => {
    const response = await api.get("/report/export", {
        params: { fromDate, toDate },
        responseType: "blob"
    });

    const url = window.URL.createObjectURL(new Blob([response.data]));
    const link = document.createElement("a");
    link.href = url;
    link.download = "booking-report.csv";
    document.body.appendChild(link);
    link.click();
    link.remove();
};

export const getRouteFrequency = async (
    fromDate: string,
    toDate: string
) => {

    const response = await api.get(
        "/report/route-frequency",
        {
            params: {
                fromDate,
                toDate
            }
        }
    );

    return response.data;

};