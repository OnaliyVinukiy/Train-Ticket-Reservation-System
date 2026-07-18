import api from "./api";
import type { SpecialRequest } from "../types/specialRequest";


export const getRequests = async (): Promise<SpecialRequest[]> => {

    const response = await api.get("/SpecialRequest");

    return response.data;

};


export const createRequest = async (
    request: SpecialRequest
) => {

    const response = await api.post(
        "/SpecialRequest",
        request
    );

    return response.data;

};


export const deleteRequest = async (
    id: number
) => {

    await api.delete(
        `/SpecialRequest/${id}`
    );

};

export const updateRequest = async (
    id: number,
    request: SpecialRequest
) => {
    await api.put(
        `/SpecialRequest/${id}`,
        request
    );
};