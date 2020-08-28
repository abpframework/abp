export const PROXY_PATH = '/proxy';
export const PROXY_CONFIG_PATH = `${PROXY_PATH}/generate-proxy.json`;
export const PROXY_WARNING_PATH = `${PROXY_PATH}/README.md`;

export const PROXY_WARNING = `# Proxy Generation Output

This directory includes the output of the latest proxy generation.
The \`services\`, \`models\`, and \`enums\` folders will be overwritten when proxy generation is run again.
Therefore, please do not place your own content in those folders.

In addition, \`generate-proxy.json\` works like a lock file.
It includes information used by the proxy generator, so please do not delete or modify it.

Finally, the name of this folder should not be changed for two reasons:
- Proxy generator will keep creating this folder and you will have multiple copies of the same content.
- ABP Suite generates imports from this folder and uses the path \`/proxy\` when doing so.

`;
