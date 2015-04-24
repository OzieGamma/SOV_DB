-- a) Compute the number of movies per year. Make sure to include tv and video movies.
SELECT ReleaseYear, COUNT(*) 
FROM Movie 
JOIN Production ON Movie.ProductionId = Production.Id 
GROUP BY ReleaseYear;

-- b) Compute the ten countries with most production companies.
SELECT CountryCode 
FROM Company 
GROUP BY CountryCode 
OFFSET 0 ROWS 
FETCH NEXT 10 ROWS ONLY;

-- c) Compute the min, max and average career duration.
--    (A career length is implied by the first and last production of a person)
-- TODO

-- d) Compute the min, max and average number of actors in a production.
SELECT MIN(COUNT(*)), MAX(COUNT(*)), AVG(COUNT(*)) 
FROM ProductionCast 
GROUP BY ProductionId;

-- e) Compute the min, max and average height of female persons.
SELECT MIN(Height), MAX(Height), AVG(Height) 
FROM Person 
WHERE Gender = 'F';

-- f) List all pairs of persons and movies where the person has both directed the movie and acted in the
-- movie. Do not include tv and video movies.
SELECT *
FROM
  (Movie JOIN Production ON Movie.ProductionId = Production.Id AND MovieType = 'Normal')
  JOIN
  ProductionCast PC
  ON PC.ProductionId = Production.Id
WHERE PC.CastRole IN ('Actor', 'Actress')
  AND PC.PersonId IN
      (SELECT PersonId FROM ProductionCast PC2 WHERE PC.ProductionId = PC2.ProductionId AND PC2.CastRole = 'Director');


-- g) List the three most popular character names
SELECT Name
FROM ProductionCharacter
GROUP BY Name
ORDER BY Name DESC
OFFSET 0 ROWS
FETCH NEXT 3 ROWS ONLY;