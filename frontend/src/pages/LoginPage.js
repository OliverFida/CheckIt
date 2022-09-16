// Component imports
import React from 'react';

import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';

export default function LoginPage(){
    
    return(
        <Container>
            <Row className="justify-content-center align-items-center" style={{ height: '100vh' }}>
                <Col xs={6} lg={3}>
                    <Form>
                        <Form.Group className="mb-3" controlId="formBasicEmail">
                            <Form.Label>Benutzername</Form.Label>
                            <Form.Control type="text"/>
                        </Form.Group>

                        <Form.Group className="mb-3" controlId="formBasicPassword">
                            <Form.Label>Passwort</Form.Label>
                            <Form.Control type="password"/>
                        </Form.Group>                
                        <Button variant="primary" type="submit">
                            Einloggen
                        </Button>
                    </Form>
                </Col>
            </Row>
        </Container>
    );
};

      