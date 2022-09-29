// Component imports
import React from 'react';
import {useNavigate} from 'react-router-dom';
import {Button, Form, Container, Row, Col} from 'react-bootstrap';

export default function LoginPage(){
    const navigate = useNavigate();

    const onLogin = () => {
        navigate("/home");
    };

    return(
        <Container>
            <Row className="justify-content-center align-items-center" style={{ height: '100vh' }}>
                <Col xs={6} lg={3}>
                    <Form>
                        <Form.Group className="mb-3" controlId="formEmail">
                            <Form.Label>Benutzername</Form.Label>
                            <Form.Control type="text"/>
                        </Form.Group>

                        <Form.Group className="mb-3" controlId="formPassword">
                            <Form.Label>Passwort</Form.Label>
                            <Form.Control type="password"/>
                        </Form.Group>                
                        <Button variant="primary" type="submit" className="mx-auto d-block" onClick={onLogin}>
                            Einloggen
                        </Button>
                    </Form>
                </Col>
            </Row>
        </Container>
    );
};

      