// Component imports
import React, {useContext} from 'react';
import {NavDropdown} from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';
import LoginAPI from '../../api/login';
import {ToastContext} from '../../contexts/ToastContext';

export default function AppNavUser(){
    const {toastContext, setToastContext} = useContext(ToastContext);
    const navigate = useNavigate();
    
    const clickProfile = () => {
        navigate("/user");
    };

    const onLogout = () => {
        setToastContext({...toastContext, toasts: []});
        LoginAPI.logout();
        navigate("/login");
    };

    return(
        <NavDropdown title={`${localStorage.getItem("loginFirstName")} ${localStorage.getItem("loginLastName")}`} align={'end'}>
            <NavDropdown.Item onClick={clickProfile}>Profil</NavDropdown.Item>
            {localStorage.getItem("loginAdmin") === "true" ? <AppNavUserAdmin /> : null}
            <NavDropdown.Divider />
            <NavDropdown.Item onClick={onLogout}>Abmelden</NavDropdown.Item>
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
        <NavDropdown.Divider />
        <NavDropdown.Item onClick={clickUserEdit}>Benutzerverwaltung</NavDropdown.Item>
        <NavDropdown.Item onClick={clickRooms}>Raumverwaltung</NavDropdown.Item>
        </>
    );
}