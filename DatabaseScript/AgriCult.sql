create database AgriCult;
use AgriCult;
-- ASP.NET Identity tables 
CREATE TABLE AspNetRoles (
    Id VARCHAR(255) NOT NULL PRIMARY KEY,
    Name VARCHAR(256),
    NormalizedName VARCHAR(256),
    ConcurrencyStamp LONGTEXT
);

CREATE TABLE AspNetUsers (
    Id VARCHAR(255) NOT NULL PRIMARY KEY,
    UserName VARCHAR(256),
    NormalizedUserName VARCHAR(256),
    Email VARCHAR(256),
    NormalizedEmail VARCHAR(256),
    EmailConfirmed BIT NOT NULL,
    PasswordHash LONGTEXT,
    SecurityStamp LONGTEXT,
    ConcurrencyStamp LONGTEXT,
    PhoneNumber LONGTEXT,
    PhoneNumberConfirmed BIT NOT NULL,
    TwoFactorEnabled BIT NOT NULL,
    LockoutEnd DATETIME,
    LockoutEnabled BIT NOT NULL,
    AccessFailedCount INT NOT NULL,
    FirstName LONGTEXT,
    LastName LONGTEXT,
    UserType LONGTEXT,
    MustChangePassword BIT NOT NULL
);

CREATE TABLE AspNetUserRoles (
    UserId VARCHAR(255) NOT NULL,
    RoleId VARCHAR(255) NOT NULL,
    PRIMARY KEY (UserId, RoleId),
    FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE,
    FOREIGN KEY (RoleId) REFERENCES AspNetRoles(Id) ON DELETE CASCADE
);

-- Farmer table
CREATE TABLE Farmers (
    Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    FirstName LONGTEXT NOT NULL,
    LastName LONGTEXT NOT NULL,
    Email LONGTEXT NOT NULL,
    FarmName LONGTEXT,
    FarmLocation LONGTEXT,
    FarmSize LONGTEXT,
    UserId VARCHAR(255)
);

-- Product table
CREATE TABLE Products (
    Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Name LONGTEXT NOT NULL,
    Category LONGTEXT NOT NULL,
    ProductionDate DATETIME NOT NULL,
    FarmerId INT NOT NULL,
    Description LONGTEXT NOT NULL,
    ImageUrl LONGTEXT,
    Price DOUBLE NOT NULL,
    FOREIGN KEY (FarmerId) REFERENCES Farmers(Id) ON DELETE CASCADE
);

select * from AspNetRoles;
select * FROM AspNetUsers;
select * FROM AspNetUserRoles ;
select * from Farmers;
select * from Products;

