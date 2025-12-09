import axios from "axios";

const URL = import.meta.env.VITE_API_BASE_URL || "http://localhost:5058/api";
console.log("API URL: " + URL)
const api = axios.create({
  baseURL: URL,
});

export default api;
