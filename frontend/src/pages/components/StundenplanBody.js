// Component imports
import React, {useState, useEffect, useContext} from 'react';
import { HomePageContext } from '../../contexts/HomePageContext';
import StundenplanBooking from './StundenplanBooking';
// API imports
import BookingsAPI from '../../api/bookings';
import timesMap from '../../api/timesMap.json';

export default function StundenplanBody(){
    const {hpContext, setHpContext} = useContext(HomePageContext);
    const [elements, setElements] = useState([]);
    
    useEffect(() => {
        var newElements = [];
        timesMap.forEach(lesson => {
            newElements.push(<StundenplanRow key={`row_${lesson.key}`} lesson={lesson} weekOffset={hpContext.weekOffset} />);
        });
        setElements(newElements);
    }, []);

    useEffect(() => {
        var temp = BookingsAPI.getBookings(hpContext.roomId);
        if(hpContext.bookings) setHpContext({...hpContext, bookings: temp});
    }, [hpContext.roomId]);

    return(
        <tbody>
            {elements}
        </tbody>
    );
}

function StundenplanRow({lesson, weekOffset}){
    const [elements, setElements] = useState([]);

    useEffect(() => {
        var newElements = [];
        for(var day = 1; day <= 5; day++){
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