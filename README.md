
# RPSSL: Rock, Paper, Scissors, Lizard, Spock Game 🕹️

This project showcases the implementation of the classic "Rock, Paper, Scissors, Lizard, Spock" game using **Domain-Driven Design (DDD)** principles.

The solution is structured using a clean architecture approach, emphasizing separation of concerns and focusing on the core business domain. This project will be extended in the future to support multiplayer games with SignalR for real-time communication, game statistics, and session persistence through cache and database integration.

---

## Solution Structure 🔍

This solution follows a **Clean Architecture** approach with clear separation between layers. Each layer plays a specific role in building the game service.

---

### 🏁 **RPSSL.Api**
- This is the entry point of the application. It handles dependency injection, service registration, and application startup.
- This layer contains all the HTTP API endpoints exposed to the users, handling game sessions and player interactions.

### 💻 **Application**
- The application layer contains CQRS (Command-Query Responsibility Segregation) patterns that orchestrate how domain entities and use cases are executed. You will find game-specific command and query handlers, DTOs, and mapping configurations here.

### 🏭 **Infrastructure**
- This layer contains external services, options. For example, it handles communication with external API services and integrations with Redis or other caching mechanisms.
- Contains database access logic such as repositories, database configurations, and migrations. It ensures proper interaction between the domain and the database.

### ⚓ **Domain**
- The heart of the application, storing the core business logic. Here we define entities, value objects, enumerations, domain events, and aggregates that represent the game and the player interactions.

---

## Tests 🧪

### ✔️ **Unit Tests**
- TODO

### ✔️ **Integration Tests**
- TODO

---

## Features 🚀

### **1. Game Sessions**
- Play "Rock, Paper, Scissors, Lizard, Spock" against a computer.
- The game session stores rounds, tracks wins, and determines the winner based on choices made by players.
- Future features: SignalR for real-time multiplayer games and session persistence.

### **2. Player Statistics**
- Track player history including total wins, losses, ties, and winning percentages.
- Query past game sessions with detailed round-by-round results.

---

## How to Run ▶️

### Run Application with Local Database
1. Set the project as the startup project in your IDE.
2. Ensure a local database connection string is set up in the appsettings for the application.
3. Run the application from your IDE or using `dotnet run`.

### Run with Docker 🐳
1. Ensure Docker is installed.
2. Run the following commands to start the application and its dependencies in containers:
   ```bash
   docker-compose up --build
   ```
3. Navigate to the exposed API endpoints to start using the game service.

---

## Future Enhancements 🏗️
- **Multiplayer Support**: Real-time play with SignalR integration.
- **Leaderboard and Statistics**: Global leaderboard tracking player performances.
- **Enhanced Caching**: Using Redis for game session storage and faster access.
- **OpenTelemetry**: Integrate tracing and monitoring for performance metrics and debugging.

---

## GitHub Workflows 🐫

TODO

---

This project is under active development!
