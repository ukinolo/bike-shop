#!/bin/bash
echo "Clenup docker compose using Dockerfiles"
docker compose -f ./BikeShop/compose.yaml down -v
docker image rm frontend office central-shop