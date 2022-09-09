CREATE TABLE [dbo].[Table]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [User_name] VARCHAR(50) NOT NULL, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Moble] NVARCHAR(50) NOT NULL, 
    [Email] NVARCHAR(50) NOT NULL, 
    [Password] NVARCHAR(50) NOT NULL, 
    [Date] DATETIME NOT NULL
)
