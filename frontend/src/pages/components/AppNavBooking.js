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
        if(rooms === null || rooms === undefined || rooms.length <= 0) return;
        setElements(rooms.map(room => <NavDropdown.Item key={`room_${room.number}`} onClick={() => {onRoomSelect(room.number, room.name, room.id, room.active ? false : true)}}>{room.name} [{room.number}]</NavDropdown.Item>));
    }, [rooms]);
    
    // useEffect(() => {
    //     if(rooms.length > 0) onRoomSelect(rooms[0].number, rooms[0].name, rooms[0].id);
    // }, [elements]);

    const onRoomSelect = (number, name, id, inactive) => {
        if(hpContext.roomSelection.id === id) return;
        setHpContext({...hpContext, roomSelection: {...hpContext.roomSelection, number: number, name: name, id: id, inactive: inactive}});
    };

    return(
        <NavDropdown title="Raumauswahl" align="end">
            {elements}
        </NavDropdown>
    );
}