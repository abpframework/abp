# MicroserviceDemo

Run demo using docker-compose:
First restore mssql database:

```sh
$ docker-compose -f docker-compose.yml -f docker-compose.migrations.yml run restore-database
```

Build and start containers:

```sh
$ docker-compose up -d
```