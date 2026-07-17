import api from "./api";
import type { Schedule } from "../types/schedule";


export const getSchedules = async (): Promise<Schedule[]> => {

    const response = await api.get("/Schedule");

    return response.data;

};


export const createSchedule = async (
    schedule: Schedule
) => {

    const response = await api.post(
        "/Schedule",
        schedule
    );

    return response.data;

};


export const updateSchedule = async (
    schedule: Schedule
) => {

    await api.put(
        `/Schedule/${schedule.id}`,
        schedule
    );

};


export const deleteSchedule = async (
    id: number
) => {

    await api.delete(
        `/Schedule/${id}`
    );

};