import * as React from 'react'
import {Component} from "react";

const {Provider, Consumer} = React.createContext('light');

class AuthContextProvider extends Component {
    state = {
        user: null,
        loading: true
    }

    login() {
        window.location = "/bff/login";
    }


    async componentDidMount() {
        try {
            if (!this.state.user) {

                this.setState({loading: true})

                let response = await fetch("bff/user", {
                    headers: {
                        'x-csrf': '1'
                    },
                })
                if (response.status === 401) {
                    this.login()
                }
                const data = await response.json();
                this.setState({user: data, loading: false});
            }
        } catch (e) {
        }


    }

    render() {
        return <Provider value={this.state.user}>
            {this.state.loading ? <span>Loading ...</span> : this.props.children}
        </Provider>;
    }
}

export {AuthContextProvider, Consumer as AuthContextConsumer};