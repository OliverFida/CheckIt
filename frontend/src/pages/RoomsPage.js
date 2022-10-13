// Component imports
import React, {useEffect, useState, useContext} from 'react';
import {Stack, Button, Table, Row, Col, Card, ButtonGroup} from 'react-bootstrap';
import AppNavBar from './components/AppNavBar';
import AppNavRooms from './components/AppNavRooms';
import RoomsContextProvider, {RoomsContext} from '../contexts/RoomsContext';
import RoomModal from './components/RoomModal';
// Style imports
import '../css/components/RoomsPage.css';
// API imports
import RoomsAPI from '../api/rooms';

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
       <Row className="justify-content-md-center mt-5">
            <Col xl={8}>
                <Card className="px-2">
                    <Table className='rooms'>
                        <thead>
                            <tr>    
                                <th>Buchbar</th>                
                                <th>Raumnummer</th>
                                <th>Raumname</th>                        
                                <th></th>
                            </tr>
                        </thead>
                        <tbody>                    
                            <RoomRows />
                        </tbody>
                    </Table>
                </Card>
            </Col>
       </Row>
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

    useEffect(() => {
        setElements(rooms?.map(room => 
            <tr key={`room_${room.id}`}>
                <td>{room.active ? "buchbar" : "nicht buchbar"}</td>
                <td key={`room_${room.number}`}>{room.number}</td>
                <td key={`room_${room.name}`}>{room.name}</td>
                <td>
                    <ButtonGroup>
                        <Button onClick={() => {onSelect(room, "edit")}} className="my-1 me-1" size="sm">
                            Bearbeiten
                        </Button>
                        <Button onClick={() => {onActivate(room, room.active ? false : true)}} variant={room.active ? "secondary" : "success"} className="my-1 me-1" size="sm">
                            {room.active ? "Deaktivieren" : "Aktivieren"}
                        </Button>
                        <Button onClick={() => {onSelect(room, "delete")}} variant="danger" className="my-1 me-1" size="sm">
                            Löschen
                        </Button>
                    </ButtonGroup>
                </td> 
            </tr>
        ));
    }, [rooms]);
    
    const onSelect = (room, mode) => {
        setRoomsContext({...roomsContext, uiControl:{...roomsContext.uiControl, roomModal: true, modalMode: mode}, rooms:{...roomsContext.rooms, selected: room}});
    };

    const onActivate = async (room, state) => {
        await RoomsAPI.setActive(room.id, state);
        setRoomsContext({...roomsContext, rooms:{...roomsContext.rooms, reload: true}});
    };
    
   

    return(
        <>
            {elements}
        </>
    );
}

