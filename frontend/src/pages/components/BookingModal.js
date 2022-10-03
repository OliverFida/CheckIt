// Component imports
import React, {useState, useEffect, useContext} from 'react';
import { Modal, Button } from 'react-bootstrap';
import {HomePageContext} from '../../contexts/HomePageContext';

export default function BookingModal(){
    const {hpContext, setHpContext} = useContext(HomePageContext);
    const [state, setState] = useState(null);

    useEffect(() => {
        if(hpContext.selectedBooking !== null){
            console.log(`Selected booking day ${hpContext.selectedBooking.day} lesson ${hpContext.selectedBooking.lesson} edit: ${hpContext.selectedBooking.editMode}`);
            setState({day: hpContext.selectedBooking.day, lesson: hpContext.selectedBooking.lesson, editMode: hpContext.selectedBooking.editMode});
        }else{
            setState(null);
        }
    }, [hpContext.selectedBooking]);

    useEffect(() => {

    }, [state]);

    const onCancel = () => {
        setHpContext({...hpContext, selectedBooking: null});
    };

    const onDelete = () => {
        onCancel();
    };
    
    const onSubmit = () => {
        onCancel();
    };

    return(
        <Modal show={state !== null} onHide={onCancel}>
            <Modal.Header>
                <Modal.Title>Buchung</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <p>Stunden: {state?.duration} <br />
                Day: {state?.day} <br />
                Lesson: {state?.lesson} <br />
                EditMode: {state?.editMode ? "true" : "false"} <br /></p>
            </Modal.Body>
            <Modal.Footer>
                <Button variant='ghost' onClick={onCancel}>Abbrechen</Button>
                {state?.editMode ? <Button variant='danger' onClick={onDelete}>Löschen</Button> : null}
                <Button variant='primary' onClick={onSubmit}>{state?.editMode ? "Speichern" : "Buchen"}</Button>
            </Modal.Footer>
        </Modal>
    );
};