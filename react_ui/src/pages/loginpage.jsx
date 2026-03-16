import React from 'react';
import axios from 'axios';
import { Form, Button, Container, Row, Col, Card } from 'react-bootstrap';
import './styles/loginStyle.css'
import { authenticate, registerUser } from '../services/auth_service'
import FormComponent from '../components/form';
import { useState } from 'react';
import { Toast, ToastContainer } from 'react-bootstrap';

const handleAuth = async (username, password) => {

  let credentials = { username: username, password: password };
  const response = await authenticate(credentials);

  localStorage.setItem('token', response.data.token);
  window.location.href = '/';


  // alert('An error occurred during login. Please try again.');

};


const Login = () => {
  const [showToast, setShowToast] = useState(false);
  const [toastMessage, setToastMessage] = useState('');
  const [isModalOpen, setIsModalOpen] = useState(false);

  const handleLogin = async (loginData) => {
    try {
      await handleAuth(loginData.username, loginData.password);
    } catch (error) {
      if(error.message.includes('Request failed with status code 401')) {
        setToastMessage('Invalid username or password. Please try again.');
      }
      else{
        setToastMessage('An error occurred during login. Please try again.');
      }
     
      setShowToast(true);
    }
  };

  const handleRegistration = async (userData) => {
    try {
      await registerUser(userData);
      setToastMessage('User registered successfully. You can now log in.');
      setShowToast(true);
      setIsModalOpen(false);
    } catch (error) {
     
      if (error.response.data.message) {
        setToastMessage(error.response.data.message);
      }
      else {
        setToastMessage('An error occurred during login. Please try again.');
      }
      setShowToast(true);
    }

    
  }


  const fields = {
    username: {
      label: 'Username', type: 'text', validation: {
        required: true,
        maxLength: 50
      }
    },
    password: {
      label: 'Password', type: 'password', validation: {
        required: true,
        maxLength: 50,
      }
    }
  };

  const registrationFields = {
    username: {
      label: 'Username', type: 'text', validation: {
        required: true,
        maxLength: 50
      }
    },
    password: {
      label: 'Password', type: 'password', validation: {
        required: true,
        maxLength: 50,
        minLength: 12,
        isPassword: true
      }
    },
    email: {
      label: 'Email Address', type: 'text', validation: {
        required: true,
        isEmail: true,
        maxLength: 50,
      }
    }
  }

  return (
    <Container className="d-flex justify-content-center align-items-center" style={{ minHeight: '100vh' }}>
      <Card style={{ width: '100%', maxWidth: '400px', padding: '20px', boxShadow: '0 4px 8px rgba(0,0,0,0.1)' }}>
        <Card.Body>
          <h2 className="text-center mb-4">Login</h2>

          <FormComponent fields={fields} onSubmit={handleLogin} />
          <button onClick={() => setIsModalOpen(true)} >Register</button>
        </Card.Body>
      </Card>

      {isModalOpen && (
        <div className="modal" style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', position: 'fixed', top: 0, left: 0, width: '100%', height: '100%', backgroundColor: 'rgba(0, 0, 0, 0.5)' }}>
          <div className="modal-content" style={{ backgroundColor: 'white', padding: '20px', borderRadius: '8px', width: '400px', textAlign: 'center' }}>
            <h2>Register User</h2>
            <FormComponent fields={registrationFields} onSubmit={handleRegistration} />
            <button onClick={() => setIsModalOpen(false)} style={{ marginTop: '10px' }}>Close</button>
          </div>
        </div>
      )}

      <ToastContainer position="top-end" className="p-3">
        <Toast onClose={() => setShowToast(false)} show={showToast} delay={8000} autohide>
          <Toast.Body>{toastMessage}</Toast.Body>
        </Toast>
      </ToastContainer>
    </Container>
  );
};

export default Login;