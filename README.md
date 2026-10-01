# Url.Shortner

A URL shortening service. It takes a long URL, returns a short, unguessable alias, and redirects anyone who visits the alias to the original URL. Other services use it through a versioned REST API.

Built with ASP.NET Core 8 minimal APIs, EF Core 8 and PostgreSQL.

## Requirements

### Functional

- Given a URL, generate a short, unique alias (the short link). It must be short enough to copy and paste easily.
- Visiting a short link redirects to the original URL.
- Callers can optionally choose a custom alias.
- Links expire after a default period. Callers can set their own expiry time.

### Non-functional

- **High availability.** If the service is down, every redirect fails, so the redirect path is the part that must stay up.
- **Low-latency redirects.** Redirection happens in real time.
- **Unguessable links.** Short codes must not be predictable or enumerable.

### Extended

- **Analytics.** Record how many times each short link was used for a redirect.
- **REST API.** Other services can create and look up short links over HTTP.

## Implementation status

The project is at the scaffolding stage. The schema exists; the shortening and redirect flows do not yet work end to end.

| Requirement | Status |
| --- | --- |
| Generate short alias | Not implemented. `POST /api/v1/shorten` exists but saves an empty record and does not generate a code. |
| Redirect to original URL | Not implemented. `GET /shorten` is a placeholder. |
| Custom alias | Not implemented. |
| Default and custom expiry | Column `expires_at` exists. No default period chosen, no enforcement. |
| Unguessable codes | Not implemented. Code generation strategy not yet decided. |
| High availability, low latency | Not addressed. No caching or deployment topology yet. |
| Analytics | Not implemented. |
| REST API | Versioned under `/api/v1/`. One endpoint, incomplete. |

## API

All write and lookup endpoints live under `/api/v1/`. Responses use this envelope:

```json
{
  "message": "Success",
  "code": 200,
  "data": "..."
}
```

### Create a short link

`POST /api/v1/shorten`

```json
{
  "url": "https://example.com/some/very/long/path?with=query"
}
```

Returns `400` with `"message": "Invalid Url"` when `url` is not an absolute URI. The success path is incomplete; see [Implementation status](#implementation-status).

Swagger UI is available at `/swagger` when running in the `Development` environment.

## Data model

One table, `shortened_urls`. EF Core maps names to snake_case through `EFCore.NamingConventions`.

| Column | Type | Notes |
| --- | --- | --- |
| `id` | `uuid` | Primary key |
| `url` | `text` | Original URL |
| `short_url` | `text` | Full short link |
| `code` | `varchar(7)` | Short code, unique index `ix_shortened_urls_code` |
| `expires_at` | `timestamptz` | |
| `created_at` | `timestamptz` | |
| `updated_at` | `timestamptz` | |

## Running locally

### Prerequisites

- .NET SDK 8.0.x (pinned in `global.json`)
- PostgreSQL, reachable on `localhost:5432` by default
- `dotnet-ef` for creating migrations: `dotnet tool install --global dotnet-ef`

### Start the API

```bash
createdb url_shortener          # or create it any other way
dotnet run --project Url.Shortner
```

The default `http` profile listens on `http://localhost:5043`. Use `--launch-profile https` to also listen on `https://localhost:7136`. Pending migrations are applied automatically on startup, so the database must be reachable or the app will not start.

### Migrations

```bash
dotnet ef migrations add <MigrationName> --project Url.Shortner
dotnet ef database update --project Url.Shortner
```

## Configuration

| Key | Environment variable | Default |
| --- | --- | --- |
| `ConnectionStrings:DefaultConnection` | `ConnectionStrings__DefaultConnection` | `Host=localhost;Database=url_shortener;Username=postgres;Password=postgres` |
| `ASPNETCORE_ENVIRONMENT` | `ASPNETCORE_ENVIRONMENT` | `Development` via `launchSettings.json` |

Override the connection string with the environment variable or user secrets outside local development. Do not commit real credentials.

## Tests

There is no test project yet.

## Deployment

Not yet defined.
