#!/bin/bash
set -e

export SHELL="/bin/bash"

minikube start

eval $(minikube docker-env)
docker-compose -f docker-compose.yml -f docker-compose.migrations.yml -f docker-compose.override.yml build

cd ./k8s
kubectl create -f $(ls -x | grep .yaml | grep -v deployment | tr " \t\n\r" ","  | sed 's/.$//')
kubectl create -f $(ls -x | grep .yaml | grep deployment | tr " \t\n\r" ","  | sed 's/.$//')
