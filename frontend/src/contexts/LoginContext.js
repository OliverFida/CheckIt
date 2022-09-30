import React, {useState, createContext} from 'react';

export const LoginContext = createContext(null);

export default function LoginContextProvider({children}){
    const [loginContext, setLoginContext] = useState({username: null, loginToken: null});

    return(
        <LoginContext.Provider value={{loginContext, setLoginContext}}>
            {children}
        </LoginContext.Provider>
    );
}