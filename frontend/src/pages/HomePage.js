// Component imports
import React, {useContext} from 'react';
import {Stack} from 'react-bootstrap';
import AppNavBar from './components/AppNavBar';
import AppNavBooking from './components/AppNavBooking';
import Stundenplan from './components/Stundenplan';
import HomePageContextProvider from '../contexts/HomePageContext';
import BookingModal, {BookingDeleteModal} from './components/BookingModal';
// import {HomePageDebugger} from './components/Debugger';
import { HomePageContext } from '../contexts/HomePageContext';

export default function HomePage(){
    return(
        <HomePageContextProvider>
            <Stack direction='vertical'>
                <AppNavBar middle={<RoomDisplay />}>
                    <AppNavBooking />
                </AppNavBar>
                <Stundenplan />
                <BookingModal />
                <BookingDeleteModal />
                {/* <HomePageDebugger /> */}
            </Stack>
        </HomePageContextProvider>
    );
};

function RoomDisplay(){
    const {hpContext, setHpContext} = useContext(HomePageContext);

    return(
        <h3>{
            hpContext.roomSelection.id ? 
                `${hpContext.roomSelection.name} [${hpContext.roomSelection.number}]`
        :
            `Kein Raum gewählt!`
        }
        </h3>
    );
}