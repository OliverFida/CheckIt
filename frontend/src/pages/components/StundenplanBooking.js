// Component imports
import React, {useState, useEffect, useContext} from 'react';
import moment from 'moment';
import { Button } from 'react-bootstrap';
import { HomePageContext } from '../../contexts/HomePageContext';

export default function StundenplanBooking({day, lesson}){
    const userId = 0;
    const {hpContext} = useContext(HomePageContext);
    const [state, setState] = useState({variant: 'light', disabled: false, onClick: null, text: ""});

    useEffect(() => {
        var targetHour = lesson.start.substring(0, 2);
        var targetMinute = lesson.start.substring(3, 5);
        var targetDate = moment().weekday(1).add(hpContext.weekOffset, 'weeks').add(day - 1, 'days');
        targetDate.set('hours', targetHour).set('minutes', targetMinute).set('seconds', 0).set('milliseconds', 0);
    
        var timeOver = false;
        if(targetDate.isBefore(moment())) timeOver = true;
        
        var foundBooking = hpContext.bookings.find(booking => moment.utc(booking.startTime).isSame(targetDate));
        if(!foundBooking){
            setState({...state, variant: 'light', disabled: timeOver, onClick: onBuchen, text: "Buchen"});
        }else{
            if(userId !== foundBooking.createdBy) var fremdGebucht = true;
            setState({...state, variant: 'danger', disabled: timeOver || fremdGebucht, onClick: onAusbuchen, text: "Gebucht"});
        }
    }, [hpContext.bookings, hpContext.weekOffset]);

    const onBuchen = () => {
        console.log(`Buchen ${day} ${lesson.key}`);
    };
    
    const onAusbuchen = () => {
        console.log(`Ausbuchen ${day} ${lesson.key}`);
    };

    return(
        <td>
            <Button className="buchenBtn" variant={state.variant} disabled={state.disabled} onClick={state.onClick}>
                {state.text}
            </Button>
        </td>
    );
}