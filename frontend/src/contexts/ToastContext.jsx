import React, {useState, createContext} from 'react';

export const ToastContext = createContext(null);

export default function ToastContextProvider({children}){
    const [toastContext, setToastContext] = useState({
        toasts: []
    });

    return(
        <ToastContext.Provider value={{toastContext, setToastContext}}>
            {children}
        </ToastContext.Provider>
    );
};