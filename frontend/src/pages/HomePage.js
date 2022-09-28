// Component imports
import React, {createElement, useEffect, useState} from 'react';
import {Stack, Navbar, Button, NavDropdown, Table, Nav} from 'react-bootstrap';
import timesMap from '../api/timesMap.json';
// API imports
import BookingsAPI, { getBookings } from '../api/bookings';

export default function HomePage(){
    return(
        <Stack direction='vertical'>
            <AppNavBar />
            <Stundenplan />
        </Stack>
    );
}

function AppNavBar(){
    const [rooms, setRooms] = useState([]);

    const debugRooms = [
        "304",
        "307",
        "Tobias sei Mama",
        "Julian"
    ];

    useEffect(() => {
        generateRooms();
    }, []);

    function generateRooms(){
        setRooms(debugRooms.map(room => <NavDropdown.Item key={`room_${room}`}>{room}</NavDropdown.Item>));
    }

    return(
        <Navbar bg='light' expand='lg'>
            <Stack direction='horizontal' style={{width: '100%', justifyContent: 'space-between'}}>
                <Navbar.Brand>CHECK-IT</Navbar.Brand>
                <Stack direction='horizontal' gap={3}>
                    <NavDropdown title='Raum'>
                        {rooms}
                    </NavDropdown>
                    <Button>Abmelden</Button>
                </Stack>
            </Stack>
        </Navbar>
    );
}

function Stundenplan(){
    const [tableHead, setTableHead] = useState([]);
    const [bookings, setBookings] = useState([]);
    const [tableBody, setTableBody] = useState([]);
    
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
        var dateToday = new Date();
        dateToday.setDate(dateToday.getDate() + (7 * weekOffset));
        
        var weekDayToday = dateToday.getDay();
        var dateMonday = new Date(dateToday);
        dateMonday.setDate(dateToday.getDate() - (weekDayToday - 1));

        var newTableHead = [];
        newTableHead.push(<th key="th0"></th>);
        for(var i = 0; i < 5; i++){            
            var useDate = new Date(dateMonday);
            useDate.setDate(useDate.getDate() + i);
            newTableHead.push(<th key={`th${i + 1}`}>{useDate.getDate()}.{useDate.getMonth() + 1}.{useDate.getFullYear()}</th>);
        }
        setTableHead(newTableHead);
    }

    function generateTableBody(){
        var newTableBody = [];

        timesMap.forEach(lesson => {
            newTableBody.push(generateRow(lesson));
        });

        setTableBody(newTableBody);
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
        var targetDateFrom = new Date();
        targetDateFrom.setDate(targetDateFrom.getDate() + (0 - targetDateFrom.getDay()) + day);
        
        var targetHourFrom = lesson.start.substring(0, 2);
        var targetMinuteFrom = lesson.start.substring(3, 5);

        targetDateFrom.setHours(parseInt(targetHourFrom));
        targetDateFrom.setMinutes(parseInt(targetMinuteFrom));
        targetDateFrom.setSeconds(0);
        targetDateFrom.setMilliseconds(0);
        
        var foundBooking = bookings.find(booking => booking.startTime === targetDateFrom.toJSON());
        
        if(foundBooking) return <td key={`booking_${day}_${lesson.key}`}>Gebucht</td>;
        return <td key={`booking_${day}_${lesson.key}`}>-</td>;
    }

    return(
        <Table>
            <thead>
                <tr>
                    {tableHead}
                </tr>
            </thead>
            <tbody>
                {tableBody}
            </tbody>
        </Table>
    );
}