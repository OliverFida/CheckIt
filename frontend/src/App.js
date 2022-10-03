// Component imports
import React from 'react';
import {BrowserRouter, Routes, Route, Navigate} from 'react-router-dom';
// Style imports
import 'bootstrap/dist/css/bootstrap.min.css';
// Page imports
import LoginPage from './pages/LoginPage';
import HomePage from './pages/HomePage';
import UserPage from './pages/UserPage';
import NoPersmissionsPage from './pages/NoPermissions';
import UserEdit from './pages/UserEdit';

export default function App() {
  return (
    <div id="App">
      <BrowserRouter>
        <Routes>
          <Route exact path='/' element={<Navigate to='/login' />} />
          <Route path='/login' element={<LoginPage />} />
          <Route path='/noperm' element={<NoPersmissionsPage />} />
          <Route path='/home' element={<LoginProtectedRoute><HomePage /></LoginProtectedRoute>} />
          <Route path='/user' element={<LoginProtectedRoute><UserPage /></LoginProtectedRoute>} />
          <Route path='/user-edit' element={<AdminProtectedRoute><UserEdit /></AdminProtectedRoute>} />
          <Route path='*' element={<Navigate to="/home" />} />
        </Routes>
      </BrowserRouter>
    </div>
  );
};

function LoginProtectedRoute({children}){
  if(localStorage.getItem("loginToken") !== null) return(<>{children}</>);
  return(<Navigate to="/login" />);
}

function AdminProtectedRoute({children}){
  var isAdmin = true;
  if(isAdmin) return(<LoginProtectedRoute>{children}</LoginProtectedRoute>);
  return(<Navigate to="/noperm" />);
}