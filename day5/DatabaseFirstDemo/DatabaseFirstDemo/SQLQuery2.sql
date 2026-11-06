CREATE DATABASE UserManagementDB;
GO

USE UserManagementDB;
GO

-- Users Table
CREATE TABLE Users
(
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(100) NOT NULL,
    Email VARCHAR(150) NOT NULL
);

-- UserProfiles Table
CREATE TABLE UserProfiles
(
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL UNIQUE,
    Phone VARCHAR(20),
    Address VARCHAR(50),

    CONSTRAINT FK_UserProfiles_Users
    FOREIGN KEY (UserId)
    REFERENCES Users(Id)
);

-- Insert User
INSERT INTO Users (Id, Name, Email)
VALUES (1, 'FATIMA', 'FATIMA@GMAIL.COM');

-- Insert UserProfile

INSERT INTO UserProfiles (UserId, Phone, Address)
VALUES (1, '0791234567', 'Irbid');

-- Display Data
SELECT * FROM Users;
SELECT * FROM UserProfiles;


SELECT @@SERVERNAME AS ServerName;