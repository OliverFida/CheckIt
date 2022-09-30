// Compopnent imports
import React from 'react';
import {Stack, Button} from 'react-bootstrap';
import {useNavigate} from 'react-router-dom';

export default function NoPermissionPage(){
    const navigate = useNavigate();

    return(
        <Stack direction='vertical'>
            <h1>Sie haben keine Berechtigungen auf diese Seite!</h1>
            <Button onClick={() => {navigate("/home")}}>Zurück</Button>
        </Stack>
    );
};