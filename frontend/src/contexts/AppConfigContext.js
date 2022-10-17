import React, {useState, useEffect, createContext} from 'react';

export const AppConfigContext = createContext(null);

export default function AppConfigContextProvider({children}){
    const [acContext, setAcContext] = useState({
        bookings:{
            days: 5,
            weeksPast: 0,
            weeksFuture: 3
        }
    });

    useEffect(() => {
        async function doAsync(){
            var appConfig = await (await fetch("./application_config.json")).json();

            setAcContext({
                ...acContext,
                bookings:{
                    ...acContext.bookings,
                    days: appConfig.SATURDAY ? 6 : 5,
                    weeksPast: appConfig.WEEKS_PAST,
                    weeksFuture: appConfig.WEEKS_FUTURE
                }
            });
        }
        doAsync();
    }, []);
    
    return(
        <AppConfigContext.Provider value={{acContext, setAcContext}}>
            {children}
        </AppConfigContext.Provider>
    );
}