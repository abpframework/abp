# Microservice Solution: CORS Configuration

````json
//[doc-nav]
{
  "Next": {
    "Name": "Communication in the Microservice solution",
    "Path": "solution-templates/microservice/communication"
  }
}
````

> You must have an ABP Business or a higher license to be able to create a microservice solution.

Cross-Origin Resource Sharing (CORS) is a security feature that allows web applications to make requests to a different domain than the one that served the web page. This feature is essential for microservice solutions that have multiple services and applications running on different domains.

In the microservice solution template, the CORS configuration is set up in the `appsettings.json` file of the related project. Generally, you need to configure CORS for authentication server and gateway applications. Since these applications are the entry points for the front-end applications, they need to accept requests from different origins. The default configuration is as follows:

```json
  "App": {
    "CorsOrigins": "http://localhost:44388,http://localhost:44324,http://localhost:44377"
  }
```