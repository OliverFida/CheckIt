// Component imports
import React, { useState, useContext, useEffect} from 'react';
import {Stack, Button, Form, Modal, Table} from 'react-bootstrap';
import AppNavBar from './components/AppNavBar';
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
                    <NewUserButton />
                </AppNavBar>
                <UserEditBody />
                <EditNameModal />
                <NewUserModal />
            </Stack>  
        </UserEditContextProvider>
    );
};

function UserEditBody(){
    return(
        <Table className='useredit'>
            <thead>
                <tr>                        
                    <th>Vorname</th>
                    <th>Nachname</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <UserEditRow />
            </tbody>
        </Table>
    );
}

function UserEditRow(){
    const [elements, setElements] = useState([]);
    const [users, setUserData] = useState(null);

    const {ueContext, setUpContext} = useContext(UserEditContext);

    const showEditUser = () => {
        setUpContext({...ueContext, showEditUserModal: true});
    };

    useEffect(() => {
        async function doAsync(){
            setUserData(await AdminAPI.getUsers());
        }
        doAsync();
    }, []);
    
    useEffect(() => {
        setElements(users?.map(user =>
            <tr key={`user_${user.id}`}>                        
                <td valign='middle' key={`user${user.firstName}`}>{user.firstName}</td>
                <td valign='middle' key={`user${user.lastname}`}>{user.lastname}</td>
                <td>
                    <Button onClick={showEditUser} className="me-2 my-1">
                        Name Ändern
                    </Button>
                    <Button className="my-1 me-2">
                        Passwort zurücksetzen
                    </Button>
                    <Button className="my-1 me-2" variant="danger">
                        Benutzer deaktivieren
                    </Button>                       
                </td>
            </tr>
           ));
    }, [users]);

    return(
        <>
            {elements}
        </>
    )
}

function EditNameModal(){
    const {ueContext, setUpContext} = useContext(UserEditContext);

    const onCancel = () => {
        setUpContext({...ueContext, showUserEditModal: false});
    }

    return (
        <Modal show={ueContext.showUserEditModal} onHide={onCancel} centered>
            <Modal.Header closeButton>
                <Modal.Title>Name ändern</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form>
                    <Form.Group className="mb-3" controlId="editFirstName">
                        <Form.Label>Neuer Vorname</Form.Label>
                        <Form.Control type="text"/>
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="editLastName">
                        <Form.Label>Neuer Nachname</Form.Label>
                        <Form.Control type="text"/>
                    </Form.Group>                                      
                </Form> 
            </Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={onCancel}>
                    Abbrechen
                </Button>
                <Button variant="primary" onClick={onCancel}>
                    Änderungen Speichern
                </Button>
            </Modal.Footer>
        </Modal>
    );
}

function NewUserModal(){
    const {ueContext, setUpContext} = useContext(UserEditContext);

    const onCancel = () => {
        setUpContext({...ueContext, showNewUserModal: false});
    }

    return(
        <Modal show={ueContext.showNewUserModal} onHide={onCancel}>
            <Modal.Header closeButton>
                <Modal.Title>Neuen Benutzer erstellen</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form>
                    <Form.Group className="mb-3" controlId="newUsertName">
                        <Form.Label>Benutzername</Form.Label>
                        <Form.Control type="text"/>
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="newFirstName">
                        <Form.Label>Vorname</Form.Label>
                        <Form.Control type="text"/>
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="newLastName">
                        <Form.Label>Nachname</Form.Label>
                        <Form.Control type="text"/>
                    </Form.Group>                                                         
                </Form> 
            </Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={onCancel}>
                    Abbrechen
                </Button>
                <Button variant="primary" onClick={onCancel}>
                    Benutzer speichern
                </Button>
            </Modal.Footer>
        </Modal>
    );
}

function NewUserButton(){
    const {ueContext, setUpContext} = useContext(UserEditContext);

    const showNewUser = () => {
        setUpContext({...ueContext, showNewUserModal: true});
    }

    return(
        <Button onClick={showNewUser} className="my-1">
            Neuen Benutzer erstellen
        </Button>
    );
}
      