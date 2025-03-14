// Component imports
import React, {useContext, useEffect} from 'react';
import {Modal} from 'react-bootstrap';
import {AppConfigContext} from '../../contexts/AppConfigContext';
// API imports
import APIBase from '../../api/base';

export default function APIStatus(){
    const {acContext, setAcContext} = useContext(AppConfigContext);

    useEffect(() => {
        var temp = setInterval(async () => {
            var response = await getStatus();
            if(!response) clearInterval(temp);
        }, 5000);
    }, []);
    
    const getStatus = async () => {
        var response = await APIBase.ping();

        await setAcContext({...acContext, service:{...acContext.service, available: response}});
        return response;
    };

    return(
        <>
        {acContext.service.available ? null : 
        <Modal show={!acContext.service.available} centered>
            <Modal.Header>
                <Modal.Title>Service Information</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <p>Der Service ist aktuell nicht verfügbar!<br />
                Bitte versuchen Sie es später erneut.<br />
                <br />
                Ist der Service länger nicht erreichbar,<br />
                wenden Sie sich bitte an einen Administrator.</p>
            </Modal.Body>
        </Modal>
        }
        </>
    );
};