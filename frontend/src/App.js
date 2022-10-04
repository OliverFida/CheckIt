// Component imports
import React from 'react';
import {BrowserRouter, Routes, Route, Navigate} from 'react-router-dom';
import {Container} from 'react-bootstrap';
// Style imports
import 'bootstrap/dist/css/bootstrap.min.css';
// Page imports
import LoginPage from './pages/LoginPage';
import HomePage from './pages/HomePage';
import UserPage from './pages/UserPage';
import RoomsPage from './pages/RoomsPage';
import NoPersmissionsPage from './pages/NoPermissions';
import UserEdit from './pages/UserEdit';

export default function App() {
  return (
    <div id="App">
      <Container>
        <BrowserRouter>
          <Routes>
            <Route exact path='/' element={<Navigate to='/login' />} />
            <Route path='/login' element={<LoginProtectedRoute reverse><LoginPage /></LoginProtectedRoute>} />
            <Route path='/rooms' element={<RoomsPage />}/>
          <Route path='/noperm' element={<NoPersmissionsPage />} />
            <Route path='/home' element={<LoginProtectedRoute><HomePage /></LoginProtectedRoute>} />
            <Route path='/user' element={<LoginProtectedRoute><UserPage /></LoginProtectedRoute>} />
            <Route path='/user-edit' element={<AdminProtectedRoute><UserEdit /></AdminProtectedRoute>} />
            <Route path='*' element={<Navigate to="/home" />} />
          </Routes>
        </BrowserRouter>
      </Container>
    </div>
  );
};

function LoginProtectedRoute({children, reverse = false}){
  if(!reverse && localStorage.getItem("loginToken") === null) return(<Navigate to="/login" />);
  if(reverse && localStorage.getItem("loginToken") !== null) return(<Navigate to="/home" />);
  return(<>{children}</>);
}

function AdminProtectedRoute({children}){
  var isAdmin = localStorage.getItem('loginAdmin');
  if(isAdmin === "true") return(<LoginProtectedRoute>{children}</LoginProtectedRoute>);
  return(<Navigate to="/noperm" />);
}