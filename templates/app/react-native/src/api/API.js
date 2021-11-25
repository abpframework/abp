import axios from 'axios';
import { getEnvVars } from '../../Environment';

const { apiUrl } = getEnvVars();

const axiosInstance = axios.create({
  baseURL: apiUrl,
  withCredentials: false
});

export default axiosInstance;
