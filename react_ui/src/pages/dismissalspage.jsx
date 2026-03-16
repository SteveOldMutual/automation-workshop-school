import React, { useState } from 'react';
import Table from '../components/table'; // Ensure this matches the export type
import { getStudents, removeStudent, updateStudent } from '../services/student_service';
import { addStudent } from '../services/student_service';
import FormComponent from '../components/form';
import { dismissStudent, getRollCall, markAbsent, markPresent, undoDismissStudent } from '../services/rollcall_service';
import { replace } from 'react-router-dom';
import './styles/dashboardStyles.css'
import Sidebar from '../components/sidebar';
import { getSchoolId } from '../services/auth_service';
import TitleBar from '../components/titlebar';
import 'bootstrap/dist/css/bootstrap.min.css';
import './styles/pageStyle.css'
import { Toast, ToastContainer } from 'react-bootstrap';


const Dismissal = () => {
  const [students, setStudents] = useState([

  ]);
  const [schoolId, setSchoolId] = useState("");
  const [rollCalls, setRollCalls] = useState([]);
   const [showToast, setShowToast] = useState(false);
    const [toastMessage, setToastMessage] = useState('');

  React.useEffect(() => {
    const fetchStudents = async () => {
      try {
        const token = localStorage.getItem('token');
        const fetchedSchoolId = (await getSchoolId(token)).schoolId;
        setSchoolId(fetchedSchoolId)
        await reloadData(fetchedSchoolId);

      } catch (error) {
        console.error('Error fetching students:', error);
      }
    };

    fetchStudents();
  }, []);

  const reloadData = async (schoolId) => {
  

    const data = await getStudents(schoolId);
    const rollData = await getRollCall(schoolId);
    setRollCalls(rollData);
    const presentStudents = rollData.rollcalls.filter(x => x.isPresent);
    const presentIds = presentStudents.map(x => x.studentId);
    const presentStdnts = data.filter(x => presentIds.includes(x.id));
    setStudents(presentStdnts);
  }

  const isDismissed = (row) => {
    if (rollCalls.rollcalls !== undefined && rollCalls.rollcalls.length > 0) {
      const rollCall = rollCalls.rollcalls.find(x => x.studentId === row.id);
      return rollCall.dismissed;
    }
  }

  const handleDismiss = async (row) => {
    await dismissStudent(row.id);
    await reloadData(schoolId);
    setToastMessage(`Student - ${row.firstName} ${row.lastName} - dismissed.`);
    setShowToast(true);
  }

  const handleUndo = async (row) => {
    await undoDismissStudent(row.id);
    await reloadData(schoolId);
    setToastMessage(`Student - ${row.firstName} ${row.lastName} - dismissal undone.`);
    setShowToast(true);
  }


  const [searchTerm, setSearchTerm] = useState("");
  const [currentPage, setCurrentPage] = useState(1);
  const itemsPerPage = 10;

  const handleSearch = (event) => {
    setSearchTerm(event.target.value.toLowerCase());
    setCurrentPage(1); // Reset to the first page on search
  };

  const filteredStudents = students.filter((student) =>
    Object.values(student).some((value) =>
      value.toString().toLowerCase().includes(searchTerm)
    )
  );

  const indexOfLastItem = currentPage * itemsPerPage;
  const indexOfFirstItem = indexOfLastItem - itemsPerPage;
  const currentStudents = filteredStudents.slice(indexOfFirstItem, indexOfLastItem);

  const totalPages = Math.ceil(filteredStudents.length / itemsPerPage);

  const handlePageChange = (pageNumber) => {
    setCurrentPage(pageNumber);
  };

  return (
    <div>
      <div className="dashboard-container"></div>
      <div
        className="dashboard-sidebar"
        style={{ position: "fixed", left: 0, top: 0, height: "100vh", width: "250px" }}
      >
        <Sidebar />
      </div>
      <div className="dashboard-canvas canvas-area" style={{ marginLeft: "250px", padding: "20px" }}>
        <TitleBar pageTitle="Dismissals"></TitleBar>
        <div style={{ marginBottom: "20px" }}>
          <input
            type="text"
            placeholder="Search..."
            className="form-control"
            value={searchTerm}
            onChange={handleSearch}
          />
        </div>
        {currentStudents && currentStudents.length > 0 ? (
          <div style={{ overflowX: "auto" }}>
            <table
              className="table table-striped table-bordered"
              style={{ width: "100%", fontSize: "1.1rem" }}
            >
              <thead className="thead-dark">
                <tr>
                  <th>FirstName</th>
                  <th>LastName</th>
                  <th>Grade</th>
                  <th>Actions</th>
                </tr>
              </thead>
              <tbody>
                {currentStudents.map((row, rowIndex) => (
                  <tr key={rowIndex}>
                    <td>{row.firstName}</td>
                    <td>{row.lastName}</td>
                    <td>{row.grade}</td>
                    <td>
                      {!isDismissed(row) && (
                        <button
                          className="btn btn-primary btn-sm"
                          onClick={async () => {
                            await handleDismiss(row);
                          }}
                        >
                          Dismiss
                        </button>
                      )}
                      {isDismissed(row) && (
                        <button
                          className="btn btn-secondary btn-sm"
                          onClick={async () => {
                            await handleUndo(row);
                          }}
                        >
                          Undo
                        </button>
                      )}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
            <div className="pagination">
              {Array.from({ length: totalPages }, (_, index) => index + 1).map((pageNumber) => (
                <button
                  key={pageNumber}
                  className={`btn btn-sm ${currentPage === pageNumber ? "btn-primary" : "btn-light"}`}
                  onClick={() => handlePageChange(pageNumber)}
                >
                  {pageNumber}
                </button>
              ))}
            </div>
          </div>
        ) : (
          <p>No students available to display.</p>
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



export default Dismissal;