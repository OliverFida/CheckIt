// Component imports
import React, {useContext, useEffect, useState} from 'react';
import {Button, Modal, Form} from 'react-bootstrap';
import { RoomsContext } from '../../contexts/RoomsContext';
import { ToastContext } from '../../contexts/ToastContext';
import { InfoToast, ErrorToast } from './Toasts';
// API imports
import RoomsAPI from '../../api/rooms';

export default function RoomModal(){
    const {roomsContext, setRoomsContext} = useContext(RoomsContext);
    const {toastContext, setToastContext} = useContext(ToastContext);
    const [state, setState] = useState(null);

    useEffect(() => {
        if(!roomsContext.uiControl.roomModal) return;
        if(roomsContext.rooms.selected === null) setState({number: "", name: "", active: true});
        if(roomsContext.rooms.selected !== null) setState(roomsContext.rooms.selected);
    }, [roomsContext.uiControl.roomModal]);

    const sendToast = async (toastElement) => {
        var temp = toastContext.toasts;

        temp.push(toastElement);

        setToastContext({...toastContext, toasts: temp});
    };

    const onCancel = () => {
        setRoomsContext({...roomsContext, uiControl:{...roomsContext.uiControl, roomModal: false}, rooms:{...roomsContext.rooms, selected: null}});
    };

    const onSubmit = () => {
        async function doAsync(){
            var response;
            if(roomsContext.uiControl.modalMode === "delete"){
                response = await RoomsAPI.deleteRoom(state.id);
                if(response.status === 200){
                    sendToast(<InfoToast>{response.data.message}</InfoToast>);
                }else{
                    sendToast(<ErrorToast>Raum konnte nicht gelöscht werden!</ErrorToast>);                    
                }
            }else if(roomsContext.uiControl.modalMode === "edit"){
                response = await RoomsAPI.editRoom(state.id, state.number, state.name);
                if(response.status === 200){
                    sendToast(<InfoToast>{response.data.message}</InfoToast>);                    
                }else{
                    sendToast(<ErrorToast>Raum konnte nicht geändert werden!</ErrorToast>);                    
                }
            }else{
                response = await RoomsAPI.addRoom(state.number, state.name);
                if(response.status === 200){
                    sendToast(<InfoToast>{response.data.message}</InfoToast>);                    
                }else{
                    sendToast(<ErrorToast>Raum konnte nicht erstellt werden!</ErrorToast>);                   
                    
                }
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
                        <Form.Control autoComplete='off' type="text" name="number" value={state?.number ? state.number : ""} onChange={onChange} />
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="roomName">
                        <Form.Label>Name</Form.Label>
                        <Form.Control autoComplete='off' type="text" name="name" value={state?.name ? state.name : ""} onChange={onChange} />
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