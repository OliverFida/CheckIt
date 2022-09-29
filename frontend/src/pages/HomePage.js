// Component imports
import React from 'react';
import {Stack} from 'react-bootstrap';
import AppNavBar from './components/AppNavBar';
import Stundenplan from './components/Stundenplan';
import HomePageContextProvider from '../contexts/HomePageContext';

export default function HomePage(){
    return(
        <HomePageContextProvider>
            <Stack direction='vertical'>
                <AppNavBar />
                <Stundenplan />
            </Stack>
        </HomePageContextProvider>
    );
};