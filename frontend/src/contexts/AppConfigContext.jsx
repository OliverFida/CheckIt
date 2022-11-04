import React, {useState, useEffect, createContext} from 'react';

export const AppConfigContext = createContext(null);

export default function AppConfigContextProvider({children}){
    const [acContext, setAcContext] = useState({
        bookings:{
            days: 5,
            weeksPast: 0,
            weeksFuture: 3
        },
        service:{
            available: true
        }
    });

    useEffect(() => {
        async function doAsync(){
            var temp = acContext;

            // Load Application Config
            try{
                var appConfig = await (await fetch("./application_config.json")).json();
                temp.bookings.days = appConfig.SATURDAY ? 6 : 5;
                temp.bookings.weeksPast = appConfig.WEEKS_PAST;
                temp.bookings.weeksFuture = appConfig.WEEKS_FUTURE;
            }catch(e){}

            setAcContext(temp);
        }
        doAsync();
    }, []);
    
    return(
        <AppConfigContext.Provider value={{acContext, setAcContext}}>
            {children}
        </AppConfigContext.Provider>
    );
}