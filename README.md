**Reactivities Web Application**
Project Overview
This project is a modern web application designed to facilitate user activities and interactions, featuring functionalities like activity posting, user subscriptions, real-time chatting and following, and media management.

**Key Features**
- User Authentication: Secure login and registration system with JWT-based authentication and private routes.
- Activity Posting & Management: Users can create, edit, and delete activities, as well as view and join activities posted by others.
- Subscriptions: Users can subscribe to join activities, fostering engagement and participation.
- Real-Time Functionality (Comments and Follow): Social interaction through comments and follows through SignalR, enhancing user engagement.
- Image Upload, Crop, and Review: Integrated with Cloudinary, users can upload images, crop, and review them before finalizing the upload.
- Activity Feeds: Real-time activity feed displaying updates from users and activities they follow.
- Comprehensive API Operations: APIs support filtering, paging, and sorting to ensure efficient data handling and retrieval across the platform.

**Technologies Used:**
-  **Frontend**: React.js, Mobx, Typescript
-  **Backend**: ASP.NET Core
-  **Database**: MSSQL
-  **Cloud**: Cloudinary
-  **Architecture**: Clean Architecture
-  **Design Patterns**:
      + CQRS (Command Query Responsibility Segregation)
      + Mediator pattern for request handling
-  **Containerization**: Docker.
-  **Hosting**: Azure
  
**Project Structure**
-  API Layer (ASP.NET Core): Provides RESTful API endpoints for client-side interaction, using CQRS and the Mediator pattern to handle commands and queries.
-  Client Layer (React): The user-facing web application developed using React.js.
-  Domain Layer: Represents the core business logic of the application, handling users, activities, and their interactions.
-  Application Layer: Contains application-specific logic, such as managing users, activities, and application flows.
-  Persistence Layer: Manages data access, including storing and retrieving activities and user data from the database.
-  Infrastructure Layer: Contains the application configs and accessors
  
**Objective**
The project is designed to provide a seamless platform for users to engage with activities, offering social interaction features and fostering community participation.
