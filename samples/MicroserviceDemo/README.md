# MicroserviceDemo

Run demo using docker-compose:

First restore mssql database
```sh
$ docker-compose -f docker-compose.yml -f docker-compose.migrations.yml run restore-database
```

Build and start containers:
```sh
$ docker-compose up -d
```

### Accessing the Web User Interfaces
- AuthServer: `http://127.0.0.1:65999`
- PublicWebsite: `http://127.0.0.1:67999`
- BackendAdminApp: `http://127.0.0.1:67997`