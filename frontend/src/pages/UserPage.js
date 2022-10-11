// Component imports
import React, { useState, useContext, useEffect } from 'react';
import {Stack, Button, Form, Modal, Row, Col, Table} from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';
import AppNavBar from './components/AppNavBar';
import UserPageContextProvider, {UserPageContext} from '../contexts/UserPageContext';
import moment from 'moment';
// Style imports
import '../css/components/UserPage.css';
// API imports
const UserAPI = require('../api/user');

export default function UserPage(){
    return(
        <UserPageContextProvider>
            <Stack direction='vertical'>
                <AppNavBar />
                <UserPageContent />
                <ChangePasswordModal />
            </Stack>
        </UserPageContextProvider>
    );
};

function UserPageContent(){
    const {upContext, setUpContext} = useContext(UserPageContext);
    const [userData, setUserData] = useState(null);

    useEffect(() => {
        async function doAsync(){
            setUserData(await UserAPI.getUserData());
        }
        doAsync();
    }, []);

    const onChangePassword = () => {
        setUpContext({...upContext, uiControl: {...upContext.uiControl, changePWModal: true}});
    };

    return(
        <Row className="justify-content-center align-items-center" style={{ height: '80vh' }}>
            <Col md={5}>
                <h2 className="text-center">Profil</h2>
                <Table bordered className='userprofile'>
                    <tbody>
                        <tr>
                            <td>Benutzername:</td>
                            <td>{userData?.username}</td>
                        </tr>
                        <tr>
                            <td>Vorname:</td>
                            <td>{userData?.firstName}</td>
                        </tr>
                        <tr>
                            <td>Nachname:</td>
                            <td>{userData?.lastname}</td>
                        </tr>
                        <tr>
                            <td>Rolle:</td>
                            <td>{userData?.role}</td>
                        </tr>
                        <tr>
                            <td>Letzter Login:</td>
                            <td>{moment(userData?.lastLogon).format("DD.MM.YYYY HH:mm")}</td>
                        </tr>
                        <tr>
                            <td>Passwort ändern: </td>
                            <td>
                                <Button onClick={onChangePassword}>
                                    Passwort ändern
                                </Button>
                            </td>
                        </tr>
                    </tbody>
                </Table>                    
            </Col>
        </Row>
    );
}

function ChangePasswordModal(){
    const navigate = useNavigate();
    const {upContext, setUpContext} = useContext(UserPageContext);
    const [values, setValues] = useState({password: "", repeat: ""});

    const onChange = (e) => {
        setValues({...values, [e.target.name]: e.target.value});
    };

    const onSubmit = () => {
        async function doAsync(){
            if(values.password === values.repeat) await UserAPI.changePassword(values.password);

            onCancel();
            localStorage.clear();
            navigate("/login");
        }
        doAsync();
    };

    const onCancel = () => {
        setUpContext({...upContext, uiControl: {...upContext.uiControl, changePWModal: false}});
    }

    return(
        <Modal show={upContext.uiControl.changePWModal} onHide={onCancel} centered>
            <Modal.Header closeButton>
                <Modal.Title>Passwort ändern</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form>
                    <Form.Group className="mb-3" controlId="changePassword">
                        <Form.Label>Neues Passwort</Form.Label>
                        <Form.Control type="password" name="password" onChange={onChange} value={values.password} />
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="changePasswordConfirm">
                        <Form.Label>Neues Passwort wiederholen</Form.Label>
                        <Form.Control type="password" name="repeat" onChange={onChange} value={values.repeat} />
                    </Form.Group>                                      
                </Form> 
            </Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={onCancel}>
                    Abbrechen
                </Button>
                <Button variant="primary" onClick={onSubmit}>
                    Speichern
                </Button>
            </Modal.Footer>
        </Modal>
    );
}