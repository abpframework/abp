#!/bin/bash
set -e

kubectl create -f $(ls -x | grep .yaml | grep -v deployment | tr " \t\n\r" ","  | sed 's/.$//')
kubectl create -f $(ls -x | grep .yaml | grep deployment | tr " \t\n\r" ","  | sed 's/.$//')

kubectl port-forward deployment.apps/auth-server 51511:51511 &
kubectl port-forward deployment.apps/backend-admin-app 51512:80 &
kubectl port-forward deployment.apps/public-website 51513:80