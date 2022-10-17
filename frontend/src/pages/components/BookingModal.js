// Component imports
import React, {useState, useEffect, useContext} from 'react';
import {Modal, Form, Button} from 'react-bootstrap';
import { HomePageContext } from '../../contexts/HomePageContext';
// Style imports
import '../../css/components/BookingModal.css';
// API imports
import BookingsAPI from '../../api/bookings';
// Helper imports
import BookingHelper from '../../helpers/bookingHelper';

export default function BookingModal(){
    const {hpContext, setHpContext} = useContext(HomePageContext);
    const [state, setState] = useState(null);

    // Bei Änderung der Visibility die aktuelle Selection in den Status laden
    useEffect(() => {
        setState(hpContext.bookings.selected);
    }, [hpContext.uiControl.bookingModal]);

    // Nach dem laden des Status
    useEffect(() => {
        if(state === null) return;

        var startDate, endDate, duration;

        if(state.mode === "new" && state.booking === null){
            startDate = BookingHelper.getNewStartDate(hpContext, state.day, {key: state.lesson});
            endDate = BookingHelper.getNewEndDate(startDate);
            
            var newBooking = {startTime: startDate.toJSON(), endTime: endDate.toJSON(), note: "", studentCount: 15, duration: 1};
            
            setState({...state, booking: newBooking});
        }
        
        if(state.mode === "edit" || state.mode === "view"){
            startDate = BookingHelper.getStartDateFromUtc(state.booking.startTime);
            endDate = BookingHelper.getEndDateFromUtc(state.booking.endTime);
            duration = BookingHelper.getDurationFromDates(startDate, endDate);

            if(endDate.toJSON() !== state.booking.endTime){
                setState({...state, booking: {...state.booking, endTime: endDate.toJSON(), duration: duration}});
            }
        }
    }, [state]);

    const onCancel = async () => {
        setHpContext({...hpContext, uiControl:{...hpContext.uiControl, bookingModal: false}, bookings:{...hpContext.bookings, selected: null}});
    };

    const onDelete = async () => {
        setHpContext({...hpContext, uiControl:{...hpContext.uiControl, bookingModal: false, bookingDeleteModal: true}});
    };

    const onSubmit = async () => {
        var startDate, endDate;
        
        startDate = BookingHelper.getStartDateFromUtc(state.booking.startTime);
        endDate = BookingHelper.getEndDateFromDuration(startDate, state.booking.duration);

        if(state.mode === "new"){
            await BookingsAPI.book(hpContext.roomSelection.id, startDate.toJSON(), endDate.toJSON(), state.booking.note, state.booking.studentCount);
        }
        
        if(state.mode === "edit"){
            await BookingsAPI.editBooking(state.booking.id, endDate.toJSON(), state.booking.note, state.booking.studentCount);
        }

        await setHpContext({...hpContext, uiControl:{...hpContext.uiControl, bookingModal: false}, bookings:{...hpContext.bookings, selected: null, reload: true}});
    };

    return(
        <Modal show={hpContext.uiControl.bookingModal} onHide={onCancel} centered>
            <Modal.Header closeButton>
                <Modal.Title>Buchung</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <RoomDisplay />
                <DurationPicker state={state} setState={setState} />
                <StudentsPicker state={state} setState={setState} />
                <NoteField state={state} setState={setState} />
            </Modal.Body>
            {hpContext.bookings.selected?.mode === "view" ? null : <Modal.Footer>
                <Button variant='ghost' onClick={onCancel}>{hpContext.bookings.selected?.mode === "view" ? "Schließen" : "Abbrechen"}</Button>
                {hpContext.bookings.selected?.mode === "edit" ? <Button variant='danger' onClick={onDelete}>Löschen</Button> : null}
                <Button variant='primary' onClick={onSubmit}>{hpContext.bookings.selected?.mode === "new" ? "Buchen" : "Speichern"}</Button>
            </Modal.Footer>}
        </Modal>
    );
};

function RoomDisplay(){
    const {hpContext, setHpContext} = useContext(HomePageContext);

    return(
        <Form.Group className="mb-3" controlId="bookingRoom">
            <Form.Label>Raum</Form.Label>
            <Form.Control type="text" defaultValue={`${hpContext.roomSelection.name} [${hpContext.roomSelection.number}]`} disabled={true} />
        </Form.Group>
    );
}

function DurationPicker({state, setState}){
    const {hpContext, setHpContext} = useContext(HomePageContext);

    const onChange = async (e) => {
        setState({...state, booking:{...state.booking, duration: e.target.value}});
    };

    return(
        <Form.Group className="mb-3" controlId="bookingDuration">
            <Form.Label>Dauer</Form.Label>
            <Form.Select value={state?.booking?.duration ? state.booking.duration : 1} onChange={onChange} disabled={hpContext.bookings.selected?.mode === "view" ? true : false}>
                <option value="1">1 Stunde</option>
                <option value="2">2 Stunden</option>
                <option value="3">3 Stunden</option>
                <option value="4">4 Stunden</option>
            </Form.Select>
        </Form.Group>
    );
}

function StudentsPicker({state, setState}){
    const {hpContext, setHpContext} = useContext(HomePageContext);

    const onChange = async (e) => {
        await setState({...state, booking:{...state.booking, studentCount: parseInt(e.target.value)}});
    };

    return(
        <Form.Group className="mb-3" controlId="bookingStudents">
            <Form.Label>Schüler</Form.Label>
            <Form.Control type="number" defaultValue={`${state?.booking?.studentCount}`} onChange={onChange} disabled={hpContext.bookings.selected?.mode === "view" ? true : false} />
        </Form.Group>
    );
}

function NoteField({state, setState}){
    const {hpContext, setHpContext} = useContext(HomePageContext);

    const onChange = async (e) => {
        await setState({...state, booking:{...state.booking, note: e.target.value}});
    };

    return(
        <Form.Group className="mb-3" controlId="bookingNotes">
            <Form.Label>Notizen</Form.Label>
            <Form.Control as="textarea" rows={3} className="bookingNotes" value={state?.booking?.note ? state.booking.note : ""} onChange={onChange} disabled={hpContext.bookings.selected?.mode === "view" ? true : false} />
        </Form.Group>
    );
}

export function BookingDeleteModal(){
    const {hpContext, setHpContext} = useContext(HomePageContext);

    const onAbort = () => {
        setHpContext({...hpContext, uiControl:{...hpContext.uiControl, bookingModal: true, bookingDeleteModal: false}});
    };

    const onSubmit = async () => {
        await BookingsAPI.del(hpContext.bookings.selected.booking.id);

        await setHpContext({...hpContext, uiControl:{...hpContext.uiControl, bookingDeleteModal: false}, bookings: {...hpContext.bookings, selected: null, reload: true}});
    };

    return(
        <Modal show={hpContext.uiControl.bookingDeleteModal} onHide={onAbort} centered>
            <Modal.Header closeButton>
                <Modal.Title>Buchung löschen</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <p>Buchung wirklich löschen?</p>
            </Modal.Body>
            <Modal.Footer>
                <Button variant='ghost' onClick={onAbort}>Abbrechen</Button>
                <Button variant='danger' onClick={onSubmit}>Löschen</Button>
            </Modal.Footer>
        </Modal>
    );
}