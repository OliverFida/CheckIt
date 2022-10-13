import React, {useState, createContext} from 'react';

export const RoomsContext = createContext(null);

export default function RoomsContextProvider({children}){
    const [roomsContext, setRoomsContext] = useState({
        uiControl:{
            roomsLoading: false,
            roomModal: false,
            modalMode: ""
        },
        rooms: {
            rooms: [],
            reload: true,
            selected: null
        }
    });

    return(
        <RoomsContext.Provider value={{roomsContext, setRoomsContext}}>
            {children}
        </RoomsContext.Provider>
    );
};