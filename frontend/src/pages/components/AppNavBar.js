// Component imports
import React, {useState, useEffect} from 'react';
import {Navbar, Stack, NavDropdown, Button} from 'react-bootstrap';
import {useNavigate} from 'react-router-dom';
// API imports
import RoomsAPI from '../../api/rooms';

export default function AppNavBar(){
    const navigate = useNavigate();
    const [rooms, setRooms] = useState([]);
    const [roomElements, setRoomElements] = useState([]);

    useEffect(() => {
        getRooms();
    }, []);

    useEffect(() => {
        generateRooms();
    }, [rooms]);

    function getRooms(){
        setRooms(RoomsAPI.getRooms());
    }

    function generateRooms(){
        setRoomElements(rooms.map(room => <NavDropdown.Item key={`room_${room}`}>{room}</NavDropdown.Item>));
    }

    const onLogout = () => {
        navigate("/login");
    };

    return(
        <Navbar bg='light' expand='lg'>
            <Stack direction='horizontal' style={{width: '100%', justifyContent: 'space-between'}}>
                <Navbar.Brand>CHECK-IT</Navbar.Brand>
                <Stack direction='horizontal' gap={3}>
                    <NavDropdown title='Raum' align={'end'}>
                        {roomElements}
                    </NavDropdown>
                    <Button onClick={onLogout}>Abmelden</Button>
                </Stack>
            </Stack>
        </Navbar>
    );
};