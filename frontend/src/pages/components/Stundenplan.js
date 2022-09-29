// Component imports
import React, {useState, useEffect} from 'react';
import {Button, ButtonGroup, Stack, Table} from 'react-bootstrap';
import timesMap from '../../api/timesMap.json';
import moment from 'moment';
// API imports
import BookingsAPI from '../../api/bookings';
// Style imports
import '../../css/components/Stundenplan.css';

export default function Stundenplan(){
    const [tableHeadElements, setTableHeadElements] = useState([]);
    const [bookings, setBookings] = useState([]);
    const [tableBodyElements, setTableBodyElements] = useState([]);
    const [weekOffset, setWeekOffset] = useState(0);
    
    useEffect(() => {
        generateTable();
    }, []);

    useEffect(() => {
        generateTable();
    }, [weekOffset]);

    useEffect(() => {
        generateTableBody();
    }, [bookings]);

    function generateTable(){
        generateTableHead();
        getBookings();
    }

    function getBookings(){
        setBookings(BookingsAPI.getBookings(null));
    }

    function generateTableHead(){
        console.log("Generate Head");
        var dateToday = moment();
        dateToday.add(weekOffset, 'weeks'); // OFDO: Graigt ned den korrekten Offset nach Button

        var dateMonday = moment().weekday(1);

        var newTableHeadElements = [];
        newTableHeadElements.push(<th key="th_0">
            <ButtonGroup>
                <Button disabled={weekOffset === 0 ? true : false} onClick={onEarlier}>Früher</Button>
                <Button disabled={weekOffset === 3 ? true : false} onClick={onLater}>Später</Button>
            </ButtonGroup>
        </th>);
        for(var day = 1; day <= 5; day++){
            var tempDate = moment(dateMonday).add(day - 1, 'days');
            newTableHeadElements.push(<th key={`th_${day}`}>{tempDate.format("DD.MM.YYYY")}</th>);
        }

        console.log("Setting Head");
        setTableHeadElements(newTableHeadElements);
    }

    function generateTableBody(){
        console.log("Generate Body");
        var newTableBody = [];

        timesMap.forEach(lesson => {
            newTableBody.push(generateRow(lesson));
        });

        setTableBodyElements(newTableBody);
    }

    function generateRow(lesson){
        var tempData = [];

        tempData.push(generateRowTitle(lesson));

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

    function generateRowTitle(lesson){
        var title;

        if(lesson.key === "break"){
            title = "Pause";
        }else{
            title = `Stunde ${lesson.key}`;
        }

        return <td key="title"><div className="rowTitle"><span><b>{title}</b></span><span>{lesson.start} - {lesson.end}</span></div></td>;
    }

    function generateBooking(day, lesson){
        var targetHour = lesson.start.substring(0, 2);
        var targetMinute = lesson.start.substring(3, 5);

        var targetDateFrom = moment().weekday(1).add((day - 1), 'days').set('hours', targetHour).set('minutes', targetMinute).set('seconds', 0).set('milliseconds', 0);

        var foundBooking = bookings.find(booking => moment.utc(booking.startTime).isSame(targetDateFrom));
        
        var timeOver;
        if(targetDateFrom.isBefore(moment())) timeOver = true;
        var disabled;

        if(foundBooking){
            var fremdGebucht = true;
            if(timeOver || fremdGebucht) disabled = true;
            return <td key={`booking_${day}_${lesson.key}`}><Button variant='danger' disabled={disabled} className="buchenBtn"><span>Gebucht</span></Button></td>;
        }
        if(timeOver) disabled = true;
        return <td key={`booking_${day}_${lesson.key}`}><Button variant='light' disabled={disabled} className="buchenBtn">Buchen</Button></td>;
    }

    const onEarlier = () => {
        setWeekOffset(weekOffset - 1);
    }

    const onLater = () => {
        setWeekOffset(weekOffset + 1);
    }

    return(
        <Stack direction='vertical'>
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
        </Stack>
    );
};