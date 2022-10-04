// Component imports
import React, {useEffect, useState} from 'react';
import {Stack, Navbar, Button, NavDropdown, Table} from 'react-bootstrap';
// API imports
import BookingsAPI from '../api/bookings';

export default function RoomsPage(){
    return(
        <Stack direction='vertical'>
            <RoomsNavBar />
            <RoomsList />
        </Stack>
    );
}

function RoomsNavBar(){
    return(
        <Navbar bg='light' expand='lg'>
            <Stack direction='horizontal' style={{width: '100%', justifyContent: 'space-between'}}>
                <Navbar.Brand>Räume</Navbar.Brand>
                <Stack direction='horizontal' gap={3}>
                    <Button>Raum erstellen</Button>
                </Stack>
            </Stack>
        </Navbar>
    );
}

function RoomsList(){
    const [tableBody, setTableBody] = useState([]);
    
    useEffect(() => {
        var dateToday = new Date();
        
    }, []);

    return(
        <Table>
            <thead>
                <tr>
                    <th></th>
                    <th>Raumnummer</th>
                    <th>Erstellt am</th>
                    <th>Ersteller</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td></td>
                    <td>307</td>
                    <td>25.09.2022</td>
                    <td>Tobias</td>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                    <td>205</td>
                    <td>28.09.2022</td>
                    <td>Tobias</td>
                    <td></td>
                </tr>
            </tbody>
        </Table>
    );
}