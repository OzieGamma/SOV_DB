
-- 2.e
SELECT Gender, MIN(Height) FROM dbo.Person GROUP BY Gender
GO

SELECT Gender, MAX(Height) FROM dbo.Person GROUP BY Gender --WHERE Gender = 'F'
GO

SELECT Gender, AVG(Height) FROM dbo.Person GROUP BY Gender --WHERE Gender = 'F'
GO

SELECT TOP(10) * FROM dbo.Person where Gender = 'F'