// Component imports
import React, {useEffect, useState, useContext} from 'react';
import { useNavigate } from 'react-router-dom';
import {Nav, NavDropdown} from 'react-bootstrap';
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
        </Nav>
    );
};

function RoomDropDown(){
    const {hpContext, setHpContext} = useContext(HomePageContext);
    const [rooms, setRooms] = useState([]);
    const [elements, setElements] = useState([]);

    useEffect(() => {
        async function doAsync(){
            var tempRooms = await RoomsAPI.getRooms();
            await setRooms(tempRooms.data);
        };
        doAsync();
    }, []);

    useEffect(() => {
        setElements(rooms.map(room => room.active ? <NavDropdown.Item key={`room_${room.number}`} onClick={() => {onRoomSelect(room.name, room.id)}}>{room.name}</NavDropdown.Item> : null));
    }, [rooms]);
    
    useEffect(() => {
        if(rooms.length > 0) onRoomSelect(rooms[0].name, rooms[0].id);
    }, [elements]);

    const onRoomSelect = (name, id) => {
        // OFDO: BUG: wiederholtes auswählen des selben Raumes zeigt keine Bookings mehr an
        setHpContext({...hpContext, roomName: name, roomId: id});
    };

    return(
        <NavDropdown title={hpContext.roomName ? hpContext.roomName : "Raum"} align="end">
            {elements}
        </NavDropdown>
    );
}