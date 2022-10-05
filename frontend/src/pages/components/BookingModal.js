// Component imports
import React, {useState, useEffect, useContext} from 'react';
import { Modal, Button, Form } from 'react-bootstrap';
import {HomePageContext} from '../../contexts/HomePageContext';
import moment from 'moment';
// API imports
import BookingsAPI from '../../api/bookings';
import timesMap from '../../api/timesMap.json';

import '../../css/components/UserPage.css';

export default function BookingModal(){
    const {hpContext, setHpContext} = useContext(HomePageContext);
    const [state, setState] = useState(null);

    useEffect(() => {
        if(hpContext.selectedBooking !== null){
            setState(hpContext.selectedBooking);
        }else{
            setState(null);
        }
    }, [hpContext.selectedBooking]);

    useEffect(() => {
        console.log(state); 
        if(state === null) return;
        if(state.booking === null){
            setState({...state, booking: {}});
        }
    }, [state]);

    const onCancel = () => {
        setHpContext({...hpContext, selectedBooking: null});
    };

    const onDelete = () => {
        onCancel();
    };
    
    const onSubmit = () => {
        async function doAsync(){
            var targetDateStart = moment().weekday(1).add(hpContext.weekOffset, 'weeks').add(state.day - 1, 'days');
            var lesson = timesMap.find(target => target.key === state.lesson);
            var lessonHourStart = lesson.start.substring(0, 2);
            var lessonMinuteStart = lesson.start.substring(3, 5);
            targetDateStart.set('hour', lessonHourStart);
            targetDateStart.set('minute', lessonMinuteStart);
            targetDateStart.set('second', 0).set('millisecond', 0);
            
            var targetDateEnd = moment(targetDateStart).add(50, 'minutes');

            await BookingsAPI.book(hpContext.roomId, targetDateStart.toJSON(), targetDateEnd.toJSON(), state.booking.note);

            onCancel();
        }
        doAsync();
    };

    return(
        <Modal show={state !== null} onHide={onCancel} centered>
            <Modal.Header closeButton>
                <Modal.Title>Buchung</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form>
                    <HoursPicker state={state} setState={setState} />
                    <NoteField state={state} setState={setState} />
                </Form>
                <p>
                    Day: {state?.day} <br />
                    Lesson: {state?.lesson} <br />
                    EditMode: {state?.editMode ? "true" : "false"}
                </p>
            </Modal.Body>
            <Modal.Footer>
                <Button variant='ghost' onClick={onCancel}>Abbrechen</Button>
                {state?.editMode ? <Button variant='danger' onClick={onDelete}>Löschen</Button> : null}
                <Button variant='primary' onClick={onSubmit}>{state?.editMode ? "Speichern" : "Buchen"}</Button>
            </Modal.Footer>
        </Modal>
    );
};

function HoursPicker({state, setState}){
    return null; //TODO
    return(
        <Form.Group className="mb-3" controlId="bookingDuration">
            <Form.Label>Stunden</Form.Label>
            <Form.Select>
                <option value="1">1</option>
                <option value="2">2</option>
                <option value="3">3</option>
                <option value="4">4</option>
            </Form.Select>
        </Form.Group>
    );
}

function NoteField({state, setState}){
    const onChange = (e) => {
        setState({...state, booking: {...state.booking, note: e.target.value}});
    };

    return(
        <Form.Group controlId="bookingNotes">
            <Form.Label>Notizen</Form.Label>
            <Form.Control as="textarea" rows={3} className="bookingNotes" value={state?.booking?.note} onChange={onChange} />
        </Form.Group>
    );
}