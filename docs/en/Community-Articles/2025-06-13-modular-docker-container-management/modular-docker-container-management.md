# 🚀 New in ABP Studio: Modular Docker Container Management

We're excited to announce a new improvement to Docker integration in **ABP Studio**!
With the latest update, you can now manage Docker containers **individually**, add or remove them dynamically, and launch them **either separately or collectively** — all within the Studio.

![solution-runner-containers](solution-runner-containers.png)

------

## 🔄 What Has Changed?

In previous versions, Docker dependencies were handled using a **single `docker-compose.override.yml` file**, which was **automatically generated** when creating a new solution if it is needed.

By default, this file included common development dependencies like PostgreSQL, Redis, RabbitMQ, etc., and was executed through a predefined script named `docker-dependencies` in the Solution Runner.

While this approach worked well for simple setups, it had some limitations:

- All services were bundled into a **single compose file**.
- Adding or removing services required modifying this central file.
- It wasn't possible to **start or stop individual containers independently**.

------

## ✅ What's New?

With the latest ABP Studio update:

- Each Docker container can now be defined in its **own `docker-compose` file**.
- Compose files can be **added or removed** from the Studio UI.
- Containers can be:
  - **Started or stopped individually**.
  - **Started/stopped in bulk**.
- The **Solution Runner** recognizes Docker containers and can run them alongside application projects.

------

## ⚠️ Important Notes Before You Start

### Required: Use `container_name` for Docker Service Matching

When working with the new modular Docker system in ABP Studio, each service **must define a `container_name`**.
This name is used by ABP Studio to **identify and map** Docker containers to their corresponding service entries in the Studio UI.

#### Why is this mandatory?

- ABP Studio relies on `container_name` to:
  - Detect whether the service is stopped or continue running.
  - Perform **start**, **stop**, and **status check** operations reliably.
  - Match Studio UI entries with actual running Docker containers.
- Without a `container_name`, container discovery may fail and **service management features might not work as expected**.

#### Example:

```
services:
  redis:
    container_name: redis
    image: redis:7
    ports:
      - "6379:6379"
    networks:
      - my-network
```

> If you do **not** define `container_name`, ABP Studio will be unable to track or control the service properly.

------

## 📥 Migrating from the Old System

If you're using the old method with a centralized compose file:

Before:

```
docker-compose -f docker-compose.override.yml up -d
```

Now:

1. Create separate `docker-compose` files for each service
    (e.g., `docker-compose.postgres.yml`, `docker-compose.redis.yml`).
2. Go to the **Solution Runner tab in ABP Studio**.
3. Use the **“Add Docker Service”** option in Studio to register each file.
4. Optionally remove or archive the old monolithic compose file.

> If your original `docker-compose.override.yml` contains multiple services, you can split it into individual files — Docker Compose and ABP Studio both support modular composition.

------

## ⚙️ Advanced: Shared Network

If your containers need to communicate over a shared network, you can define an external network in each compose file like this:

```
networks:
  my-network:
    external: true
```

ABP Studio automatically creates the network if they do no exist. But if you like you can create the network once using:

```
docker network create my-network
```

------

## 📚 Additional Resources

- Docker Compose Documentation
- ABP Studio Documentation (link to updated docs when ready)
- [Report Issues on GitHub](https://github.com/abpframework/abp/issues)