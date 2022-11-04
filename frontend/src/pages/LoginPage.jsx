// Component imports
import React, {useContext} from 'react';
import {useNavigate} from 'react-router-dom';
import {Stack, Button, Form, Container, Row, Col} from 'react-bootstrap';
import AppNavBar from './components/AppNavBar';
import { ToastContext } from '../contexts/ToastContext';
import {InfoToast, ErrorToast} from './components/Toasts';
// API imports
import LoginAPI from '../api/login';


export default function LoginPage(){
    const {toastContext, setToastContext} = useContext(ToastContext);
    const navigate = useNavigate();

    const sendToast = async (toastElement) => {
        var temp = toastContext.toasts;

        temp.push(toastElement);

        setToastContext({...toastContext, toasts: temp});
    };

    const onLogin = async (e) => {
        e.preventDefault();
        var response = await LoginAPI.login(e.target.username.value.toLowerCase(), e.target.password.value);
        
        if(response.status === 200){
            sendToast(<InfoToast>Du wurdest eingeloggt!</InfoToast>);

            navigate("/home");
        }else{
            sendToast(<ErrorToast>{response.response.data.value.message}</ErrorToast>);
        }
    };

    return(
        <Stack direction='vertical'>
            <AppNavBar />
            <Container>
                <Row className="justify-content-center align-items-center" style={{ height: 'calc(100vh - 60px)' }}>
                    <Col xs={6} lg={3}>
                        <Form onSubmit={onLogin}>
                            <Form.Group className="mb-3" controlId="formEmail">
                                <Form.Label>Benutzername</Form.Label>
                                <Form.Control type="text" name="username" />
                            </Form.Group>

                            <Form.Group className="mb-3" controlId="formPassword">
                                <Form.Label>Passwort</Form.Label>
                                <Form.Control type="password" name="password" />
                            </Form.Group>                
                            <Button variant="primary" className="mx-auto d-block" type='submit'>Einloggen</Button>
                        </Form>
                    </Col>
                </Row>
            </Container>
        </Stack>
    );
}