// Component imports
import React, {useEffect, useState, useContext} from 'react';
import {Nav, NavDropdown} from 'react-bootstrap';
import { HomePageContext } from '../../contexts/HomePageContext';
// API imports
import RoomsAPI from '../../api/rooms';

export default function AppNavBooking(){return(
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
        setElements(rooms.map(room => room.active ? <NavDropdown.Item key={`room_${room.number}`} onClick={() => {onRoomSelect(room.name, room.id)}}>{room.name} [{room.number}]</NavDropdown.Item> : null));
    }, [rooms]);
    
    useEffect(() => {
        if(rooms.length > 0) onRoomSelect(rooms[0].name, rooms[0].id);
    }, [elements]);

    const onRoomSelect = (name, id) => {
        if(hpContext.roomSelection.id === id) return;
        setHpContext({...hpContext, roomSelection: {...hpContext.roomSelection, name: name, id: id}});
    };

    return(
        <NavDropdown title={hpContext.roomSelection.name ? hpContext.roomSelection.name : "Raum"} align="end">
            {elements}
        </NavDropdown>
    );
}