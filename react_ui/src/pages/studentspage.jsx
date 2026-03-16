import React, { useState } from 'react';
import Table from '../components/table'; // Ensure this matches the export type
import { getStudents, removeStudent, updateStudent } from '../services/student_service';
import { addStudent } from '../services/student_service';
import FormComponent from '../components/form';
import './styles/dashboardStyles.css'
import Sidebar from '../components/sidebar';
import { getSchoolId } from '../services/auth_service';
import TitleBar from '../components/titlebar';
import { Toast, ToastContainer } from 'react-bootstrap';

const Students = () => {
  const [students, setStudents] = useState([

  ]);
  const [showToast, setShowToast] = useState(false);
  const [toastMessage, setToastMessage] = useState('');
  const [schoolId, setSchoolId] = useState("");

  React.useEffect(() => {
    const fetchStudents = async () => {
      try {
        const token = localStorage.getItem('token');
        const fetchedSchoolId = (await getSchoolId(token)).schoolId;
        const data = await getStudents(fetchedSchoolId);
        setSchoolId(fetchedSchoolId);
        setStudents(data);
      } catch (error) {
        console.error('Error fetching students:', error);
      }
    };

    fetchStudents();
  }, []);

  const handleEdit = (row) => {
    setEditingStudent(row);
  };

  const handleDelete = async (id) => {
    try {
      await removeStudent(id);
      setStudents((prevStudents) => prevStudents.filter(student => student.id !== id));
    } catch (error) {
      console.error('Error deleting student:', error);
    }
  };

  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingStudent, setEditingStudent] = useState(null); // Define editingStudent state

  const handleAdd = () => {
    setIsModalOpen(true);
  };

  const handleCloseModal = () => {
    setIsModalOpen(false);
    setEditingStudent(null)
  };

  const handleAddStudent = async (studentData) => {

    const stdnt = { ...studentData, SchoolId: schoolId };
    if (editingStudent) {
      await updateStudent(studentData.id, studentData);
      setStudents((prevStudents) => prevStudents.filter(student => student.id !== studentData.id));
      setStudents((prevStudents) => [...prevStudents, studentData]);
      setToastMessage('Student details updated.');
      setShowToast(true);
    }
    else {
      await addStudent(stdnt);
      // Add the new student to the state
      setStudents((prevStudents) => [...prevStudents, stdnt]);
      setToastMessage('Student added successfully.');
      setShowToast(true);
    }
    handleCloseModal();

  }

  const fields = {
    firstName: {
      label: 'FirstName', type: 'text', validation: {
        required: true,
        maxLength: 50,
      }
    },
    lastName: {
      label: 'LastName', type: 'text', validation: {
        required: true,
        maxLength: 50,
      }
    },
    age: { label: 'Age', type: 'number',validation:{ 
      required: true,
      min: 6,
      max: 19
    } },
    grade: { label: 'Grade', type: `number`, validation:{ 
      required: true,
      min: 1,
      max: 12
    }  }
  };
  return (
    <div>
      <div className="dashboard-container">
        <div className="dashboard-sidebar" style={{ position: 'fixed', left: 0, top: 0, height: '100vh', width: '250px' }}>
          <Sidebar />
        </div>
        <div className="dashboard-canvas" style={{ marginLeft: '250px', padding: '20px' }}>
          <TitleBar pageTitle="Student Management"></TitleBar>
          <div>
            <Table
              data={students}
              actionButtons={[
                {
                  label: 'Edit',
                  onClick: (row) => {
                    handleEdit(row); // Set the student to be edited
                    setIsModalOpen(true);
                  },
                },
                {
                  label: 'Delete',
                  onClick: (row) => handleDelete(row.id), // Pass the delete handler to the Table component
                },
              ]}
              hideColumns={['id', 'schoolId']}
            />
            <div style={{ padding: '10px 0' }}>
              <button className="btn btn-primary" onClick={handleAdd}>Add</button>
            </div>

            {isModalOpen && (
              <div className="modal" style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', position: 'fixed', top: 0, left: 0, width: '100%', height: '100%', backgroundColor: 'rgba(0, 0, 0, 0.5)' }}>
                <div className="modal-content" style={{ backgroundColor: 'white', padding: '20px', borderRadius: '8px', width: '400px', textAlign: 'center' }}>
                  <h2>{editingStudent ? `Edit Student` : `Add Student`}</h2>
                  <FormComponent fields={fields} initialValues={editingStudent ? editingStudent : {}} onSubmit={handleAddStudent} />
                  <button onClick={handleCloseModal} style={{ marginTop: '10px' }}>Close</button>
                </div>
              </div>
            )}
          </div>
        </div>
      </div>
      <ToastContainer position="top-end" className="p-3">
        <Toast onClose={() => setShowToast(false)} show={showToast} delay={8000} autohide>
          <Toast.Body>{toastMessage}</Toast.Body>
        </Toast>
      </ToastContainer>
    </div>
  );
};



export default Students;