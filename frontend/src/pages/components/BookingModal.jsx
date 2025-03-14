// Component imports
import React, {useState, useEffect, useContext} from 'react';
import {Modal, Form, Button} from 'react-bootstrap';
import { HomePageContext } from '../../contexts/HomePageContext';
import { ToastContext } from '../../contexts/ToastContext';
import {InfoToast, ErrorToast} from './Toasts';
// Style imports
import '../../css/components/BookingModal.css';
// API imports
import BookingsAPI from '../../api/bookings';
import AdminAPI from '../../api/admin';
// Helper imports
import BookingHelper from '../../helpers/bookingHelper';

export default function BookingModal(){
    const {hpContext, setHpContext} = useContext(HomePageContext);
    const {toastContext, setToastContext} = useContext(ToastContext);
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
            
            var newBooking = {startTime: startDate.toJSON(), endTime: endDate.toJSON(), note: "", studentCount: 15, duration: 1, user:{username: localStorage.getItem('loginUsername')}};
            
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

    const sendToast = async (toastElement) => {
        var temp = toastContext.toasts;

        temp.push(toastElement);

        setToastContext({...toastContext, toasts: temp});
    };

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

        var response;
        if(state.mode === "new"){
            response = await BookingsAPI.book(hpContext.roomSelection.id, startDate.toJSON(), endDate.toJSON(), state.booking.note, state.booking.studentCount, state.booking.user.username);
            
            if(response.status === 201){
                sendToast(<InfoToast>{response.data.message}</InfoToast>);
            }else{
                sendToast(<ErrorToast>{response.response.data.message}</ErrorToast>);
            }
        }
        
        if(state.mode === "edit"){
            response = await BookingsAPI.editBooking(state.booking.id, endDate.toJSON(), state.booking.note, state.booking.studentCount);

            if(response.status === 200){
                sendToast(<InfoToast>{response.data.message}</InfoToast>);
            }else{
                sendToast(<ErrorToast>{response.response.data.message}</ErrorToast>);
            }
        }

        await setHpContext({...hpContext, uiControl:{...hpContext.uiControl, bookingModal: false}, bookings:{...hpContext.bookings, selected: null, reload: true}});
    };

    const onAsAdmin = async () => {
        setHpContext({...hpContext, bookings: {...hpContext.bookings, selected: {...hpContext.bookings.selected, mode: "edit"}}});
        setState({...state, mode: "edit"});
    }

    return(
        <Modal show={hpContext.uiControl.bookingModal} onHide={onCancel} centered>
            <Modal.Header closeButton>
                <Modal.Title>Buchung</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <RoomDisplay />
                {localStorage.getItem('loginAdmin') === "true" && hpContext.bookings.selected?.mode === "new" ? <UserPicker state={state} setState={setState} /> : null}
                <DurationPicker state={state} setState={setState} />
                <StudentsPicker state={state} setState={setState} />
                <NoteField state={state} setState={setState} />
            </Modal.Body>
            {hpContext.bookings.selected?.mode === "view" ? null : <Modal.Footer>
                <Button variant='ghost' onClick={onCancel}>{hpContext.bookings.selected?.mode === "view" ? "Schließen" : "Abbrechen"}</Button>
                {hpContext.bookings.selected?.mode === "edit" ? <Button variant='danger' onClick={onDelete}>Löschen</Button> : null}
                <Button variant='primary' onClick={onSubmit}>{hpContext.bookings.selected?.mode === "new" ? "Buchen" : "Speichern"}</Button>
            </Modal.Footer>}
            {hpContext.bookings.selected?.mode === "view" && localStorage.getItem('loginAdmin') === "true" ? <Modal.Footer>
                <Button variant='primary' onClick={onAsAdmin}>Als Admin bearbeiten</Button>
            </Modal.Footer> : null}
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

function UserPicker({state, setState}){
    const {hpContext, setHpContext} = useContext(HomePageContext);
    const [elements, setElements] = useState([]);

    useEffect(() => {
        async function doAsync(){
            var response = await AdminAPI.getUsers();
            var users = Array.from(response.data).filter(user => user.active === true);
            
            var temp = [];
            users.forEach(user => {
                temp.push(<option key={`option-${user.username}`} value={user.username}>{user.firstName} {user.lastname}</option>);
            });
            setElements(temp);
        }   
        doAsync();
    }, []);

    const onChange = async (e) => {
        setState({...state, booking:{...state.booking, user:{...state.booking.user, username: e.target.value}}});
    };

    return(
        <Form.Group className="mb-3" controlId="bookingUser">
            <Form.Label>Benutzer</Form.Label>
            <Form.Select onChange={onChange} disabled={hpContext.bookings.selected?.mode === "new" ? false : true}>
                {elements}
            </Form.Select>
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
            <Form.Control type="number" value={`${state?.booking?.studentCount}`} onChange={onChange} disabled={hpContext.bookings.selected?.mode === "view" ? true : false} />
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
    const {toastContext, setToastContext} = useContext(ToastContext);

    const sendToast = async (toastElement) => {
        var temp = toastContext.toasts;

        temp.push(toastElement);

        setToastContext({...toastContext, toasts: temp});
    }

    const onAbort = () => {
        setHpContext({...hpContext, uiControl:{...hpContext.uiControl, bookingModal: true, bookingDeleteModal: false}});
    };

    const onSubmit = async () => {
        var response = await BookingsAPI.del(hpContext.bookings.selected.booking.id);
        if(response.status === 200){
            sendToast(<InfoToast>{response.data.message}</InfoToast>);
        }else{
            sendToast(<ErrorToast>Buchung konnte nicht gelöscht werden!</ErrorToast>);
        }

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