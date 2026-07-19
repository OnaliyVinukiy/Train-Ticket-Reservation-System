import api from "./api";

export interface ChatRequest {
    message: string;
}

export interface ChatResponse {
    reply: string;
    availability: string;
    priceTrend: string;
    recommendation: string;
}

export const sendMessage = async (message: string) => {
    const response = await api.post<ChatResponse>("/chatbot", { message });
    return response.data;
};