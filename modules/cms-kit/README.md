# CMS Kit

## Updating Client Proxies

This project have 3 types of client proxies. Before updating client proxies, make sure `Volo.CmsKit.Web.Unified` project is running.
Then you can update Client proxies in 3 different projects. Execute the following commands in the directory of the each project.

- CMS Kit Public (**Volo.CmsKit.Public.HttpApi.Client**)
    ```bash
    abp generate-proxy -t csharp -url https://localhost:44349 -m cms-kit --without-contracts
    ```

- CMS Kit Common (**Volo.CmsKit.Common.HttpApi.Client**)

    ```bash
    abp generate-proxy -t csharp -url https://localhost:44349 -m cms-kit-common --without-contracts
    ```

- CMS Kit Admin (**Volo.CmsKit.Admin.HttpApi.Client**)

    ```bash
    abp generate-proxy -t csharp -url https://localhost:44349 -m cms-kit-admin --without-contracts
    ```

