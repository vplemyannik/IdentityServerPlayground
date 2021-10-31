import React, {Component} from 'react';
import {Route} from 'react-router';
import {Layout} from './components/Layout';
import {Home} from './components/Home';
import {FetchData} from './components/FetchData';
import {Counter} from './components/Counter';

import './custom.css'
import {AuthContextProvider} from "./Auth/AuthProvider";
import {FetchDataGrpc} from "./components/FetchDataGrpc";


export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <AuthContextProvider>
                <Layout>
                    <Route exact path='/' component={Home}/>
                    <Route path='/counter' component={Counter}/>
                    <Route path='/fetch-data' component={FetchData}/>
                    <Route path='/fetch-data-grpc' component={FetchDataGrpc}/>
                </Layout>
            </AuthContextProvider>
        );
    }
}
