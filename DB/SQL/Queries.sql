-- a) Compute the number of movies per year. Make sure to include tv and video movies.
SELECT ReleaseYear, COUNT(*) 
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
  FROM (SELECT Id FROM Person) AS ShortPerson
  JOIN ProductionCast ON ProductionCast.PersonId = ShortPerson.Id
  JOIN (SELECT Id, ReleaseYear FROM Production) AS ShortProd ON ShortProd.Id = ProductionCast.ProductionId
  GROUP BY ShortPerson.Id
  ) AS temp;

-- d) Compute the min, max and average number of actors in a production.
SELECT MIN(number.value) AS MinNumber, MAX(number.value) AS MaxNumber, AVG(number.value) AS AvgNumber
FROM 
  (
  SELECT COUNT(*) AS value
  FROM ProductionCast
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
SELECT TOP 3 Name, COUNT(*) AS NameCount
FROM ProductionCharacter
GROUP BY Name
ORDER BY COUNT(*) DESC;

-- 3.a) Find the actors and actresses (and report the productions) who played in a production where they were 55 or more year older than the youngest actor/actress playing.
-- SELECT P.FirstName, P.LastName, Production.Title
-- FROM (
-- SELECT Person.FirstName, Person.LastName, Person.BirthDate, MIN(Person.BirthDate) AS MinBirthDay, ProductionCast.ProductionId
-- FROM Person
-- JOIN ProductionCast ON ProductionCast.PersonId = Person.Id
-- WHERE ProductionCast.CastRole IN ('Actor', 'Actress')
-- ) AS P 
-- JOIN Production ON Production.id = P.ProductionId
-- WHERE DATEDIFF(YEAR, P.BirthDate, P.MinBirthDay) >= 55



-- 3.b) Given an actor, compute his most productive year.
SELECT TOP 1 SubQuery.ReleaseYear, NumReleases
FROM (  SELECT Production.ReleaseYear, COUNT(*) AS NumReleases
        FROM ProductionCast
        JOIN Production ON ProductionCast.ProductionId = Production.Id
        WHERE ProductionCast.PersonId = 4 -- Given
        GROUP BY Production.ReleaseYear ) AS SubQuery
ORDER BY SubQuery.NumReleases DESC

-- 3.c) Given a year, list the company with the highest number of productions in each genre.
--SELECT *
--FROM (
--    SELECT Company.Id, Production.Genre, COUNT(*) AS ProductionCount
--    FROM Production
--    JOIN ProductionCompany ON Production.Id = ProductionCompany.ProductionId
--    JOIN Company ON Company.Id = ProductionCompany.CompanyId
--    WHERE ProductionCompany.Kind = 'ProductionCompany'
--    AND Production.Genre IS NOT NULL
--    AND Production.ReleaseYear = 2015 -- Given
--    GROUP BY Company.Id, Production.Genre) AS SubQuery
--GROUP BY SubQuery.Genre


-- 3.d) Compute who worked with spouses/children/potential relatives on the same production. (You can assume that the same real surname implies a relation)
SELECT DISTINCT a.FirstName, b.FirstName, a.LastName
FROM Person a
JOIN Person b ON a.LastName = b.LastName AND a.Id <> b.Id -- TODO: Make sure we only get one of the pair ('bob', 'john') and  ('john', 'bob')
JOIN ProductionCast castA ON castA.PersonId = a.Id
JOIN ProductionCast castB ON castB.PersonId = b.Id AND castA.ProductionId = castB.ProductionId

-- 3.e) Compute the of average number of actors per production per year
-- 3.f) Compute the average number of episodes per season.
SELECT SubQuery.SeasonNumber, AVG(SubQuery.NumEpisodes)
FROM (  SELECT SeriesEpisode.SeriesId, SeriesEpisode.SeasonNumber, COUNT(*) AS NumEpisodes
        FROM SeriesEpisode
        WHERE SeriesEpisode.SeasonNumber IS NOT NULL
        GROUP BY SeriesEpisode.SeriesId, SeriesEpisode.SeasonNumber) AS SubQuery
GROUP BY SubQuery.SeasonNumber
ORDER BY SubQuery.SeasonNumber ASC

-- 3.g) Compute the average number of seasons per series.
SELECT AVG(CAST(SubSubQuery.NumSeasons AS FLOAT))
FROM (  SELECT SubQuery.SeriesId, COUNT(*) AS NumSeasons
        FROM (  SELECT DISTINCT SeriesEpisode.SeriesId, SeriesEpisode.SeasonNumber
                FROM SeriesEpisode
                GROUP BY SeriesEpisode.SeriesId, SeriesEpisode.SeasonNumber) AS SubQuery
        GROUP BY SubQuery.SeriesId) AS SubSubQuery

-- 3.h) Compute the top ten tv-series (by number of seasons).
-- 3.i) Compute the top ten tv-series (by number of episodes per season).
-- 3.j) Find actors, actresses and directors who have movies (including tv movies and video movies) released after their death.
-- 3.k) For each year, show three companies that released the most movies.
-- 3.l) List all living people who are opera singers ordered from youngest to oldest.
-- 3.m) List 10 most ambiguous credits (pairs of people and productions) ordered by the degree of ambiguity.
-- A credit is ambiguous if either a person has multiple alternative names or a production has multiple alternative titles. 
-- The degree of ambiguity is a product of the number of possible names (real name + all alternatives) and the number of possible titles (real + alternatives).
-- 3.n) For each country, list the most frequent character name that appears in the productions of a production company (not a distributor) from that country.