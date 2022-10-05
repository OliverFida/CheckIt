// Component imports
import React, {useEffect, useState} from 'react';
import {Stack, Button, Table, Modal, Form} from 'react-bootstrap';
import AppNavBar from './components/AppNavBar';

import '../css/components/RoomsPage.css';
// API imports
import BookingsAPI from '../api/bookings';

export default function RoomsPage(){    

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
            <AppNavBar>
                <Button onClick={handleShowModalOne}>Neuen Raum erstellen</Button>
            </AppNavBar>
            
                <Table className='rooms'>
                <thead>
                    <tr>                    
                        <th>Raumnummer</th>
                        <th>Erstellt am</th>
                        <th>Ersteller</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    <tr>                    
                        <td>307</td>
                        <td>25.09.2022</td>
                        <td>Tobias</td>
                        <td>
                            <Button onClick={handleShowModalTwo} className="my-1 me-2">
                                Raum bearbeiten
                            </Button>
                            <Button variant="danger" className="my-1 me-2">
                                Raum löschen
                            </Button>
                        </td>                        
                    </tr>
                    <tr>                   
                        <td>205</td>
                        <td>28.09.2022</td>
                        <td>Tobias</td>
                        <td></td>
                    </tr>
                </tbody>
            </Table>

            <Modal show={show === 'modal-one'} onHide={handleClose} centered>
            <Modal.Header closeButton>
                <Modal.Title>Neuen Raum erstellen</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form>
                    <Form.Group className="mb-3" controlId="setNumber">
                        <Form.Label>Nummer</Form.Label>
                        <Form.Control type="text"/>
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="setName">
                        <Form.Label>Name</Form.Label>
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


            <Modal show={show === 'modal-two'} onHide={handleClose} centered>
            <Modal.Header closeButton>
                <Modal.Title>Raum bearbeiten</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form>
                    <Form.Group className="mb-3" controlId="editNumber">
                        <Form.Label>Neue Nummer</Form.Label>
                        <Form.Control type="text"/>
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="editName">
                        <Form.Label>Neuer Name</Form.Label>
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

        </Stack>
        </>
    );
}