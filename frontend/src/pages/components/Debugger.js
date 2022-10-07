// Component imports
import React, {useContext} from 'react';
import { HomePageContext } from '../../contexts/HomePageContext';

export function HomePageDebugger(){
    const {hpContext, setHpContext} = useContext(HomePageContext);

    return(
        <div style={{position: 'fixed', bottom: 0, left: 0, padding: 5, backgroundColor: '#ee9999'}}>
            <h5><b>Debugger</b></h5><br />
            <b>RoomID:</b> {hpContext.roomSelection.id}<br />
            <b>RoomName:</b> {hpContext.roomSelection.name}<br />
            <b>Bookings:</b> {hpContext.bookings.bookings !== [] ? "true" : "false"}<br />
            <b>Booking Selected:</b> {hpContext.bookings.selected !== null ? "true" : "false"}<br />
            {hpContext.bookings.selected !== null ?
            <>
            <br />
            <b>ID:</b> {hpContext.bookings.selected.booking?.id}<br />
            <b>View Mode:</b> {hpContext.bookings.selected.viewMode ? "true" : "false"}<br />
            <b>Edit Mode:</b> {hpContext.bookings.selected.editMode ? "true" : "false"}<br />
            <b>Day:</b> {hpContext.bookings.selected.day}<br />
            <b>Lesson:</b> {hpContext.bookings.selected.lesson}<br />
            </>
            : null}
        </div>
    );
};