# GHLearning-ThreeLayer
Gordon Hung Learning ThreeLayer

The three-tier architecture model, which is the fundamental framework for the logical design model, segments an application's components into three tiers of services. These tiers do not necessarily correspond to physical locations on various computers on a network, but rather to logical layers of the application. How the pieces of an application are distributed in a physical topology can change, depending on the system requirements.

## 1. ApiService
- ApiService typically refers to a service layer responsible for handling API requests and responses. It's the part of your application that exposes endpoints for the client to interact with the backend system.
- It usually processes HTTP requests (like GET, POST, etc.), invokes business logic or other services, and returns the result to the client in a structured format, typically JSON.

## 2. Core
- Core refers to the foundational part of an application that contains the essential and shared components. This may include cross-cutting concerns like configuration settings, utility functions, common interfaces, or shared services used by various parts of the application.
- In some applications, the Core layer could also include essential infrastructure for the app, such as logging, authentication, or dependency injection.
## 3. Migrations
- Migrations are related to database schema management, especially in Object-Relational Mapping (ORM) frameworks like Entity Framework in .NET.
- When you modify your data models (e.g., changing a class or adding new properties), migrations help manage the changes to the database schema automatically by generating SQL scripts or commands that update the database structure.
## 4. Repositories
- Repositories implement the Repository design pattern, which is used to abstract away data access logic. Instead of directly interacting with the database, the application interacts with repositories to handle CRUD (Create, Read, Update, Delete) operations.
- Repositories provide a clean separation between the business logic and data access, making the application more maintainable and testable.
## 5. Services
- Services encapsulate the core business logic of the application. These are the components where the actual functionality (like calculations, validations, or processing) is performed.
- Services typically interact with repositories to fetch or store data and perform the necessary operations, returning results to controllers (or API layers) or other consumers.
