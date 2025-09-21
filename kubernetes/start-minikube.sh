#!/bin/bash

set -e

minikube start --driver=docker --kubernetes-version=v1.33.1
minikube addons enable storage-provisioner

# apply ingress manually
# because of something my minikube does not have access to the internet
# so I need to manually pull docker images with:
# docker pull registry.k8s.io/ingress-nginx/controller:v1.12.2
# docker pull registry.k8s.io/ingress-nginx/kube-webhook-certgen:v1.5.3

# then I need to add this images to minikube
# minikube image load registry.k8s.io/ingress-nginx/controller:v1.12.2
# minikube image load registry.k8s.io/ingress-nginx/kube-webhook-certgen:v1.5.3


# then I need to use file from this addess in order to use local minikube images
# because ingress imagePullPolicy is Always and I need imagePullPolicy IfNotPresent

# kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.12.2/deploy/static/provider/cloud/deploy.yaml
minikube addons enable ingress