# MicroserviceDemo

Run demo using docker-compose:

First restore mssql database
```sh
$ docker-compose -f docker-compose.yml -f docker-compose.migrations.yml run restore-database
```

Build and start containers
```sh
$ docker-compose up -d
```

Add this line to your `hosts` file
```
127.0.0.1	auth-server
```
- Windows: `C:\Windows\System32\Drivers\etc\hosts`
- Linux & macOS: `/etc/hosts`


### Accessing the Web User Interfaces
- Kibana: `http://localhost:51510`
- AuthServer: `http://localhost:51511`
- BackendAdminApp: `http://localhost:51512`
- PublicWebsite: `http://localhost:51513`