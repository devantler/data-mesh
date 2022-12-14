# Data Mesh

[![codecov](https://codecov.io/gh/devantler/data-mesh/branch/main/graph/badge.svg?token=9lh1Z59deC)](https://codecov.io/gh/devantler/data-mesh)

<!-- TODO: Update the README to describe the DataProduct. Trigger -->

This repo contains a Data Mesh Provisiong API that can provision data products. A Data Product is an IaaS unit (small k8s cluster) with dedicated data storage, data processing, data discovery-, and data governance- tooling for a specific domain model. A domain model is a single data model that covers a concrete domain, e.g. Accounts, Books, Authors etc.

The Data Mesh is built to support most cloud providers and provisioning tools by being built for Kubernetes. For local development, the platform uses [kind](https://kind.sigs.k8s.io/).

## Prerequisites

- .NET 6.0+
- Docker (for local development)
- Go (for local development)
- Kind (for local development)

## Getting started

### Local development

A Kafka cluster and LinkedIn DataHub are required to create a local development environment. A Kafka Cluster can be instantiated in Docker by following these steps:

1. Run the docker-compose file in `docker/` by running the `start.sh` script.
2. Create an Avro Schema on `http://localhost:8080/`.
3. Set the subject and version variables in the `src/Devantler.DataMesh.DataProduct/appsettings.json` to the subject and version of the schema you created.
4. Run the Data Product by running `dotnet run` in the `src/Devantler.DataMesh.DataProduct` directory.

Following the steps will create a Data Product from the schema you created. The Data Product will be available on `https://localhost:7186/` with a rest endpoint on `https://localhost:7186/swagger` and a graphql endpoint on `https://localhost:7186/graphql`.
