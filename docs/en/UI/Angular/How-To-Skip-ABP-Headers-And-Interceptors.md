# How to Skip HTTP interceptors and ABP headers

The ABP Framework adds several HTTP headers to the HttpClient, such as the "Auth token" or "tenant Id". 
The ABP Server must possess the information but the ABP user may not want to send this informations to an external server.
ExternalHttpClient and IS EXTERNAL REQUEST HttpContext Token were added in V6.0.4.
The ABP Http interceptors check the value of the `IS_EXTERNAL_REQUEST` token. If the token is True then ABP-specific headers won't be added to Http Request.
The `ExternalHttpClient` extends from `HTTPClient` and sets the `IS_EXTERNAL_REQUEST` context token to true. 
When you are using `ExternalHttpClient` as HttpClient in your components, it does not add ABP-specific headers.

Note: With `IS_EXTERNAL_REQUEST` or without it, ABP loading service works. 
