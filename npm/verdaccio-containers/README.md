## Running containers that publishing packages to the Verdaccio

### Goal

We test all templates before the releasing. However, since we test packages without publishing, we may be faced breaking errors. That's why, this compose prepares the real-like test environment.


The duty of this compose is to publish the packages to the [Verdaccio](https://verdaccio.org/) (a custom NPM server) running in the container and to serve the Angular pro template (copied from `abp/templates/app/angular` folder) that is consumed these packages from the Verdaccio.


### Before starting

Make sure the [Docker](https://docs.docker.com/get-docker/) and [Docker Compose](https://docs.docker.com/compose/install/) are installed on your PC.

Before starting, the following command should be run to prepare the environment:

```bash
npm install && npm run prepare
```

### Getting Started

To build and up the compose, run the following command:

```bash
docker-compose rm -f && docker-compose build --build-arg next_version="<version here>" && docker-compose up
```

> Be sure to replace the "<version here>" with the version you want published. E.g: 4.1.0

This command;
- Removes the containers if worked before
- Builds the containers
- Runs the containers.

The processes may take 30~ minutes.

The Angular app will be running when you see the "Listening on http://localhost:4200" log. If you see the log, open the browser and navigate to the http://localhost:4200.

The running Angular app can connect to the backend that is running on http://localhost:44305. Go to the `abp/templates/app/aspnet-core/src/MyCompanyName.MyProjectName.DbMigrator` folder and run the project to create (or update) the database. Then run the HttpApiHost project which is in the `abp/templates/app/aspnet-core/src/MyCompanyName.MyProjectName.HttpApi.HostWithIds` folder. Then refresh the browser. You'll see the Angular app is working properly.
