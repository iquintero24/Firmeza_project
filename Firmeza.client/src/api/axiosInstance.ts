import axios from 'axios';

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

// create of instaces of axios

const api = axios.create({
    baseURL: API_BASE_URL,
    headers: {
        'Content-Type': 'application/json',
    },
});

// config interceptor for JWT 
api.interceptors.request.use((config) => {
    // try get token 
    const token = localStorage.getItem('jwt_token');
    
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    
    return config;
},(err) => {
     return Promise.reject(err);
    }
);

export default api;