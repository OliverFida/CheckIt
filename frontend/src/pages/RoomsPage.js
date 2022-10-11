// Component imports
import React, {useEffect, useState, useContext} from 'react';
import {Stack, Button, Table, Modal, Form} from 'react-bootstrap';
import AppNavBar from './components/AppNavBar';
import RoomsContextProvider, {RoomsContext} from '../contexts/RoomsContext';

// Style imports
import '../css/components/RoomsPage.css';
// API imports
const RoomsAPI = require('../api/rooms');

export default function RoomsPage(){    

   

    return(
        <RoomsContextProvider>
            <Stack direction='vertical'>
                <AppNavBar>
                    <NewRoomButton />
                </AppNavBar>
                <RoomBody />
                <NewRoomModal />
                <EditRoomModal />
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
                <RoomRow />
            </tbody>
        </Table>
    );
}

function RoomRow(){
    const [rooms, setRooms] = useState(null);
    const [elements, setElements] = useState([]);
    const {roomsContext, setUpContext} = useContext(RoomsContext);

    const showEdit = () => {
        setUpContext({...roomsContext, showEditRoomModal: true});
    };

    useEffect(() => {
        async function doAsync(){
            var tempRooms = await RoomsAPI.getRooms();
            await setRooms(tempRooms.data);
        };
        doAsync();
    }, []);
    
    useEffect(() => {
        setElements(rooms?.map(room => room.active ? 
            <tr key={`room_${room.id}`}>
                <td key={`room_${room.number}`}>{room.number}</td>
                <td key={`room_${room.name}`}>{room.name}</td>
                <td>
                    <Button onClick={showEdit} className="my-1 me-2">
                        Raum bearbeiten
                    </Button>
                    <Button variant="danger" className="my-1 me-2">
                        Raum löschen
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

function EditRoomModal(){
    const {roomsContext, setUpContext} = useContext(RoomsContext);

    const onCancel = () => {
        setUpContext({...roomsContext, showEditRoomModal: false});
    }

    return(
        <Modal show={roomsContext.showEditRoomModal} onHide={onCancel} centered>
        <Modal.Header closeButton>
            <Modal.Title>Raum bearbeiten</Modal.Title>
        </Modal.Header>
        <Modal.Body>
            <Form>
                <Form.Group className="mb-3" controlId="editNumber">
                    <Form.Label>Neue Nummer</Form.Label>
                    <Form.Control type="text"/>
                </Form.Group>
                <Form.Group className="mb-3" controlId="editName">
                    <Form.Label>Neuer Name</Form.Label>
                    <Form.Control type="text"/>
                </Form.Group>                                      
            </Form> 
        </Modal.Body>
        <Modal.Footer>
            <Button variant="secondary" onClick={onCancel}>
                Abbrechen
            </Button>
            <Button variant="primary" onClick={onCancel}>
                Änderungen Speichern
            </Button>
        </Modal.Footer>
        </Modal>
    );
}

function NewRoomModal(){
    const {roomsContext, setUpContext} = useContext(RoomsContext);

    const onCancel = () => {
        setUpContext({...roomsContext, showNewRoomModal: false});
    }

    return(
        <Modal show={roomsContext.showNewRoomModal} onHide={onCancel} centered>
            <Modal.Header closeButton>
                <Modal.Title>Neuen Raum erstellen</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form>
                    <Form.Group className="mb-3" controlId="setNumber">
                        <Form.Label>Nummer</Form.Label>
                        <Form.Control type="text"/>
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="setName">
                        <Form.Label>Name</Form.Label>
                        <Form.Control type="text"/>
                    </Form.Group>                                      
                </Form> 
            </Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={onCancel}>
                    Abbrechen
                </Button>
                <Button variant="primary" onClick={onCancel}>
                    Änderungen Speichern
                </Button>
            </Modal.Footer>
        </Modal>
    );
}

function NewRoomButton(){
    const {roomsContext, setUpContext} = useContext(RoomsContext);

    const showNewRoom = () => {
        setUpContext({...roomsContext, showNewRoomModal: true});
    };

    return(
        <Button onClick={showNewRoom}>Neuen Raum erstellen</Button>
    );
}
 

