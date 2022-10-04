// Component imports
import React, {useState, useEffect, useContext} from 'react';
import { Modal, Button, Form } from 'react-bootstrap';
import {HomePageContext} from '../../contexts/HomePageContext';

import '../../css/components/UserPage.css';

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
        <Modal show={state !== null} onHide={onCancel} centered>
            <Modal.Header>
                <Modal.Title>Buchung</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <p> 
                <Form>
                    <Form.Group className="mb-3" controlId="bookingDuration">
                        <Form.Label>Stunden {state?.duration}</Form.Label>
                        <Form.Select aria-label="stundenauswahl">
                            <option>Dauer auswählen</option>
                            <option value="1">One</option>
                            <option value="2">Two</option>
                            <option value="3">Three</option>
                        </Form.Select>
                    </Form.Group>
                    <Form.Group controlId="bookingNotes">
                        <Form.Label>Notizen</Form.Label>
                        <Form.Control as="textarea" rows={3} className="bookingNotes" />
                    </Form.Group>
                </Form><br />
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