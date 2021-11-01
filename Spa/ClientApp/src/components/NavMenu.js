import React, {Component} from 'react';
import {Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink} from 'reactstrap';
import {Link} from 'react-router-dom';
import './NavMenu.css';
import {AuthContextConsumer} from "../Auth/AuthProvider";

export class NavMenu extends Component {
    static displayName = NavMenu.name;

    constructor(props) {
        super(props);

        this.toggleNavbar = this.toggleNavbar.bind(this);
        this.state = {
            collapsed: true
        };
    }

    toggleNavbar() {
        this.setState({
            collapsed: !this.state.collapsed
        });
    }

    async logout(user) {
        const url = user.filter(x => x.type === 'bff:logout_url')[0]
        window.location = url.value
    }

    render() {
        return (
            <AuthContextConsumer>
                {
                    user => (
                        <header>
                            <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3"
                                    light>
                                <Container>
                                    <NavbarBrand tag={Link} to="/">Spa</NavbarBrand>
                                    <NavbarToggler onClick={this.toggleNavbar} className="mr-2"/>
                                    <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed}
                                              navbar>
                                        <ul className="navbar-nav flex-grow">
                                            <NavItem>
                                                <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
                                            </NavItem>
                                            <NavItem>
                                                <NavLink tag={Link} className="text-dark" to="/counter">Counter</NavLink>
                                            </NavItem>
                                            <NavItem>
                                                <NavLink tag={Link} className="text-dark" to="/fetch-data">Fetch data</NavLink>
                                            </NavItem>
                                            <NavItem>
                                                <NavLink tag={Link} className="text-dark" to="/fetch-data-grpc">Fetch data GRPC</NavLink>
                                            </NavItem>
                                            <NavItem>
                                                <NavLink tag={Link} className="text-dark" to="/GraphQL">GraphQL</NavLink>
                                            </NavItem>
                                            <NavItem>
                                                <div className="text-dark" onClick={() => this.logout(user)}>Logout</div>
                                            </NavItem>
                                        </ul>
                                    </Collapse>
                                </Container>
                            </Navbar>
                        </header>
                    )
                }
               
            </AuthContextConsumer>
        );
    }
}
