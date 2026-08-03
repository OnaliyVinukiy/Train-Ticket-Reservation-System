import axios from "axios";

const reportApi = axios.create({
    baseURL: import.meta.env.VITE_REPORTING_API_URL
});

export default reportApi;