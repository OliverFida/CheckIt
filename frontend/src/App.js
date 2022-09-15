// Component imports
import {BrowserRouter, Routes, Route} from 'react-router-dom';
// Style imports
import 'antd/dist/antd.css';
// Page imports
import LoginPage from './pages/LoginPage';
import HomePage from './pages/HomePage';

export default function App() {
  return (
    <div id="App">
      <BrowserRouter>
        <Routes>
          <Route path='/' element={<LoginPage />} />
          <Route path='/home' element={<HomePage />} />
        </Routes>
      </BrowserRouter>
    </div>
  );
};