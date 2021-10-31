import React, {Component} from 'react';

export class FetchDataGrpc extends Component {
    static displayName = FetchDataGrpc.name;

    constructor(props) {
        super(props);
        this.state = {message: '', loading: true};
    }

    componentDidMount() {
        this.populateWeatherData();
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : <h1>{this.state.message}</h1>

        return (
            <div>
                <h1 id="tabelLabel">Weather forecast</h1>
                <p>This component demonstrates fetching data from the server.</p>
                {contents}
            </div>
        );
    }

    async populateWeatherData() {
        const response = await fetch('orders-module/api/v1/orders/grpc');
        const data = await response.text();
        console.log(data)
        this.setState({message: data, loading: false});
    }
}
