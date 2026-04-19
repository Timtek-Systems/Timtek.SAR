# Timtek.SAR

Timtek Search and Rescue Case Manager — a multi-user web application for coordinating lost pet/animal search operations using drones and ground searchers.

## Documentation

Project documentation is maintained in an Obsidian vault under `documentation/Timtek Search and Rescue Case Manager/`.

## Branching Strategy

This project uses **Git Flow**:

| Branch | Purpose |
|---|---|
| `main` | Production releases only. Tagged with version numbers. |
| `develop` | Integration branch. All feature branches merge here. |
| `feature/*` | New features. Branch from `develop`, merge back to `develop`. |
| `release/*` | Release preparation. Branch from `develop`, merge to `main` and `develop`. |
| `hotfix/*` | Urgent production fixes. Branch from `main`, merge to `main` and `develop`. |

## Tech Stack

- **Platform:** ASP.NET Web Application & API (C#)
- **Architecture:** Clean Architecture
- **Database:** Entity Framework Core
- **Testing:** xUnit, FluentAssertions, NSubstitute/Moq
