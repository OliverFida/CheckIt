// Component imports
import React from 'react';
import {Stack, Table} from 'react-bootstrap';
import StundenplanHead from './StundenplanHead';
import StundenplanBody from './StundenplanBody';
// Style imports
import '../../css/components/Stundenplan.css';

export default function Stundenplan(){
    return(
        <Stack direction='vertical'>
            <Table>
                <StundenplanHead />
                <StundenplanBody />
            </Table>
        </Stack>
    );
};