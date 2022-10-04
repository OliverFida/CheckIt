// Component imports
import React from 'react';
import {Navbar, Stack} from 'react-bootstrap';
import AppNavUser from './AppNavUser';

export default function AppNavBar({children}){
    return(
        <Navbar bg='light' expand='lg'>
            <Stack direction='horizontal' style={{width: '100%', justifyContent: 'space-between'}}>
                <Navbar.Brand href='/home'>CHECK-IT</Navbar.Brand>
                {children}
                {localStorage.getItem('loginToken') !== null ? <AppNavUser /> : null}
            </Stack>
        </Navbar>
    );
};