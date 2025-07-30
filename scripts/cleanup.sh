#!/bin/bash

# Cleanup Kubernetes resources
kubectl delete -f ../k8s/namespace.yaml
kubectl delete -f ../k8s/configmap.yaml
kubectl delete -f ../k8s/secret.yaml
kubectl delete -f ../k8s/deployment.yaml
kubectl delete -f ../k8s/service.yaml
kubectl delete -f ../k8s/hpa.yaml
kubectl delete -f ../k8s/sqlserver/sqlserver-deployment.yaml
kubectl delete -f ../k8s/sqlserver/sqlserver-service.yaml
kubectl delete -f ../k8s/sqlserver/sqlserver-pvc.yaml

echo "Cleanup completed."