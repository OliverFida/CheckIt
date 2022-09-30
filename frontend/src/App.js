// Component imports
import React from 'react';
import {BrowserRouter, Routes, Route, Navigate} from 'react-router-dom';
// Style imports
import 'bootstrap/dist/css/bootstrap.min.css';
// Page imports
import LoginPage from './pages/LoginPage';
import HomePage from './pages/HomePage';
import UserPage from './pages/UserPage';
import UserEdit from './pages/UserEdit';

export default function App() {
  return (
    <div id="App">
      <BrowserRouter>
        <Routes>
          <Route exact path='/' element={<Navigate to='/login' />} />
          <Route path='/login' element={<LoginPage />} />
          <Route path='/home' element={<HomePage />} />
          <Route path='/user' element={<UserPage />} />
          <Route path='/user-edit' element={<UserEdit />} />
        </Routes>
      </BrowserRouter>
    </div>
  );
};