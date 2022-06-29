import React from 'react';
import {BrowserRouter as Router, Route, Switch} from 'react-router-dom';


import {Home} from './components/Home';
import {AddHotel} from './components/AddUser';
import {EditHotel} from './components/EditUser';
import {AddRoom} from './components/AddUser';
import {EditRoom} from './components/EditUser';
import {GlobalProvider} from './context/GlobalState';






function App() {
  return (
    <div style={{maxWidth:"30rem",margin:"4rem auto"}}>

      <GlobalProvider>
        <Router>
          <Switch>
             <Route exact path="/" component={Home}/>
             <Route path="/add" component={AddUser}/>
             <Route path="/edit/:id" component={EditUser}/>
           
            </Switch>
 
        </Router>
      </GlobalProvider>