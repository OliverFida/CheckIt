// Component imports
import React, { useState, useContext, useEffect} from 'react';
import {Stack, Button, Table, ButtonGroup, Row, Col, Card} from 'react-bootstrap';
import AppNavBar from './components/AppNavBar';
import AppNavUserEdit from './components/AppNavUserEdit';
import UserEditModal from './components/UserEditModal';
import UserEditContextProvider, {UserEditContext} from '../contexts/UserEditContext';


// Style imports
import '../css/components/UserPage.css';
// Api imports
import AdminAPI from '../api/admin';

export default function UserEdit(){
    return(
        <UserEditContextProvider>
            <Stack direction='vertical'>
                <AppNavBar>
                    <AppNavUserEdit />
                </AppNavBar>
                <UserEditBody />
                <UserEditModal />
            </Stack>  
        </UserEditContextProvider>
    );
};

function UserEditBody(){
    return(
        <Row className="justify-content-md-center mt-5">
            <Col xl={8}>
               <Card className="px-2">
                    <Table className='useredit'>
                        <thead>
                            <tr>
                                <th>Benutzername</th>                        
                                <th>Vorname</th>
                                <th>Nachname</th>
                                <th>Rolle</th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody>
                            <UserEditRow />
                        </tbody>
                    </Table>
               </Card>
            </Col>
        </Row>
    );
}

function UserEditRow(){
    const {ueContext, setUeContext} = useContext(UserEditContext);
    const [elements, setElements] = useState([]);
    const [users, setUsers] = useState([]);
    
    useEffect(() => {
        if(!ueContext.users.reload) return;

        async function doAsync(){
            var response = await AdminAPI.getUsers();
            setUsers(response.data);
            setUeContext({...ueContext, users:{...ueContext.users, reload: false}});
        }
        doAsync();
    }, [ueContext.users.reload]);
    
    useEffect(() => {
        setElements(users.map(user => user.username != 'admin' ?
            <tr key={`user_${user.username}`}>                        
                <td valign='middle' key={`user${user.username}`}>{user.username}</td>
                <td valign='middle' key={`user${user.firstName}`}>{user.firstName}</td>
                <td valign='middle' key={`user${user.lastname}`}>{user.lastname}</td>
                <td valign='middle' key={`user_role${user.role}`}>{user.role}</td>
                <td>
                    <ButtonGroup>
                        <Button onClick={() => {onEdit(user)}} className="my-1 me-1" size="sm">
                            Bearbeiten
                        </Button>
                        <Button onClick={() => {onResetPW(user)}} className="my-1 me-1" size="sm">
                            Passwort zurücksetzen
                        </Button>
                        <Button onClick={() => {onDeactivateUser(user, user.active ? false : true)}} className="my-1 me-1" size="sm" variant={user.active ? "secondary" : "success"}
                        disabled={localStorage.getItem("loginUsername") === user.username.toLowerCase() ? true : false}>
                            {user.active ? "Deaktivieren" : "Aktivieren"}
                        </Button>    
                        <Button onClick={() => {onDeleteUser(user)}} className="my-1 me-1" size="sm" variant="danger"
                        disabled={localStorage.getItem("loginUsername") === user.username.toLowerCase() ? true : false}>
                            Löschen
                        </Button>    
                    </ButtonGroup>                 
                </td>
            </tr>
            :
            null
           ));
    }, [users]);

    const onEdit = (user) => {
        setUeContext({...ueContext, uiControl:{...ueContext.uiControl, userModal: true, modalMode: "edit"}, users:{...ueContext.users, selected: user}});
    };

    const onResetPW = (user) => {
        setUeContext({...ueContext, uiControl:{...ueContext.uiControl, userModal: true, modalMode: "resetPW"}, users:{...ueContext.users, selected: user}});
    };
    
    const onDeactivateUser = async (user, state) => {
        await AdminAPI.setUserActive(user.username, state);
        setUeContext({...ueContext, users:{...ueContext.users, reload: true}});
    };

    const onDeleteUser = async (user) => {
        setUeContext({...ueContext, uiControl:{...ueContext.uiControl, userModal: true, modalMode: "delete"}, users:{...ueContext.users, selected: user}});
    };
    
    return(
        <>
            {elements}
        </>
    );
}