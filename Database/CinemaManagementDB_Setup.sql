/*
  CinemaManagement - SQL Server setup
  Creates a NEW database named CinemaManagementDB so that an existing CinemaDB is not overwritten.
  Run this script in SQL Server Management Studio (SSMS).
*/
IF DB_ID(N'CinemaManagementDB') IS NULL
    CREATE DATABASE CinemaManagementDB;
GO
USE CinemaManagementDB;
GO

IF OBJECT_ID(N'dbo.Users', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Users
    (
        UserID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Users PRIMARY KEY,
        Username NVARCHAR(80) NOT NULL CONSTRAINT UQ_Users_Username UNIQUE,
        FullName NVARCHAR(120) NOT NULL,
        PasswordHash CHAR(64) NOT NULL,
        Role VARCHAR(20) NOT NULL CONSTRAINT CK_Users_Role CHECK (Role IN ('Admin','Manager','Customer')),
        IsActive BIT NOT NULL CONSTRAINT DF_Users_IsActive DEFAULT (1),
        CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_Users_CreatedAt DEFAULT (SYSDATETIME())
    );
END;
GO

IF OBJECT_ID(N'dbo.Movies', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Movies
    (
        MovieID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Movies PRIMARY KEY,
        Title NVARCHAR(200) NOT NULL,
        Genre NVARCHAR(100) NULL,
        Description NVARCHAR(MAX) NULL,
        DurationMinutes INT NOT NULL CONSTRAINT CK_Movies_Duration CHECK (DurationMinutes > 0),
        AgeRating NVARCHAR(20) NULL,
        ReleaseDate DATE NULL,
        IsActive BIT NOT NULL CONSTRAINT DF_Movies_IsActive DEFAULT (1)
    );
END;
GO

IF OBJECT_ID(N'dbo.Rooms', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Rooms
    (
        RoomID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Rooms PRIMARY KEY,
        RoomName NVARCHAR(80) NOT NULL CONSTRAINT UQ_Rooms_RoomName UNIQUE,
        Capacity INT NOT NULL CONSTRAINT CK_Rooms_Capacity CHECK (Capacity > 0),
        IsActive BIT NOT NULL CONSTRAINT DF_Rooms_IsActive DEFAULT (1)
    );
END;
GO

IF OBJECT_ID(N'dbo.Showtimes', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Showtimes
    (
        ShowtimeID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Showtimes PRIMARY KEY,
        MovieID INT NOT NULL CONSTRAINT FK_Showtimes_Movies REFERENCES dbo.Movies(MovieID),
        RoomID INT NOT NULL CONSTRAINT FK_Showtimes_Rooms REFERENCES dbo.Rooms(RoomID),
        StartTime DATETIME2 NOT NULL,
        EndTime DATETIME2 NOT NULL,
        TicketPrice DECIMAL(12,2) NOT NULL CONSTRAINT CK_Showtimes_Price CHECK (TicketPrice >= 0),
        CONSTRAINT CK_Showtimes_Time CHECK (EndTime > StartTime)
    );
END;
GO

IF OBJECT_ID(N'dbo.Bookings', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Bookings
    (
        BookingID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Bookings PRIMARY KEY,
        UserID INT NOT NULL CONSTRAINT FK_Bookings_Users REFERENCES dbo.Users(UserID),
        ShowtimeID INT NOT NULL CONSTRAINT FK_Bookings_Showtimes REFERENCES dbo.Showtimes(ShowtimeID),
        TicketCode VARCHAR(30) NOT NULL CONSTRAINT UQ_Bookings_TicketCode UNIQUE,
        BookingTime DATETIME2 NOT NULL CONSTRAINT DF_Bookings_BookingTime DEFAULT (SYSDATETIME()),
        TotalAmount DECIMAL(12,2) NOT NULL CONSTRAINT CK_Bookings_Total CHECK (TotalAmount >= 0),
        Status VARCHAR(20) NOT NULL CONSTRAINT DF_Bookings_Status DEFAULT ('Pending') CONSTRAINT CK_Bookings_Status CHECK (Status IN ('Pending','Paid','Cancelled')),
        IsCheckedIn BIT NOT NULL CONSTRAINT DF_Bookings_CheckedIn DEFAULT (0),
        CheckInTime DATETIME2 NULL
    );
END;
GO

IF OBJECT_ID(N'dbo.BookingSeats', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.BookingSeats
    (
        BookingSeatID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_BookingSeats PRIMARY KEY,
        BookingID INT NOT NULL CONSTRAINT FK_BookingSeats_Bookings REFERENCES dbo.Bookings(BookingID),
        ShowtimeID INT NOT NULL CONSTRAINT FK_BookingSeats_Showtimes REFERENCES dbo.Showtimes(ShowtimeID),
        SeatCode NVARCHAR(10) NOT NULL,
        SeatPrice DECIMAL(12,2) NOT NULL CONSTRAINT CK_BookingSeats_Price CHECK (SeatPrice >= 0),
        CONSTRAINT UQ_BookingSeats_Showtime_Seat UNIQUE (ShowtimeID, SeatCode)
    );
END;
GO

/* Demo accounts. Passwords are SHA-256 hashes calculated by DatabaseHelper.HashPassword. */
IF NOT EXISTS (SELECT 1 FROM dbo.Users)
BEGIN
    INSERT INTO dbo.Users(Username, FullName, PasswordHash, Role, IsActive) VALUES
    (N'admin',   N'Quản trị viên', 'e86f78a8a3caf0b60d8e74e5942aa6d86dc150cd3c03338aef25b7d2d7e3acc7', 'Admin', 1),
    (N'manager', N'Quản lý rạp',  'e8392925a98c9c22795d1fc5d0dfee5b9a6943f6b768ec5a2a0c077e5ed119cf', 'Manager', 1),
    (N'customer',N'Khách hàng mẫu','98ec654a8df28f8f0f8f02220483d46916b85017de0b74d8ec755c28cb8539a8', 'Customer', 1);
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Movies)
BEGIN
    INSERT INTO dbo.Movies(Title, Genre, Description, DurationMinutes, AgeRating, ReleaseDate, IsActive) VALUES
    (N'Hành Trình Ánh Sáng', N'Phiêu lưu', N'Phim mẫu để kiểm thử chức năng đặt vé.', 118, N'T13', '2026-01-01', 1),
    (N'Thành Phố Bí Ẩn', N'Tâm lý', N'Phim mẫu thứ hai.', 105, N'T16', '2026-02-01', 1),
    (N'Vũ Trụ Rực Rỡ', N'Khoa học viễn tưởng', N'Phim mẫu thứ ba.', 124, N'T13', '2026-03-01', 1);
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Rooms)
BEGIN
    INSERT INTO dbo.Rooms(RoomName, Capacity, IsActive) VALUES
    (N'Phòng 01', 60, 1), (N'Phòng 02', 60, 1), (N'Phòng 03', 80, 1);
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Showtimes)
BEGIN
    DECLARE @Tomorrow DATETIME2 = DATEADD(HOUR, 10, CAST(DATEADD(DAY, 1, CAST(GETDATE() AS DATE)) AS DATETIME2));
    INSERT INTO dbo.Showtimes(MovieID, RoomID, StartTime, EndTime, TicketPrice) VALUES
    (1, 1, @Tomorrow, DATEADD(MINUTE, 118, @Tomorrow), 95000),
    (2, 2, DATEADD(HOUR, 3, @Tomorrow), DATEADD(MINUTE, 105, DATEADD(HOUR, 3, @Tomorrow)), 85000),
    (3, 3, DATEADD(HOUR, 6, @Tomorrow), DATEADD(MINUTE, 124, DATEADD(HOUR, 6, @Tomorrow)), 100000);
END;
GO
PRINT N'Hoàn tất tạo CSDL CinemaManagementDB. Tài khoản demo: admin/Admin@123, manager/Manager@123, customer/Customer@123.';
GO
