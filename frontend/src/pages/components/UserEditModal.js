// Component imports
import React, {useContext, useEffect, useState} from 'react';
import {Button, Modal, Form} from 'react-bootstrap';
import { UserEditContext } from '../../contexts/UserEditContext';
import {ToastContext} from '../../contexts/ToastContext';
import { ErrorToast, InfoToast } from './Toasts';
// API imports
import AdminAPI from '../../api/admin';
// Helper imports
import UserEditHelper from '../../helpers/userEditHelper';

export default function UserEditModal(){
    const {ueContext, setUeContext} = useContext(UserEditContext);
    const {toastContext, setToastContext} = useContext(ToastContext);
    const [state, setState] = useState(null);
    const [roles, setRoles] = useState(["User", "Admin"]);
    const [roleElements, setRoleElements] = useState([]);

    useEffect(() => {
        async function doAsync(){
            if(!ueContext.uiControl.userModal) return;
            
            if(ueContext.users.selected === null) setState({}); //TODO
            if(ueContext.users.selected !== null) setState(ueContext.users.selected);
        }
        doAsync();
    }, [ueContext.uiControl.userModal]);

    useEffect(() => {
        setRoleElements(roles.map(role => <option key={`option_${role}`} value={role}>{role}</option>));
    }, [roles]);

    const onCancel = () => {
        setUeContext({...ueContext, uiControl:{...ueContext.uiControl, userModal: false}, users:{...ueContext.users, selected: null}});
    };

    const onSubmit = async () => {
        if(ueContext.uiControl.modalMode === "new"){
            await AdminAPI.addUser(state.username, state.firstName, state.lastname, state.role);
        }else if(ueContext.uiControl.modalMode === "edit"){
            await AdminAPI.editUser(state.username, state.firstName, state.lastname, state.role);
        }else if(ueContext.uiControl.modalMode === "resetPW"){
            var newPw = UserEditHelper.generatePassword();

            var response = await AdminAPI.resetPassword(state.username, newPw);
            if(response.status === 200){
                var temp = toastContext.toasts;

                temp.push(<InfoToast preventAutoHide={true}>Passwort zurückgesetzt!<br />
                Das Passwort lautet: {newPw}</InfoToast>);

                setToastContext({...toastContext, toasts: temp});
            }else{
                var temp = toastContext.toasts;

                temp.push(<ErrorToast>{response.response.data.message}</ErrorToast>);

                setToastContext({...toastContext, toasts: temp});
            }
        }else if(ueContext.uiControl.modalMode === "delete"){
            await AdminAPI.deleteUser(state.username);
        }
        setUeContext({...ueContext, uiControl:{...ueContext.uiControl, userModal: false}, users:{...ueContext.users, selected: null, reload: true}});
    };

    const onChange = (e) => {
        setState({...state, [e.target.name]: e.target.value});
    };

    return(
        <Modal show={ueContext.uiControl.userModal} onHide={onCancel} centered>
            <Modal.Header closeButton>
                <Modal.Title>{
                    ueContext.uiControl.modalMode === "edit" ? "Benutzer bearbeiten" :
                    ueContext.uiControl.modalMode === "resetPW" ? "Passwort zurücksetzen" :
                    ueContext.uiControl.modalMode === "delete" ? "Benutzer löschen" :
                    "Benutzer erstellen"    
                }</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form onSubmit={e => {e.preventDefault()}}>
                    {
                        ueContext.uiControl.modalMode === "new" ?
                        <>
                        <Form.Group className="mb-3" controlId="userName">
                            <Form.Label>Benutzername</Form.Label>
                            <Form.Control autoComplete='off' type="text" name="username" value={state?.username ? state.username : ""} onChange={onChange} />
                        </Form.Group>
                        <Form.Group className="mb-3" controlId="firstName">
                            <Form.Label>Vorname</Form.Label>
                            <Form.Control autoComplete='off' type="text" name="firstName" value={state?.firstName ? state.firstName : ""} onChange={onChange} />
                        </Form.Group>
                        <Form.Group className="mb-3" controlId="lastName">
                            <Form.Label>Nachname</Form.Label>
                            <Form.Control autoComplete='off' type="text" name="lastname" value={state?.lastname ? state.lastname : ""} onChange={onChange} />
                        </Form.Group> 
                        <Form.Group className="mb-3" controlId="role">
                            <Form.Label>Rolle</Form.Label>
                            <Form.Select defaultValue={state?.role} name="role" onChange={onChange}>
                                {roleElements}
                            </Form.Select>
                        </Form.Group> 
                        </>
                        :
                        null
                    }
                    {
                        ueContext.uiControl.modalMode === "edit" ?
                        <>
                        <Form.Group className="mb-3" controlId="firstName">
                            <Form.Label>Vorname</Form.Label>
                            <Form.Control autoComplete='off' type="text" name="firstName" value={state?.firstName ? state.firstName : ""} onChange={onChange} />
                        </Form.Group>
                        <Form.Group className="mb-3" controlId="lastName">
                            <Form.Label>Nachname</Form.Label>
                            <Form.Control autoComplete='off' type="text" name="lastname" value={state?.lastname ? state.lastname : ""} onChange={onChange} />
                        </Form.Group> 
                        <Form.Group className="mb-3" controlId="role">
                            <Form.Label>Rolle</Form.Label>
                            <Form.Select defaultValue={`${state?.role}`} name="role" onChange={onChange}>
                                {roleElements}
                            </Form.Select>
                        </Form.Group> 
                        </>
                        :
                        null
                    }
                    {
                        ueContext.uiControl.modalMode === "resetPW" ?
                        <p>Passwort zurücksetzen?</p>
                        :
                        null
                    }
                    {
                        ueContext.uiControl.modalMode === "delete" ?
                        <p>Benutzer löschen?</p>
                        :
                        null
                    }
                </Form>
            </Modal.Body>
            <Modal.Footer>
                <Button variant='ghost' onClick={onCancel}>Abbrechen</Button>
                <Button variant={
                    ueContext.uiControl.modalMode === "resetPW" ? "danger" : 
                    ueContext.uiControl.modalMode === "delete" ? "danger" : 
                    "primary"
                    } onClick={onSubmit}>
                    {
                        ueContext.uiControl.modalMode === "edit" ? "Speichern" :
                        ueContext.uiControl.modalMode === "resetPW" ? "Zurücksetzen" :
                        ueContext.uiControl.modalMode === "delete" ? "Löschen" :
                        "Erstellen"
                    }
                </Button>
            </Modal.Footer>
        </Modal>
    )
};