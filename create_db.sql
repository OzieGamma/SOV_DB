CREATE TABLE [dbo].[Poeple] (
    [Id]                        BIGINT             NOT NULL,
    [Name]                      NVARCHAR (255)     NOT NULL,
    [Gender]                    NCHAR (1)          NOT NULL,
    [Trivia]                    NTEXT              NULL,
    [BirthDate]                 DATETIMEOFFSET (7) NULL,
    [DeathDate]                 DATETIMEOFFSET (7) NULL,
    [BirthName]                 NVARCHAR (255)     NULL,
    [ShortBio]                  NTEXT              NULL,
    [SpouseId]			BIGINT NULL,
    [Height]                    DECIMAL (18)       NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    
    FOREIGN KEY ([SpouseId]) REFERENCES [SpouseInfos]([Id]),
    CHECK ([Gender]='F' OR [Gender]='M')
);

CREATE TABLE [dbo].[SpouseInfos] (
    [Id]        BIGINT                NULL,
[Name]                NVARCHAR (255)     NULL,
    [BeginDate]           NVARCHAR (255)     NULL,
    [EndDate]             NVARCHAR (255)     NULL,
    [EndNotes]            TEXT               NULL,
    [ChildrenCount]       SMALLINT           NULL,
    [ChildrenDescription] TEXT               NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


CREATE TABLE [dbo].[Quotes] (
    [PersonId] BIGINT NOT NULL,
    [Quote]	NVARCHAR(1024) NULL,
    FOREIGN KEY ([PersonId]) REFERENCES [Poeple]([Id])
);

CREATE TABLE [dbo].[AlternativePersonNames] (
    [PersonId] BIGINT NOT NULL,
    [Name]	NVARCHAR(255) NOT NULL,
    FOREIGN KEY ([PersonId]) REFERENCES [Poeple]([Id])
);

CREATE TABLE [dbo].[VideoGames] (
    [Id]	BIGINT NOT NULL,
    [Title]	NVARCHAR(255) NOT NULL,
    [Year] INT NULL,
    [Genre] NVARCHAR(255),
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CHECK ([Genre] = 'VideoGame')
);

CREATE TABLE [dbo].[Movies] (
    [Id]	BIGINT NOT NULL,
    [Title]	NVARCHAR(255) NOT NULL,
    [Year] INT NULL,
    [Type] NVARCHAR(255),
    [Genre] NVARCHAR(255),
    [Budget] DECIMAL (18),
    [BudgetCurrency] NVARCHAR(255),
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CHECK ([Type] = 'VideoGame'),
    CHECK ([Genre] = 'VideoGame'),
    CHECK ([BudgetCurrency] = 'VideoGame')
);


CREATE TABLE [dbo].[Series] (
    [Id]	BIGINT NOT NULL,
    [Title]	NVARCHAR(255) NOT NULL,
    [Year] INT NULL,
    [Genre] NVARCHAR(255),
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CHECK ([Genre] = 'VideoGame')
);


CREATE TABLE [dbo].[Episodes] (
    [Id]	BIGINT NOT NULL,
    [Title]	NVARCHAR(255) NOT NULL,
    [Year] INT NULL,
    [SeriesId] BIGINT,
    [SeasonNumber] INT,
    [EpisodeNumber] INT,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([SeriesId]) REFERENCES [Series]([Id])
);


GO

--Create the view that combines all productions.
CREATE VIEW [dbo].[Productions]
WITH SCHEMABINDING
AS
SELECT [Id], [Title], [Year]
FROM [dbo].[VideoGames]
UNION ALL
SELECT [Id], [Title], [Year]
FROM [dbo].[Movies]
UNION ALL
SELECT [Id], [Title], [Year]
FROM [dbo].[Series]
UNION ALL
SELECT [Id], [Title], [Year]
FROM [dbo].[Episodes]

GO

CREATE TABLE [dbo].[AlternativeProductionTitle] (
    [ProductionId]	BIGINT NOT NULL,
    [Name] NVARCHAR(255),
    FOREIGN KEY ([ProductionId]) REFERENCES [Productions]([Id])
);

CREATE TABLE [dbo].[ProductionCast] (
    [ProductionId]	BIGINT NOT NULL,
    [PersonId]	BIGINT NOT NULL,
    [CharacterId]	BIGINT NOT NULL,
    [Role] NVARCHAR(255),
    CHECK ([Role] = 'VideoGame')
);

CREATE TABLE [dbo].[Character] (
    [Id]	BIGINT NOT NULL,
    [Name] NVARCHAR(255),
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Company] (
    [Id]	BIGINT NOT NULL,
    [Name] NVARCHAR(255),
    [CountryCode] NVARCHAR(255),
    [Type] NVARCHAR(255),
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CHECK ([CountryCode] = 'VideoGame'),
    CHECK ([Type] = 'VideoGame')
);

CREATE TABLE [dbo].[ProcutionCompanies] (
    [ProductionId]	BIGINT NOT NULL,
    [CompanyId] BIGINT NOT NULL
);