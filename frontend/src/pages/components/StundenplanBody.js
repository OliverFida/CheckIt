// Component imports
import React, {useState, useEffect, useContext} from 'react';
import { HomePageContext } from '../../contexts/HomePageContext';
import StundenplanBooking from './StundenplanBooking';
import moment from 'moment';
// API imports
import timesMap from '../../api/timesMap.json';
import BookingsAPI from '../../api/bookings';

const amountWeeks = 10;
const amountDays = 6; // TODO Samstag nach Config

export default function StundenplanBody(){
    const {hpContext, setHpContext} = useContext(HomePageContext);
    const [elements, setElements] = useState([]);
    
    useEffect(() => {
        var newElements = [];
        timesMap.forEach(lesson => {
            newElements.push(<StundenplanRow key={`row_${lesson.key}`} lesson={lesson} />);
        });
        setElements(newElements);
    }, []);

    useEffect(() => {
        async function doAsync(){
            if(hpContext.roomSelection.id === null) return; //TODO: Double reloading
            await setHpContext({...hpContext, uiControl: {...hpContext.uiControl, bookingsLoading: true}});
            var temp = await BookingsAPI.getBookings(hpContext.roomSelection.id, moment().weekday(1).toJSON(), moment().weekday(amountDays).add(amountWeeks - 1, 'weeks').toJSON());
            await setHpContext({...hpContext, uiControl: {...hpContext.uiControl, bookingsLoading: false}, bookings: {...hpContext.bookings, reload: false, bookings: temp}});
        }
        doAsync();
    }, [hpContext.roomSelection.id, hpContext.bookings.reload]);

    return(
        <tbody>
            {elements}
        </tbody>
    );
}

function StundenplanRow({lesson}){
    const [elements, setElements] = useState([]);

    useEffect(() => {
        var newElements = [];
        for(var day = 1; day <= amountDays; day++){
            if(lesson.key !== "break"){
                newElements.push(<StundenplanBooking key={`booking_${day}_${lesson.key}`} day={day} lesson={lesson} />);
            }else{
                newElements.push(<td key={`break_${day}`} />);
            }
        }
        setElements(newElements);
    }, []);

    return(
        <tr>
            <StundenplanRowTitle lesson={lesson} />
            {elements}
        </tr>
    );
}

function StundenplanRowTitle({lesson}){
    const [title, setTitle] = useState("");

    useEffect(() => {
        if(lesson.key !== "break"){
            setTitle(`Stunde ${lesson.key}`);
        }else{
            setTitle("Pause");
        }
    }, []);

    return(
        <td key="title">
            <div className="rowTitle">
                <span><b>{title}</b></span>
                <span>{lesson.start} - {lesson.end}</span>
            </div>
        </td>
    );
}