// Component imports
import React, {useEffect, useState} from 'react';
import {Stack, Navbar, Button, NavDropdown, Table} from 'react-bootstrap';
// API imports
import BookingsAPI from '../api/bookings';

export default function HomePage(){
    return(
        <Stack direction='vertical'>
            <AppNavBar />
            <Stundenplan />
        </Stack>
    );
}

function AppNavBar(){
    return(
        <Navbar bg='light' expand='lg'>
            <Stack direction='horizontal' style={{width: '100%', justifyContent: 'space-between'}}>
                <Navbar.Brand>CHECK-IT</Navbar.Brand>
                <Stack direction='horizontal' gap={3}>
                    <NavDropdown title='Klasse'>
                        <NavDropdown.Item>1ISBA1</NavDropdown.Item>
                        <NavDropdown.Item>3ISBA1</NavDropdown.Item>
                    </NavDropdown>
                    <NavDropdown title='Raum'>
                        <NavDropdown.Item>304</NavDropdown.Item>
                        <NavDropdown.Item>307</NavDropdown.Item>
                        <NavDropdown.Item>Tobias sei Mama</NavDropdown.Item>
                    </NavDropdown>
                    <Button>Abmelden</Button>
                </Stack>
            </Stack>
        </Navbar>
    );
}

function Stundenplan(){
    const [tableBody, setTableBody] = useState([]);
    
    useEffect(() => {
        var dateToday = new Date();
        
    }, []);

    return(
        <Table>
            <thead>
                <tr>
                    <th></th>
                    <th>MO 12.09.22</th>
                    <th>DI 13.09.22</th>
                    <th>MI 14.09.22</th>
                    <th>DO 15.09.22</th>
                    <th>FR 16.09.22</th>
                </tr>
            </thead>
            <tbody>
                {tableBody}
            </tbody>
        </Table>
    );
}