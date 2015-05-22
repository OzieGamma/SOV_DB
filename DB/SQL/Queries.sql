-- a) Compute the number of movies per year. Make sure to include tv and video movies.
SELECT ReleaseYear, COUNT(*) AS NumMovies
FROM Movie JOIN Production ON Movie.ProductionId = Production.Id 
GROUP BY ReleaseYear;

-- b) Compute the ten countries with most production companies.
SELECT TOP 10 CountryCode 
FROM Company 
WHERE CountryCode IS NOT NULL
GROUP BY CountryCode
ORDER BY COUNT(*) DESC;

-- c) Compute the min, max and average career duration.
--    (A career length is implied by the first and last production of a person)
SELECT MIN(temp.Duration) AS MinDuration, MAX(temp.Duration) AS MaxDuration, AVG(temp.Duration) AS AvgDuration
FROM
(
    SELECT MAX(ShortProd.ReleaseYear) - MIN(ShortProd.ReleaseYear) + 1 AS Duration
    FROM ProductionCast
    JOIN (SELECT Id, ReleaseYear FROM Production) AS ShortProd ON ShortProd.Id = ProductionCast.ProductionId
    GROUP BY ProductionCast.PersonId
) AS temp;

-- d) Compute the min, max and average number of actors in a production.
SELECT MIN(number.value) AS MinNumber, MAX(number.value) AS MaxNumber, AVG(number.value) AS AvgNumber
FROM 
(
    SELECT COUNT(*) AS value
    FROM ProductionCast
    WHERE CastRole IN ('Actor', 'Actress')
    GROUP BY ProductionId
) AS number;

-- e) Compute the min, max and average height of female persons.
SELECT MIN(Height) AS MinHeight, MAX(Height) AS MaxHeignt, AVG(Height) AS AvgHeight
FROM Person 
WHERE Gender = 'F';

-- f) List all pairs of persons and movies where the person has both directed the movie and acted in the
-- movie. Do not include tv and video movies.
SELECT Person.FirstName, Person.LastName, Production.Title
FROM
  Movie 
  JOIN Production ON Movie.ProductionId = Production.Id AND MovieType = 'Normal'
  JOIN ProductionCast ON ProductionCast.ProductionId = Production.Id
  JOIN Person ON Person.Id = ProductionCast.PersonId
WHERE ProductionCast.CastRole IN ('Actor', 'Actress')
  AND ProductionCast.PersonId IN
      (SELECT PersonId FROM ProductionCast PC2 WHERE ProductionCast.ProductionId = PC2.ProductionId AND PC2.CastRole = 'Director');


-- g) List the three most popular character names
SELECT TOP 3 Name, COUNT(*) AS Popularity
FROM ProductionCast -- Counts several appearances of the same character several times.
JOIN ProductionCharacter ON ProductionCharacter.Id = ProductionCast.CharacterId
GROUP BY ProductionCharacter.Name
ORDER BY COUNT(*) DESC

-- 3.a) Find the actors and actresses (and report the productions) who played in a production where they were 55 or more year older than the youngest actor/actress playing.
SELECT DISTINCT FirstName, LastName, Production.Title
FROM(
    SELECT DISTINCT ProductionId, MIN(Person.BirthDate) AS MinBirthDay
    FROM Person
    JOIN ProductionCast ON PersonId = Person.Id
    WHERE ProductionCast.CastRole IN ('Actor', 'Actress')
    GROUP BY ProductionId
) AS MinBirthDayPerProduction
JOIN Production ON Production.id = MinBirthDayPerProduction.ProductionId
JOIN ProductionCast ON MinBirthDayPerProduction.ProductionId = ProductionCast.PersonId
JOIN Person ON Person.Id = ProductionCast.PersonId AND DATEDIFF(YEAR, Person.BirthDate, MinBirthDay) >= 55

-- 3.b) Given an actor, compute his most productive year.
-- Here is a query that computes it for each actor:

-- Our query will display all exe-quos
SELECT DISTINCT Person.FirstName, Person.LastName, PersonId, ReleaseYear AS MostProductiveYear, NumReleases
FROM(
    SELECT DISTINCT *, RANK() OVER (PARTITION BY PersonId ORDER BY NumReleases DESC) AS Rank
    FROM (
        SELECT DISTINCT ProductionCast.PersonId, Production.ReleaseYear, COUNT(*) AS NumReleases
        FROM ProductionCast
        JOIN Production ON ProductionCast.ProductionId = Production.Id
        GROUP BY ProductionCast.PersonId, Production.ReleaseYear
    ) AS ProductionsPerYearPerPerson
) AS ProductionsPerYearPerPersonWithRank
JOIN Person ON  Person.Id = PersonId
WHERE Rank <= 1

-- 3.c) Given a year, list the company with the highest number of productions in each genre.

-- Our query will display all exe-quos
SELECT DISTINCT ReleaseYear, Genre, ProductionCount, Company.Name
FROM(
    SELECT DISTINCT *, RANK() OVER (PARTITION BY ReleaseYear, Genre ORDER BY ProductionCount DESC) AS Rank
    FROM (
        SELECT Company.Id AS CompanyId, Production.Genre, Production.ReleaseYear, COUNT(*) AS ProductionCount
        FROM Production
        JOIN ProductionCompany ON Production.Id = ProductionCompany.ProductionId
        JOIN Company ON Company.Id = ProductionCompany.CompanyId
        WHERE ProductionCompany.Kind = 'ProductionCompany'
        AND Genre IS NOT NULL
        AND ReleaseYear IS NOT NULL
        GROUP BY Company.Id, Production.ReleaseYear, Production.Genre
    ) AS ProductionsPerCompanyPerGenre
) AS ProductionsPerCompanyPerGenreWithRank
JOIN Company ON  Company.Id = CompanyId
WHERE Rank <= 1
ORDER BY ReleaseYear ASC, Genre DESC

-- 3.d) Compute who worked with spouses/children/potential relatives on the same production. (You can assume that the same real surname implies a relation)
SELECT DISTINCT a.FirstName AS FirstPerson, b.FirstName AS SecondPerson, a.LastName
FROM Person a
JOIN Person b ON a.LastName = b.LastName AND a.Id > b.Id -- TODO: Make sure we only get one of the pair ('bob', 'john') and  ('john', 'bob')
JOIN ProductionCast castA ON castA.PersonId = a.Id
JOIN ProductionCast castB ON castB.PersonId = b.Id AND castA.ProductionId = castB.ProductionId

-- 3.e) Compute the of average number of actors per production per year
SELECT DISTINCT ReleaseYear, AVG(CAST(NumActors AS FLOAT)) AS AvgNumActorsPerProduction
FROM(
    SELECT DISTINCT Production.ReleaseYear, ProductionCast.ProductionId, COUNT(*) AS NumActors
    FROM ProductionCast
    JOIN Production ON Production.Id = ProductionId
    WHERE CastRole IN ('Actor', 'Actress') AND ReleaseYear IS NOT NULL
    GROUP BY Production.ReleaseYear, ProductionCast.ProductionId
) AS NumActorsInProduction
GROUP BY ReleaseYear
ORDER BY ReleaseYear ASC


-- 3.f) Compute the average number of episodes per season.
SELECT SubQuery.SeasonNumber, AVG(CAST(SubQuery.NumEpisodes AS FLOAT))
FROM(
    SELECT SeriesEpisode.SeriesId, SeriesEpisode.SeasonNumber, COUNT(*) AS NumEpisodes
    FROM SeriesEpisode
    WHERE SeriesEpisode.SeasonNumber IS NOT NULL
    GROUP BY SeriesEpisode.SeriesId, SeriesEpisode.SeasonNumber
) AS SubQuery
GROUP BY SubQuery.SeasonNumber
ORDER BY SubQuery.SeasonNumber ASC

-- 3.g) Compute the average number of seasons per series.
SELECT AVG(CAST(SeasonsPerSeries.NumSeasons AS FLOAT))
FROM(
    SELECT EpisodesInASeason.SeriesId, COUNT(*) AS NumSeasons
    FROM(
        SELECT DISTINCT SeriesEpisode.SeriesId, SeriesEpisode.SeasonNumber
        FROM SeriesEpisode
        GROUP BY SeriesEpisode.SeriesId, SeriesEpisode.SeasonNumber
    ) AS EpisodesInASeason
    GROUP BY EpisodesInASeason.SeriesId
) AS SeasonsPerSeries

-- 3.h) Compute the top ten tv-series (by number of seasons).
SELECT TOP 10 Production.Title, SeasonsPerSeries.NumSeasons
FROM(
    SELECT EpisodesInASeason.SeriesId, COUNT(*) AS NumSeasons
    FROM(
        SELECT DISTINCT SeriesEpisode.SeriesId, SeriesEpisode.SeasonNumber
        FROM SeriesEpisode
        GROUP BY SeriesEpisode.SeriesId, SeriesEpisode.SeasonNumber
    ) AS EpisodesInASeason
    GROUP BY EpisodesInASeason.SeriesId
) AS SeasonsPerSeries
JOIN Production ON Production.Id = SeasonsPerSeries.SeriesId
ORDER BY SeasonsPerSeries.NumSeasons DESC

-- 3.i) Compute the top ten tv-series (by number of episodes per season).
SELECT TOP 10 Production.Title, AvgNumEpisodesPerSeasonOfASeries.AvgNumEpisodes
FROM(
    SELECT EpisodesInASeasonOfASeries.SeriesId, AVG(CAST(EpisodesInASeasonOfASeries.NumEpisodes AS FLOAT)) AS AvgNumEpisodes
    FROM(
        SELECT SeriesEpisode.SeriesId, SeriesEpisode.SeasonNumber, COUNT(*) AS NumEpisodes
        FROM SeriesEpisode
        WHERE SeriesEpisode.SeasonNumber IS NOT NULL
        GROUP BY SeriesEpisode.SeriesId, SeriesEpisode.SeasonNumber
    ) AS EpisodesInASeasonOfASeries
    GROUP BY EpisodesInASeasonOfASeries.SeriesId
) AS AvgNumEpisodesPerSeasonOfASeries
JOIN Production ON Production.Id = AvgNumEpisodesPerSeasonOfASeries.SeriesId
ORDER BY AvgNumEpisodesPerSeasonOfASeries.AvgNumEpisodes DESC

-- 3.j) Find actors, actresses and directors who have movies (including tv movies and video movies) released after their death.
SELECT DISTINCT Person.FirstName, Person.LastName, Production.Title
FROM ProductionCast
JOIN Person ON ProductionCast.PersonId = Person.Id
JOIN Production ON Production.Id = ProductionCast.ProductionId AND Production.ReleaseYear > DATEPART(YEAR, Person.DeathDate)
WHERE ProductionCast.CastRole IN ('Actor', 'Actress', 'Director')

-- 3.k) For each year, show three companies that released the most movies.
SELECT DISTINCT ReleaseYear AS Year, Company.Name, NumMovies, Rank
FROM(
    SELECT DISTINCT *, RANK() OVER (PARTITION BY ReleaseYear ORDER BY NumMovies DESC) AS Rank
    FROM (
        SELECT ProductionCompany.CompanyId, Production.ReleaseYear, COUNT(*) AS NumMovies
        FROM Movie
        JOIN Production ON Movie.ProductionId = Production.Id
        JOIN ProductionCompany ON Production.Id = ProductionCompany.ProductionId
        WHERE Production.ReleaseYear IS NOT NULL
        GROUP BY ProductionCompany.CompanyId, Production.ReleaseYear
    ) AS MoviesPerCompanyInAYear
) AS MoviesPerCompanyInAYearWithRank
JOIN Company ON Company.Id = CompanyId
WHERE Rank <= 3
ORDER BY ReleaseYear, NumMovies DESC

SELECT SERVERPROPERTY('COLLATION')

-- 3.l) List all living people who are opera singers ordered from youngest to oldest.
SELECT FirstName, LastName, BirthDate
FROM Person
WHERE Trivia LIKE '%opera%' COLLATE SQL_Latin1_General_CP1_CI_AS  -- Make sure it is case insensitive
    OR Quotes LIKE '%opera%' COLLATE SQL_Latin1_General_CP1_CI_AS 
    OR ShortBio LIKE '%opera%' COLLATE SQL_Latin1_General_CP1_CI_AS 
    AND BirthDate IS NOT NULL 
ORDER BY BirthDate DESC

-- 3.m) List 10 most ambiguous credits (pairs of people and productions) ordered by the degree of ambiguity.
-- A credit is ambiguous if either a person has multiple alternative names or a production has multiple alternative titles. 
-- The degree of ambiguity is a product of the number of possible names (real name + all alternatives) 
-- and the number of possible titles (real + alternatives).
SELECT DISTINCT TOP 10 PersonWithAlternatives.Id AS PersonId, 
             ProductionWithAlternatives.Id AS ProductionId,
             PersonWithAlternatives.NumPersonNames * ProductionWithAlternatives.NumProductionNames AS Ambiguity 
FROM (
    SELECT DISTINCT Person.Id, COUNT(*) AS NumPersonNames
    FROM Person
    JOIN AlternativePersonName ON AlternativePersonName.PersonId = Person.Id
    GROUP BY Person.Id
) AS PersonWithAlternatives
JOIN ProductionCast ON ProductionCast.PersonId = PersonWithAlternatives.Id
JOIN (
    SELECT DISTINCT Production.Id, COUNT(*) AS NumProductionNames
    FROM Production
    JOIN AlternativeProductionTitle ON AlternativeProductionTitle.ProductionId = Production.Id
    GROUP BY Production.Id
) AS ProductionWithAlternatives ON ProductionWithAlternatives.Id = ProductionCast.ProductionId
ORDER BY PersonWithAlternatives.NumPersonNames * ProductionWithAlternatives.NumProductionNames DESC

-- 3.n) For each country, list the most frequent character name that appears in the productions of a production company (not a distributor) from that country.
-- Will give all options if several.
SELECT  CountryCode, Name, NameCount
FROM(
    SELECT DISTINCT *, RANK() OVER (PARTITION BY CountryCode ORDER BY NameCount DESC) AS Rank
    FROM (
        SELECT DISTINCT Company.CountryCode, ProductionCharacter.Name, COUNT(*) AS NameCount
        FROM Company
        JOIN ProductionCompany ON ProductionCompany.Kind = 'ProductionCompany' AND ProductionCompany.CompanyId = Company.Id
        JOIN Production ON Production.Id = ProductionCompany.ProductionId
        JOIN ProductionCast ON ProductionCast.ProductionId = Production.Id
        JOIN ProductionCharacter ON ProductionCharacter.Id = ProductionCast.CharacterId
        WHERE Company.CountryCode IS NOT NULL
        GROUP BY Company.CountryCode, ProductionCharacter.Name
    ) AS NameWithNameCountPerCountry
) AS NameWithNameCountPerCountryWithRank
WHERE Rank <= 1
ORDER BY CountryCode ASC