import axios from 'axios';
//const baseUrl = 'http://10.129.33.200:5002';
const baseUrl = process.env.REACT_APP_API_URL;


const authenticate = async (credentials) => {
    try {
        const response = await axios.post(`${baseUrl}/api/Authentication/login`, credentials);
        return response; // Assuming the API returns the authentication token or user details in the `data` field
    } catch (error) {
        console.error('Error Authenticating:', error);
        throw error;
    }
};

const registerUser = async (credentials) => {
    try {
        const response = await axios.post(`${baseUrl}/api/Authentication/register`, credentials);
        return response; // Assuming the API returns the authentication token or user details in the `data` field
    } catch (error) {
        console.error('Error Registering user:', error);
        throw error;
    }
};

const getSchoolId = async (token) => {
    try {
        const response = await axios.post(`${baseUrl}/api/UserManagement/getSchoolId`, {token: token
           
        });
        return response.data; // Assuming the API returns an array of students in the `data` field
    } catch (error) {
        console.error('Error getting school id:', error);
        throw error;
    }
}



export  { authenticate , getSchoolId, registerUser};