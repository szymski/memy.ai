import axios from "axios";

interface ApiConfig {
  baseUrl: string;
  authToken?: string;
}

export const apiConfig: ApiConfig = {
  baseUrl: "http://localhost:5104",
  authToken: undefined
};

export const api = axios.create({
  baseURL: apiConfig.baseUrl,
  responseType: "json",
  headers: {
    "Content-Type": "application/json",
  },
  transformRequest: [
    (data, headers) => {
      if (apiConfig.authToken) {
        headers.setAuthorization(`Bearer ${apiConfig.authToken}`);
      }
    },
  ],
});
