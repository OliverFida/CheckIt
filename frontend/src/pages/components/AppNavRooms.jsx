// Component imports
import React, {useContext} from 'react';
import {Nav, Button} from 'react-bootstrap';
import { RoomsContext } from '../../contexts/RoomsContext';

export default function AppNavRooms(){
    const {roomsContext, setRoomsContext} = useContext(RoomsContext);

    const onCreate = () => {
        setRoomsContext({...roomsContext, uiControl:{...roomsContext.uiControl, roomModal: true, modalMode: "new"}, rooms:{...roomsContext.rooms, selected: null}});
    };

    return(
        <Nav>
            <Button onClick={onCreate}>Raum erstellen</Button>
        </Nav>
    );
};