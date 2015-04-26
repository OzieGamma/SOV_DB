-- People and their alternative names

CREATE TABLE Person (
    Id INT NOT NULL,
    FirstName NVARCHAR (MAX) NOT NULL,
    LastName NVARCHAR (MAX),
    Gender NCHAR (1),
    Trivia NVARCHAR (MAX),
    Quotes NVARCHAR (MAX),
    BirthDate DATE,
    DeathDate DATE,
    BirthName NVARCHAR (MAX),
    ShortBio NVARCHAR (MAX),
    SpouseInfo NVARCHAR (MAX),
    Height INT,
    PRIMARY KEY (Id),
    CHECK (Gender IN ('F', 'M'))
);

CREATE TABLE AlternativePersonName (
    PersonId INT NOT NULL,
    Name NVARCHAR(MAX) NOT NULL,
    FOREIGN KEY (PersonId) REFERENCES Person(Id)
);

-- Productions: Video games, movies, series, episodes, and their alternative names

CREATE TABLE Production (
    Id INT NOT NULL,
    Title NVARCHAR(MAX) NOT NULL,
    ReleaseYear INT,
    Genre NVARCHAR(12),
    PRIMARY KEY (Id),
    CHECK (Genre IN ('Action', 'Adventure', 'Animation', 'Biography', 'Comedy', 
                     'Crime', 'Documentary', 'Drama', 'Family', 'Fantasy', 
                     'FilmNoir', 'GameShow', 'History', 'Horror', 'Music', 
                     'Musical', 'Mystery', 'News', 'RealityTV', 'Romance', 
                     'SciFi', 'Short', 'Sport', 'TalkShow', 'Thriller', 'War',
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

CREATE TABLE SeriesEpisode (
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
    Title NVARCHAR(MAX),
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
    CharacterId INT,
    CastRole NVARCHAR(MAX) NOT NULL,
    FOREIGN KEY (ProductionId) REFERENCES Production(Id),
    FOREIGN KEY (PersonId) REFERENCES Person(Id),
    FOREIGN KEY (CharacterId) REFERENCES ProductionCharacter(Id),
    CHECK (CastRole IN ('Actor', 'Actress', 'Cinematographer', 'Composer', 
                        'CostumeDesigner', 'Director', 'Editor', 
                        'MiscellaneousCrew', 'Producer', 
                        'ProductionDesigner', 'Writer'))
);

-- Companies

CREATE TABLE Company (
    Id INT NOT NULL,
    CountryCode NVARCHAR(4),
    Name NVARCHAR(MAX) NOT NULL,
    PRIMARY KEY (Id)
    -- No constraint on CountryCode,
    -- ISO-3166 codes change more often than one would think
);

CREATE TABLE ProductionCompany (
    ProductionId INT NOT NULL,
    CompanyId INT NOT NULL,
    Kind NVARCHAR(MAX)
    FOREIGN KEY (ProductionId) REFERENCES Production(Id),
    FOREIGN KEY (CompanyId) REFERENCES Company(Id),
    CHECK (Kind IN ('ProductionCompany', 'Distributor'))
);