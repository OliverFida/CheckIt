// Component imports
import React from 'react';
import {Stack} from 'react-bootstrap';
import AppNavBar from './components/AppNavBar';
import AppNavBooking from './components/AppNavBooking';
import Stundenplan from './components/Stundenplan';
import HomePageContextProvider from '../contexts/HomePageContext';
import BookingModal from './components/BookingModal';
import {HomePageDebugger} from './components/Debugger';

export default function HomePage(){
    return(
        <HomePageContextProvider>
            <Stack direction='vertical'>
                <AppNavBar>
                    <AppNavBooking />
                </AppNavBar>
                <Stundenplan />
                <BookingModal />
                <HomePageDebugger />
            </Stack>
        </HomePageContextProvider>
    );
};