CREATE TABLE [dbo].[Person] (
    [Id]                        BIGINT             NOT NULL,
    [Name]                      NVARCHAR (255)     NOT NULL,
    [Gender]                    NCHAR (6)          NOT NULL,
    [Trivia]                    NTEXT              NULL,
    [Quotes]                    NTEXT              NULL,
    [BirthDate]                 DATETIMEOFFSET (7) NULL,
    [DeathDate]                 DATETIMEOFFSET (7) NULL,
    [BirthName]                 NVARCHAR (255)     NULL,
    [ShortBio]                  NTEXT              NULL,
    [SpouseName]                NVARCHAR (255)     NULL,
    [SpouseIsInDatabase]        BIT                NULL,
    [SpouseBeginDate]           NVARCHAR (255)     NULL,
    [SpouseEndDate]             NVARCHAR (255)     NULL,
    [SpouseEndNotes]            TEXT               NULL,
    [SpouseChildrenCount]       SMALLINT           NULL,
    [SpouseChildrenDescription] TEXT               NULL,
    [Height]                    DECIMAL (18)       NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CHECK ([Gender]='F' OR [Gender]='M')
);

