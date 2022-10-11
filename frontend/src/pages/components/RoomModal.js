// Component imports
import React, {useContext, useEffect, useState} from 'react';
import {Button, Modal, Form} from 'react-bootstrap';
import { RoomsContext } from '../../contexts/RoomsContext';
// API imports
const RoomsAPI = require('../../api/rooms');

export default function RoomModal(){
    const {roomsContext, setRoomsContext} = useContext(RoomsContext);
    const [state, setState] = useState(null);

    useEffect(() => {
        if(!roomsContext.uiControl.roomModal) return;
        if(roomsContext.rooms.selected === null) setState({number: "", name: "", active: true});
        if(roomsContext.rooms.selected !== null) setState(roomsContext.rooms.selected);
    }, [roomsContext.uiControl.roomModal]);

    const onCancel = () => {
        setRoomsContext({...roomsContext, uiControl:{...roomsContext.uiControl, roomModal: false}, rooms:{...roomsContext.rooms, selected: null}});
    };

    const onSubmit = () => {
        async function doAsync(){
            if(roomsContext.uiControl.modalMode === "delete"){
                await RoomsAPI.deleteRoom(state.id);
            }else if(roomsContext.uiControl.modalMode === "edit"){
                await RoomsAPI.editRoom(state.id, state.number, state.name);
            }else{
                await RoomsAPI.addRoom(state.number, state.name);
            }
            setRoomsContext({...roomsContext, uiControl:{...roomsContext.uiControl, roomModal: false}, rooms: {...roomsContext.rooms, selected: null, reload: true}});
        }
        doAsync();
    };

    const onChange = (e) => {
        setState({...state, [e.target.name]: e.target.value});
    };

    return(
        <Modal show={roomsContext.uiControl.roomModal} onHide={onCancel} centered>
            <Modal.Header closeButton>
                <Modal.Title>Raum {roomsContext.uiControl.modalMode === "new" ? "erstellen" : roomsContext.uiControl.modalMode === "delete" ? "löschen" : "bearbeiten"}</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                {roomsContext.uiControl.modalMode !== "delete" ? 
                <Form onSubmit={e => {e.preventDefault()}}>
                    <Form.Group className="mb-3" controlId="roomNumber">
                        <Form.Label>Nummer</Form.Label>
                        <Form.Control type="text" name="number" value={state?.number ? state.number : ""} onChange={onChange} />
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="roomName">
                        <Form.Label>Name</Form.Label>
                        <Form.Control type="text" name="name" value={state?.name ? state.name : ""} onChange={onChange} />
                    </Form.Group> 
                </Form>
                :
                <p>Raum wirklich löschen?</p>
                }
            </Modal.Body>
            <Modal.Footer>
                <Button variant='ghost' onClick={onCancel}>Abbrechen</Button>
                <Button variant={roomsContext.uiControl.modalMode === "delete" ? "danger" : "primary"} onClick={onSubmit}>{
                    roomsContext.uiControl.modalMode === "new" ? "Erstellen" :
                    roomsContext.uiControl.modalMode === "delete" ? "Löschen" :
                    "Speichern"
                }</Button>
            </Modal.Footer>
        </Modal>
    );
};