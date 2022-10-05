import React, {useState, createContext} from 'react';

export const UserPageContext = createContext({showChangePasswordModal: false});

export default function UserPageContextProvider({children}){
    const [upContext, setUpContext] = useState({});

    return(
        <UserPageContext.Provider value={{upContext, setUpContext}}>
            {children}
        </UserPageContext.Provider>
    );
};