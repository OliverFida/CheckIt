// Component imports
import React, {useState, useEffect, useContext} from 'react';
import moment from 'moment';
import { Button, ButtonGroup } from 'react-bootstrap';
import { HomePageContext } from '../../contexts/HomePageContext';

export default function StundenplanHead(){
    const {hpContext, setHpContext} = useContext(HomePageContext);
    const [elements, setElements] = useState([]);

    useEffect(() => {
            console.log("RoomID changed [Head]: " + hpContext.roomId);
            var dateMonday = moment().weekday(1).add(hpContext.weekOffset, 'weeks');
        
        var newElements = [];
        for(var day = 1; day <= 5; day++){
            var tempDate = moment(dateMonday).add(day - 1, 'days');
            newElements.push(<th key={`th_${day}`}>{tempDate.format("DD.MM.YYYY")}</th>);
        }
        setElements(newElements);
    }, [hpContext.roomId]);

    const onToday = () => {
        setHpContext({...hpContext, weekOffset: 0});
    }

    const onEarlier = () => {
        setHpContext({...hpContext, weekOffset: hpContext.weekOffset - 1});
    }

    const onLater = () => {
        setHpContext({...hpContext, weekOffset: hpContext.weekOffset + 1});
    }

    return(
        <thead>
            <tr>
                <th key="th_0">
                    <ButtonGroup>
                        <Button disabled={hpContext.weekOffset === 0 ? true : false} onClick={onEarlier}>Früher</Button>
                        <Button disabled={hpContext.weekOffset === 0 ? true : false} onClick={onToday}>Heute</Button>
                        <Button disabled={hpContext.weekOffset === 5 ? true : false} onClick={onLater}>Später</Button>
                    </ButtonGroup>
                </th>
                {elements}
            </tr>
        </thead>
    );
}