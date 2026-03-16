import React, { useState } from 'react';
import Table from '../components/table'; // Ensure this matches the export type
import { getStudents, removeStudent, updateStudent } from '../services/student_service';
import { addStudent } from '../services/student_service';
import FormComponent from '../components/form';
import { getRollCall, markAbsent, markPresent } from '../services/rollcall_service';
import { replace } from 'react-router-dom';
import './styles/dashboardStyles.css'
import Sidebar from '../components/sidebar';
import { getSchoolId } from '../services/auth_service';
import TitleBar from '../components/titlebar';
import { Toast, ToastContainer } from 'react-bootstrap';

const RollCall = () => {
  const [students, setStudents] = useState([

  ]);

  const [rollCalls, setRollCalls] = useState([]);


  const [schoolId, setSchoolId] = useState("");
  const [showToast, setShowToast] = useState(false);
      const [toastMessage, setToastMessage] = useState('');

  React.useEffect(() => {
    const fetchStudents = async () => {
      try {
        const token = localStorage.getItem('token');
        const fetchedSchoolId = (await getSchoolId(token)).schoolId;
        setSchoolId(fetchedSchoolId);
        await reloadData(fetchedSchoolId);
      } catch (error) {
        console.error('Error fetching students:', error);
      }
    };

    fetchStudents();
  }, []);

  const reloadData = async (schoolId) => {
    const data = await getStudents(schoolId);
    setStudents(data);

    const rollData = await getRollCall(schoolId);
    setRollCalls(rollData);
  }

  const handleAbsent = async (row) => {
    let presence = isPresent(row);
 
    if (presence === undefined) { 
      try{
        presence = rollCalls.rollcalls.find(x => x.studentId === row.id).isPresent;
      }
      catch{}
      
    }
    if (presence === undefined || presence) {

      await markAbsent(row.id, schoolId);

      await reloadData(schoolId);
      setToastMessage(`Student - ${row.firstName} ${row.lastName} - marked as absent.`);
      setShowToast(true);
    }


  }

  const handlePresent = async (row) => {
    let presence = isPresent(row);
   
    console.log(presence);
    if (presence === undefined) {
      try {
        console.log(rollCalls.rollcalls);
        presence = rollCalls.rollcalls.find(x => x.studentId === row.id).isPresent;
      } catch { }

    }
    if (presence === undefined || !presence) {
     
      await markPresent(row.id, schoolId);
      setToastMessage(`Student - ${row.firstName} ${row.lastName} - marked as present.`);
      setShowToast(true);
    }
    await reloadData(schoolId);
  }

  const isPresent = (row) => {
    try {
      const stdnt = students.filter(x => x.id === row.id);
      if (stdnt !== undefined) {
        return stdnt.isPresent;
      }
    } catch {
      return undefined;
    }

  }

  const checkIsPresent = (row, present) => {
    if (rollCalls.rollcalls !== undefined && rollCalls.rollcalls.length > 0) {
      const rollCall = rollCalls.rollcalls.find(x => x.studentId === row.id);

      if (rollCall === undefined) {
        const _present = isPresent(row)
        if (_present === null || _present === undefined) return "btn-light";
        if (_present && present) { return "btn-success"; }
        else if (!_present && !present) { return "btn-danger" };
      }
      else {
        if (rollCall.isPresent && present) { return "btn-success"; }
        else if (!rollCall.isPresent && !present) { return "btn-danger" };
      }
    }

    return "btn-light";
  }



  const [searchTerm, setSearchTerm] = useState('');
  const [currentPage, setCurrentPage] = useState(1);
  const itemsPerPage = 10;

  const filteredStudents = students.filter((student) =>
    Object.values(student).some((value) =>
      value.toString().toLowerCase().includes(searchTerm.toLowerCase())
    )
  );

  const totalPages = Math.ceil(filteredStudents.length / itemsPerPage);
  const paginatedStudents = filteredStudents.slice(
    (currentPage - 1) * itemsPerPage,
    currentPage * itemsPerPage
  );

  const handleSearch = (event) => {
    setSearchTerm(event.target.value);
    setCurrentPage(1); // Reset to first page on new search
  };

  const handlePageChange = (page) => {
    setCurrentPage(page);
  };

  return (
    <div>
      <div className="dashboard-container"></div>
        <div className="dashboard-sidebar" style={{ position: 'fixed', left: 0, top: 0, height: '100vh', width: '250px' }}>
          <Sidebar />
        </div>
        <div className="dashboard-canvas" style={{ marginLeft: '250px', padding: '20px' }}>
          <TitleBar pageTitle="Rollcall"></TitleBar>
          <div className="mb-3">
            <input
              type="text"
              className="form-control"
              placeholder="Search..."
              value={searchTerm}
              onChange={handleSearch}
            />
          </div>
          {(!students || students.length === 0) ? (
            <div>No students available.</div>
          ) : (
            <div>
              <table className="table table-bordered table-hover">
                <thead className="thead-dark">
                  <tr>
                    <th>FirstName</th>
                    <th>LastNames</th>
                    <th>Grade</th>
                    <th>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {paginatedStudents.map((row, rowIndex) => (
                    <tr key={rowIndex}>
                      <td>{row.firstName}</td>
                      <td>{row.lastName}</td>
                      <td>{row.grade}</td>
                      <td>
                        <button
                          type="button"
                          className={`btn ${checkIsPresent(row, true)}`}
                          onClick={async () => {
                            await handlePresent(row);
                          }}
                        >
                          Present
                        </button>
                        <button
                          type="button"
                          className={`btn ${checkIsPresent(row, false)}`}
                          onClick={async () => {
                            await handleAbsent(row);
                          }}
                        >
                          Absent
                        </button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
              <div className="pagination">
                <button
                  className="btn btn-secondary"
                  disabled={currentPage === 1}
                  onClick={() => handlePageChange(currentPage - 1)}
                >
                  Previous
                </button>
                {Array.from({ length: totalPages }, (_, index) => (
                  <button
                    key={index}
                    className={`btn ${currentPage === index + 1 ? 'btn-primary' : 'btn-light'}`}
                    onClick={() => handlePageChange(index + 1)}
                  >
                    {index + 1}
                  </button>
                ))}
                <button
                  className="btn btn-secondary"
                  disabled={currentPage === totalPages}
                  onClick={() => handlePageChange(currentPage + 1)}
                >
                  Next
                </button>
              </div>
            </div>
          )}
        </div>
        <ToastContainer position="top-end" className="p-3">
                      <Toast onClose={() => setShowToast(false)} show={showToast} delay={8000} autohide>
                        <Toast.Body>{toastMessage}</Toast.Body>
                      </Toast>
                    </ToastContainer>
      </div>
  );
};



export default RollCall;