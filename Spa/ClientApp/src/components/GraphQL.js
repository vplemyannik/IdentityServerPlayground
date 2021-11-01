import React, {Component} from 'react';
import {
    ApolloClient,
    InMemoryCache,
    gql
} from "@apollo/client";

import { split, HttpLink } from '@apollo/client';
import { getMainDefinition } from '@apollo/client/utilities';
import { WebSocketLink } from '@apollo/client/link/ws';

const httpLink = new HttpLink({
    uri: '/orders-module/graphql',
    credentials: "include"
});

const wsLink = new WebSocketLink({
    uri: 'wss://localhost:7000/orders-module/graphql',
    options: {
        reconnect: true,
        connectionParams: {
            credentials: 'include',
        },
    }
});

const splitLink = split(
    ({ query }) => {
        const definition = getMainDefinition(query);
        return (
            definition.kind === 'OperationDefinition' &&
            definition.operation === 'subscription'
        );
    },
    wsLink,
    httpLink,
);

export class GraphQL extends Component {
    static displayName = GraphQL.name;

    constructor(props) {
        super(props);
        this.state = {message: '', count: 0};
        this.client = new ApolloClient({
            link: splitLink,
            cache: new InMemoryCache()
        });
    }

    componentDidMount() {
        const observer = this.client.subscribe({
            query: gql`
       subscription {
            messageAdded {
                message
            }
        }`
        })

        observer.subscribe(value => {
                this.setState({...this.state, message: value.data.messageAdded.message})
            }
        )
    }

    handleClick = async () => {
        await this.client.mutate({
            mutation: gql`
        mutation{
            addMessage(message: { message: "Generated-${this.state.count++}" }){
                message
            }
        }`
        })
        this.setState({...this.state, count: this.state.count++})
    }

    render() {
        return (
            <div>
                <h1 id="tabelLabel">Weather forecast</h1>
                <p>This component demonstrates GRAPHQL web sockets with BFF</p>
                <button onClick={this.handleClick}>Generate and SendMessage</button>
                <h1>{this.state.message}</h1>
            </div>
        );
    }

    async populateWeatherData() {
        const response = await fetch('orders-module/api/v1/orders');
        const data = await response.text();
        this.setState({message: data, loading: false});
    }
}
