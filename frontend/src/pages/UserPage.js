// Component imports
import React, { useState, useContext, useEffect } from 'react';
import {Stack, Button, Form, Modal, Row, Col, Table} from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';
import AppNavBar from './components/AppNavBar';
import UserPageContextProvider, {UserPageContext} from '../contexts/UserPageContext';
import { ToastContext } from '../contexts/ToastContext';
import {InfoToast, ErrorToast, WarningToast} from './components/Toasts';
import moment from 'moment';
// Style imports
import '../css/components/UserPage.css';
// API imports
import UserAPI from '../api/user';

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
    const {toastContext, setToastContext} = useContext(ToastContext);
    const [values, setValues] = useState({password: "", repeat: ""});

    const sendToast = async (toastElement) => {
        var temp = toastContext.toasts;

        temp.push(toastElement);

        setToastContext({...toastContext, toasts: temp});
    };

    const onChange = (e) => {
        setValues({...values, [e.target.name]: e.target.value});
    };

    const onSubmit = () => {
        async function doAsync(){
            if(values.password === values.repeat){
                var response = await UserAPI.changePassword(values.password);
                if(response.status === 200){
                    sendToast(<InfoToast>{response.data.message}</InfoToast>);
                    
                    onCancel();
                    localStorage.clear();
                    navigate("/login");
                }else{
                    sendToast(<ErrorToast>Passwort konnte nicht geändert werden!</ErrorToast>);
                }
            }else{
                sendToast(<WarningToast>Die Passwörter stimmen nicht überein!</WarningToast>);
            }
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
                <Form onSubmit={e => {e.preventDefault()}}>
                    <Form.Group className="mb-3" controlId="changePassword">
                        <Form.Label>Neues Passwort</Form.Label>
                        <Form.Control autoComplete='off' type="password" name="password" onChange={onChange} value={values.password} />
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="changePasswordConfirm">
                        <Form.Label>Neues Passwort wiederholen</Form.Label>
                        <Form.Control autoComplete='off' type="password" name="repeat" onChange={onChange} value={values.repeat} />
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