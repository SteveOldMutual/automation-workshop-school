import axios from 'axios';
import { DateTime } from 'luxon';
//const baseUrl = `http://10.129.33.200:5002`;
const baseUrl = process.env.REACT_APP_API_URL;


 const getRollCall = async (schoolId) => {
    try {
        const response = await axios.get(`${baseUrl}/api/RollCall?dateTime=${DateTime.now().toFormat("MM-dd-yyyy")}&schoolId=${schoolId}`);
        return response.data; // Assuming the API returns an array of students in the `data` field
    } catch (error) {
        console.error('Error retrieving rollcall:', error);
        throw error;
    }
};


 const getStudentRollCall = async (id, schoolId) => {
    try {
        const response = await axios.get(`${baseUrl}/api/RollCall/${id}?schoolId=${schoolId}`);
        return response.data; // Assuming the API returns an array of students in the `data` field
    } catch (error) {
        console.error('Error marking student absent:', error);
        throw error;
    }
};

const markAbsent = async (id, schoolId) => {
    try {
        const response = await axios.get(`${baseUrl}/api/RollCall/MarkAbsent/${id}?schoolId=${schoolId}`);
        return response.data; // Assuming the API returns an array of students in the `data` field
    } catch (error) {
        console.error('Error marking student absent:', error);
        throw error;
    }
};

const markPresent = async (id, schoolId) => {
    try {
        const response = await axios.get(`${baseUrl}/api/RollCall/MarkPresent/${id}?schoolId=${schoolId}`);
        return response.data; // Assuming the API returns an array of students in the `data` field
    } catch (error) {
        console.error('Error marking student absent:', error);
        throw error;
    }
};

const dismissStudent = async (id) => {
    try {
        const response = await axios.get(`${baseUrl}/api/RollCall/Dismiss/${id}`);
        return response.data; // Assuming the API returns an array of students in the `data` field
    } catch (error) {
        console.error('Error dismissing student:', error);
        throw error;
    }
};

const undoDismissStudent = async (id) => {
    try {
        const response = await axios.get(`${baseUrl}/api/RollCall/UndoDismiss/${id}`);
        return response.data; // Assuming the API returns an array of students in the `data` field
    } catch (error) {
        console.error('Error undoing student dismissal:', error);
        throw error;
    }
};


export { getRollCall , markPresent, markAbsent, dismissStudent, undoDismissStudent};