import React, {useState, createContext} from 'react';

export const RoomsContext = createContext({showEditRoomModal: false, showNewRoomModal: false});

export default function RoomsContextProvider({children}){
    const [roomsContext, setUpContext] = useState({});

    return(
        <RoomsContext.Provider value={{roomsContext, setUpContext}}>
            {children}
        </RoomsContext.Provider>
    );
};