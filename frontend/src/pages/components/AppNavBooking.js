// Component imports
import React, {useEffect, useState, useContext} from 'react';
import { useNavigate } from 'react-router-dom';
import {Nav, Button, NavDropdown} from 'react-bootstrap';
import { HomePageContext } from '../../contexts/HomePageContext';
// API imports
import RoomsAPI from '../../api/rooms';
import LoginAPI from '../../api/login';

export default function AppNavBooking(){
    const navigate = useNavigate();
    
    const onLogout = () => {
        LoginAPI.logout();
        navigate("/login");
    };

    return(
        <Nav>
            <RoomDropDown />
            <Nav.Item>
                <Button onClick={onLogout}>Abmelden</Button>
            </Nav.Item>
        </Nav>
    );
};

function RoomDropDown(){
    const {hpContext, setHpContext} = useContext(HomePageContext);
    const [rooms, setRooms] = useState([]);
    const [elements, setElements] = useState([]);

    useEffect(() => {
        setRooms(RoomsAPI.getRooms());
    }, []);

    useEffect(() => {
        setElements(rooms.map(room => <NavDropdown.Item key={`room_${room.number}`} onClick={() => {onRoomSelect(room.name, room.id)}}>{room.name}</NavDropdown.Item>));
    }, [rooms]);

    const onRoomSelect = (name, id) => {
        setHpContext({...hpContext, roomName: name, roomId: id});
    };

    return(
        <NavDropdown title={hpContext.roomName ? hpContext.roomName : "Raum"} align="end">
            {elements}
        </NavDropdown>
    );
}