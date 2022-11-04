// Component imports
import React, {useContext, useState, useEffect} from 'react';
import {Toast, ToastContainer} from 'react-bootstrap';

import { ToastContext } from '../../contexts/ToastContext';

export default function Toasts(){
    const {toastContext, setToastContext} = useContext(ToastContext);

    return(
        <ToastContainer position="top-center">
            {toastContext.toasts}
        </ToastContainer>
    );
}

export function CustomToast({title, preventAutoHide, children}){
    const [state, setState] = useState(false);
    const autoHide = preventAutoHide ? false : true;

    useEffect(() => {
        setState(true);
    }, []);

    const onClose = async () => {
        setState(false);
    };

    return(
        <Toast show={state} onClose={onClose} delay={6000} autohide={autoHide} key="children">
            <Toast.Header>
                <strong className="me-auto">
                    {title}
                </strong>
            </Toast.Header>
            <Toast.Body>
                {children}
            </Toast.Body>
        </Toast>
    )
};

export function InfoToast({preventAutoHide, children}){
    return(
        <CustomToast title="Information" preventAutoHide={preventAutoHide}>
            {children}
        </CustomToast>
    );
};

export function WarningToast({children}){
    return(
        <CustomToast title="Warnung">
            {children}
        </CustomToast>
    );
};

export function ErrorToast({children}){
    return(
        <CustomToast title="Fehler">
            {children}
        </CustomToast>
    );
};