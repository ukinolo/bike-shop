#!/bin/bash

set -e

minikube start --driver=docker --kubernetes-version=v1.33.1
minikube addons enable storage-provisioner
minikube addons enable ingress