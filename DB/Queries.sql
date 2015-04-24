-- a) Compute the number of movies per year. Make sure to include tv and video movies.
SELECT ReleaseYear, COUNT(*) FROM Movie JOIN Production ON Movie.ProductionId = Production.Id GROUP BY ReleaseYear

-- b) Compute the ten countries with most production companies.
-- c) Compute the min, max and average career duration.
--    (A career length is implied by the first and last production of a person)
-- d) Compute the min, max and average number of actors in a production.
-- e) Compute the min, max and average height of female persons.
SELECT MIN(Height), MAX(Height), AVG(Height) FROM Person WHERE Gender = 'F'

-- f) List all pairs of persons and movies where the person has both directed the movie and acted in the
-- movie. Do not include tv and video movies.
-- g) List the three most popular character names