import api from "./api";
import type { WeeklyReport } from "../types/weeklyReport";


export const getWeeklyReport = async (): Promise<WeeklyReport[]> => {

    const response = await api.get("/Report/weekly");

    return response.data;

};