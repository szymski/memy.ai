import {createContext, useContext, useState} from "react";

type AuthContextType = {
  isAuthenticated: boolean;
} & ({
  isAuthenticated: false;
} | {
  isAuthenticated: true;
  user: User;
});

interface User {
  id: number;
  username: string;
  email: string;
}

export const AuthContext = createContext<AuthContextType>({} as AuthContextType);

export const AuthProvider = ({children}: { children: React.ReactNode }) => {
  const [state, setState] = useState<AuthContextType>({
    isAuthenticated: false,
  });
  
  return (
    <AuthContext.Provider value={state}>
      {children}
    </AuthContext.Provider>
  )
}

export const useAuth = () => useContext(AuthContext)