-- Create Messages table
CREATE TABLE [dbo].[Messages] (
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [FromName] NVARCHAR(100) NOT NULL,
    [FromEmail] NVARCHAR(255) NOT NULL,
    [ToName] NVARCHAR(100) NOT NULL,
    [ToEmail] NVARCHAR(255) NOT NULL,
    [Subject] NVARCHAR(200) NOT NULL,
    [Body] NVARCHAR(MAX) NOT NULL,
    [Submitted] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT [PK_Messages] PRIMARY KEY CLUSTERED ([Id] ASC)
);


CREATE TABLE SendAttempts (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    MessageId INT NOT NULL,
    DateTime DATETIME NULL,
    Successful BIT NOT NULL,
    ErrorMessage NVARCHAR(MAX) NULL,
    SendAttempts INT NOT NULL
);