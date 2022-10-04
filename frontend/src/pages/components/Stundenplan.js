// Component imports
import React, {useContext} from 'react';
import { HomePageContext } from '../../contexts/HomePageContext';
import {Stack, Table} from 'react-bootstrap';
import StundenplanHead from './StundenplanHead';
import StundenplanBody from './StundenplanBody';
// Style imports
import '../../css/components/Stundenplan.css';

export default function Stundenplan(){
    const {hpContext} = useContext(HomePageContext);
    
    if(hpContext.roomId === null) return (<h1>Keine Räume zur Auswahl</h1>);
    return(
        <Stack direction='vertical'>
            <Table>
                <StundenplanHead />
                <StundenplanBody />
            </Table>
        </Stack>
    );
};