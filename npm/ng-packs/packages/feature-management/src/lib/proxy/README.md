# Proxy Generation Output

This directory includes the output of the latest proxy generation.
The files and folders in it will be overwritten when proxy generation is run again.
Therefore, please do not place your own content in this folder.

In addition, `generate-proxy.json` works like a lock file.
It includes information used by the proxy generator, so please do not delete or modify it.

Finally, the name of the files and folders should not be changed for two reasons:
- Proxy generator will keep creating them at those paths and you will have multiple copies of the same content.
- ABP Suite generates files which include imports from this folder.

