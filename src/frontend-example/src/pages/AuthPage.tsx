import {useAuth} from "../contexts/AuthContext.tsx";
import styles from './AuthPage.module.css'

export const AuthPage = () => {
  const auth = useAuth();

  return (
    <>
      <h3>AuthPage</h3>
      
      {auth.isAuthenticated && (<>
        <p>You are authorized</p>
      </>)}

      {!auth.isAuthenticated && (<>
        <p>You are not authorized.</p>
        <div className={styles.loginBox}>
          <input type="text" placeholder="login"/>
          <input type="password" placeholder="password"/>
          <button>Login</button>
        </div>
      </>)}
    </>
  )
}