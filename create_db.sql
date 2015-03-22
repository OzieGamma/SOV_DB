-- Person & PersonSpouse

CREATE TABLE Person (
    Id        INT NOT NULL,
    Name      NVARCHAR (255) NOT NULL,
    Gender    NCHAR (1),
    Trivia    NVARCHAR (255),
    Quotes    NVARCHAR (255),
    BirthDate DATE,
    DeathDate DATE,
    BirthName NVARCHAR (255),
    ShortBio  NVARCHAR (255),
    Height    NUMERIC (5),
    SpouseId  INT,
    PRIMARY KEY (Id),
    FOREIGN KEY (SpouseId) REFERENCES PersonSpouse(Id),
    CHECK (Gender = 'F' OR Gender = 'M' OR Gender IS NULL)
);

CREATE TABLE PersonSpouse (
    Id		            INT NOT NULL,
    Name                NVARCHAR (255),
    IsInDatabase        BIT,
    BeginDate           NVARCHAR (255),
    EndDate             NVARCHAR (255),
    EndNotes            NVARCHAR (255),
    ChildrenCount       INT,
    ChildrenDescription NVARCHAR (255),
    PRIMARY KEY ("Id")
);