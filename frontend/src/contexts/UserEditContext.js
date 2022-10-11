import React, {useState, createContext} from 'react';

export const UserEditContext = createContext({showEditUserModal: false, showNewUserModal: false});

export default function UserEditContextProvider({children}){
    const [ueContext, setUeContext] = useState({});

    return(
        <UserEditContext.Provider value={{ueContext, setUeContext}}>
            {children}
        </UserEditContext.Provider>
    );
};