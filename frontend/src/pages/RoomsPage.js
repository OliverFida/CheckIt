// Component imports
import React, {useEffect, useState, useContext} from 'react';
import {Stack, Button, Table, Modal, Form} from 'react-bootstrap';
import AppNavBar from './components/AppNavBar';
import AppNavRooms from './components/AppNavRooms';
import RoomsContextProvider, {RoomsContext} from '../contexts/RoomsContext';
import RoomModal from './components/RoomModal';
// Style imports
import '../css/components/RoomsPage.css';
// API imports
const RoomsAPI = require('../api/rooms');

export default function RoomsPage(){
    return(
        <RoomsContextProvider>
            <Stack direction='vertical'>
                <AppNavBar>
                    <AppNavRooms />
                </AppNavBar>
                <RoomBody />
                <RoomModal />
            </Stack>
        </RoomsContextProvider>
    );
}

function RoomBody(){
    return(
        <Table className='rooms'>
            <thead>
                <tr>                    
                    <th>Raumnummer</th>
                    <th>Raumname</th>                        
                    <th></th>
                </tr>
            </thead>
            <tbody>                    
                <RoomRows />
            </tbody>
        </Table>
    );
}

function RoomRows(){
    const {roomsContext, setRoomsContext} = useContext(RoomsContext);
    const [rooms, setRooms] = useState(null);
    const [elements, setElements] = useState([]);
    
    useEffect(() => {
        if(!roomsContext?.rooms.reload) return;

        async function doAsync(){
            var tempRooms = await RoomsAPI.getRooms();
            await setRooms(tempRooms.data);
            setRoomsContext({...roomsContext, rooms:{...roomsContext.rooms, reload: false}});
        };
        doAsync();
    }, [roomsContext?.rooms.reload]);

    const onSelect = (room, mode) => {
        setRoomsContext({...roomsContext, uiControl:{...roomsContext.uiControl, roomModal: true, modalMode: mode}, rooms:{...roomsContext.rooms, selected: room}});
    };
    
    useEffect(() => {
        setElements(rooms?.map(room => room.active ? 
            <tr key={`room_${room.id}`}>
                <td key={`room_${room.number}`}>{room.number}</td>
                <td key={`room_${room.name}`}>{room.name}</td>
                <td>
                    <Button onClick={() => {onSelect(room, "edit")}} className="my-1 me-2">
                        Bearbeiten
                    </Button>
                    <Button onClick={() => {onSelect(room, "delete")}} variant="danger" className="my-1 me-2">
                        Löschen
                    </Button>
                </td> 
            </tr>
            : null));
    }, [rooms]);
   

    return(
        <>
            {elements}
        </>
    );
}

