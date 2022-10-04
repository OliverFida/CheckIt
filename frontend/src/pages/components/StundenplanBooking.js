// Component imports
import React, {useState, useEffect, useContext} from 'react';
import moment from 'moment';
import { Button } from 'react-bootstrap';
import { HomePageContext } from '../../contexts/HomePageContext';

export default function StundenplanBooking({day, lesson}){
    const username = "dageorg";
    const {hpContext, setHpContext} = useContext(HomePageContext);
    const [state, setState] = useState({variant: 'light', disabled: false, onClick: null, text: "", user: null});

    useEffect(() => {
        var targetHour = lesson.start.substring(0, 2);
        var targetMinute = lesson.start.substring(3, 5);
        var targetDate = moment().weekday(1).add(hpContext.weekOffset, 'weeks').add(day - 1, 'days');
        targetDate.set('hours', targetHour).set('minutes', targetMinute).set('seconds', 0).set('milliseconds', 0);
    
        var timeOver = false;
        if(targetDate.isBefore(moment())) timeOver = true;
        
        var foundBooking = hpContext.bookings.find(booking => moment.utc(booking.startTimeUTC).isSame(targetDate));
        if(!foundBooking){
            setState({...state, variant: 'light', disabled: timeOver, onClick: onBuchen, text: "Buchen", user: null});
        }else if(username !== foundBooking.user.username){
            setState({...state, variant: 'danger', disabled: true, onClick: null, text: "Gebucht von", user: `${foundBooking.user.firstName} ${foundBooking.user.lastname}`});
        }else{
            setState({...state, variant: 'success', disabled: timeOver, onClick: onAusbuchen, text: "Gebucht", user: null});
        }
    }, [hpContext.bookings, hpContext.weekOffset]);

    const onBuchen = () => {
        setHpContext({...hpContext, selectedBooking: {day: day, lesson: lesson.key, editMode: false}});
    };
    
    const onAusbuchen = () => {
        setHpContext({...hpContext, selectedBooking: {day: day, lesson: lesson.key, editMode: true}});
    };

    return(
        <td>
            <Button className="buchenBtn" variant={state.variant} disabled={state.disabled} onClick={state.onClick}>
                {state.text}<br />{state.user}
            </Button>
        </td>
    );
}