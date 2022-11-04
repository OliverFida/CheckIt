// Component imports
import React, {useContext} from 'react';
import { HomePageContext } from '../../contexts/HomePageContext';

export function HomePageDebugger(){
    const {hpContext, setHpContext} = useContext(HomePageContext);

    return(
        <div style={{position: 'fixed', bottom: 0, left: 0, padding: 5, backgroundColor: '#eebbbb99'}}>
            <h5><b>Debugger</b></h5><br />
            <b>RoomID:</b> {hpContext.roomSelection.id}<br />
            <b>RoomName:</b> {hpContext.roomSelection.name}<br />
            <b>Bookings:</b> {hpContext.bookings.bookings.length > 0 ? "true" : "false"}<br />
            <b>Booking Selected:</b> {hpContext.uiControl.bookingModal ? "true" : "false"}<br />
            {hpContext.bookings.selected !== null ?
            <>
            <br />
            <b>Booking:</b> {hpContext.bookings.selected.booking ? "true" : "false"}<br />
            <b>ID:</b> {hpContext.bookings.selected.booking?.id}<br />
            <b>Mode:</b> {hpContext.bookings.selected?.mode}<br />
            <b>Day:</b> {hpContext.bookings.selected.day}<br />
            <b>Lesson:</b> {hpContext.bookings.selected.lesson}<br />
            </>
            : null}
        </div>
    );
};