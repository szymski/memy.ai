import reactLogo from './assets/react.svg'
import './App.css'
import {BrowserRouter, Link, Route, Routes} from "react-router-dom";
import {AuthPage} from "./pages/AuthPage.tsx";

function App() {
  return (
    <>
      <div>
        <a href="#">
          <img src={reactLogo} className="logo react" alt="React logo"/>
        </a>
      </div>
      <h3>memy.ai - experimental frontend features</h3>
      <BrowserRouter>
        <Routes>
          <Route path={"auth"} Component={AuthPage}/>
          <Route path={"test"} Component={() => (
            <>
              Hello on test!
            </>
          )}/>
          <Route index Component={() =>
            <>
              <ul style={{listStyle: "none", padding: 0}}>
                <p>Please navigate to a page: </p>
                <li>
                  <Link to={"auth"}>Auth</Link>
                </li>
                <li>
                  <Link to={"test"}>Test</Link>
                </li>
              </ul>
            </>
          }/>
        </Routes>
      </BrowserRouter>
    </>
  )
}

export default App
