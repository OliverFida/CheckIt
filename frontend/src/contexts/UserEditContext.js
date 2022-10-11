import React, {useState, createContext} from 'react';

export const UserEditContext = createContext({showEditUserModal: false, showNewUserModal: false});

export default function UserEditContextProvider({children}){
    const [ueContext, setUpContext] = useState({});

    return(
        <UserEditContext.Provider value={{ueContext, setUpContext}}>
            {children}
        </UserEditContext.Provider>
    );
};