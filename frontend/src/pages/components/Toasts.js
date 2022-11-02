// Component imports
import React, {useContext, useState} from 'react';
import {Toast, ToastContainer} from 'react-bootstrap';

import ToastContextProvider, { ToastContext } from '../../contexts/ToastContext';

export default function Toasts(){
    const {toastContext, setToastContext} = useContext(ToastContext);
    const [show, setShow] = useState(true);
    const [values, setValues] = useState({title: "Test", type: "danger", message: "hallo"});

    return(
        <ToastContextProvider>
            <ToastContainer position="top-center">
                <Toast onClose={() => setShow(false)} show={show} bg={values.type} animation={true} className="mt-5">
                    <Toast.Header>
                        <strong className="me-auto">{values.title}</strong>
                    </Toast.Header>
                    <Toast.Body className="text-white">{values.message}</Toast.Body>
                </Toast>
            </ToastContainer>
        </ToastContextProvider>
    )
}
