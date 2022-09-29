import React, {useState, createContext} from 'react';

export const HomePageContext = createContext(null);

export default function HomePageContextProvider({children}){
    const [hpContext, setHpContext] = useState({roomId: null, roomName: null, weekOffset: 0, bookings: []});

    return(
        <HomePageContext.Provider value={{hpContext, setHpContext}}>
            {children}
        </HomePageContext.Provider>
    );
}