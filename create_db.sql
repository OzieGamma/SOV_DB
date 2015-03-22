-- Disable referential integrity on all tables, and drop them. Not pretty, but it works. 
EXEC sp_MSforeachtable "DECLARE @name NVARCHAR (MAX); SET @name = PARSENAME('?', 1); EXEC sp_MSdropconstraints @name";
EXEC sp_MSforeachtable "DROP TABLE ?";

-- Person, alt names, and spouses

CREATE TABLE PersonSpouse (
    Id INT NOT NULL,
    Name NVARCHAR (MAX),
    IsInDatabase BIT,
    BeginDate NVARCHAR (MAX),
    EndDate NVARCHAR (MAX),
    EndNotes NVARCHAR (MAX),
    ChildrenCount INT,
    ChildrenDescription NVARCHAR (MAX),
    PRIMARY KEY (Id)
);

CREATE TABLE Person (
    Id INT NOT NULL,
    Name NVARCHAR (MAX) NOT NULL,
    Gender NCHAR (1),
    Trivia NVARCHAR (MAX),
    Quotes NVARCHAR (MAX),
    BirthDate DATE,
    DeathDate DATE,
    BirthName NVARCHAR (MAX),
    ShortBio NVARCHAR (MAX),
    Height NUMERIC (5),
    SpouseId INT,
    PRIMARY KEY (Id),
    FOREIGN KEY (SpouseId) REFERENCES PersonSpouse(Id),
    CHECK (Gender IN ('F', 'M'))
);

CREATE TABLE AlternativePersonName (
    PersonId INT NOT NULL,
    Name NVARCHAR(MAX) NOT NULL,
    FOREIGN KEY (PersonId) REFERENCES Person(Id)
);

-- Productions: Video games, movies, series, episodes, and alternative names

CREATE TABLE Production (
    Id INT NOT NULL,
    Title NVARCHAR(MAX) NOT NULL,
    ReleaseYear INT,
    Genre NVARCHAR(12),
    PRIMARY KEY (Id),
    CHECK (Genre IN ('Action', 'Adventure', 'Animation', 'Biography', 'Comedy', 'Crime', 
                     'Documentary', 'Drama', 'Family', 'Fantasy', 'FilmNoir', 'GameShow',
                     'History', 'Horror', 'Music', 'Mystery', 'News', 'RealityTV',
                     'Romance', 'SciFi', 'Short', 'Sport', 'TalkShow', 'Thriller', 'War',
                     'Western'))
);

CREATE TABLE VideoGame (
    ProductionId INT NOT NULL,
    PRIMARY KEY (ProductionId),
    FOREIGN KEY (ProductionId) REFERENCES Production(Id)
);

CREATE TABLE Movie (
    ProductionId INT NOT NULL,
    MovieType NVARCHAR(6) NOT NULL,
    PRIMARY KEY (ProductionId),
    FOREIGN KEY (ProductionId) REFERENCES Production(Id),
    CHECK (MovieType IN ('Normal', 'Video', 'TV'))
);

CREATE TABLE Series (
    ProductionId INT NOT NULL,
    BeginningYear INT,
    EndYear INT,
    PRIMARY KEY (ProductionId),
    FOREIGN KEY (ProductionId) REFERENCES Production(Id)
);

CREATE TABLE Episode (
    ProductionId INT NOT NULL,
    SeriesId INT NOT NULL,
    SeasonNumber INT,
    EpisodeNumber INT,
    PRIMARY KEY (ProductionId),
    FOREIGN KEY (ProductionId) REFERENCES Production(Id),
    FOREIGN KEY (SeriesId) REFERENCES Series(ProductionId)
);

CREATE TABLE AlternativeProductionTitle (
    ProductionId INT NOT NULL,
    Name NVARCHAR(MAX),
    FOREIGN KEY (ProductionId) REFERENCES Production(Id)
);


-- Production cast: Characters, and the cast

CREATE TABLE ProductionCharacter (
    Id INT NOT NULL,
    Name NVARCHAR(MAX) NOT NULL,
    PRIMARY KEY (Id)
);

CREATE TABLE ProductionCast (
    ProductionId INT NOT NULL,
    PersonId INT NOT NULL,
    CharacterId INT NOT NULL,
    CastRole NVARCHAR(MAX),
    FOREIGN KEY (ProductionId) REFERENCES Production(Id),
    FOREIGN KEY (PersonId) REFERENCES Person(Id),
    FOREIGN KEY (CharacterId) REFERENCES ProductionCharacter(Id),
    CHECK (CastRole IN ('Actor', 'Actress', 'Cinematographer', 'Composer', 'CostumeDesigner', 'Director', 
                        'Editor', 'MiscellaneousCrew', 'Producer', 'ProductionDesigner', 'Writer'))
);

-- Companies

CREATE TABLE Company (
    Id INT NOT NULL,
    Name NVARCHAR(MAX) NOT NULL,
    CountryCode NVARCHAR(4),
    PRIMARY KEY (Id)
    -- No constraint on CountryCode, as ISO-3166 codes change more often than one would think
);

CREATE TABLE ProductionCompany (
    ProductionId INT NOT NULL,
    CompanyId INT NOT NULL,
    Kind NVARCHAR(MAX)
    FOREIGN KEY (ProductionId) REFERENCES Production(Id),
    FOREIGN KEY (CompanyId) REFERENCES Company(Id),
    CHECK (Kind IN ('ProductionCompany', 'Distributor'))
);