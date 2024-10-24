# Configuring SSL certificate(HTTPS)

A website needs an SSL certificate to keep user data secure, verify ownership, prevent attackers from creating a fake version of the site, and gain user trust.

This document introduces how to get and use an SSL certificate (HTTPS) for your application.

## Get an SSL Certificate from a Certificate Authority

You can get a SSL certificate from a certificate authority (CA) such as [Let's Encrypt](https://letsencrypt.org/) or [Cloudflare](https://www.cloudflare.com/learning/ssl/what-is-an-ssl-certificate/) and so on.

Once you have a certificate, you need to configure your web server to use it. The following references show how to configure your web server to use a certificate.

* [Host ASP.NET Core on Linux with Apache: HTTPS configuration](https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-apache)
* [Host ASP.NET Core on Linux with Nginx: HTTPS configuration](https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-nginx)
* [How to Set Up SSL on IIS 7 or later](https://learn.microsoft.com/en-us/iis/manage/configuring-security/how-to-set-up-ssl-on-iis)

### How to get a free SSL certificate from Let's Encrypt?

Let's Encrypt is **a free, automated, and open certificate authority (CA)**. It gives the digital certificates to enable HTTPS (SSL/TLS) for websites. To get a free SSL certificate, we will use [acme.sh](https://github.com/acmesh-official/acme.sh) and Cloudflare DNS API to get a free SSL certificate from [Let's Encrypt](https://letsencrypt.org/).

> If you have any problem with the following steps, you can read the [acme.sh](https://github.com/acmesh-official/acme.sh/wiki/dnsapi) tutorial.

#### Install [acme.sh](https://github.com/acmesh-official/acme.sh)

Ensure that you have `curl` command in your terminal. And run the following command on your terminal:

```bash
curl https://get.acme.sh | sh -s email=my@example.com
```

#### [Cloudflare DNS API token](https://dash.cloudflare.com/profile/api-tokens)

You will need to create an API token which either:

(i) has permission to edit a single specific DNS zone; or
(ii) has permission to edit multiple DNS zones.

You can do this via your Cloudflare profile page under the API Tokens section. When you create the token, under Permissions, select Zone > DNS > Edit, and under Zone Resources, only include the specific DNS zones within which you need to perform ACME DNS challenges.

The API token is a 40-character string that may contain uppercase letters, lowercase letters, numbers, and underscores. You must provide it to acme.sh by setting the environment variable CF_Token to its value, e.g. run export CF_Token="Y_jpG9AnfQmuX5Ss9M_qaNab6SQwme3HWXNDzRWs".

**(i) Single DNS zone**
You must give acme.sh the zone ID of the DNS zone it needs to edit. This is a 32-character hexadecimal string (e.g. 763eac4f1bcebd8b5c95e9fc50d010b4), and should not be confused with the zone name (e.g. example.com). This zone ID can be found via the Cloudflare dashboard on the zone's Overview page in the right-hand sidebar.

You provide this info by setting the environment variable CF_Zone_ID to this zone ID, e.g. run export CF_Zone_ID="763eac4f1bcebd8b5c95e9fc50d010b4".

**(ii) Multiple DNS zones**
You must give acme.sh the account ID of the Cloudflare account to which the relevant DNS zones belong. This is a 32-character hexadecimal string, and should not be confused with other account identifiers, such as the account email address (e.g. alice@example.com) or global API key (which is also a 32-character hexadecimal string). This account ID can be found via the Cloudflare dashboard, as the end of the URL when logged in, or on the Overview page of any of your zones, in the right-hand sidebar, beneath the zone ID.

You provide this info by setting the environment variable CF_Account_ID to this account ID, e.g. run export CF_Account_ID="763eac4f1bcebd8b5c95e9fc50d010b4".

#### Issue a certificate

```bash
> export CF_Token='your_token'
> export CF_Account_ID='your_account_id'
> export CF_Zone_ID='your_zone_id'
> acme.sh --issue --dns dns_cf -d getabp.net

[Info] Domains have changed.
[Info] Using CA: https://acme.zerossl.com/v2/DV90
[Info] Single domain='getabp.net'
[Info] Getting webroot for domain='getabp.net'
[Info] Adding TXT value: 1uEeVFfmwXM7N21Wi9PitgEnhJbl4W4dHeRkapGkRSs for domain: _acme-challenge.getabp.net
[Info] Adding record
[Info] Added, OK
[Info] The TXT record has been successfully added.
[Info] Let's check each DNS record now. Sleeping for 20 seconds first.
[Info] You can use '--dnssleep' to disable public dns checks.
[Info] See: https://github.com/acmesh-official/acme.sh/wiki/dnscheck
[Info] Checking getabp.net for _acme-challenge.getabp.net
[Info] Success for domain getabp.net '_acme-challenge.getabp.net'.
[Info] All checks succeeded
[Info] Verifying: getabp.net
[Info] Processing. The CA is processing your order, please wait. (1/30)
[Info] Success
[Info] Removing DNS records.
[Info] Removing txt: 1uEeVFfmwXM7N21Wi9PitgEnhJbl4W4dHeRkapGkRSs for domain: _acme-challenge.getabp.net
[Info] Successfully removed
[Info] Verification finished, beginning signing.
[Info] Let's finalize the order.
[Info] Le_OrderFinalize='https://acme.zerossl.com/v2/DV90/order/1AP31vqE7rzxCmvpDsDgvA/finalize'
[Info] Order status is 'processing', let's sleep and retry.
[Info] Sleeping for 15 seconds then retrying
[Info] Polling order status: https://acme.zerossl.com/v2/DV90/order/1AP31vqE7rzxCmvpDsDgvA
[Info] Downloading cert.
[Info] Le_LinkCert='https://acme.zerossl.com/v2/DV90/cert/o1jBkRs8LVVBiEZShd4Yow'
[Info] Cert success.
-----BEGIN CERTIFICATE-----
MIID9jCCA3ygAwIBAgIQbJr7iNOSnkMXJjkwngvQRzAKBggqhkjOPQQDAzBLMQsw
CQYDVQQGEwJBVDEQMA4GA1UEChMHWmVyb1NTTDEqMCgGA1UEAxMhWmVyb1NTTCBF
Q0MgRG9tYWluIFNlY3VyZSBTaXRlIENBMB4XDTI0MDkyODAwMDAwMFoXDTI0MTIy
NzIzNTk1OVowFTETMBEGA1UEAxMKZ2V0YWJwLm5ldDBZMBMGByqGSM49AgEGCCqG
SM49AwEHA0IABJ.....io0Kq3W2o0eAgXDVXw2QJ6RlZKi0RGha/D92u/OAqvjX0I4
YEPRRgAm6l2oLg==
-----END CERTIFICATE-----
[Info] Your cert is in: getabp.net.cer
[Info] Your cert key is in: getabp.net.key
[Info] The intermediate CA cert is in: ca.cer
[Info] And the full-chain cert is in: fullchain.cer
```

#### Convert the certificate to PFX format(IIS format)

```bash
openssl pkcs12 -export \
  -in getabp.net.cer \
  -inkey getabp.net.key \
  -out getabp.net.pfx \
  -passout pass:
```

If you want to set a password for the PFX file, you can set the password with `-passout pass:your_password`.

## Common Exceptions

If you encounter the following exceptions, it means your **certificate is not trusted by the client or the certificate is not valid**. 
You will may see the following SSL certificate errors in your browser when you try to access the website.

```cs
---> System.Net.Http.HttpRequestException: The SSL connection could not be established, see inner exception.
---> System.Security.Authentication.AuthenticationException: The remote certificate is invalid according to the validation procedure: RemoteCertificateNameMismatch
```

```cs
---> System.Net.Http.HttpRequestException: The SSL connection could not be established, see inner exception.
---> System.Security.Authentication.AuthenticationException: The remote certificate is invalid because of errors in the certificate chain: UntrustedRoot
```

## References

* [ABP IIS Deployment](./index.md)
* [acme.sh](https://github.com/acmesh-official/acme.sh)
* [acme.sh DNS API](https://github.com/acmesh-official/acme.sh/wiki/dnsapi#dns_cf)
* [HTTPS in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/enforcing-ssl)
* [Let's Encrypt](https://letsencrypt.org/getting-started)
* [Cloudflare's Free SSL / TLS](https://www.cloudflare.com/application-services/products/ssl/)
