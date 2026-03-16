import axios from 'axios';
//const baseUrl = `http://10.129.33.200:5002`;
const baseUrl = process.env.REACT_APP_API_URL;
const getStudents = async (schoolId) => {
    try {
        const response = await axios.get(`${baseUrl}/api/student?schoolId=${schoolId}`);
        return response.data; // Assuming the API returns an array of students in the `data` field
    } catch (error) {
        console.error('Error fetching students:', error);
        throw error;
    }
};

const addStudent = async (student) => {
    try {
        const response = await axios.post(`${baseUrl}/api/student`, student);
        return response.data; // Assuming the API returns the added student in the `data` field
    } catch (error) {
        console.error('Error adding student:', error);
        throw error;
    }
};

const updateStudent = async (id, student) => {
    try {
        const response = await axios.put(`${baseUrl}/api/student/${id}`, student);
        return response.data; // Assuming the API returns the added student in the `data` field
    } catch (error) {
        console.error('Error updating student:', error);
        throw error;
    }
};


const removeStudent = async (id) => {
    try {
        const response = await axios.delete(`${baseUrl}/api/student/${id}`);
        return response.data; // Assuming the API returns the added student in the `data` field
    } catch (error) {
        console.error('Error removing student:', error);
        throw error;
    }
};


export { getStudents, addStudent , removeStudent, updateStudent};