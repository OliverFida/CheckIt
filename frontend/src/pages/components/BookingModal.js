// Component imports
import React, {useState, useEffect, useContext} from 'react';
import { Modal, Button, Form } from 'react-bootstrap';
import {HomePageContext} from '../../contexts/HomePageContext';
import moment from 'moment';
// Style imports
import '../../css/components/UserPage.css';
// API imports
import timesMap from '../../api/timesMap.json';
const BookingsAPI = require('../../api/bookings');

export default function BookingModal(){
    const {hpContext, setHpContext} = useContext(HomePageContext);
    const [state, setState] = useState(null);

    useEffect(() => {
        setState(hpContext.bookings.selected);
    }, [hpContext.bookings.selected]);

    useEffect(() => {
        if(state === null) return;
        var targetDateStart, targetDateEnd;

        if(state.booking === null || state.booking === undefined){
            targetDateStart = moment().weekday(1).add(hpContext.weekSelection.offset, 'weeks').add(state.day - 1, 'days');
            var lesson = timesMap.find(target => target.key === state.lesson);
            var lessonHourStart = lesson.start.substring(0, 2);
            var lessonMinuteStart = lesson.start.substring(3, 5);
            targetDateStart.set('hour', lessonHourStart);
            targetDateStart.set('minute', lessonMinuteStart);
            targetDateStart.set('second', 0).set('millisecond', 0);

            targetDateEnd = moment(targetDateStart).add(50, 'minutes');

            setState({...state, booking: {startTime: targetDateStart.toJSON(), endTime: targetDateEnd.toJSON(), note: ""}});
        }

        if(state.booking !== null && state.booking !== undefined){
            targetDateStart = moment.utc(state.booking.startTime).local();
            targetDateEnd = moment.utc(state.booking.endTime).local();
            if(targetDateEnd.get('seconds') !== 0) targetDateEnd.add(1, 'second');

            var startLesson = timesMap.find(target => target.start === targetDateStart.format('HH:mm'));
            var endLesson = timesMap.find(target => target.end === targetDateEnd.format('HH:mm'));
            var duration = endLesson.key - startLesson.key + 1;

            if(duration === state.duration) return;
            setState({...state, duration: duration});
        }
    }, [state]);

    const onCancel = () => {
        setHpContext({...hpContext, bookings: {...hpContext.bookings, selected: null}});
    };

    const onDelete = () => {
        async function doAsync(){
            await BookingsAPI.del(state.booking.id);

            await setHpContext({...hpContext, bookings: {...hpContext.bookings, selected: null, reload: true}});
        }
        doAsync();
    };
    
    const onSubmit = () => {
        async function doAsync(){
            console.log(state.booking);

            if(!state.editMode){                
                await BookingsAPI.book(hpContext.roomSelection.id, state.booking.startTime, state.booking.endTime, state.booking.note);
    
                await setHpContext({...hpContext, bookings: {...hpContext.bookings, selected: null, reload: true}});
            }else{
                var endTime = moment.utc(state.booking.endTime)
                if(endTime.get('seconds') !== 0) endTime.add(1, 'second');

                await BookingsAPI.editBooking(state.booking.id, endTime.toJSON(), state.booking.note);

                await setHpContext({...hpContext, bookings: {...hpContext.bookings, selected: null, reload: true}});
            }
        }
        doAsync();
    };

    return(
        <Modal show={state !== null} onHide={onCancel} centered>
            <Modal.Header closeButton>
                <Modal.Title>Buchung</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form onSubmit={e => {e.preventDefault()}}>
                    {/* {<HoursPicker parentState={state} setParentState={setState} /> */}
                    <NoteField state={state} setState={setState} />
                </Form>
            </Modal.Body>
            {!state?.viewMode ?
            <Modal.Footer>
                <Button variant='ghost' onClick={onCancel}>Abbrechen</Button>
                {state?.editMode ? <Button variant='danger' onClick={onDelete}>Löschen</Button> : null}
                <Button variant='primary' onClick={onSubmit}>{state?.editMode ? "Speichern" : "Buchen"}</Button>
            </Modal.Footer>
            : null}
        </Modal>
    );
};

function HoursPicker({parentState, setParentState}){
    const onChange = (e) => {
        // TODO: Edit Mode correct default Value & selection not working
        var hours = e.target.value;
        var targetLesson = timesMap.find(target => target.key === parentState.lesson + parseInt(hours) - 1);
        var lessonHourEnd = targetLesson.end.substring(0, 2);
        var lessonMinuteEnd = targetLesson.end.substring(3, 5);
        
        var endTime = moment.utc(parentState.booking.startTime).local().set('hours', lessonHourEnd).set('minutes', lessonMinuteEnd);

        setParentState({...parentState, duration: hours, booking:{...parentState.booking, endTime: endTime.toJSON()}});
    };

    return(
        <Form.Group className="mb-3" controlId="bookingDuration">
            <Form.Label>Stunden</Form.Label>
            <Form.Select defaultValue={parentState?.duration ? parentState.duration : 1} onSelect={onChange} disabled={parentState?.viewMode}>
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
            <Form.Control autoComplete='off' as="textarea" rows={3} className="bookingNotes" value={state?.booking?.note} onChange={onChange} disabled={state?.viewMode} />
        </Form.Group>
    );
}