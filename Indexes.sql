/* Query 3C */
CREATE NONCLUSTERED INDEX Query3CIndex
ON Production (ReleaseYear, Genre)
INCLUDE (Id)

/* Query 3M */
CREATE NONCLUSTERED INDEX Query3MIndex
ON ProductionCast (ProductionId)
INCLUDE (PersonId)

/* Query 3N */
CREATE NONCLUSTERED INDEX Query3NIndex
ON ProductionCast (ProductionId)
INCLUDE (CharacterId)