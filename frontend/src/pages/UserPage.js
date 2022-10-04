// Component imports
import React, { useState} from 'react';
import {Stack, Button, Form, Modal, Table} from 'react-bootstrap';
import AppNavBar from './components/AppNavBar';

import '../css/components/UserPage.css';


export default function UserPage(){      

    const [show, setShow] = useState(false);
    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);

    return(
        <>
            <AppNavBar>
            </AppNavBar>

            <div>
                <Button onClick={handleShow}>
                    Passwort ändern
                </Button>
            </div>


            <Modal show={show} onHide={handleClose} centered>
            <Modal.Header closeButton>
                <Modal.Title>Passwort ändern</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form>
                    <Form.Group className="mb-3" controlId="changePassword">
                        <Form.Label>Neues Passwort</Form.Label>
                        <Form.Control type="text"/>
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="changePasswordConfirm">
                        <Form.Label>Neues Passwort wiederholen</Form.Label>
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

        </>
    );
};

      