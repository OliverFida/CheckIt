// Component imports
import React, {useContext} from 'react';
import {Nav, Button} from 'react-bootstrap';
import { UserEditContext } from '../../contexts/UserEditContext';

export default function AppNavUserEdit(){
    const {ueContext, setUeContext} = useContext(UserEditContext);

    const onNew = () => {
        setUeContext({...ueContext, uiControl:{...ueContext.uiControl, userModal: true, modalMode: "new"}, users:{...ueContext.users, selected: null}});
    };

    return(
        <Nav>
            <Button variant='primary' onClick={onNew}>Neuer Benutzer</Button>
        </Nav>
    );
}