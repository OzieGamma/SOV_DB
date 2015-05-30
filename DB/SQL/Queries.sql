/* 2.a) Compute the number of movies per year. Make sure to include tv and video movies.
NOTE: we decided to remove entries with release year = NULL. We think the user wouldn't want to see it.
 SQL Server Execution Times:
   CPU time = 0 ms,  elapsed time = 0 ms.
SQL Server parse and compile time: 
   CPU time = 0 ms, elapsed time = 0 ms.

    (139 row(s) affected)
    
Table 'Worktable'. Scan count 0, logical reads 0, physical reads 0, read-ahead reads 0, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'Workfile'. Scan count 0, logical reads 0, physical reads 0, read-ahead reads 0, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'Production'. Scan count 1, logical reads 28651, physical reads 0, read-ahead reads 0, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'Movie'. Scan count 1, logical reads 5526, physical reads 0, read-ahead reads 0, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.

    (1 row(s) affected)
    

 SQL Server Execution Times:
   CPU time = 734 ms,  elapsed time = 775 ms.
SQL Server parse and compile time: 
   CPU time = 0 ms, elapsed time = 0 ms.

 SQL Server Execution Times:
   CPU time = 0 ms,  elapsed time = 0 ms.
*/
SELECT ReleaseYear, COUNT(*) AS NumMovies
FROM Movie JOIN Production ON Movie.ProductionId = Production.Id
WHERE ReleaseYear IS NOT NULL
GROUP BY ReleaseYear

/* 2.b) Compute the ten countries with most production companies.
    NOTE: We removed NULL country codes because it doesn't really make sense to report them.
 SQL Server Execution Times:
   CPU time = 0 ms,  elapsed time = 0 ms.
SQL Server parse and compile time: 
   CPU time = 0 ms, elapsed time = 3 ms.

    (10 row(s) affected)
    
Table 'Worktable'. Scan count 0, logical reads 0, physical reads 0, read-ahead reads 0, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'Workfile'. Scan count 0, logical reads 0, physical reads 0, read-ahead reads 0, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'Company'. Scan count 1, logical reads 2544, physical reads 3, read-ahead reads 2540, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.

    (1 row(s) affected)
    

 SQL Server Execution Times:
   CPU time = 94 ms,  elapsed time = 125 ms.
SQL Server parse and compile time: 
   CPU time = 0 ms, elapsed time = 0 ms.

 SQL Server Execution Times:
   CPU time = 0 ms,  elapsed time = 0 ms.

*/
SELECT TOP 10 CountryCode 
FROM Company 
WHERE CountryCode IS NOT NULL
GROUP BY CountryCode
ORDER BY COUNT(*) DESC;

/* 2.c) Compute the min, max and average career duration.
    (A career length is implied by the first and last production of a person)
    
 SQL Server Execution Times:
   CPU time = 0 ms,  elapsed time = 0 ms.
SQL Server parse and compile time: 
   CPU time = 0 ms, elapsed time = 0 ms.
Warning: Null value is eliminated by an aggregate or other SET operation.

    (1 row(s) affected)
    
Table 'Worktable'. Scan count 0, logical reads 0, physical reads 0, read-ahead reads 0, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'Workfile'. Scan count 240, logical reads 136832, physical reads 26686, read-ahead reads 196402, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'ProductionCast'. Scan count 1, logical reads 300485, physical reads 10, read-ahead reads 256659, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'Production'. Scan count 1, logical reads 28651, physical reads 52, read-ahead reads 5153, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.

    (1 row(s) affected)
    

 SQL Server Execution Times:
   CPU time = 50203 ms,  elapsed time = 56075 ms.
SQL Server parse and compile time: 
   CPU time = 0 ms, elapsed time = 0 ms.

 SQL Server Execution Times:
   CPU time = 0 ms,  elapsed time = 0 ms.

*/
SELECT MIN(temp.Duration) AS MinDuration, MAX(temp.Duration) AS MaxDuration, AVG(temp.Duration) AS AvgDuration
FROM
(
    SELECT MAX(ShortProd.ReleaseYear) - MIN(ShortProd.ReleaseYear) + 1 AS Duration
    FROM ProductionCast
    JOIN (SELECT Id, ReleaseYear FROM Production) AS ShortProd ON ShortProd.Id = ProductionCast.ProductionId
    GROUP BY ProductionCast.PersonId
) AS temp;

/* 2.d) Compute the min, max and average number of actors in a production.
SQL Server Execution Times:
   CPU time = 0 ms,  elapsed time = 0 ms.
SQL Server parse and compile time: 
   CPU time = 0 ms, elapsed time = 3 ms.

    (1 row(s) affected)
    
Table 'Worktable'. Scan count 0, logical reads 0, physical reads 0, read-ahead reads 0, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'Workfile'. Scan count 2, logical reads 10016, physical reads 1124, read-ahead reads 8892, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'ProductionCast'. Scan count 1, logical reads 300485, physical reads 0, read-ahead reads 202561, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.

    (1 row(s) affected)
    

 SQL Server Execution Times:
   CPU time = 27953 ms,  elapsed time = 28198 ms.
SQL Server parse and compile time: 
   CPU time = 0 ms, elapsed time = 0 ms.

 SQL Server Execution Times:
   CPU time = 0 ms,  elapsed time = 0 ms.
*/
SELECT MIN(number.value) AS MinNumber, MAX(number.value) AS MaxNumber, AVG(number.value) AS AvgNumber
FROM 
(
    SELECT COUNT(*) AS value
    FROM ProductionCast
    WHERE CastRole IN ('Actor', 'Actress')
    GROUP BY ProductionId
) AS number;

/* 2.e) Compute the min, max and average height of female persons.
    NOTE: SQL Server warned that entries with null where being eliminated by aggregate functions, so we removed them explicitly
 SQL Server Execution Times:
   CPU time = 0 ms,  elapsed time = 0 ms.
SQL Server parse and compile time: 
   CPU time = 0 ms, elapsed time = 0 ms.
SQL Server parse and compile time: 
   CPU time = 0 ms, elapsed time = 0 ms.

    (1 row(s) affected)
    
Table 'Person'. Scan count 1, logical reads 66751, physical reads 2, read-ahead reads 13928, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.

    (1 row(s) affected)
    

 SQL Server Execution Times:
   CPU time = 438 ms,  elapsed time = 461 ms.
SQL Server parse and compile time: 
   CPU time = 0 ms, elapsed time = 0 ms.

 SQL Server Execution Times:
   CPU time = 0 ms,  elapsed time = 0 ms.
*/
SELECT MIN(Height) AS MinHeight, MAX(Height) AS MaxHeignt, AVG(Height) AS AvgHeight
FROM Person 
WHERE Height IS NOT NULL AND Gender = 'F';

/* 2.f) List all pairs of persons and movies where the person has both directed the movie and acted in the
 movie. Do not include tv and video movies.
 
 SQL Server Execution Times:
   CPU time = 0 ms,  elapsed time = 0 ms.
SQL Server parse and compile time: 
   CPU time = 110 ms, elapsed time = 126 ms.

    (100492 row(s) affected)
    
Table 'Person'. Scan count 0, logical reads 457383, physical reads 1371, read-ahead reads 28659, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'Workfile'. Scan count 0, logical reads 0, physical reads 0, read-ahead reads 0, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'Worktable'. Scan count 0, logical reads 0, physical reads 0, read-ahead reads 0, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'Production'. Scan count 1, logical reads 28651, physical reads 0, read-ahead reads 28544, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'ProductionCast'. Scan count 2, logical reads 600970, physical reads 0, read-ahead reads 405122, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'Movie'. Scan count 1, logical reads 5526, physical reads 0, read-ahead reads 0, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.

    (1 row(s) affected)
    

 SQL Server Execution Times:
   CPU time = 39531 ms,  elapsed time = 41404 ms.
SQL Server parse and compile time: 
   CPU time = 0 ms, elapsed time = 0 ms.

 SQL Server Execution Times:
   CPU time = 0 ms,  elapsed time = 0 ms.
 
 */
SELECT Person.FirstName, Person.LastName, Production.Title
FROM
  Movie 
  JOIN Production ON Movie.ProductionId = Production.Id AND MovieType = 'Normal'
  JOIN ProductionCast ON ProductionCast.ProductionId = Production.Id
  JOIN Person ON Person.Id = ProductionCast.PersonId
WHERE ProductionCast.CastRole IN ('Actor', 'Actress')
  AND ProductionCast.PersonId IN
      (SELECT PersonId FROM ProductionCast PC2 WHERE ProductionCast.ProductionId = PC2.ProductionId AND PC2.CastRole = 'Director');


/* 2.g) List the three most popular character names
 SQL Server Execution Times:
   CPU time = 0 ms,  elapsed time = 0 ms.
SQL Server parse and compile time: 
   CPU time = 0 ms, elapsed time = 6 ms.

    (3 row(s) affected)
    
Table 'Worktable'. Scan count 0, logical reads 0, physical reads 0, read-ahead reads 0, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'Workfile'. Scan count 154, logical reads 131488, physical reads 18921, read-ahead reads 138607, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'ProductionCast'. Scan count 1, logical reads 300485, physical reads 0, read-ahead reads 202561, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'ProductionCharacter'. Scan count 1, logical reads 27687, physical reads 3, read-ahead reads 27683, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.

    (1 row(s) affected)
    

 SQL Server Execution Times:
   CPU time = 56703 ms,  elapsed time = 60061 ms.
SQL Server parse and compile time: 
   CPU time = 0 ms, elapsed time = 0 ms.

 SQL Server Execution Times:
   CPU time = 0 ms,  elapsed time = 0 ms.
*/
SELECT TOP 3 Name, COUNT(*) AS Popularity
FROM ProductionCast -- Counts several appearances of the same character several times.
JOIN ProductionCharacter ON ProductionCharacter.Id = ProductionCast.CharacterId
GROUP BY ProductionCharacter.Name
ORDER BY COUNT(*) DESC

/* 3.a) Find the actors and actresses (and report the productions) who played in a production where they were 55 or more year older
     than the youngest actor/actress playing.
     SQL Server Execution Times:
   CPU time = 0 ms,  elapsed time = 0 ms.
SQL Server parse and compile time: 
   CPU time = 47 ms, elapsed time = 64 ms.
Warning: Null value is eliminated by an aggregate or other SET operation.

    (4089 row(s) affected)
    
Table 'Person'. Scan count 2, logical reads 115856, physical reads 0, read-ahead reads 115642, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'Worktable'. Scan count 0, logical reads 0, physical reads 0, read-ahead reads 0, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'Workfile'. Scan count 242, logical reads 210040, physical reads 29067, read-ahead reads 226477, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'ProductionCast'. Scan count 2, logical reads 600970, physical reads 0, read-ahead reads 405122, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'Production'. Scan count 1, logical reads 28613, physical reads 0, read-ahead reads 28544, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.

    (1 row(s) affected)
    

 SQL Server Execution Times:
   CPU time = 142000 ms,  elapsed time = 147304 ms.
SQL Server parse and compile time: 
   CPU time = 0 ms, elapsed time = 0 ms.

 SQL Server Execution Times:
   CPU time = 0 ms,  elapsed time = 0 ms.

*/
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

/* 3.b) Given an actor, compute his most productive year.
 Here is a query that computes it for each actor:

 NOTE: Our query will display all exe-quos
  SQL Server Execution Times:
   CPU time = 0 ms,  elapsed time = 0 ms.
SQL Server parse and compile time: 
   CPU time = 5 ms, elapsed time = 5 ms.

    (6094822 row(s) affected)
    
Table 'Worktable'. Scan count 0, logical reads 0, physical reads 0, read-ahead reads 0, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'Workfile'. Scan count 4, logical reads 18224, physical reads 2022, read-ahead reads 16202, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'ProductionCast'. Scan count 1, logical reads 300485, physical reads 0, read-ahead reads 202561, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'Production'. Scan count 1, logical reads 28651, physical reads 0, read-ahead reads 11301, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'Person'. Scan count 1, logical reads 66751, physical reads 0, read-ahead reads 66565, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.

    (1 row(s) affected)
    

 SQL Server Execution Times:
   CPU time = 66687 ms,  elapsed time = 95458 ms.
SQL Server parse and compile time: 
   CPU time = 0 ms, elapsed time = 0 ms.

 SQL Server Execution Times:
   CPU time = 0 ms,  elapsed time = 0 ms.
 */
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

/* 3.c) Given a year, list the company with the highest number of productions in each genre.
NOTE: NULL for Genre & ReleaseYear doesn't really make sense, so we removed it.

SQL Server Execution Times:
   CPU time = 0 ms,  elapsed time = 0 ms.
SQL Server parse and compile time: 
   CPU time = 0 ms, elapsed time = 14 ms.

    (6526 row(s) affected)
    
Table 'Workfile'. Scan count 0, logical reads 0, physical reads 0, read-ahead reads 0, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'Worktable'. Scan count 0, logical reads 0, physical reads 0, read-ahead reads 0, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'Company'. Scan count 2, logical reads 5088, physical reads 3, read-ahead reads 2540, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'ProductionCompany'. Scan count 1, logical reads 26783, physical reads 0, read-ahead reads 26783, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'Production'. Scan count 1, logical reads 28651, physical reads 0, read-ahead reads 28544, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.

    (1 row(s) affected)
    

 SQL Server Execution Times:
   CPU time = 5593 ms,  elapsed time = 5771 ms.
SQL Server parse and compile time: 
   CPU time = 0 ms, elapsed time = 0 ms.

 SQL Server Execution Times:
   CPU time = 0 ms,  elapsed time = 0 ms.


 Our query will display all exe-quos
*/
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

/* 3.d) Compute who worked with spouses/children/potential relatives on the same production. 
(You can assume that the same real surname implies a relation)

Query execution time too long >= 30 min. With TOP 10 we achieved 70000ms
*/
SELECT DISTINCT a.FirstName AS FirstPerson, b.FirstName AS SecondPerson, a.LastName
FROM Person a
JOIN Person b ON a.LastName = b.LastName AND a.Id > b.Id -- Make sure we only get one of the pair ('bob', 'john') and  ('john', 'bob')
JOIN ProductionCast castA ON castA.PersonId = a.Id
JOIN ProductionCast castB ON castB.PersonId = b.Id AND castA.ProductionId = castB.ProductionId

/* 3.e) Compute the of average number of actors per production per year
SQL Server Execution Times:
   CPU time = 0 ms,  elapsed time = 0 ms.
SQL Server parse and compile time: 
   CPU time = 7 ms, elapsed time = 7 ms.

    (136 row(s) affected)
    
Table 'Worktable'. Scan count 0, logical reads 0, physical reads 0, read-ahead reads 0, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'Workfile'. Scan count 2, logical reads 1952, physical reads 116, read-ahead reads 1836, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'Production'. Scan count 1, logical reads 28651, physical reads 3, read-ahead reads 28647, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'ProductionCast'. Scan count 1, logical reads 300485, physical reads 0, read-ahead reads 300485, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.

    (1 row(s) affected)
    

 SQL Server Execution Times:
   CPU time = 32562 ms,  elapsed time = 33128 ms.
SQL Server parse and compile time: 
   CPU time = 0 ms, elapsed time = 0 ms.

 SQL Server Execution Times:
   CPU time = 0 ms,  elapsed time = 0 ms.
*/
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


/* 3.f) Compute the average number of episodes per season.

 SQL Server Execution Times:
   CPU time = 0 ms,  elapsed time = 0 ms.
SQL Server parse and compile time: 
   CPU time = 0 ms, elapsed time = 4 ms.

    (110 row(s) affected)
    
Table 'Worktable'. Scan count 0, logical reads 0, physical reads 0, read-ahead reads 0, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'Workfile'. Scan count 0, logical reads 0, physical reads 0, read-ahead reads 0, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'SeriesEpisode'. Scan count 1, logical reads 9660, physical reads 3, read-ahead reads 9656, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.

    (1 row(s) affected)
    

 SQL Server Execution Times:
   CPU time = 516 ms,  elapsed time = 563 ms.
SQL Server parse and compile time: 
   CPU time = 0 ms, elapsed time = 0 ms.

 SQL Server Execution Times:
   CPU time = 0 ms,  elapsed time = 0 ms.
*/
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

/* 3.h) Compute the top ten tv-series (by number of seasons).

 SQL Server Execution Times:
   CPU time = 0 ms,  elapsed time = 0 ms.
SQL Server parse and compile time: 
   CPU time = 0 ms, elapsed time = 3 ms.

    (10 row(s) affected)
    
Table 'Production'. Scan count 0, logical reads 113, physical reads 0, read-ahead reads 7, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'Worktable'. Scan count 0, logical reads 0, physical reads 0, read-ahead reads 0, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'Workfile'. Scan count 0, logical reads 0, physical reads 0, read-ahead reads 0, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'SeriesEpisode'. Scan count 1, logical reads 9660, physical reads 0, read-ahead reads 0, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.

    (1 row(s) affected)
    

 SQL Server Execution Times:
   CPU time = 593 ms,  elapsed time = 617 ms.
SQL Server parse and compile time: 
   CPU time = 0 ms, elapsed time = 0 ms.

 SQL Server Execution Times:
   CPU time = 0 ms,  elapsed time = 0 ms.

*/
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

/* 3.i) Compute the top ten tv-series (by number of episodes per season).

 SQL Server Execution Times:
   CPU time = 0 ms,  elapsed time = 0 ms.
SQL Server parse and compile time: 
   CPU time = 0 ms, elapsed time = 5 ms.

    (10 row(s) affected)
    
Table 'Production'. Scan count 0, logical reads 119, physical reads 0, read-ahead reads 13, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'Worktable'. Scan count 0, logical reads 0, physical reads 0, read-ahead reads 0, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'Workfile'. Scan count 0, logical reads 0, physical reads 0, read-ahead reads 0, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'SeriesEpisode'. Scan count 1, logical reads 9660, physical reads 0, read-ahead reads 0, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.

    (1 row(s) affected)
    

 SQL Server Execution Times:
   CPU time = 578 ms,  elapsed time = 620 ms.
SQL Server parse and compile time: 
   CPU time = 0 ms, elapsed time = 0 ms.

 SQL Server Execution Times:
   CPU time = 0 ms,  elapsed time = 0 ms.

*/
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

/* 3.j) Find actors, actresses and directors who have movies (including tv movies and video movies) released after their death.
 SQL Server Execution Times:
   CPU time = 0 ms,  elapsed time = 0 ms.
SQL Server parse and compile time: 
   CPU time = 0 ms, elapsed time = 14 ms.

    (58360 row(s) affected)
    
Table 'Production'. Scan count 0, logical reads 79010215, physical reads 25936, read-ahead reads 2303, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'ProductionCast'. Scan count 1, logical reads 300485, physical reads 0, read-ahead reads 300485, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'Person'. Scan count 1, logical reads 60642, physical reads 0, read-ahead reads 39337, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.

    (1 row(s) affected)
    

 SQL Server Execution Times:
   CPU time = 105969 ms,  elapsed time = 111416 ms.
SQL Server parse and compile time: 
   CPU time = 0 ms, elapsed time = 0 ms.

 SQL Server Execution Times:
   CPU time = 0 ms,  elapsed time = 0 ms.
*/
SELECT DISTINCT Person.FirstName, Person.LastName, Production.Title
FROM ProductionCast
JOIN Person ON ProductionCast.PersonId = Person.Id
JOIN Production ON Production.Id = ProductionCast.ProductionId AND Production.ReleaseYear > DATEPART(YEAR, Person.DeathDate)
WHERE ProductionCast.CastRole IN ('Actor', 'Actress', 'Director')

/* 3.k) For each year, show three companies that released the most movies.

 SQL Server Execution Times:
   CPU time = 0 ms,  elapsed time = 0 ms.
SQL Server parse and compile time: 
   CPU time = 13 ms, elapsed time = 13 ms.

    (402 row(s) affected)
    
Table 'Worktable'. Scan count 0, logical reads 0, physical reads 0, read-ahead reads 0, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'Workfile'. Scan count 0, logical reads 0, physical reads 0, read-ahead reads 0, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'ProductionCompany'. Scan count 1, logical reads 26783, physical reads 0, read-ahead reads 26783, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'Production'. Scan count 1, logical reads 28651, physical reads 0, read-ahead reads 804, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'Movie'. Scan count 1, logical reads 5526, physical reads 3, read-ahead reads 5522, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'Company'. Scan count 1, logical reads 2197, physical reads 3, read-ahead reads 2540, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.

    (1 row(s) affected)
    

 SQL Server Execution Times:
   CPU time = 4234 ms,  elapsed time = 4309 ms.
SQL Server parse and compile time: 
   CPU time = 0 ms, elapsed time = 0 ms.

 SQL Server Execution Times:
   CPU time = 0 ms,  elapsed time = 0 ms.

*/
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

/* 3.l) List all living people who are opera singers ordered from youngest to oldest.
 SQL Server Execution Times:
   CPU time = 0 ms,  elapsed time = 0 ms.
SQL Server parse and compile time: 
   CPU time = 0 ms, elapsed time = 4 ms.

    (4790 row(s) affected)
    
Table 'Person'. Scan count 1, logical reads 66751, physical reads 0, read-ahead reads 18437, lob logical reads 10836, lob physical reads 4155, lob read-ahead reads 0.

    (1 row(s) affected)
    

 SQL Server Execution Times:
   CPU time = 7906 ms,  elapsed time = 8901 ms.
SQL Server parse and compile time: 
   CPU time = 0 ms, elapsed time = 0 ms.

 SQL Server Execution Times:
   CPU time = 0 ms,  elapsed time = 0 ms.

*/
SELECT FirstName, LastName, BirthDate
FROM Person
WHERE Trivia LIKE '%opera%' COLLATE SQL_Latin1_General_CP1_CI_AS  -- Make sure it is case insensitive
    OR Quotes LIKE '%opera%' COLLATE SQL_Latin1_General_CP1_CI_AS 
    OR ShortBio LIKE '%opera%' COLLATE SQL_Latin1_General_CP1_CI_AS 
    AND BirthDate IS NOT NULL 
ORDER BY BirthDate DESC

/* 3.m) List 10 most ambiguous credits (pairs of people and productions) ordered by the degree of ambiguity.
NOTE: we list the ids since by deffinition of ambiguity we would have to list a lot of confusing names.

 A credit is ambiguous if either a person has multiple alternative names or a production has multiple alternative titles. 
 The degree of ambiguity is a product of the number of possible names (real name + all alternatives) 
 and the number of possible titles (real + alternatives).
 SQL Server Execution Times:
   CPU time = 0 ms,  elapsed time = 0 ms.
SQL Server parse and compile time: 
   CPU time = 15 ms, elapsed time = 18 ms.

    (10 row(s) affected)
    
Table 'Workfile'. Scan count 0, logical reads 0, physical reads 0, read-ahead reads 0, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'Worktable'. Scan count 0, logical reads 0, physical reads 0, read-ahead reads 0, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'ProductionCast'. Scan count 1, logical reads 300485, physical reads 0, read-ahead reads 300485, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'Production'. Scan count 1, logical reads 28651, physical reads 0, read-ahead reads 0, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'AlternativeProductionTitle'. Scan count 1, logical reads 3649, physical reads 0, read-ahead reads 3649, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'Person'. Scan count 1, logical reads 66751, physical reads 0, read-ahead reads 0, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'AlternativePersonName'. Scan count 1, logical reads 6703, physical reads 0, read-ahead reads 6703, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.

    (1 row(s) affected)
    

 SQL Server Execution Times:
   CPU time = 19297 ms,  elapsed time = 19536 ms.
SQL Server parse and compile time: 
   CPU time = 0 ms, elapsed time = 0 ms.

 SQL Server Execution Times:
   CPU time = 0 ms,  elapsed time = 0 ms.

 */
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

/* 3.n) For each country, list the most frequent character name that appears in the productions of a production company (not a distributor) from that country.
 Will give all options if several.
 
 SQL Server Execution Times:
   CPU time = 0 ms,  elapsed time = 0 ms.
SQL Server parse and compile time: 
   CPU time = 15 ms, elapsed time = 34 ms.

    (632 row(s) affected)
    
Table 'Workfile'. Scan count 2, logical reads 11560, physical reads 1328, read-ahead reads 10232, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'Worktable'. Scan count 0, logical reads 0, physical reads 0, read-ahead reads 0, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'ProductionCast'. Scan count 1, logical reads 300485, physical reads 0, read-ahead reads 300485, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'ProductionCharacter'. Scan count 1, logical reads 27687, physical reads 3, read-ahead reads 27683, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'Production'. Scan count 1, logical reads 28651, physical reads 0, read-ahead reads 28544, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'ProductionCompany'. Scan count 1, logical reads 26783, physical reads 0, read-ahead reads 26783, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
Table 'Company'. Scan count 1, logical reads 2544, physical reads 3, read-ahead reads 2540, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.

    (1 row(s) affected)
    

 SQL Server Execution Times:
   CPU time = 234907 ms,  elapsed time = 240857 ms.
SQL Server parse and compile time: 
   CPU time = 0 ms, elapsed time = 0 ms.

 SQL Server Execution Times:
   CPU time = 0 ms,  elapsed time = 0 ms.

 */
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