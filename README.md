# FleetPulse

FleetPulse is a small fleet-management application used as a hands-on learning project for building and operating a cloud-native application on AWS.

The goal is to learn the pieces of a real application end to end: application architecture, containers, messaging, databases, AWS networking, IAM, ECS, CI/CD, and static frontend delivery.

This is intentionally a learning project rather than a production-ready platform. The infrastructure is kept relatively simple and cost-conscious so that each AWS component can be understood and operated directly.

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

The API and worker run as separate ECS/Fargate services. RabbitMQ is used for asynchronous messaging through MassTransit. PostgreSQL is hosted in Amazon RDS. The React frontend is built as static files, uploaded to S3, and served through CloudFront.

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
* API containerized with a multi-stage Docker build.
* Worker containerized separately from the API.
* Docker builds use the repository root as build context so projects referenced by the API and worker can be restored correctly.
* ARM64 images are built for the ECS Fargate runtime used by the project.
* API image is stored in ECR and deployed to the `fleetpulse-api` ECS service.
* Worker image is stored in ECR and deployed to the `fleetpulse-worker` ECS service.
* API has an ECS health check and exposes `/health` on port 8080.
* PostgreSQL runs in Amazon RDS and the API connects to it using configuration stored in SSM Parameter Store.
* RabbitMQ runs in ECS and the API/worker communicate through it using MassTransit.
* ECS networking and security groups are configured for the running services.
* Frontend is built with Vite and deployed to S3.
* CloudFront serves the frontend using a private S3 origin with CloudFront access control.
* GitHub Actions authenticates to AWS using OIDC rather than storing long-lived AWS access keys.
* The GitHub workflow detects which part of the application changed, so backend images are not rebuilt unnecessarily for frontend-only changes.
* When the API changes, the workflow discovers the current running API task's public IP and injects it into the frontend build so the deployed frontend points at the current API task.

## CI/CD flow

The workflow lives in `.github/workflows/docker-publish.yml`.

A simplified flow is:

```text
Push to main
    |
    v
Detect changed paths
    |
    +---- API changed ------> Build ARM64 image -> ECR -> ECS
    |
    +---- Worker changed ---> Build ARM64 image -> ECR -> ECS
    |
    +---- Frontend/API -----> Discover API IP -> Vite build -> S3 -> CloudFront
```

The API is deliberately included in the frontend deployment trigger because replacing an ECS task can result in a different public IP. The current setup therefore rebuilds the frontend when the API is deployed.

## What is left

The project is functional, but several areas remain intentionally open for further learning:

1. **Stable API endpoint**

   * Remove the frontend's dependency on an ECS task public IP.
   * Learn the AWS options for exposing an ECS service through a stable endpoint and compare their cost and operational trade-offs.

2. **Observability**

   * Improve CloudWatch logging and make logs easier to navigate.
   * Add useful application and infrastructure metrics.
   * Learn alarms and basic operational dashboards.

3. **Deployment and failure behavior**

   * Understand ECS rolling deployments in more detail.
   * Explore health checks, failed deployments, rollback behavior, and service stabilization.

4. **Container/image lifecycle**

   * Add an ECR lifecycle policy to clean up old or untagged images.
   * Learn how image tags and immutable digests should be used together.

5. **Security hardening**

   * Review security-group rules and reduce unnecessary public exposure.
   * Review IAM permissions and separate execution/task responsibilities where appropriate.
   * Review secrets and configuration management.

6. **Infrastructure as Code**

   * Recreate the AWS infrastructure with Terraform or another IaC approach.
   * Compare manually created resources with reproducible infrastructure.

7. **Application quality**

   * Add automated tests.
   * Add CI checks such as build/test validation before deployment.
   * Improve error handling and operational diagnostics.

8. **Cost management**

   * Understand the cost of each running AWS component.
   * Clean up resources that are not needed while learning.
   * Document the cost-conscious choices made in this project.

## Learning notes

The project is being built incrementally. The infrastructure choices are therefore not presented as a claim that this is the only or universally recommended AWS architecture.

The repository is a record of what was built, what was learned, what trade-offs were encountered, and what remains to be explored.

## Repository structure

```text
FleetPulse/
├── FleetPulse.Api/             # ASP.NET Core API
├── FleetPulse.Contracts/       # Shared contracts/messages
├── FleetPulse.Infrastructure/  # Data and infrastructure concerns
├── FleetPulse.Worker/          # Background worker / message consumer
├── frontend/                   # React + Vite frontend
├── .github/workflows/          # GitHub Actions CI/CD
├── docker-compose.yml          # Local development stack
├── .dockerignore
└── FleetPulse.slnx
```

## Status

**Working end to end:** local application + AWS deployment + automated deployment pipeline.

**Still evolving:** stable API exposure, observability, security hardening, cost controls, automated tests, and infrastructure as code.
