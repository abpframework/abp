# Differences between Features & Global Features

[Features](Features.md) & [Global Features](Global-Features.md) are totally different systems.

- Features can be switched at runtime but global features can't be.
- Features can be managed by users & system administrators while application running but Global Features system is for Developers and can be managed at development time.
- Features can be used as Feature-Flag but Global Features can't.
- Features loads all disabled features to application and just hide them but Global Features system doesn't load disabled features and even prevents table creating in database.