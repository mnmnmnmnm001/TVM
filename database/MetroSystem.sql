--CREATE DATABASE MetroSystem

USE [MetroSystem];
GO

CREATE TABLE Stations (
    StationID INT PRIMARY KEY IDENTITY(1,1),
    NameVN NVARCHAR(255) NOT NULL,
    NameEN NVARCHAR(255) NOT NULL,
    StationOrder INT NOT NULL
);

CREATE TABLE FareSteps (
    StepID INT PRIMARY KEY IDENTITY(1,1),
    FromStationCount INT NOT NULL,
    ToStationCount INT NOT NULL,
    CashPrice DECIMAL(18, 2) NOT NULL,
    NonCashPrice DECIMAL(18, 2) NOT NULL
);

CREATE TABLE tblUser (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    UserName VARCHAR(255) UNIQUE NOT NULL,
    [Password] VARCHAR(255) NOT NULL,
    FullName NVARCHAR(100),
    [Role] NVARCHAR(20) DEFAULT 'Admin'
);

CREATE TABLE Tickets (
    TicketID INT PRIMARY KEY IDENTITY(1,1),
    FromStationID INT NOT NULL,
    ToStationID INT NOT NULL,
    Amount DECIMAL(18, 2) NOT NULL,
    PaymentMethod NVARCHAR(50) NOT NULL, -- 'Cash', 'QR', 'CreditCard'
    PurchaseDate DATETIME DEFAULT GETDATE(),
    Status NVARCHAR(20) DEFAULT 'Success',
    CONSTRAINT FK_From FOREIGN KEY (FromStationID) REFERENCES Stations(StationID),
    CONSTRAINT FK_To FOREIGN KEY (ToStationID) REFERENCES Stations(StationID)
);
GO

INSERT INTO Stations (NameVN, NameEN, StationOrder) VALUES 
(N'Bến Thành', N'Ben Thanh', 1), (N'Nhà hát TP', N'Opera House', 2),
(N'Ba Son', N'Ba Son', 3), (N'Văn Thánh', N'Van Thanh Park', 4),
(N'Tân Cảng', N'Tan Cang', 5), (N'Thảo Điền', N'Thao Dien', 6),
(N'An Phú', N'An Phu', 7), (N'Rạch Chiếc', N'Rach Chiec', 8),
(N'Phước Long', N'Phuoc Long', 9), (N'Bình Thái', N'Binh Thai', 10),
(N'Thủ Đức', N'Thu Duc', 11), (N'Khu CN cao', N'High-Tech Park', 12),
(N'Đại học Quốc gia', N'National University', 13), (N'Bến xe Suối Tiên', N'Suoi Tien Terminal', 14);


INSERT INTO FareSteps (FromStationCount, ToStationCount, CashPrice, NonCashPrice) VALUES 
(0, 5, 7000, 6000),   -- x <= 5 ga
(6, 6, 9000, 8000),   -- 6 ga
(7, 7, 10000, 9000),  -- 7 ga
(8, 8, 12000, 11000), -- 8 ga
(9, 10, 14000, 13000),
(11, 12, 16000, 15000),
(13, 13, 18000, 17000),
(14, 14, 20000, 19000); -- full ga

--Admin account
INSERT INTO tblUser (UserName, [Password], FullName, [Role]) 
VALUES ('admin', '123456', N'Quản trị viên Metro', 'Admin');
GO