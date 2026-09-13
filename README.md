# Student Engagement Portal

ASP.NET Core MVC + Entity Framework Core (code-first) implementation of the
Student Engagement Portal, built from the design report submitted for
SWE5307 (Web Design and Programming, HE27329).

## Stack

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core 8, code-first, SQL Server / LocalDB
- ASP.NET Core Identity for authentication and role-based authorisation (Student / Admin)
- Bootstrap 5 for responsive layout
- xUnit for unit and functional (WebApplicationFactory) tests

## Project layout

```
StudentEngagementPortal.sln
StudentEngagementPortal/                  # MVC web app
StudentEngagementPortal.Tests/            # xUnit unit tests (logic)
StudentEngagementPortal.FunctionalTests/  # xUnit + WebApplicationFactory (page flows)
```

## Running locally

Requires the .NET 8 SDK and SQL Server LocalDB (installed with Visual
Studio's ASP.NET workload, or standalone).

```bash
cd StudentEngagementPortal
dotnet restore
dotnet ef migrations add InitialCreate --project StudentEngagementPortal.csproj
dotnet run
```

`Program.cs` applies pending migrations and seeds demo data automatically
on startup, so after the first migration you can just `dotnet run` from
then on. It creates:

- **Admin** — `admin@sep.ac.uk` / `Admin#12345`
- **Student** — `student@sep.ac.uk` / `Student#12345`
- Three sample events and one announcement

Open the printed `https://localhost:xxxx` URL. The `EF Core Tools`
package is already referenced, so `dotnet ef` works from inside the
`StudentEngagementPortal` folder without a global install; if it's not
found, run `dotnet tool install --global dotnet-ef` first.

### Using a different connection string

Edit `appsettings.json` → `ConnectionStrings:DefaultConnection`, or override
it with `dotnet user-secrets` for anything containing a real password.

## Running the tests

```bash
dotnet test
```

This runs both test projects:

- **StudentEngagementPortal.Tests** — unit tests for the capacity/registration
  logic on the `Event` model and the validation rules on the form view
  models (`EventFormViewModel`, `RegisterViewModel`, `ContactFormViewModel`).
- **StudentEngagementPortal.FunctionalTests** — boots the real app via
  `WebApplicationFactory<Program>` against an in-memory EF Core provider
  and verifies page flows: public pages load, and protected pages
  (`/Events`, `/Admin/Dashboard`, `/Announcements`) redirect anonymous
  users to `/Account/Login`, matching the `[Authorize]` attributes on
  those controllers.

## Functional requirement → implementation map

| Requirement | Where |
|---|---|
| Student register / log in | `AccountController`, `Views/Account` |
| View upcoming events | `EventsController.Index`, `Views/Events/Index` |
| Register / unregister for an event | `EventsController.Register` / `Unregister` |
| Submit contact/support form | `MessagesController.Create`, `Views/Messages/Create` |
| Admin login | Same `AccountController`, role checked via Identity |
| Admin create/edit/delete events | `EventsController` (`[Authorize(Roles = "Admin")]` actions) |
| Admin view sign-up lists | `EventsController.SignUps` |
| Admin post announcements | `AnnouncementsController.Create` |
| Admin triage support messages | `MessagesController.Index` / `Respond` |

## Known limitations / next steps

- No email confirmation flow — `EmailConfirmed` is set `true` at creation
  for simplicity; a production deployment would wire up a real mail sender.
- No pagination on the events/announcements lists — acceptable at the
  scale of a single campus but would need it at real volume.
- WCAG 2.2 AA colour-contrast and keyboard-navigation checks were done
  manually against the rendered pages (see the technical report); no
  automated accessibility test is included in this codebase.
