export default `
.content-header-title {
    font-size: 24px;
}

.entry-row {
    margin-bottom: 15px;
}

#main-navbar-tools a.dropdown-toggle {
    text-decoration: none;
    color: #fff;
}

.navbar .dropdown-submenu {
    position: relative;
}
.navbar .dropdown-menu {
    margin: 0;
    padding: 0;
}
    .navbar .dropdown-menu a {
        font-size: .9em;
        padding: 10px 15px;
        display: block;
        min-width: 210px;
        text-align: left;
        border-radius: 0.25rem;
        min-height: 44px;
    }
.navbar .dropdown-submenu a::after {
    transform: rotate(-90deg);
    position: absolute;
    right: 16px;
    top: 18px;
}
.navbar .dropdown-submenu .dropdown-menu {
    top: 0;
    left: 100%;
}

.card-header .btn {
    padding: 2px 6px;
}
.card-header h5 {
    margin: 0;
}
.container > .card {
    box-shadow: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075) !important;
}

@media screen and (min-width: 768px) {
    .navbar .dropdown:hover > .dropdown-menu {
        display: block;
    }

    .navbar .dropdown-submenu:hover > .dropdown-menu {
        display: block;
    }
}
.input-validation-error {
    border-color: #dc3545;
}
.field-validation-error {
    font-size: 0.8em;
}
`;
