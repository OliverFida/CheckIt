// Component imports
import React from 'react';
import {Stack} from 'react-bootstrap';
import AppNavBar from './components/AppNavBar';
import Stundenplan from './components/Stundenplan';

export default function HomePage(){
    return(
        <Stack direction='vertical'>
            <AppNavBar />
            <Stundenplan />
        </Stack>
    );
};