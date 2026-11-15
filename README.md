# Simple Social Media API

A small REST API for a social network, built with **C#**, **ASP.NET Core** and **Entity Framework Core** on top of **SQL Server**. It covers users, posts, comments and likes, and was built as a portfolio project to practise layered API design in .NET.

## Tech stack

| Area | Technology |
| --- | --- |
| Language / runtime | C# on .NET 10 |
| Web framework | ASP.NET Core Web API (controllers) |
| Data access | Entity Framework Core 10 (database-first) |
| Database | SQL Server (Express) |
| API docs | OpenAPI + [Scalar](https://scalar.com/) UI |

## Features

- **Users** — create, list, get by id, update and delete.
- **Posts** — create, list, get by id and delete. Each post returns its author and the users who liked it.
- **Comments** — a comment is itself a post, linked to the post it replies to. This means comments can be commented on too, which allows threads.
- **Likes** — users can like and unlike posts and comments; each post lists who liked it.
- **Cascading deletes** — deleting a post, comment or user also removes the replies and likes that depend on it, inside a single transaction.
- **DTOs** separate what the API exposes from the database entities, so fields such as email and password are never returned.
- **Validation and error handling** — invalid input returns `400`, missing resources `404` and duplicates `409`, in the standard Problem Details format.
- **Service layer** behind interfaces, injected into the controllers through dependency injection.

## Architecture

```
SocialMediaAPI/
├── SocialMediaAPI.slnx
└── SocialMediaAPI/
    ├── Controllers/   # HTTP endpoints (User, Post, Comment, Like)
    ├── Services/      # Business logic, one interface + implementation per resource
    ├── Dtos/          # Request and response shapes, with validation rules
    ├── Models/        # EF Core entities
    ├── Data/          # AppDbContext and entity mapping
    ├── Exceptions/    # API exceptions and the handler that turns them into HTTP responses
    └── Program.cs     # App setup and dependency injection
script.sql             # Creates the database, tables and SQL functions
dbValues.sql           # Sample data
```

A request goes `Controller → Service → AppDbContext → SQL Server`. Controllers only deal with HTTP; the services hold the logic and map entities to DTOs. When a service cannot fulfil a request it throws a `NotFoundException` or `ConflictException`, and a global exception handler converts it into the matching HTTP response.

### Data model

```
users ──< posts ──< comments (postCommented)
  │         │  └──── comments (id: the comment is also a post)
  └──< likes >──┘
```

- `users` — `id`, `username` (unique), `email` (unique), `uName` (display name), `password`, `dateOfBirth`, `CreatedAt`
- `posts` — `id`, `userID`, `postDate`, `text` (up to 250 characters)
- `comments` — `id` (the post that holds the comment's content), `postCommented` (the post being replied to)
- `likes` — composite key `(userID, postID)`

`script.sql` also creates two SQL functions, `likeNumber(@id)` and `commentsNumber(@id)`, which count the likes and comments of a post.

## Getting started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server Express (the default connection string points to the `.\SQLEXPRESS` instance)

### 1. Clone the repository

```bash
git clone https://github.com/SalvadorAntunes/Simple-Social-Media-API.git
cd Simple-Social-Media-API
```

### 2. Create the database

Run the two scripts in order, either in SQL Server Management Studio or with `sqlcmd`:

```bash
sqlcmd -S .\SQLEXPRESS -E -i script.sql
sqlcmd -S .\SQLEXPRESS -E -i dbValues.sql
```

`script.sql` creates the `SocialMediaProject` database and its tables. `dbValues.sql` is optional and fills it with sample users, posts, comments and likes.

### 3. Check the connection string

The default is:

```
Server=.\SQLEXPRESS;Database=SocialMediaProject;Trusted_Connection=true;TrustServerCertificate=true;
```

If your SQL Server instance is different, change it in both `SocialMediaAPI/SocialMediaAPI/appsettings.json` and `SocialMediaAPI/SocialMediaAPI/Data/AppDbContext.cs` (`OnConfiguring`).

### 4. Run the API

```bash
dotnet run --project SocialMediaAPI/SocialMediaAPI
```

The API listens on `http://localhost:5291` (and `https://localhost:7126` with the `https` launch profile).

### 5. Explore the endpoints

In development, interactive documentation is available at:

- Scalar UI: `http://localhost:5291/scalar/v1`
- OpenAPI document: `http://localhost:5291/openapi/v1.json`

There are also ready-made requests in `SocialMediaAPI/SocialMediaAPI/SocialMediaAPI.http`, which Visual Studio, Rider and VS Code (REST Client extension) can run directly.

## API reference

All endpoints return `400` when the request body is invalid and `404` when a resource does not exist.

### Users — `/api/User`

| Method | Route | Description |
| --- | --- | --- |
| `GET` | `/api/User` | List all users |
| `GET` | `/api/User/{id}` | Get a user by id |
| `POST` | `/api/User` | Create a user |
| `PUT` | `/api/User/{id}` | Update a user |
| `DELETE` | `/api/User/{id}` | Delete a user, their posts, comments and likes |

`username` and `uName` take up to 15 characters, and `username` and `email` must be unique (`409` otherwise).

Request body for `POST` and `PUT`:

```json
{
  "username": "joaosilva",
  "email": "joao.silva@mail.com",
  "uName": "João S.",
  "password": "my-password",
  "dateOfBirth": "1995-03-14"
}
```

### Posts — `/api/Post`

| Method | Route | Description |
| --- | --- | --- |
| `GET` | `/api/Post` | List all posts |
| `GET` | `/api/Post/{id}` | Get a post by id |
| `POST` | `/api/Post` | Create a post |
| `DELETE` | `/api/Post/{id}` | Delete a post, its comments and likes |

Request body for `POST`:

```json
{
  "userId": 1,
  "text": "Hello, world!"
}
```

Example response:

```json
{
  "id": 11,
  "userId": 1,
  "postDate": "2026-07-27T21:15:00.1234567",
  "text": "Hello, world!",
  "commentIdNavigation": null,
  "commentPostCommentedNavigations": [],
  "user": { "id": 1, "username": "joaosilva", "uName": "João S." },
  "users": []
}
```

`user` is the author and `users` is the list of users who liked the post. `postDate` is in UTC, and `text` takes up to 250 characters.

### Comments — `/api/Comment`

| Method | Route | Description |
| --- | --- | --- |
| `GET` | `/api/Comment` | List all comments |
| `GET` | `/api/Comment/{id}` | Get a comment by id |
| `POST` | `/api/Comment` | Comment on a post |
| `DELETE` | `/api/Comment/{id}` | Delete a comment, its replies and likes |

Request body for `POST`:

```json
{
  "postInfo": {
    "userId": 2,
    "text": "Nice post!"
  },
  "postCommented": 1
}
```

### Likes — `/api/Like`

| Method | Route | Description |
| --- | --- | --- |
| `POST` | `/api/Like` | Like a post (`409` if already liked) |
| `DELETE` | `/api/Like` | Remove a like (`404` if the like does not exist) |

Request body for both:

```json
{
  "userId": 1,
  "postId": 3
}
```

## Project status

This is a learning project and still a work in progress:

- **Authentication** — the JWT packages are referenced and the `Jwt` settings exist, but no endpoint is protected yet, so any client can act as any user.
- **Passwords** — stored as received, without hashing.
- **Tests** — the endpoints have been tested by hand against a local database; there are no automated tests yet.

### Planned next steps

- Add JWT authentication and password hashing
- Expose like and comment counts on posts
- Add automated tests

## Author

**Salvador Antunes** — [github.com/SalvadorAntunes](https://github.com/SalvadorAntunes)
