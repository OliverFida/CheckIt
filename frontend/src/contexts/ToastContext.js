import React, {useState, createContext} from 'react';

export const ToastContext = createContext(null);

export default function ToastContextProvider({children}){
    const [toastContext, setToastContext] = useState({
        toasts: [
            {
                title: "Überschrift",
                type: 'typ',
                message: 'Eine Nachricht'
            }
        ]
    });

    return(
        <ToastContext.Provider value={{toastContext, setToastContext}}>
            {children}
        </ToastContext.Provider>
    );
};