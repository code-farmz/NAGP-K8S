# DevOps K8s .NET Project - Complete Documentation

## Table of Contents
1. [Project Overview](#project-overview)
2. [Architecture](#architecture)
3. [Technology Stack](#technology-stack)
4. [Project Structure](#project-structure)
5. [API Specification](#api-specification)
6. [Database Design](#database-design)
7. [Kubernetes Configuration](#kubernetes-configuration)
8. [Docker Configuration](#docker-configuration)
9. [Deployment Guide](#deployment-guide)
10. [Monitoring & Scaling](#monitoring--scaling)
11. [Security Configuration](#security-configuration)
12. [Troubleshooting](#troubleshooting)
13. [Development Guidelines](#development-guidelines)

## Project Overview

This is a cloud-native .NET microservice application designed for Kubernetes deployment. The project demonstrates modern DevOps practices including containerization, orchestration, auto-scaling, and infrastructure as code.

### Key Features
- RESTful API for employee management
- Containerized .NET 9.0 Web API
- SQL Server database with persistent storage
- Kubernetes-native deployment
- Horizontal Pod Autoscaling (HPA)
- Health checks and monitoring
- Swagger API documentation
- Production-ready configuration

### Business Requirements
- **Employee Management**: Create and retrieve employee records
- **API Documentation**: Interactive Swagger UI
- **Scalability**: Auto-scaling based on CPU and memory usage
- **High Availability**: Multi-replica deployment
- **Data Persistence**: Reliable SQL Server storage

## Architecture

### High-Level Architecture
```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   User/Client   │────│  Load Balancer  │────│   API Gateway   │
└─────────────────┘    └─────────────────┘    └─────────────────┘
                                                        │
                       ┌────────────────────────────────┴────────────────────────────────┐
                       │                     Kubernetes Cluster                          │
                       │                                                                 │
                       │  ┌─────────────┐    ┌─────────────┐    ┌─────────────┐        │
                       │  │   WebAPI    │    │   WebAPI    │    │   WebAPI    │        │
                       │  │   Pod 1     │    │   Pod 2     │    │   Pod N     │        │
                       │  └─────────────┘    └─────────────┘    └─────────────┘        │
                       │           │                   │                   │            │
                       │           └───────────────────┼───────────────────┘            │
                       │                              │                                │
                       │                    ┌─────────────┐                           │
                       │                    │ SQL Server  │                           │
                       │                    │    Pod      │                           │
                       │                    └─────────────┘                           │
                       │                           │                                  │
                       │                 ┌─────────────────┐                         │
                       │                 │ Persistent      │                         │
                       │                 │ Volume (PVC)    │                         │
                       │                 └─────────────────┘                         │
                       └─────────────────────────────────────────────────────────────┘
```

### Component Interaction
1. **Client** sends HTTP requests to the Kubernetes cluster
2. **NodePort Service** routes traffic to available Web API pods
3. **Web API pods** process requests and interact with SQL Server
4. **SQL Server** provides data persistence with PVC storage
5. **HPA** monitors resource usage and scales pods automatically

## Technology Stack

### Backend Technologies
- **.NET 9.0**: Latest LTS version of .NET
- **ASP.NET Core Web API**: RESTful API framework
- **Entity Framework Core**: ORM for database operations
- **SQL Server 2022**: Relational database
- **Swagger/OpenAPI**: API documentation

### DevOps & Infrastructure
- **Docker**: Containerization platform
- **Kubernetes**: Container orchestration
- **Google Cloud Platform (GCP)**: Cloud provider
- **Google Kubernetes Engine (GKE)**: Managed Kubernetes
- **Persistent Volumes**: Data persistence
- **Horizontal Pod Autoscaler**: Auto-scaling

### Development Tools
- **C#**: Primary programming language
- **Visual Studio Code**: Development environment
- **Git**: Version control
- **Docker Hub**: Container registry

## Project Structure

```
devops-k8s-dotnet-project/
├── src/
│   └── WebApi/
│       ├── Controllers/
│       │   └── EmployeesController.cs      # API endpoints
│       ├── Contracts/
│       │   ├── DTO/
│       │   │   └── EmployeeDto.cs          # Data transfer objects
│       │   └── Interface/
│       │       └── IEmployeeService.cs     # Service interface
│       ├── Data/
│       │   └── Models/
│       │       ├── ApplicationDbContext.cs # EF DbContext
│       │       └── Employee.cs             # Entity model
│       ├── Domain/
│       │   └── Services/
│       │       └── EmployeeService.cs      # Business logic
│       ├── Program.cs                      # Application entry point
│       ├── appsettings.json               # Configuration
│       ├── appsettings.Development.json   # Dev configuration
│       └── WebApi.csproj                  # Project file
├── k8s/
│   ├── namespace.yaml                     # Kubernetes namespace
│   ├── configmap.yaml                     # Configuration data
│   ├── secret.yaml                        # Sensitive data
│   ├── deployment.yaml                    # Web API deployment
│   ├── service.yaml                       # NodePort service
│   ├── hpa.yaml                          # Horizontal Pod Autoscaler
│   └── sqlserver/
│       ├── sqlserver-statefulset.yaml     # SQL Server StatefulSet
│       ├── sqlserver-service.yaml         # SQL Server service
│       └── sqlserver-pvc.yaml            # Legacy PVC (not used)
├── scripts/
│   ├── deploy.sh                         # Deployment script (Linux/Mac)
│   ├── build-and-deploy.ps1              # Deployment script (Windows)
│   └── cleanup.sh                        # Cleanup script
├── docs/
│   ├── architecture.md                   # Architecture documentation
│   └── PROJECT_DOCUMENTATION.md          # This file
├── Dockerfile                            # Container build instructions
├── docker-compose.yml                   # Local development setup
└── README.md                            # Project overview
```

## API Specification

### Base URL
- **Development**: `http://localhost:8080`
- **Production**: `http://NODE_IP:30080`

### Authentication
Currently, the API does not implement authentication. All endpoints are publicly accessible.

### Endpoints

#### 1. Get All Employees
- **Method**: `GET`
- **Path**: `/employees`
- **Description**: Retrieves all employee records
- **Response**: Array of employee objects

**Example Request**:
```bash
curl -X GET "http://NODE_IP:30080/employees" -H "accept: application/json"
```

**Example Response**:
```json
[
    {
        "id": 1,
        "name": "John Doe",
        "email": "john.doe@example.com",
        "createdAt": "2025-07-30T12:03:25.9292271Z"
    },
    {
        "id": 2,
        "name": "Jane Smith",
        "email": "jane.smith@example.com",
        "createdAt": "2025-07-30T12:03:25.9292426Z"
    }
]
```

#### 2. Create Employee
- **Method**: `POST`
- **Path**: `/employees`
- **Description**: Creates a new employee record
- **Request Body**: Employee object (without ID)

**Example Request**:
```bash
curl -X POST "http://NODE_IP:30080/employees" \
     -H "accept: application/json" \
     -H "Content-Type: application/json" \
     -d '{
       "name": "Alice Johnson",
       "email": "alice.johnson@example.com"
     }'
```

**Example Response**:
```json
{
    "id": 3,
    "name": "Alice Johnson",
    "email": "alice.johnson@example.com",
    "createdAt": "2025-07-30T14:30:15.1234567Z"
}
```

#### 3. API Documentation
- **Method**: `GET`
- **Path**: `/swagger`
- **Description**: Interactive API documentation
- **URL**: `http://NODE_IP:30080/swagger`

### Error Responses

#### 400 Bad Request
```json
{
    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
    "title": "One or more validation errors occurred.",
    "status": 400,
    "errors": {
        "Name": ["The Name field is required."],
        "Email": ["The Email field is required."]
    }
}
```

#### 500 Internal Server Error
```json
{
    "message": "Internal server error"
}
```

## Database Design

### Employee Table Schema

```sql
CREATE TABLE [dbo].[Employees] (
    [Id] int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL DEFAULT (getutcdate())
);
```

### Entity Relationships
- Currently, the application has a single `Employee` entity
- Future extensions could include departments, roles, etc.

### Data Seeding
The application automatically seeds initial data:
- John Doe (john.doe@example.com)
- Jane Smith (jane.smith@example.com)

### Connection Configuration
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=sqlserver-service;Database=EmployeeDB;User Id=sa;Password=${SA_PASSWORD};TrustServerCertificate=true;"
  }
}
```

## Kubernetes Configuration

### Namespace
```yaml
apiVersion: v1
kind: Namespace
metadata:
  name: devops-k8s-dotnet-project
```

### Deployment Strategy
- **Web API**: RollingUpdate (default) with 4 replicas
- **SQL Server**: Recreate strategy to prevent data corruption
- **Resource Limits**: CPU and memory limits defined for both components

### Service Configuration
- **Type**: NodePort
- **External Port**: 30080
- **Internal Port**: 80
- **Target Port**: 80 (Web API container)

### Persistent Storage
- **Volume**: 10Gi persistent volume for SQL Server
- **Storage Class**: Uses default storage class
- **Access Mode**: ReadWriteOnce

### Auto-scaling Configuration
```yaml
apiVersion: autoscaling/v2
kind: HorizontalPodAutoscaler
metadata:
  name: employeeapp-webapi-hpa
spec:
  scaleTargetRef:
    apiVersion: apps/v1
    kind: Deployment
    name: employeeapp-webapi
  minReplicas: 2
  maxReplicas: 10
  metrics:
    - type: Resource
      resource:
        name: cpu
        target:
          type: Utilization
          averageUtilization: 70
    - type: Resource
      resource:
        name: memory
        target:
          type: Utilization
          averageUtilization: 80
```

## Docker Configuration

### Multi-stage Build
The Dockerfile uses a multi-stage build approach:

1. **Base Stage**: Runtime environment with .NET 9.0 runtime
2. **Build Stage**: SDK environment for building the application
3. **Publish Stage**: Optimized build for production
4. **Final Stage**: Minimal runtime image

### Image Details
- **Base Image**: `mcr.microsoft.com/dotnet/aspnet:9.0`
- **Build Image**: `mcr.microsoft.com/dotnet/sdk:9.0`
- **Registry**: Docker Hub (`jitenderkundu/devops-k8s-dotnet-project`)
- **Port**: 80
- **Working Directory**: `/app`

### Environment Variables
- `ASPNETCORE_URLS=http://+:80`
- `ASPNETCORE_ENVIRONMENT=Production`

## Deployment Guide

### Prerequisites
- Docker installed and configured
- Kubernetes cluster (GKE, AKS, EKS, or local)
- kubectl configured to connect to your cluster
- DockerHub account (optional, for custom images)

### Step-by-Step Deployment

#### 1. Prepare the Environment
```bash
# Clone the repository
git clone https://github.com/code-farmz/NAGP-K8S.git
cd NAGP-K8S

# Verify kubectl connection
kubectl cluster-info
```

#### 2. Configure GCP (if using GKE)
```bash
# Create GKE cluster
gcloud container clusters create devops-cluster \
    --zone=us-central1-a \
    --num-nodes=3 \
    --enable-autoscaling \
    --min-nodes=1 \
    --max-nodes=10

# Get credentials
gcloud container clusters get-credentials devops-cluster --zone=us-central1-a

# Configure firewall for NodePort
gcloud compute firewall-rules create allow-nodeport \
    --allow tcp:30000-32767 \
    --source-ranges 0.0.0.0/0 \
    --description "Allow NodePort services"
```

#### 3. Deploy to Kubernetes
```bash
# Option A: Use PowerShell script (Windows)
.\build-and-deploy.ps1

# Option B: Use bash script (Linux/Mac)
chmod +x scripts/deploy.sh
./scripts/deploy.sh

# Option C: Manual deployment
kubectl apply -f k8s/namespace.yaml
kubectl apply -f k8s/configmap.yaml
kubectl apply -f k8s/secret.yaml
kubectl apply -f k8s/sqlserver/
kubectl apply -f k8s/deployment.yaml
kubectl apply -f k8s/service.yaml
kubectl apply -f k8s/hpa.yaml
```

#### 4. Verify Deployment
```bash
# Check all resources
kubectl get all -n devops-k8s-dotnet-project

# Check pod status
kubectl get pods -n devops-k8s-dotnet-project -w

# Check logs
kubectl logs -l app=employeeapp-webapi -n devops-k8s-dotnet-project

# Get access information
kubectl get nodes -o wide
kubectl get service employee-api-service -n devops-k8s-dotnet-project
```

#### 5. Access the Application
```bash
# Get node external IP and service NodePort
NODE_IP=$(kubectl get nodes -o jsonpath='{.items[0].status.addresses[?(@.type=="ExternalIP")].address}')
NODE_PORT=$(kubectl get service employee-api-service -n devops-k8s-dotnet-project -o jsonpath='{.spec.ports[0].nodePort}')

echo "Access your application at: http://$NODE_IP:$NODE_PORT"
echo "Swagger documentation: http://$NODE_IP:$NODE_PORT/swagger"
```

### Cleanup
```bash
# Delete all resources
kubectl delete namespace devops-k8s-dotnet-project

# Or use cleanup script
chmod +x scripts/cleanup.sh
./scripts/cleanup.sh
```

## Monitoring & Scaling

### Horizontal Pod Autoscaler (HPA)
The application uses HPA to automatically scale based on:
- **CPU Utilization**: Target 70%
- **Memory Utilization**: Target 80%
- **Min Replicas**: 2
- **Max Replicas**: 10

### Monitoring Commands
```bash
# Monitor HPA status
kubectl get hpa -n devops-k8s-dotnet-project -w

# View resource usage
kubectl top pods -n devops-k8s-dotnet-project
kubectl top nodes

# Monitor scaling events
kubectl describe hpa employeeapp-webapi-hpa -n devops-k8s-dotnet-project
```

### Resource Limits
```yaml
resources:
  requests:
    memory: "256Mi"
    cpu: "250m"
  limits:
    memory: "512Mi"
    cpu: "500m"
```

### Health Checks
The application includes built-in health checks:
- **Liveness Probe**: Ensures the application is running
- **Readiness Probe**: Ensures the application is ready to serve traffic

## Security Configuration

### Secrets Management
Sensitive data is stored in Kubernetes secrets:
```yaml
apiVersion: v1
kind: Secret
metadata:
  name: db-credentials
  namespace: devops-k8s-dotnet-project
type: Opaque
data:
  connection-string: <base64-encoded-connection-string>
  sa-password: <base64-encoded-password>
```

### Network Security
- Services are isolated within the Kubernetes namespace
- SQL Server is only accessible within the cluster
- Web API is exposed via NodePort for external access

### Container Security
- Non-root user execution (where possible)
- Minimal base images
- No unnecessary packages or tools

### Recommendations for Production
1. **Enable HTTPS/TLS**: Configure SSL certificates
2. **Authentication**: Implement JWT or OAuth2
3. **Authorization**: Add role-based access control
4. **Network Policies**: Restrict pod-to-pod communication
5. **Image Scanning**: Scan container images for vulnerabilities
6. **Secrets Rotation**: Implement automated secret rotation

## Troubleshooting

### Common Issues

#### 1. Pods Not Starting
```bash
# Check pod status and events
kubectl describe pods -n devops-k8s-dotnet-project

# Check logs
kubectl logs <pod-name> -n devops-k8s-dotnet-project

# Common causes:
# - Image pull errors
# - Resource constraints
# - Configuration issues
```

#### 2. Database Connection Issues
```bash
# Check SQL Server status
kubectl get pods -l app=sqlserver -n devops-k8s-dotnet-project

# Check SQL Server logs
kubectl logs <sqlserver-pod> -n devops-k8s-dotnet-project

# Test connectivity
kubectl exec -it <webapi-pod> -n devops-k8s-dotnet-project -- ping sqlserver-service
```

#### 3. Service Not Accessible
```bash
# Verify service configuration
kubectl get service employee-api-service -n devops-k8s-dotnet-project

# Check endpoints
kubectl get endpoints employee-api-service -n devops-k8s-dotnet-project

# Verify firewall rules (GCP)
gcloud compute firewall-rules list --filter="name=allow-nodeport"
```

#### 4. HPA Not Scaling
```bash
# Check HPA status
kubectl describe hpa employeeapp-webapi-hpa -n devops-k8s-dotnet-project

# Verify metrics server
kubectl get apiservice v1beta1.metrics.k8s.io -o yaml

# Check resource requests are set
kubectl describe deployment employeeapp-webapi -n devops-k8s-dotnet-project
```

### Debug Commands
```bash
# Get all resources in namespace
kubectl get all -n devops-k8s-dotnet-project

# Describe deployment
kubectl describe deployment employeeapp-webapi -n devops-k8s-dotnet-project

# Check events
kubectl get events -n devops-k8s-dotnet-project --sort-by='.lastTimestamp'

# Port forward for local access
kubectl port-forward service/employee-api-service 8080:80 -n devops-k8s-dotnet-project

# Execute commands in pod
kubectl exec -it <pod-name> -n devops-k8s-dotnet-project -- /bin/bash
```

## Development Guidelines

### Code Structure
- **Controllers**: Handle HTTP requests and responses
- **Services**: Implement business logic
- **DTOs**: Data transfer objects for API communication
- **Models**: Entity models for database operations

### Best Practices
1. **Dependency Injection**: Use built-in DI container
2. **Async/Await**: Use async methods for database operations
3. **Error Handling**: Implement proper exception handling
4. **Logging**: Use structured logging with ILogger
5. **Configuration**: Use appsettings.json and environment variables

### Environment Management
- **Development**: Local Docker Compose setup
- **Staging**: Kubernetes cluster with staging configuration
- **Production**: Production Kubernetes cluster with production configuration

---

