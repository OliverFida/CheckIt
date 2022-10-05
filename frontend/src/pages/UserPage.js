// Component imports
import React, { useState} from 'react';
import {Button, Form, Modal, Row, Col, Table} from 'react-bootstrap';
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
            
            <Row className="justify-content-center align-items-center" style={{ height: '80vh' }}>
                <Col md={5}>
                    <h2 className="text-center">Profil</h2>
                    <Table bordered className='userprofile'>
                        <tbody>
                            <tr>
                                <td>Benutzername:</td>
                                <td>Username</td>
                            </tr>
                            <tr>
                                <td>Vorname:</td>
                                <td>Vorname</td>
                            </tr>
                            <tr>
                                <td>Nachname:</td>
                                <td>Nachname</td>
                            </tr>
                            <tr>
                                <td>Rolle:</td>
                                <td>Benutzerrolle</td>
                            </tr>
                            <tr>
                                <td>Letzter Login:</td>
                                <td>04.10.2022</td>
                            </tr>
                            <tr>
                                <td>Passwort ändern: </td>
                                <td>
                                    <Button onClick={handleShow}>
                                        Passwort ändern
                                    </Button>
                                </td>
                            </tr>
                        </tbody>
                    </Table>                    
                </Col>
            </Row>


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

      