// Component imports
import React from 'react';
import {NavDropdown} from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';

import LoginAPI from '../../api/login';

export default function AppNavBooking(){
    const navigate = useNavigate();
    
    const clickProfile = () => {
        navigate("/user");
    };

    const clickUserEdit = () => {
        navigate("/user-edit");
    };

    const onLogout = () => {
        LoginAPI.logout();
        navigate("/login");
    };

    return(
        <NavDropdown title="Vorname Nachname">
            <NavDropdown.Item onClick={clickProfile}>Profil</NavDropdown.Item>
            <NavDropdown.Item onClick={clickUserEdit}>Benutzerverwaltung</NavDropdown.Item>
            <NavDropdown.Item onClick={onLogout}>Abmelden</NavDropdown.Item>
        </NavDropdown>
    );
};