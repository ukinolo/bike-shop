#!/bin/bash

set -e

kubectl apply -f central-shop-deploy.yaml
export BACKEND_NAME=NoviSad
export SHORT_NAME=ns
envsubst < office-shop-deploy.yaml | kubectl apply -f -
export BACKEND_NAME=Kragujevac
export SHORT_NAME=kg
envsubst < office-shop-deploy.yaml | kubectl apply -f -
export BACKEND_NAME=Subotica
export SHORT_NAME=su
envsubst < office-shop-deploy.yaml | kubectl apply -f -
kubectl apply -f frontend-deploy.yaml
kubectl rollout status deployment/frontend-deployment
kubectl apply -f ingress.yaml
kubectl wait --namespace ingress-nginx \
  --for=condition=ready pod \
  --selector=app.kubernetes.io/component=controller \
  --timeout=120s

FRONTEND_URL="http://$(minikube ip)/"
echo "==================================================="
echo "✅ All components are deployed!"
echo "🌐 Access your frontend at: $FRONTEND_URL"
echo "==================================================="
