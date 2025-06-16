# Glow-Up Social Media Platform

A modern, feature-rich social media platform built with .NET 8, implementing clean architecture and real-time communication capabilities.

## 🌟 Features

### User Management & Authentication

* User registration and authentication using ASP.NET Identity
* JWT-based authentication
* Profile management with customizable avatars and cover photos
* Follow/unfollow functionality
* Mutual followers discovery

### Content Sharing

* Multi-format post creation (text, images, videos)
* Smart content categorization:

  * Posts (text and images)
  * Clips (videos ≤ 1 minute)
  * Videos (videos > 1 minute)
* Interactive engagement through reactions and comments
* Nested comment system
* Post sharing capabilities
* Favorite posts system

### Real-time Features

* Real-time messaging using SignalR
* Instant notifications for:

  * New followers
  * Post interactions (likes, comments, shares)
  * Direct messages
* Live updates for user activities

### BlackHat Community

* Specialized community section
* Voting system
* Category-based organization
* Enhanced commenting system

### Activity Tracking

* Comprehensive activity logging
* User interaction history
* Content engagement analytics

## 🛠 Technology Stack

* **.NET 8**
* **Entity Framework Core**
* **SQL Server**
* **SignalR**
* **AutoMapper**
* **FFmpeg**
* **JWT Authentication**

## 📋 Prerequisites

* .NET 8 SDK
* SQL Server
* Visual Studio 2022 or VS Code
* FFmpeg (for video processing)

## 🚀 Getting Started

1. **Clone the repository**

2. **Update database connection string**

   * Navigate to `appsettings.json`
   * Update the connection string with your SQL Server details

3. **Apply database migrations**

4. **Install FFmpeg**

   * Download from [FFmpeg official website](https://ffmpeg.org/download.html)
   * Add to system PATH

5. **Run the application**

## 🏗 Project Structure

(To be filled if needed: explanation of layers such as API, Application, Domain, Infrastructure)

## 🔌 API Endpoints

### Authentication

* POST `/api/Account/register`
* POST `/api/Account/login`

### Users

* GET `/api/Users/profile/{userId}`
* PUT `/api/Users/update-profile`
* POST `/api/Users/follow/{followerId}/{followeeId}`

### Posts

* POST `/api/Posts/AddNewPost`
* GET `/api/Posts/GetAllPosts`
* POST `/api/Posts/AddReactToPost/{postId}`

### Messages

* POST `/api/Messages/send`
* GET `/api/Messages/conversation/{userId}`

## 🔒 Security

* JWT Authentication
* Password hashing
* Input validation
* File upload validation
* CORS policy implementation

## 🚦 Real-time Communication

### SignalR Hubs

* ChatHub for direct messaging
* NotificationHub for real-time notifications

## ⚙️ Configuration

Important configuration files:

* `appsettings.json`: Application settings
* `Program.cs`: Service configuration
* `AddApplicationServicesExtenstion.cs`: DI configuration

## 📝 Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

