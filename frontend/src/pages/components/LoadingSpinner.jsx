// Component imports
import React from 'react';
import {Stack, Spinner} from 'react-bootstrap';

export default function LoadingSpinner({children, loading}){
    return(
        <>
        <Spinner animation='border' style={{display: loading ? "block" : "none"}} />
        <Stack style={{display: loading ? "none" : "block"}}>{children}</Stack>
        </>
    );
};