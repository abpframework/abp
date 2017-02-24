docker rm $(docker ps -aq)
docker-compose up -d mongodb
docker-compose up -d abpidentity_httpapihost
docker-compose up -d abpdesk_web
sleep 2
docker-compose scale abpdesk_web=2
sleep 2
docker-compose up -d load_balancer