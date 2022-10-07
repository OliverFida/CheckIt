// Component imports
import React from 'react';
import {Navbar, Stack, Button} from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';
import AppNavUser from './AppNavUser';

export default function AppNavBar({children}){
    const navigate = useNavigate();

    const onHome = () => {
        navigate("/home");
    }

    return(
        <Navbar bg='light' expand='lg' style={{paddingLeft: '20px', paddingRight: '20px'}}>
            <Stack direction='horizontal' style={{width: '100%', justifyContent: 'space-between'}}>
                <Navbar.Brand>
                    <Stack direction='horizontal' style={{gap: '10px'}}>
                        CHECK-IT
                        {localStorage.getItem('loginToken') !== null ? <Button onClick={onHome}>Buchungen</Button> : null}
                    </Stack>
                </Navbar.Brand>
                <Stack direction='horizontal' style={{gap: '10px'}}>
                    {children}
                    {localStorage.getItem('loginToken') !== null ? <AppNavUser /> : null}
                </Stack>
            </Stack>
        </Navbar>
    );
};