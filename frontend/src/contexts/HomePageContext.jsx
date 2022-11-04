import React, {useState, createContext} from 'react';

export const HomePageContext = createContext(null);

export default function HomePageContextProvider({children}){
    const [hpContext, setHpContext] = useState({
        uiControl: {
            bookingsLoading: false,
            bookingModal: false,
            bookingDeleteModal: false,
        },
        roomSelection: {
            id: null,
            number: null,
            name: null,
            inactive: false
        },
        weekSelection: {
            offset: 0
        },
        bookings: {
            reload: false,
            bookings: [],
            selected: null
        }
    });

    return(
        <HomePageContext.Provider value={{hpContext, setHpContext}}>
            {children}
        </HomePageContext.Provider>
    );
}