// Component imports
import React from 'react';
import {Nav, Button} from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';

export default function AppNavBooking(){
    const navigate = useNavigate();
    
    const onButtonPress = () => {
        navigate("/user-edit");
    };

    return(
        <Nav>
            <Nav.Item>
                <Button onClick={onButtonPress}>Benutzerverwaltung</Button>
            </Nav.Item>
        </Nav>
    );
};