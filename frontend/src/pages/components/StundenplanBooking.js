// Component imports
import React, {useState, useEffect, useContext} from 'react';
import { Button } from 'react-bootstrap';
import { HomePageContext } from '../../contexts/HomePageContext';
// Helper imports
import BookingHelper from '../../helpers/bookingHelper';

export default function StundenplanBooking({day, lesson}){
    const {hpContext, setHpContext} = useContext(HomePageContext);
    const [state, setState] = useState({variant: 'light', disabled: false, onClick: null, text: "", user: null, booking: null});
    
    // Update when bookings or weekOffset change
    useEffect(() => {
        var startDate = BookingHelper.getNewStartDate(hpContext, day, lesson);
        var endDate = BookingHelper.getNewEndDate(startDate);

        var lessonOver = BookingHelper.checkLessonOver(endDate);
        if(hpContext.roomSelection.inactive) lessonOver = true;
        var booking = BookingHelper.findBooking(hpContext, startDate, endDate);

        if(booking){
            var isOwn = BookingHelper.checkIsOwn(booking);

            if(isOwn){
                setState({...state, variant: 'success', disabled: lessonOver, onClick: () => {onClick("edit", booking)}, text: "Gebucht", user: null});
            }else{
                setState({...state, variant: 'danger', disabled: lessonOver, onClick: () => {onClick("view", booking)}, text: "Gebucht von", user: `${booking.user.firstName} ${booking.user.lastname}`});
            }
        }else{
            setState({...state, variant: 'light', disabled: lessonOver, onClick: () => {onClick("new", null)}, text: "Buchen", user: null});
        }
    }, [hpContext.bookings.bookings, hpContext.weekSelection.offset]);

    const onClick = async (mode, booking) => {
        setHpContext({...hpContext, uiControl:{...hpContext.uiControl, bookingModal: true}, bookings:{...hpContext.bookings, selected:{day: day, lesson: lesson.key, booking: booking, mode: mode}}});
    };

    return(
        <td>
            <Button className='buchenBtn' variant={state.variant} disabled={state.disabled} onClick={state.onClick}>
                {state.text}<br />
                {state.user}
            </Button>
        </td>
    )
}