// Component imports
import React, { useState} from 'react';
import {Stack, Button, Form, Modal, Table} from 'react-bootstrap';
import AppNavBar from './components/AppNavBar';

import '../css/components/UserPage.css';


export default function UserEdit(){
    const [show, setShow] = useState(false);

    const handleShowModalOne = () => {
        setShow("modal-one")
       }
       
    const handleShowModalTwo = () => {
    setShow("modal-two")
    }
    
    const handleClose = () => {
    setShow("close")
    }
 

    return(
        <>
             <Stack direction='vertical'>
                <AppNavBar />
            </Stack>
       
       <Table>
            <thead>
                <tr>                        
                    <th>Vorname</th>
                    <th>Nachname</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr>                        
                    <td valign='middle'>Name</td>
                    <td valign='middle'>Nachname</td>
                    <td>
                        <Button onClick={handleShowModalOne} className="me-2 my-1">
                            Name Ändern
                        </Button>
                        <Button className="my-1 me-2">
                            Passwort zurücksetzen
                        </Button>                       
                    </td>
                </tr>
            </tbody>
        </Table>
        <Button onClick={handleShowModalTwo} className="my-1">
            Neuen Benutzer erstellen
        </Button>

        <Modal show={show === 'modal-one'} onHide={handleClose}>
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
                <Button variant="secondary" onClick={handleClose}>
                    Abbrechen
                </Button>
                <Button variant="primary" onClick={handleClose}>
                    Änderungen Speichern
                </Button>
            </Modal.Footer>
        </Modal>



        <Modal show={show === 'modal-two'} onHide={handleClose}>
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
                    <Form.Group className="mb-3" controlId="newPassword">
                        <Form.Label>Passwort</Form.Label>
                        <Form.Control type="password"/>
                    </Form.Group> 
                    <Form.Group className="mb-3" controlId="newPasswordConfirm">
                        <Form.Label>Passwort</Form.Label>
                        <Form.Control type="password"/>
                    </Form.Group>                                      
                </Form> 
            </Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={handleClose}>
                    Abbrechen
                </Button>
                <Button variant="primary" onClick={handleClose}>
                    Benutzer speichern
                </Button>
            </Modal.Footer>
        </Modal>
           
        </>
    );
};

      