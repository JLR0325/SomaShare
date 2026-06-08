# Getting Started with running SomaShare

## Prerequisites

- .NET 10 SDK or later
- SQL Server LocalDB (installed with Visual Studio)
- Git

## Initial Setup (Fresh Clone)

### 1. Clone the Repository
```bash
git clone https://github.com/JLR0325/SomaShare.git
cd SomaShare
```

### 2. Restore NuGet Packages
```bash
dotnet restore
```

### 3. Build the Solution
```bash
dotnet build
```

### 4. Run the Application
```bash
cd SomaShare
dotnet run
```

The application will:
- **Automatically apply pending EF Core migrations** on first run
- **Create the database** in LocalDB (no manual setup needed)
- **Seed initial data** including roles, admin user, and sample data

## Database Initialization

The database is automatically initialized when the application starts via `DbInitializer.cs`. This process:

Is safe to run multiple times
Checks for existing tables/users before creating
Handles constraint violations
Seeds sample data only on fresh databases

### Default Credentials

**Admin Account:**
- Email: `admin@somaafrika.edu`
- Password: `Admin@123`

**Sample Student Account:**
- Email: `student@uni.ac.za`
- Password: `Student@123`

## Database Connection

The app uses SQL Server LocalDB with the connection string:
```
Server=(localdb)\MSSQLLocalDB;Database=SomaShareDB;Trusted_Connection=True;
```

Database files are stored in `%LOCALAPPDATA%\Microsoft\Microsoft SQL Server Local DB\` and are not tracked in version control.

## Troubleshooting

### Database Already Exists
If you get errors like "There is already an object named 'X' in the database":
1. The database may already exist from a previous run
2. The application will skip re-creating it automatically
3. If you want a fresh database, delete the LocalDB database:
   ```bash
   SqlLocalDb delete SomaShareDB
   ```
4. Then run the app again - a new database will be created

### Migrations Issue
If you encounter migration conflicts:
```bash
# Verify pending migrations
dotnet ef migrations list

# Apply migrations manually (normally not needed)
dotnet ef database update
```

## Development Workflow

### Making Database Changes
When modifying EF models:

```bash
# Add a migration for your changes
dotnet ef migrations add DescriptiveNameOfChange

# The migration will auto-apply on next app run
dotnet run
```

## Project Structure

- `/Data` - Database context and migrations
- `/Models` - EF Core entity models
- `/Controllers` - MVC controllers
- `/Views` - Razor views
- `/Services` - Business logic services
- `/Migrations` - EF Core migration history

## Notes

- The database automatically initializes on application startup
- No manual database creation or migration steps required for fresh clones
- LocalDB provides a lightweight SQL Server for local development
- All sensitive operations (role creation, user creation) are guarded against duplicates
