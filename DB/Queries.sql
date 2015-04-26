-- a) Compute the number of movies per year. Make sure to include tv and video movies.
SELECT ReleaseYear, COUNT(*) 
FROM Movie JOIN Production ON Movie.ProductionId = Production.Id 
GROUP BY ReleaseYear;

-- b) Compute the ten countries with most production companies.
SELECT TOP 10 CountryCode 
FROM Company 
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