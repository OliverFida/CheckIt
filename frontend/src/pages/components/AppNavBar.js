// Component imports
import React from 'react';
import {Navbar, Stack} from 'react-bootstrap';

export default function AppNavBar({children}){
    return(
        <Navbar bg='light' expand='lg'>
            <Stack direction='horizontal' style={{width: '100%', justifyContent: 'space-between'}}>
                <Navbar.Brand>CHECK-IT</Navbar.Brand>
                {children}
            </Stack>
        </Navbar>
    );
};