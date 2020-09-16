export const PROXY_PATH = '/proxy';
export const PROXY_CONFIG_PATH = `${PROXY_PATH}/generate-proxy.json`;
export const PROXY_WARNING_PATH = `${PROXY_PATH}/README.md`;

export const PROXY_WARNING = `# Proxy Generation Output

This directory includes the output of the latest proxy generation.
The files and folders in it will be overwritten when proxy generation is run again.
Therefore, please do not place your own content in this folder.

In addition, \`generate-proxy.json\` works like a lock file.
It includes information used by the proxy generator, so please do not delete or modify it.

Finally, the name of the files and folders should not be changed for two reasons:
- Proxy generator will keep creating them at those paths and you will have multiple copies of the same content.
- ABP Suite generates files which include imports from this folder.

> **Important Notice:** If you are building a module and are planning to publish to npm,
> some of the generated proxies are likely to be exported from public-api.ts file. In such a case,
> please make sure you export files directly and not from barrel exports. In other words,
> do not include index.ts exports in your public-api.ts exports.
`;
