#!/bin/bash

# Set variables
NAMESPACE="devops-k8s-dotnet"
DEPLOYMENT_NAME="webapi"
SQLSERVER_DEPLOYMENT_NAME="sqlserver"

# Create namespace
kubectl create namespace $NAMESPACE

# Apply Kubernetes configurations
kubectl apply -f ../k8s/namespace.yaml -n $NAMESPACE
kubectl apply -f ../k8s/configmap.yaml -n $NAMESPACE
kubectl apply -f ../k8s/secret.yaml -n $NAMESPACE
kubectl apply -f ../k8s/sqlserver/sqlserver-deployment.yaml -n $NAMESPACE
kubectl apply -f ../k8s/sqlserver/sqlserver-service.yaml -n $NAMESPACE
kubectl apply -f ../k8s/sqlserver/sqlserver-pvc.yaml -n $NAMESPACE
kubectl apply -f ../k8s/deployment.yaml -n $NAMESPACE
kubectl apply -f ../k8s/service.yaml -n $NAMESPACE
kubectl apply -f ../k8s/ingress.yaml -n $NAMESPACE
kubectl apply -f ../k8s/hpa.yaml -n $NAMESPACE

# Wait for the deployments to be ready
kubectl rollout status deployment/$DEPLOYMENT_NAME -n $NAMESPACE
kubectl rollout status deployment/$SQLSERVER_DEPLOYMENT_NAME -n $NAMESPACE

echo "Deployment completed successfully!"