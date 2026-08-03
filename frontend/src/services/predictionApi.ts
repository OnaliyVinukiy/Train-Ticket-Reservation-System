import axios from "axios";

const predictionApi = axios.create({
    baseURL: import.meta.env.VITE_PREDICTION_API_URL
});

export default predictionApi;