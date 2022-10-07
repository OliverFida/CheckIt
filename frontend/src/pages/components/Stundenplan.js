// Component imports
import React, {useContext} from 'react';
import { HomePageContext } from '../../contexts/HomePageContext';
import {Stack, Table} from 'react-bootstrap';
import StundenplanHead from './StundenplanHead';
import StundenplanBody from './StundenplanBody';
import LoadingSpinner from './LoadingSpinner';
// Style imports
import '../../css/components/Stundenplan.css';

export default function Stundenplan(){
    const {hpContext} = useContext(HomePageContext);
    
    return(
        <Stack direction='vertical'>
            {hpContext.roomId}
            <LoadingSpinner loading={!hpContext.bookingsLoaded}>
                <Table className='stundenplan'>
                    <StundenplanHead />
                    <StundenplanBody />
                </Table>
            </LoadingSpinner>
        </Stack>
    );
};