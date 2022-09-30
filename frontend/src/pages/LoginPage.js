// Component imports
import React, {useContext, useState, useEffect} from 'react';
import {useNavigate} from 'react-router-dom';
import {Stack, Button, Form, Container, Row, Col} from 'react-bootstrap';
import AppNavBar from './components/AppNavBar';
import LoginContextProvider, { LoginContext } from '../contexts/LoginContext';
// API imports
import LoginAPI from '../api/login';

export default function LoginPage(){
    return(
        <LoginContextProvider>
            <LoginPageContent />
        </LoginContextProvider>
    )
};

function LoginPageContent(){
    const {loginContext} = useContext(LoginContext);
    const [values, setValues] = useState({username: "", password: ""});
    const navigate = useNavigate();

    const onValueChange = (event) => {
        setValues({...values, [event.target.name]: event.target.value});
    };

    const onLogin = () => {
        var response = LoginAPI.login(values.username, values.password);
        if(response.statusCode === 200){
            navigate("/home");
        }else{
            setValues({...values, password: ""});
        }
    };

    return(
        <Stack direction='vertical'>
            <AppNavBar />
            <Container>
                <Row className="justify-content-center align-items-center" style={{ height: '80vh' }}>
                    <Col xs={6} lg={3}>
                        <Form>
                            <Form.Group className="mb-3" controlId="formEmail">
                                <Form.Label>Benutzername</Form.Label>
                                <Form.Control type="text" onChange={onValueChange} name="username" value={values.username} />
                            </Form.Group>

                            <Form.Group className="mb-3" controlId="formPassword">
                                <Form.Label>Passwort</Form.Label>
                                <Form.Control type="password" onChange={onValueChange} name="password" value={values.password} />
                            </Form.Group>                
                            <Button variant="primary" className="mx-auto d-block" onClick={onLogin}>
                                Einloggen
                            </Button>
                        </Form>
                        <p><b>Debug Username:</b> dageorg<br />
                        <b>Debug Password:</b> linux</p>
                    </Col>
                </Row>
            </Container>
        </Stack>
    );
}