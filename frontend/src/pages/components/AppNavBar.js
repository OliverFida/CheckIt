// Component imports
import React from 'react';
import {Navbar, Nav, Stack, Button} from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';
import AppNavUser from './AppNavUser';
import Logo from '../../assets/checkit-logo.png';
// Style imports
import '../../css/components/AppNavBar.css';

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
                        <img src={Logo} alt="Check-It Logo" id="appNavLogo" />
                        CHECK-IT
                        {localStorage.getItem('loginToken') !== null ? <Button variant='ghost' onClick={onHome}>Buchungen</Button> : null}
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