import React, { useState } from 'react';
import Table from '../components/table'; // Ensure this matches the export type
import { getStudents, removeStudent, updateStudent } from '../services/student_service';
import { addStudent } from '../services/student_service';
import FormComponent from '../components/form';
import { getRollCall, markAbsent, markPresent } from '../services/rollcall_service';
import { replace } from 'react-router-dom';
import DashboardTile from '../components/dasboard_tile';
import DashboardTilePercent from '../components/dasboard_tile_percent';
import "./styles/dashboardStyles.css"
import Sidebar from '../components/sidebar';
import TitleBar from '../components/titlebar';
import { getSchoolId } from '../services/auth_service';


const RollCall = () => {
  const [students, setStudents] = useState([

  ]);

  const [rollCalls, setRollCalls] = useState([]);

  React.useEffect(() => {
    const fetchStudents = async () => {
      try {
        const token = localStorage.getItem('token');
        const schoolId = await getSchoolId(token);
        await reloadData(schoolId.schoolId);

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

  const totalCheckedStudents = (present) => {
    if (rollCalls.rollcalls === undefined) return 0;
    return rollCalls.rollcalls.filter(x => x.isPresent === present).length;
  }

  const totalDismissedStudents = () => {
    if (rollCalls.rollcalls === undefined) return 0;
    return rollCalls.rollcalls.filter(x => x.dismissed).length;
  }

  const rollcallCount = () => {
    if (rollCalls.rollcalls === undefined) return 0;
    return rollCalls.rollcalls.length;
  }

  const studentCount = () => {
    if (students === undefined) return 0;
    return students.length;
  }

  return (
    <div className="dashboard-container">
      <div className="dashboard-sidebar" style={{ position: 'fixed', left: 0, top: 0, height: '100vh', width: '250px' }}>
        <Sidebar />
      </div>
      <div className="dashboard-canvas" style={{ marginLeft: '250px', padding: '20px' }}>
        <TitleBar pageTitle="School Dashboard"></TitleBar>
        <div >
          <div className="tile-set">
            <DashboardTile tileTitle="Unchecked Students" tileValue={studentCount() - rollcallCount()}></DashboardTile>
            <DashboardTile tileTitle="Checked Students" tileValue={rollcallCount()}></DashboardTile>
            <DashboardTilePercent tileTitle="Checkin Progress" tileValue={rollcallCount()} tileTotal={studentCount()}></DashboardTilePercent>
          </div>

          <div className="tile-set">
            <DashboardTile tileTitle="Present Students" tileValue={totalCheckedStudents(true)}></DashboardTile>
            <DashboardTile tileTitle="Absent Students" tileValue={totalCheckedStudents(false)}></DashboardTile>
            <DashboardTilePercent tileTitle="Dismissal Progress" tileValue={totalDismissedStudents()} tileTotal={totalCheckedStudents(true)}></DashboardTilePercent>
          </div>
        </div>
      </div>
    </div>
  );
};



export default RollCall;