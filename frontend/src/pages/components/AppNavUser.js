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

    const clickRooms = () => {
        navigate("/rooms");
    }

    const onLogout = () => {
        LoginAPI.logout();
        navigate("/login");
    };

    return(
        <NavDropdown title="Vorname Nachname" align={'end'}>
            <NavDropdown.Item onClick={clickProfile}>Profil</NavDropdown.Item>
            {localStorage.getItem("loginAdmin") === "true" ? <NavDropdown.Item onClick={clickUserEdit}>Benutzerverwaltung</NavDropdown.Item> : null}
            {localStorage.getItem("loginAdmin") === "true" ? <NavDropdown.Item onClick={clickRooms}>Raumverwaltung</NavDropdown.Item> : null}
            <NavDropdown.Item onClick={onLogout} className="border-top">Abmelden</NavDropdown.Item>
        </NavDropdown>
    );
};