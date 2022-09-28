// Component imports
import React, { useState} from 'react';

import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import Modal from 'react-bootstrap/Modal';
import Table from 'react-bootstrap/Table';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';

export default function UserPage(){

    const [show, setShow] = useState(false);

    const editUserClose = () => setShow(false);
    const editUserShow = () => setShow(true);   

    return(
        <>
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
                            <Button onClick={editUserShow}>
                                Ändern
                            </Button>
                        </td>
                    </tr>
                </tbody>
            </Table>

            <Modal show={show} onHide={editUserClose}>
                <Modal.Header closeButton>
                    <Modal.Title>Name ändern</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form>
                        <Form.Group className="mb-3" controlId="formNewFirstName">
                            <Form.Label>Neuer Vorname</Form.Label>
                            <Form.Control type="text"/>
                        </Form.Group>
                        <Form.Group className="mb-3" controlId="formNewLastName">
                            <Form.Label>Neuer Nachname</Form.Label>
                            <Form.Control type="text"/>
                        </Form.Group>                                      
                    </Form> 
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={editUserClose}>
                        Abbrechen
                    </Button>
                    <Button variant="primary" onClick={editUserClose}>
                        Änderungen Speichern
                    </Button>
                </Modal.Footer>
            </Modal>
        </>
    );
};

      