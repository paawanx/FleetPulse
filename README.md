# FleetPulse

FleetPulse is a small fleet-management application built as a hands-on learning project.

The purpose of the project is to learn how to design, containerize, deploy, operate, and evolve a modern application using .NET and AWS. The infrastructure is intentionally built incrementally so that the reasoning behind each component and the trade-offs involved can be understood.

This is a learning project, not a production-ready fleet-management platform.

## Architecture

```text
                         GitHub
                            |
                     GitHub Actions
                            |
              +-------------+-------------+
              |                           |
          Backend                     Frontend
              |                           |
         Docker / ECR                npm / Vite
              |                           |
         ECS Fargate                  S3 bucket
              |                           |
        +-----+------+                CloudFront
        |            |
       API         Worker
        |            |
        +-----+------+
              |
          RabbitMQ
              |
             RDS
          PostgreSQL
```

The API and worker run as separate ECS/Fargate services. RabbitMQ provides asynchronous messaging through MassTransit. PostgreSQL is hosted in Amazon RDS. The React frontend is built as static files, uploaded to S3, and served through CloudFront.

Lambda and API Gateway are planned as the next serverless part of the project, with a real FleetPulse use case rather than being added simply to demonstrate another AWS service.

## Current stack

### Application

* .NET 10
* ASP.NET Core Web API
* Entity Framework Core
* PostgreSQL
* MassTransit
* RabbitMQ
* React
* Vite

### Local development

* Docker
* Docker Compose

### AWS

* Amazon ECR
* Amazon ECS / Fargate
* Amazon RDS for PostgreSQL
* Amazon S3
* Amazon CloudFront
* AWS Lambda (planned)
* Amazon API Gateway (planned)
* AWS Systems Manager Parameter Store
* IAM
* CloudWatch Logs
* VPC / subnets / security groups

### CI/CD

* GitHub Actions
* GitHub OIDC federation with AWS
* Path-based change detection
* Automated Docker builds and ECR pushes
* Automated ECS task-definition revision and service deployment
* Automated frontend build, S3 upload, and CloudFront invalidation

## What is working

* Local multi-container development with Docker Compose.
* API containerized using a multi-stage Docker build.
* Worker containerized separately from the API.
* Docker builds use the repository root as the build context so referenced projects can be restored correctly.
* ARM64 images are built for the ECS Fargate runtime used by the project.
* API image is stored in ECR and deployed to the `fleetpulse-api` ECS service.
* Worker image is stored in ECR and deployed to the `fleetpulse-worker` ECS service.
* API exposes `/health` and has an ECS container health check.
* PostgreSQL runs in Amazon RDS.
* Application database configuration is stored in SSM Parameter Store rather than in the container image.
* RabbitMQ runs in ECS and API/worker messaging uses MassTransit.
* ECS networking and security groups are configured for the running services.
* React frontend is built with Vite and deployed to S3.
* CloudFront serves the frontend using a private S3 origin.
* GitHub Actions authenticates to AWS using OIDC rather than long-lived AWS access keys.
* The workflow detects which part of the application changed, avoiding unnecessary backend builds.
* When the API is deployed, the workflow discovers the current API task's public IP and injects it into the frontend build.
* Frontend deployment includes uploading the generated static files to S3 and invalidating CloudFront.

## CI/CD flow

The workflow lives in:

```text
.github/workflows/docker-publish.yml
```

The current flow is approximately:

```text
Push to main
    |
    v
Detect changed paths
    |
    +---- API changed ------> Build ARM64 image
    |                              |
    |                              v
    |                             ECR
    |                              |
    |                              v
    |                         ECS service
    |
    +---- Worker changed ---> Build ARM64 image
    |                              |
    |                              v
    |                             ECR
    |                              |
    |                              v
    |                         ECS service
    |
    +---- Frontend/API -----> Discover API IP
                                   |
                                   v
                              Vite build
                                   |
                                   v
                                  S3
                                   |
                                   v
                              CloudFront
```

The frontend deployment also runs when the API changes because the current architecture exposes the API through the ECS task's public IP. Replacing an ECS task can therefore result in a different IP address.

This is a deliberate temporary design and is one of the next architectural problems to solve.

## Image tagging

ECR images use a short Git commit SHA as the deployment tag.

For example:

```text
v0.1.4
90512fb
latest
```

The commit-based tag provides a direct relationship between an image and the source revision that produced it, while `latest` provides a convenient moving reference.

The deployment process can use the immutable image digest when exact image identity matters.

## What is left

The project is functional, but several areas remain intentionally open for further learning.

### 1. Lambda + API Gateway

Add a meaningful serverless workload to FleetPulse.

Topics to explore:

* .NET Lambda functions
* Lambda execution roles
* API Gateway integration
* Event-driven Lambda triggers
* EventBridge and/or S3 events
* Configuration
* Timeouts
* Retries
* Cold starts
* Failure handling
* Lambda versus ECS for different workloads

The goal is to understand when serverless is a good architectural choice rather than simply adding a Lambda function.

### 2. Stable API endpoint

The frontend currently depends on the public IP of the running ECS API task.

Replace this with a stable endpoint and explore the trade-offs between:

* Application Load Balancer
* API Gateway
* DNS
* Other AWS networking options

Consider cost, TLS, health checks, routing, scalability, and operational complexity.

### 3. Observability

Improve the operational visibility of the application.

Explore:

* Structured application logging
* CloudWatch Logs
* Request/correlation IDs
* Application metrics
* ECS metrics
* Health/readiness checks
* CloudWatch alarms
* Basic operational dashboards
* Troubleshooting failed requests and deployments

### 4. CI/CD quality gates

The pipeline currently focuses heavily on building and deploying.

Add:

* Unit tests
* Integration tests where useful
* Build validation
* Test gates before deployment
* Clear separation between validation and deployment

The goal is to make the deployment pipeline protect the application rather than simply automate deployment.

### 5. ECS deployment behavior

Understand what happens when a deployment succeeds, fails, or becomes unhealthy.

Explore:

* Rolling deployments
* Desired count
* Deployment configuration
* Minimum/maximum healthy percentages
* Task health checks
* Task replacement
* Service stabilization
* Failed deployments
* Rollbacks

### 6. Container and ECR lifecycle

Clean up the container lifecycle.

Explore:

* ECR lifecycle policies
* Untagged images
* Old image cleanup
* Image retention
* Tags versus immutable digests

### 7. Security hardening

Review the current AWS security model.

Explore:

* Least-privilege IAM
* ECS task role versus execution role
* Security-group rules
* Public versus private networking
* RDS exposure
* SSM Parameter Store
* GitHub OIDC permissions
* Removing unnecessary public access

### 8. Infrastructure as Code

Recreate the infrastructure using Terraform.

The goal is to move from manually created AWS resources toward reproducible infrastructure.

Potential resources include:

```text
VPC
ECS
ECR
RDS
S3
CloudFront
IAM
SSM
Lambda
API Gateway
```

### 9. Application quality

Continue improving the application itself.

Explore:

* Unit tests
* Integration tests
* API validation
* Error handling
* Persistence patterns
* Messaging reliability
* Idempotency
* Retry behavior
* Dead-letter/error handling

### 10. Cost management

Understand what the application actually costs to run.

Review:

* ECS/Fargate
* RDS
* CloudFront
* S3
* CloudWatch
* ECR
* NAT/networking
* Lambda
* API Gateway

The goal is to understand the relationship between architecture, usage, reliability, and cost.

## Learning notes

The infrastructure is being built incrementally.

Some decisions in the current implementation are intentionally temporary. They exist because they allow the application to be deployed and the underlying AWS concepts to be learned before introducing additional infrastructure.

The project is therefore not intended to present a single "correct" AWS architecture.

The repository documents what was built, what was learned, the problems encountered, and the next architectural questions to investigate.

Some of the questions being explored include:

* When should a workload use ECS versus Lambda?
* When does an application need a load balancer?
* How should a service get a stable endpoint?
* How should secrets be managed?
* How should CI/CD authenticate with AWS?
* How should asynchronous work be handled?
* How do health checks affect deployments?
* How should cloud infrastructure be reproduced?
* How do architecture decisions affect cost?

## Repository structure

```text
FleetPulse/
├── FleetPulse.Api/              # ASP.NET Core API
├── FleetPulse.Contracts/        # Shared contracts/messages
├── FleetPulse.Infrastructure/   # Data and infrastructure concerns
├── FleetPulse.Worker/           # Background worker / message consumer
├── frontend/                    # React + Vite frontend
├── .github/workflows/           # GitHub Actions CI/CD
├── docker-compose.yml           # Local development stack
├── .dockerignore
└── FleetPulse.slnx
```

## Status

**Working end to end**

* Local development
* Containerized API and worker
* PostgreSQL
* RabbitMQ/MassTransit
* ECS/Fargate deployment
* ECR
* S3 frontend hosting
* CloudFront
* GitHub Actions
* GitHub OIDC authentication
* Automated backend and frontend deployment

**Currently being explored**

* Lambda
* API Gateway
* Stable API endpoint
* Observability
* CI/CD quality gates
* ECS deployment resilience
* Security hardening
* Terraform
* Automated testing
* Cost management
