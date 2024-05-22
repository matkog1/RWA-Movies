# Video Streaming Service Web Application

## Overview

This project is a web application designed for managing a video streaming service. It encompasses three main modules: integration with third-party services, administrative management, and public user access. The application is built using **ASP.NET Core** technologies to provide a robust and scalable solution for video content management.

## Modules

### Integration Module

-   **Description**: Automates processes like video content upload.
-   **Features**:
    -   **RESTful API** for video content CRUD operations with properties like name, description, image, total time, streaming URL, genre, and tags.
    -   **Pagination, filtering, and sorting** capabilities.
    -   **Endpoints** for managing genres and tags.
    -   **JWT token authentication** and user registration with email validation.
    -   **Notification management** via email with unsent notification handling.
    -   Utilizes **Swagger** for API documentation and **HTTPS** for security.

### Administrative Module

-   **Description**: Allows administrators to manage video content and other related data.
-   **Features**:
    -   Pages for **CRUD operations** on video content with support for **pagination and filtering**.
    -   **Country management** page with pagination.
    -   **Tag and genre management** pages with AJAX support.
    -   **User management** pages with filtering and soft-delete functionality.
    -   Implements **data mapping** between the data access layer and business layer.

### Public Module

-   **Description**: Enables registered users to access and stream video content.
-   **Features**:
    -   **User registration** and **login** pages.
    -   **Video content selection** page with cards displaying video details and filtering options.
    -   Individual **video content page** with playback functionality.
    -   **User profile management** and email verification.
    -   Displays **logged-in user's username** on all pages and includes **logout** functionality.
    -   **AJAX-based pagination** for video content selection.

## Key Technologies

-   **ASP.NET Core Web API** for the integration module.
-   **ASP.NET Core MVC** for the administrative and public modules.
-   **Swagger** for API documentation.
-   **JWT** for authentication.
-   **SMTP** for email notifications.

## Notes

-   Uses the **RwaMovies database structure**.
-   Emphasizes a **seamless user experience** with thorough data validation.
-   Prioritizes **best coding practices** and **user-friendly interface design**.
