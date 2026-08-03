import predictionApi from "./predictionApi";

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
    const response = await predictionApi.post<ChatResponse>("/chatbot", { message });
    return response.data;
};