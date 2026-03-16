import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Login from './pages/loginpage';
import Students from './pages/studentspage'
import ProtectedRoute from './components/protected_route';
import RollCall from './pages/rollcallpage';
import Dismissal from './pages/dismissalspage';
import 'bootstrap/dist/css/bootstrap.min.css';
import Dashboard from './pages/dashboardpage';



const App = () => {
  return (
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route element={<ProtectedRoute />}>
          <Route path="/" element={<Dashboard />} />
          {/* Add more protected routes as needed */}
        </Route>
        <Route element={<ProtectedRoute />}>
          <Route path="/students" element={<Students />} />
          {/* Add more protected routes as needed */}
        </Route>
        <Route element={<ProtectedRoute />}>
          <Route path="/rollcall" element={<RollCall />} />
          {/* Add more protected routes as needed */}
        </Route>
        <Route element={<ProtectedRoute />}>
          <Route path="/dismissals" element={<Dismissal />} />
          {/* Add more protected routes as needed */}
        </Route>
       
      </Routes>
  );
};

export default App;