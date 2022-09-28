// Component imports
import React, {useState, useEffect} from 'react';
import {Table} from 'react-bootstrap';
import timesMap from '../../api/timesMap.json';
import moment from 'moment';
// API imports
import BookingsAPI from '../../api/bookings';

export default function Stundenplan(){
    const [tableHeadElements, setTableHeadElements] = useState([]);
    const [bookings, setBookings] = useState([]);
    const [tableBodyElements, setTableBodyElements] = useState([]);
    
    useEffect(() => {
        generateTableHead();
        getBookings();
    }, []);

    useEffect(() => {
        generateTableBody();
    }, [bookings]);

    function getBookings(){
        setBookings(BookingsAPI.getBookings(null));
    }

    function generateTableHead(){
        var weekOffset = 0;
        var dateToday = moment();
        dateToday.add(weekOffset, 'weeks');

        var dateMonday = moment().isoWeekday(1);

        var newTableHeadElements = [];
        newTableHeadElements.push(<th key="th_0"></th>);
        for(var day = 1; day <= 5; day++){
            var tempDate = moment(dateMonday).add(day - 1, 'days');
            newTableHeadElements.push(<th key={`th_${day}`}>{tempDate.format("DD.MM.YYYY")}</th>);
        }

        setTableHeadElements(newTableHeadElements);
    }

    function generateTableBody(){
        var newTableBody = [];

        timesMap.forEach(lesson => {
            newTableBody.push(generateRow(lesson));
        });

        setTableBodyElements(newTableBody);
    }

    function generateRow(lesson){
        var tempData = [];

        if(lesson.key === "break"){
            tempData.push(<td key="title">Pause</td>);
        }else{
            tempData.push(<td key="title">Stunde {lesson.key}</td>);
        }

        for(var day = 1; day <= 5; day++){
            if(lesson.key !== "break"){
                tempData.push(generateBooking(day, lesson));
            }else{
                tempData.push(<td key={`break_${day}`}></td>);
            }
        }

        var tempRow = <tr key={lesson.key}>{tempData}</tr>;

        return tempRow;
    }

    function generateBooking(day, lesson){
        var targetHour = lesson.start.substring(0, 2);
        var targetMinute = lesson.start.substring(3, 5);

        var targetDateFrom = moment().isoWeekday(1).add((day - 1), 'days').set('hours', targetHour).set('minutes', targetMinute).set('seconds', 0).set('milliseconds', 0);

        var foundBooking = bookings.find(booking => booking.startTime === targetDateFrom.toJSON());
        
        if(foundBooking) return <td key={`booking_${day}_${lesson.key}`}>Gebucht</td>;
        return <td key={`booking_${day}_${lesson.key}`}>-</td>;
    }

    return(
        <Table>
            <thead>
                <tr>
                    {tableHeadElements}
                </tr>
            </thead>
            <tbody>
                {tableBodyElements}
            </tbody>
        </Table>
    );
};