// Component imports
import React from 'react';
import {NavDropdown} from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';

import LoginAPI from '../../api/login';

export default function AppNavUser(){
    const navigate = useNavigate();
    
    const clickProfile = () => {
        navigate("/user");
    };

    const onLogout = () => {
        LoginAPI.logout();
        navigate("/login");
    };

    return(
        <NavDropdown title={`${localStorage.getItem("loginFirstName")} ${localStorage.getItem("loginLastName")}`} align={'end'}>
            <NavDropdown.Item onClick={clickProfile}>Profil</NavDropdown.Item>
            {localStorage.getItem("loginAdmin") === "true" ? <AppNavUserAdmin /> : null}
            <NavDropdown.Item onClick={onLogout} className="border-top">Abmelden</NavDropdown.Item>
        </NavDropdown>
    );
};

function AppNavUserAdmin(){
    const navigate = useNavigate();
    
    const clickUserEdit = () => {
        navigate("/user-edit");
    };

    const clickRooms = () => {
        navigate("/rooms");
    }

    return(
        <>
        <NavDropdown.Item onClick={clickUserEdit}>Benutzerverwaltung</NavDropdown.Item>
        <NavDropdown.Item onClick={clickRooms}>Raumverwaltung</NavDropdown.Item>
        </>
    );
}