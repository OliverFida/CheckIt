// Component imports
import React, {useState, useEffect, useContext} from 'react';
import {Navbar, Stack, Nav, NavDropdown, Button} from 'react-bootstrap';
import {useNavigate} from 'react-router-dom';
import {HomePageContext} from '../../contexts/HomePageContext';
// API imports
import RoomsAPI from '../../api/rooms';

export default function AppNavBar(){
    const navigate = useNavigate();
    
    const onLogout = () => {
        navigate("/login");
    };

    return(
        <Navbar bg='light' expand='lg'>
            <Stack direction='horizontal' style={{width: '100%', justifyContent: 'space-between'}}>
                <Navbar.Brand>CHECK-IT</Navbar.Brand>
                <Nav>
                    <RoomDropDown />
                    <Nav.Item>
                        <Button onClick={onLogout}>Abmelden</Button>
                    </Nav.Item>
                </Nav>
            </Stack>
        </Navbar>
    );
};

function RoomDropDown(){
    const {hpContext, setHpContext} = useContext(HomePageContext);
    const [rooms, setRooms] = useState([]);
    const [elements, setElements] = useState([]);

    useEffect(() => {
        setRooms(RoomsAPI.getRooms());
    }, []);

    useEffect(() => {
        setElements(rooms.map(room => <NavDropdown.Item key={`room_${room.number}`} onClick={() => {onRoomSelect(room.name, room.id)}}>{room.name}</NavDropdown.Item>));
    }, [rooms]);

    const onRoomSelect = (name, id) => {
        setHpContext({...hpContext, roomName: name, roomId: id});
    };

    return(
        <NavDropdown title={hpContext.roomName ? hpContext.roomName : "Raum"} align="end">
            {elements}
        </NavDropdown>
    );
}