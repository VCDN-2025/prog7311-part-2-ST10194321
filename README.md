
# Agri-Energy Connect

A prototype ASP.NET Core MVC web application that bridges South African farmers with green energy providers. Employees can onboard farmers, and farmers can manage their products. Employees can also view and filter all products through a secure and user-friendly interface.

---

## Table of Contents
1. [Overview](#overview)  
2. [Architecture & Design Pattern](#architecture--design-patterns)  
3. [Database Setup](#database-setup)  
4. [Getting Started](#getting-started)  
5. [Running the Application](#running-the-application)  
6. [Core Functionality](#core-functionality)  
7. [Demo Video](#demo-video)  
8. [Dependencies](#dependencies)  
9. [Troubleshooting](#troubleshooting)  
10. [Contributors](#contributors)  
11. [References](#references)  

---

## Overview

**Agri-Energy Connect** has the goal to combine sustainable farming practices
with the innovation of green energy solutions but creating an online platform that will
allow farmers, green energy professionals and enthusiasts can collaborate in order to
improve South Africa’s effort into being more sustainable in the agriculture sector. This
collaboration will streamline operational processes and promote environmental
sustainability by o>ering valuable resources
to professionals. It will implement an
accessible marketplace to connect farmers
to sustainable products

### Users
- **Employees**
  - Register new farmers
  - View all farmers and products
- **Farmers**
  - Manage their own agricultural products
  - Add Products

This project simulates enterprise-level workflows in a sustainable agriculture context.

---

## Architecture & Design Patterns

The application follows established design paradigms to ensure maintainability and scalability:

- **MVC (Model-View-Controller)**
  **MVC** is a software design pattern that separates an application into three core components:

- **Model** – Manages the application’s data and business rules.  
- **View** – Handles the user interface and presentation layer.  
- **Controller** – Processes user input and coordinates between the Model and View.

> “This pattern has the code in a structured format, therefore separating concerns.” — *(Microsoft, 2024)*

By following the MVC structure, code is better organized and easier to manage. This separation allowed me to work simultaneously on different aspects of the application—front-end, back-end, and business logic—without causing conflicts or logic overlap.

#### Why MVC for Agri-Energy Connect?

Using MVC in the **Agri-Energy Connect** application offers multiple advantages:
- **Maintainability**: Changes in one layer do not impact others.
- **Scalability**: New features can be added without affecting the core structure.

#### Implementation in Agri-Energy Connect

- **Model**:  
  Manages data related to:
  - User profiles (e.g., Farmers, Employees)
  - Product listings (e.g., Green energy products, marketplace items)
  - Community or support content (e.g., Forums, FAQs)

- **View**:  
  Presents this data across various Razor pages with a clean and accessible interface, styled using Bootstrap. Designed to accommodate users with varying technical expertise.

- **Controller**:  
  Handles all user interactions, such as:
  - Submitting product listings
  - Viewing and filtering marketplace items
  - Managing user login and sessions  
  It responds to user inputs, updates the Model, and returns the appropriate View.

---
## Database Setup

1. **Install MySQL Server** and **MySQL Workbench**  
   Download and install from the [official MySQL website](https://dev.mysql.com/downloads/installer/). Ensure the server is running.

2. **Create the Database Using the Provided Script**  
   - Open **MySQL Workbench**.  
   - Open the script file `AgriCult.sql` located in the `DatabaseScripts` folder of this repository.  
   - Run the script to create the `AgriCult` database along with its tables and seed data (if included).

3. **Configure your connection string** in `appsettings.json`:  
   Update your connection string to match your local MySQL credentials. Example:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Port=3306;Database=AgriCult;User=root;Password=your_password;"
   }
4. **Install Pomelo.EntityFrameworkCore.MySql** (if not already installed):
   ```
   dotnet add package Pomelo.EntityFrameworkCore.MySql
   ```

---

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [MySQL Server & MySQL Workbench](https://dev.mysql.com/downloads/installer/)
- Visual Studio 2022 or Visual Studio Code

### Clone the Repository

You can clone this repository using GitHub Desktop or the command line:

#### Using GitHub Desktop
1. Open GitHub Desktop.
2. Go to **File > Clone Repository**.
3. Paste the repo URL: `https://github.com/ST10194321/prog7311-part-2-ST10194321.git`.
4. Click **Clone**.

#### Or via command line:
```bash
git clone https://github.com/ST10194321/prog7311-part-2-ST10194321.git
cd AgriEnergyConnect
```

---

## Running the Application

### Restore Packages

```bash
dotnet restore
```

### Build & Run

```bash
dotnet run
```

Then open your browser and navigate to:  
[https://localhost:yourport](https://localhost:yourport)

---

## Core Functionality

### Employee Role

- Login: `employee@agrienergy.com` / `Pass@123`
- Add new farmers (auto-generated password displayed after creation)
- View list of all farmers and their products
- Filter products by:
  - Date range
  - Category
  - Farmer name

### Farmer Role

- Login using generated credentials
- Add new products (Name, Category, Description, Price, and Optional Image & Production Date)
- View only their own products

> All routes are secured with role-based authorization: `[Authorize(Roles = "...")]`

---

## Demo Video

Watch a full walkthrough covering:
- Project architecture
- Database setup
- Employee and Farmer workflows
- UI responsiveness and filtering features

📺 **[Agri-Energy Connect Prototype Demo](#)** 

---

## Dependencies

- **ASP.NET Core MVC** — Web framework
- **Entity Framework Core** — ORM for data access
- **Bootstrap 5** — Responsive front-end framework

---

## Troubleshooting

| Issue                          | Solution                                                                 |
|-------------------------------|--------------------------------------------------------------------------|
| EF Core not recognized         | Ensure `dotnet-ef` is installed globally                                 |
| Database connection error      | Confirm SQL Server LocalDB is installed and connection string is correct |
| HTTPS redirection error        | Use `https://localhost:5001` or disable HTTPS in `launchSettings.json`   |

---

## Contributors

- **Jose Gonzalves** — Head Developer
> ST10194321

---
## References
//Title: Pro C 7 with.NET and .NET Core 
//Author: Andrew Troelsen; Philip Japikse 
// Date: 2017 
// Code version: Version 1 
//Availability: Textbook / Ebook

// Title: Generating random passwords
// Author: Stack Overflow Community
// Date: 2008
// Code version: N/A
// Availability: Online at https://stackoverflow.com/questions/54991/generating-random-passwords


// Title: Role-based authorization in ASP.NET Core
// Author: Microsoft Docs Team
// Date: 2024
// Code version: ASP.NET Core 8.0
// Availability: Online at https://learn.microsoft.com/en-us/aspnet/core/security/authorization/roles?view=aspnetcore-8.0


// Title: Filtering Data - LINQ (C#)
// Author: Microsoft Docs Team
// Date: 2024
// Code version: .NET
// Availability: Online at https://learn.microsoft.com/en-us/dotnet/csharp/linq/standard-query-operators/filtering-data

// Title: Upload Files in ASP.NET MVC 5
// Author: Suraj Kumar (C# Corner)
// Date: 2019
// Code version: ASP.NET MVC 5
// Availability: Online at https://www.c-sharpcorner.com/article/upload-files-in-asp-net-mvc-5/

// Title: How to add static list of items in MVC Html.DropDownList
// Author: Stack Overflow Community
// Date: 2009
// Code version: N/A
// Availability: Online at https://stackoverflow.com/questions/867117/how-to-add-static-list-of-items-in-mvc-html-dropdownlist

// Title: Implement CRUD operations in ASP.NET Core MVC
// Author: Microsoft Docs Team
// Date: 2024
// Code version: ASP.NET Core 9.0
// Availability: Online at https://learn.microsoft.com/en-us/aspnet/core/data/ef-mvc/crud?view=aspnetcore-9.0

// Title: Role-Based Access Control (RBAC) in C# and ASP.NET Core
// Author: Nwonah R. (Medium)
// Date: 2023
// Code version: ASP.NET Core
// Availability: Online at https://medium.com/@nwonahr/role-based-access-control-rbac-in-c-and-asp-net-core-the-security-backbone-of-modern-apps-dea1204a0870

// Title: CSS Tutorial
// Author: W3Schools.com
// Date: 2024
// Code version: N/A
// Availability: Online at https://www.w3schools.com/css/

// Title: Learn to Code Advanced HTML & CSS
// Author: Shay Howe
// Date: 2014
// Code version: N/A
// Availability: Online at https://learn.shayhowe.com/advanced-html 

