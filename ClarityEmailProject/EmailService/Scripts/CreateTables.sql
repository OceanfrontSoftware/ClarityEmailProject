-- Create Messages table
CREATE TABLE [dbo].[Messages] (
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [FromName] NVARCHAR(100) NOT NULL,
    [FromEmail] NVARCHAR(255) NOT NULL,
    [ToName] NVARCHAR(100) NOT NULL,
    [ToEmail] NVARCHAR(255) NOT NULL,
    [Subject] NVARCHAR(200) NOT NULL,
    [Body] NVARCHAR(MAX) NOT NULL,
    CONSTRAINT [PK_Messages] PRIMARY KEY CLUSTERED ([Id] ASC)
);

-- Create SendAttempts table
CREATE TABLE [dbo].[SendAttempts] (
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [MessageId] INT NOT NULL,
    [DateTime] DATETIME2 NOT NULL,
    [Successful] BIT NOT NULL,
    [ErrorMessage] NVARCHAR(MAX) NULL,
    CONSTRAINT [PK_SendAttempts] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SendAttempts_Messages] FOREIGN KEY ([MessageId]) 
        REFERENCES [dbo].[Messages] ([Id]) ON DELETE CASCADE
);

-- Create indexes
CREATE NONCLUSTERED INDEX [IX_SendAttempts_MessageId] 
    ON [dbo].[SendAttempts] ([MessageId] ASC);

-- Add default constraint for DateTime
ALTER TABLE [dbo].[SendAttempts] 
    ADD CONSTRAINT [DF_SendAttempts_DateTime] 
    DEFAULT (GETUTCDATE()) FOR [DateTime]; 