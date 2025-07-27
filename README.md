# PersonalFinanceTracker

## Introduction

Personal financial management is a critical task for individuals aiming to monitor their income and expenditures effectively. The **PersonalFinanceTracker** project presents a software solution designed to assist users in recording, categorizing, and managing their financial transactions. Implemented as a web API using ASP.NET Core and Entity Framework Core, the application provides functionalities to create, track, and analyze personal transactions with an emphasis on accuracy, scalability, and usability.

## Objectives

The primary objectives of the PersonalFinanceTracker project are:

- To provide users with an intuitive and reliable platform for recording income and expense transactions.
- To maintain accurate user balances dynamically reflecting the current financial state.
- To categorize transactions effectively into income and expense types for detailed financial insights.
- To ensure data integrity and performance through asynchronous programming patterns.
- To enable users to generate comprehensive transaction reports in PDF format for record-keeping and review.

## System Architecture

### Overview

The system adopts a layered architecture pattern with distinct separation of concerns between data access, business logic, and presentation (API endpoints). It employs the ASP.NET Core Web API framework for building RESTful services and Entity Framework Core as the Object-Relational Mapper (ORM) for database interactions.

### Components

- **API Layer**: Hosts HTTP endpoints to handle client requests, including transaction creation, retrieval, and invoice generation.
- **Data Access Layer**: Utilizes EF Core `DbContext` to interact with the underlying relational database, managing entity states and relationships.
- **Models**: Define domain entities including `User`, `Transaction`, `Category`, and `Payment`, each with appropriate navigation properties reflecting relational mappings.
- **Services**: Implements business logic such as transaction processing, balance updates, and PDF report generation.
- **PDF Generation**: Uses IronPdf library to render HTML/CSS content into professional PDF documents, facilitating transaction report export.

### Entity Relationships

- **User**: Represents an account holder with properties including a unique identifier, username, and current balance. Maintains a one-to-many relationship with `Transaction`.
- **Transaction**: Captures financial activity, storing details such as amount, date, description, category, and payment status. Each transaction is associated with one user and one category.
- **Category**: Enumerates transaction classifications as either income or expense, providing semantic context for each transaction.
- **Payment**: Tracks the processing state of each transaction with statuses like `Pending`, `Completed`, or `Failed`.

## Implementation Details

### Asynchronous Programming

All database operations are implemented using asynchronous patterns (`async/await`) to prevent blocking calls and to enhance throughput and responsiveness of the API, especially under concurrent access scenarios.

### Transaction Creation Workflow

- Validates incoming transaction data.
- Eagerly loads associated `User` and `Category` entities.
- Creates a new `Transaction` and associated `Payment` record.
- Updates the user's balance based on transaction type.
- Sets payment status to `Completed` or `Failed` depending on the updated balance.
- Persists all changes atomically to the database.

### PDF Invoice Generation

- Aggregates all transactions belonging to a user.
- Constructs an HTML representation of the transaction data styled with CSS.
- Converts HTML content into PDF documents using IronPdf.
- Returns the PDF as a downloadable file via an API endpoint.

## Conclusion

The PersonalFinanceTracker project successfully delivers a robust platform for personal financial management. Its modular architecture, adherence to asynchronous programming best practices, and integration of dynamic PDF reporting make it a practical tool for users seeking to maintain detailed and accessible financial records.

Future enhancements may include richer analytics, user authentication and authorization, and multi-currency support to broaden the application's applicability.
