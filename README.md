# 🎬 Media Tracker

A C# console application for managing a personal film and TV library. The application uses the **TMDB API** to search for media and **Entity Framework Core with SQLite** to store and manage saved items locally.

## 📌 Project Overview

Media Tracker allows users to search for films and TV shows through **The Movie Database (TMDB) API** and save selected titles to a local database.

Once media has been added, users can view their library, update watch statuses and ratings, or remove items.

### Main Features

- 🔎 Search for movies and TV shows using the TMDB API
- ➕ Add selected media to a personal library
- 📚 View all saved media
- 🔄 Update watch status
- ⭐ Add a personal rating from 1–10
- 🗑️ Delete media items
- 💾 Store data locally using SQLite
- 🛡️ Prevent duplicate TMDB entries from being added

## 🛠️ Technologies & Tools

- **C#**
- **.NET 10**
- **Entity Framework Core 10**
- **SQLite**
- **TMDB API**
- **System.Text.Json**
- **HttpClient**

## 🧠 Key Concepts Demonstrated

This project demonstrates several software development concepts, including:

- Object-oriented programming
- Classes and constructors
- Enums
- Entity Framework Core
- Database modelling
- SQLite database management
- Database migrations
- CRUD operations
- REST API consumption
- HTTP requests
- JSON deserialization
- Input validation
- Exception handling
- LINQ queries
- Separation of data models and DTOs

## 🌐 TMDB API Integration

The application communicates with the **TMDB API** to search for media.

Users can choose between:

- 🎥 Movies
- 📺 TV Shows

The application sends a search request to TMDB and processes the JSON response using `System.Text.Json`.

The returned information is converted into DTO objects before the selected media is saved to the local database.

## 🗄️ Database

The application uses **SQLite** through **Entity Framework Core**.

The database is automatically configured as:

```text
Media_Tracker.db
```

and is stored in the user's **Documents** folder.

### Media Item

Each saved media item contains information including:

| Property | Description |
| --- | --- |
| `Id` | Local database identifier |
| `TmdbId` | TMDB identifier |
| `Title` | Media title |
| `ReleaseYear` | Release year |
| `Genre` | Genre information |
| `Overview` | Media overview |
| `RuntimeMinutes` | Runtime in minutes |
| `Type` | Movie, TV show or anime |
| `Status` | Current watch status |
| `UserRating` | Personal rating from 1–10 |
| `DateAdded` | Date the item was added |

## 📋 Watch Status

The application supports several watch statuses:

```text
PlanToWatch
CurrentlyWatching
Completed
OnHold
Dropped
Rewatching
```

These are implemented using a C# `enum`.

## 🔄 CRUD Operations

The application demonstrates the main database operations:

### Create

Users can search TMDB and save a selected movie or TV show to their local library.

### Read

Users can view all media currently stored in the database.

### Update

Users can update the watch status and personal rating of saved media.

### Delete

Users can remove media items from their library.

## 📁 Project Structure

```text
MediaTracker/
│
├── Data/
│   ├── MediaItem.cs
│   ├── MediaTrackerDbContext.cs
│   ├── MediaType.cs
│   ├── WatchStatus.cs
│   │
│   └── Migrations/
│       ├── InitialCreate.cs
│       ├── InitialCreate.Designer.cs
│       └── MediaTrackerDbContextModelSnapshot.cs
│
├── Dtos/
│   └── TmdbMovieDto.cs
│
├── MediaTracker.csproj
├── Program.cs
└── MediaTracker.slnx
```

## 🚀 How to Run

### Prerequisites

You will need:

- [.NET 10 SDK](https://dotnet.microsoft.com/)
- A TMDB API account/token
- A development environment such as Visual Studio or VS Code

### Setup

1. Clone the repository.

2. Open the project in your preferred C# development environment.

3. Restore the required NuGet packages:

```bash
dotnet restore
```

4. Configure your TMDB API authentication.

5. Build the project:

```bash
dotnet build
```

6. Run the application:

```bash
dotnet run
```

## 🖥️ Application Menu

When the application starts, the main menu provides the following options:

```text
========================================
      FILM & TV MEDIA TRACKER
========================================
1. Search & Add Media (TMDB)
2. View All Saved Media
3. Update Watch Status & Rating
4. Delete Media Item
5. Exit
========================================
```

## 🎯 Project Purpose

This project was developed to gain practical experience building a C# application that interacts with an external API and persists data using a relational database.

It combines **C# programming, REST API integration, JSON processing, Entity Framework Core, SQLite and CRUD operations** into a single application.

## 📚 What I Practised

Through this project, I practised:

- Building a C# console application
- Working with external REST APIs
- Sending HTTP requests using `HttpClient`
- Processing JSON responses
- Creating and using DTOs
- Designing data models
- Using Entity Framework Core
- Working with SQLite
- Creating database migrations
- Performing CRUD operations
- Using LINQ to query data
- Handling invalid user input and API errors
