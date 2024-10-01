#!/bin/bash

# Configure Dotenv
cp .env.example .env
cp api/.env.example api/.env
cp api/.env.example api/.env

# Building Frontend
cd ui/
ng build
cd ../

# Run Containers
docker-compose build
docker-compose up -d

sleep 2
docker-compose up -d

# Run Migrations
cd api/
dotnet ef database update
cd ../
