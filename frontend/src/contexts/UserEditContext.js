import React, {useState, createContext} from 'react';

export const UserEditContext = createContext(null);

export default function UserEditContextProvider({children}){
    const [ueContext, setUeContext] = useState({
        uiControl:{
            usersLoading: false,
            userModal: false,
            modalMode: ""
        },
        users: {
            users: [],
            reload: true,
            selected: null
        }
    });

    return(
        <UserEditContext.Provider value={{ueContext, setUeContext}}>
            {children}
        </UserEditContext.Provider>
    );
};