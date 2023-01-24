# How to Skip HTTP interceptors and ABP headers

The ABP Framework adds several HTTP headers to the HttpClient, such as the "Auth token" or "tenant Id." 
ABP Server must possess the information but the ABP user may not want to send this informations to a external server.
ExternalHttpClient and IS EXTERNAL REQUEST HttpContext Token were added in V6.0.4.
THe ABP Http interceptors check value of `IS_EXTERNAL_REQUEST` token. When the token is True, ABP-specific headers don't add to Http Request.
The `ExternalHttpClient`  extends from `HTTPClient` and set `IS_EXTERNAL_REQUEST` Context token to true. 
When you are using `ExternalHttpClient` as HttpClient in your components, it does not add ABP-specific headers.

Note: With `IS_EXTERNAL_REQUEST` or without, ABP loading service work. 
