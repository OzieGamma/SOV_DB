-- Drop all tables
IF EXISTS(SELECT NULL FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Person') DROP TABLE Person;
IF EXISTS(SELECT NULL FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PersonSpouse') DROP TABLE PersonSpouse;

-- Person & PersonSpouse

CREATE TABLE PersonSpouse (
    Id		            INT NOT NULL,
    Name                NVARCHAR (MAX),
    IsInDatabase        BIT,
    BeginDate           NVARCHAR (100),
    EndDate             NVARCHAR (100),
    EndNotes            NVARCHAR (MAX),
    ChildrenCount       INT,
    ChildrenDescription NVARCHAR (MAX),
    PRIMARY KEY (Id)
);

CREATE TABLE Person (
    Id        INT NOT NULL,
    Name      NVARCHAR (MAX) NOT NULL,
    Gender    NCHAR (1),
    Trivia    NVARCHAR (MAX),
    Quotes    NVARCHAR (MAX),
    BirthDate DATE,
    DeathDate DATE,
    BirthName NVARCHAR (MAX),
    ShortBio  NVARCHAR (MAX),
    Height    NUMERIC (5),
    SpouseId  INT,
    PRIMARY KEY (Id),
    FOREIGN KEY (SpouseId) REFERENCES PersonSpouse(Id),
    CHECK (Gender = 'F' OR Gender = 'M' OR Gender IS NULL)
);