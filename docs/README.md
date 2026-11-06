## Architecture

The application is structured around a layered architecture that promotes separation of concerns between HTTP request handling, business logic, and data persistence.

- **Controllers:** Responsible for processing incoming HTTP requests and generating the corresponding responses.
- **Services:** Manage claim and cover-related operations while implementing the application's business logic.
- **Data:** Includes the database contexts responsible for accessing and persisting claims, covers, and audit information.
- **Auditing:** Handles the recording of claim and cover-related events through the `IAuditer` abstraction.

The application uses ASP.NET Core's built-in dependency injection mechanism to manage dependencies. This allows controllers to interact with service and auditing interfaces without being directly coupled to their concrete implementations.

## Technologies Used

- C# and ASP.NET Core
- Entity Framework Core
- MongoDB
- SQL Server
- Docker and Testcontainers
- Swagger / OpenAPI

## Running the Application

### Prerequisites

Before running the application, ensure that you have:

- The .NET SDK version compatible with the project.
- Docker Desktop installed and running.

### Setup and Execution

1. Clone or download the repository to your local machine.
2. Open a terminal in the directory containing the solution or project.
3. Restore the required NuGet packages:

   ```bash
   dotnet restore
   ```

4. Build the solution to verify that all projects compile successfully:

   ```bash
   dotnet build
   ```

5. Start the application using:

   ```bash
   dotnet run --project Claims
   ```

6. Once the application has started, navigate to the Swagger URL provided in the console output to view and interact with the available API endpoints.

**Note:** The application is configured to launch the required database containers automatically during startup. Make sure Docker Desktop is running before launching the application and allow sufficient time for the containers to initialize and become available.
