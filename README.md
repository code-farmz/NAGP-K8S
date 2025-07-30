# DevOps K8s .NET Project

This project is a Kubernetes-based DevOps assignment that implements a .NET Web API microservice with a SQL Server database. The application provides employee list and create via a RESTful API and is designed to be deployed in a Kubernetes environment.

## Quick Start

### Prerequisites
- Docker installed(to build and push image)
- Kubernetes cluster (any cloud provider)
- kubectl configured to connect to your cluster
- .NET Core SDK (for local development)

### Deployment

1. **Clone repo**
```
   git clone https://github.com/code-farmz/NAGP-K8S.git
   cd {to-the-cloned-folder}
```

2. **Build and push Docker Image(Optional step to build and push your own image)**
```bash
docker build -f Dockerfile -t devops-k8s-dotnet-project:latest .
ocker tag devops-k8s-dotnet-project:latest {yourusername}/devops-k8s-dotnet-project:latest
docker push {yourusername}/devops-k8s-dotnet-project:latest
```
**Note:- if you build your own image update deployment.yaml file to point to that image**

3. **Deploy to Kubernetes**
```bash
# Create namespace
kubectl apply -f k8s/namespace.yaml

#Deploy config map and secret
kubectl apply -f k8s/configmap.yaml
kubectl apply -f k8s/secret.yaml

# Deploy SQL Server
kubectl apply -f k8s/sqlserver/sqlserver-pvc.yaml
kubectl apply -f k8s/sqlserver/sqlserver-deployment.yaml
kubectl apply -f k8s/sqlserver/sqlserver-service.yaml

# Deploy application
kubectl apply -f k8s/deployment.yaml
kubectl apply -f k8s/service.yaml
```

4. **Access the Application**
```bash
# Get public access IP
kubectl get nodes -o wide

# You can now access the API Documentation at `http://{publicip}:30080/swagger`.
```

### API Endpoints

- `GET /employee` - Get all employee records
- `POST /employee` - Create new employee
- `GET /swagger` - API documentation

### Cleanup

```
use cleanup.sh available under scriptss folder
```

## Project Structure

The project is organized into several directories:

- **src**: Contains the source code for the Web API.
  - **WebApi**: The main Web API project.
    - **Controllers**: Contains API controllers.
    - **Contracts**: Contains interfaces and dto.
    - **Domain**: Contains services.
    - **Data**: Contains the database context and db models.
    - **Program.cs**: The entry point of the application.
    - **appsettings.json**: Configuration settings.
    - **appsettings.Development.json**: Development-specific settings.
    - **WebApi.csproj**: Project file for the Web API.

- **k8s**: Contains Kubernetes configuration files.
  - **namespace.yaml**: Defines the Kubernetes namespace.
  - **configmap.yaml**: Holds configuration data.
  - **secret.yaml**: Stores sensitive information.
  - **deployment.yaml**: Defines the deployment for the Web API.
  - **service.yaml**: Defines the service for the Web API.
  - **sqlserver**: Contains SQL Server deployment files.
    - **sqlserver-deployment.yaml**: Deployment for SQL Server.
    - **sqlserver-service.yaml**: Service for SQL Server.
    - **sqlserver-pvc.yaml**: Persistent Volume Claim for SQL Server.

- **docker**: Contains Docker-related files.
  - **Dockerfile**: Instructions to build the Docker image.

- **scripts**: Contains deployment and cleanup scripts.
  - **deploy.sh**: Script to deploy the application.
  - **cleanup.sh**: Script to clean up resources.

- **docs**: Contains documentation files.
  - **architecture.md**: Documentation on application architecture.

- **.gitignore**: Specifies files to be ignored by Git.

- **docker-compose.yml**: Defines services for local development.

## DockerHub Image Links

- DockerHub Image: [jitenderkundu/devops-k8s-dotnet-project](https://hub.docker.com/r/your-dockerhub-username/devops-k8s-dotnet-project)

## Sample API Output

To fetch employee records, send a GET request to the following endpoint:
```
GET /employees
```

### Example Response:
```json
[
    {
        "id": 1,
        "name": "John Doe",
        "email": "john.doe@example.com",
        "createdAt": "2025-07-24T12:03:25.9292271Z"
    },
    {
        "id": 2,
        "name": "Jane Smith",
        "email": "jane.smith@example.com",
        "createdAt": "2025-07-24T12:03:25.9292426Z"
    }
]
```

## Conclusion

This project demonstrates the integration of a .NET Web API with a SQL Server database, deployed in a Kubernetes environment, showcasing modern DevOps practices.