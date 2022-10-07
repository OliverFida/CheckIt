// Component imports
import React, {useState, useEffect, useContext} from 'react';
import moment from 'moment';
import { Button } from 'react-bootstrap';
import { HomePageContext } from '../../contexts/HomePageContext';

export default function StundenplanBooking({day, lesson}){
    const {hpContext, setHpContext} = useContext(HomePageContext);
    const [state, setState] = useState({variant: 'light', disabled: false, onClick: null, text: "", user: null, tooltip: "", booking: null});

    useEffect(() => {
        var targetHourStart = lesson.start.substring(0, 2);
        var targetMinuteStart = lesson.start.substring(3, 5);
        var targetDateStart = moment().weekday(1).add(hpContext.weekOffset, 'weeks').add(day - 1, 'days');
        targetDateStart.set('hours', targetHourStart).set('minutes', targetMinuteStart).set('seconds', 1).set('milliseconds', 0);
        var targetHourEnd = lesson.end.substring(0, 2);
        var targetMinuteEnd = lesson.end.substring(3, 5);
        var targetDateEnd = moment(targetDateStart);
        targetDateEnd.set('hours', targetHourEnd).set('minutes', targetMinuteEnd).set('seconds', 0).set('milliseconds', 0);
    
        var timeOver = false;
        if(targetDateStart.isBefore(moment())) timeOver = true;
        
        var foundBooking = hpContext.bookings.find(booking => {
            if(targetDateStart.add(1, 'second').isBetween(moment.utc(booking.startTime), moment.utc(booking.endTime).add(1, 'second'))
            && targetDateEnd.subtract(1, 'second').isBetween(moment.utc(booking.startTime), moment.utc(booking.endTime).add(1, 'second'))){
                return true;
            }
            return false;
        });
        
        var loginUsername = localStorage.getItem('loginUsername');
        if(!foundBooking){
            setState({...state, variant: 'light', disabled: timeOver, onClick: () => {onClick(false, false)}, text: "Buchen", user: null, tooltip: ""});
        }else if(loginUsername !== foundBooking.user.username){
            setState({...state, variant: 'danger', disabled: timeOver, onClick: ()=> {onClick(false, true)}, text: "Gebucht von", user: `${foundBooking.user.firstName} ${foundBooking.user.lastname}`, tooltip: foundBooking.note});
        }else{
            setState({...state, variant: 'success', disabled: timeOver, onClick: () => {onClick(true, false)}, text: "Gebucht von", user: `${foundBooking.user.firstName} ${foundBooking.user.lastname}`, tooltip: foundBooking.note, booking: foundBooking});
        }
    }, [hpContext.bookings, hpContext.weekOffset]);

    const onClick = (editMode, viewMode) => {
        setHpContext({...hpContext, selectedBooking: {day: day, lesson: lesson.key, booking: state.booking, editMode: editMode, viewMode: viewMode}});
    }

    return(
        <td>
            <Button className="buchenBtn" variant={state.variant} disabled={state.disabled} onClick={state.onClick}>
                {state.text}<br />{state.user}                   
            </Button>                
        </td>
    );
}