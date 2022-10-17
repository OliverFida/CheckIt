// Component imports
import React, {useState, useEffect, useContext} from 'react';
import moment from 'moment';
import { Button, ButtonGroup } from 'react-bootstrap';
import { HomePageContext } from '../../contexts/HomePageContext';

export default function StundenplanHead(){
    const {hpContext, setHpContext} = useContext(HomePageContext);
    const [elements, setElements] = useState([]);

    const amountWeeks = 10;
    const amountDays = 6;

    useEffect(() => {
        var dateMonday = moment().weekday(1).add(hpContext.weekSelection.offset, 'weeks');
        
        var weekDayNames = ["Montag", "Dienstag", "Mittwoch", "Donnerstag", "Freitag", "Samstag"];
        var newElements = [];
        for(var day = 1; day <= amountDays; day++){
            var tempDate = moment(dateMonday).add(day - 1, 'days');
            newElements.push(<th key={`th_${day}`}>{weekDayNames[day - 1]}<br /><span className="muted">{tempDate.format("DD.MM.YYYY")}</span></th>);
        }
        setElements(newElements);
    }, [hpContext.weekSelection.offset]);

    const onToday = () => {
        setHpContext({...hpContext, weekSelection: {...hpContext.weekSelection, offset: 0}});
    }

    const onEarlier = () => {
        setHpContext({...hpContext, weekSelection: {...hpContext.weekSelection, offset: hpContext.weekSelection.offset - 1}});
    }
    
    const onLater = () => {
        setHpContext({...hpContext, weekSelection: {...hpContext.weekSelection, offset: hpContext.weekSelection.offset + 1}});
    }

    // TODO Week offset nach Config
    return(
        <thead>
            <tr>
                <th key="th_0">
                    <ButtonGroup>
                        <Button disabled={hpContext.weekSelection.offset === 0 - amountWeeks ? true : false} onClick={onEarlier}>Früher</Button>
                        <Button disabled={hpContext.weekSelection.offset === 0 ? true : false} onClick={onToday}>Heute</Button>
                        <Button disabled={hpContext.weekSelection.offset === amountWeeks ? true : false} onClick={onLater}>Später</Button>
                    </ButtonGroup>
                </th>
                {elements}
            </tr>
        </thead>
    );
}