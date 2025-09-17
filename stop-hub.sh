#!/bin/bash
echo "Clenup docker compose using dockehub"
docker compose -f ./compose.yaml down -v
docker image rm ukinolo/frontend-bike-shop ukinolo/office-bike-shop ukinolo/central-bike-shop