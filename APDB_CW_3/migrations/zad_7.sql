ALTER TABLE dbo.Student ADD Password nvarchar(100) DEFAULT NULL;
ALTER TABLE dbo.Student ADD Salt nvarchar(100) DEFAULT NULL;
ALTER TABLE dbo.Student ADD Role nvarchar(100) DEFAULT 'Student';
ALTER TABLE dbo.Student ADD RefreshToken nvarchar(255) DEFAULT NULL;

INSERT INTO dbo.Student (IndexNumber, FirstName, LastName, BirthDate, IdEnrollment, Salt, Role, RefreshToken, Password)
VALUES ('s1', 'Mafia', 'Boss LVL100', '1945-04-29', 1, null, 'employee', null, null);

INSERT INTO dbo.Student (IndexNumber, FirstName, LastName, BirthDate, IdEnrollment, Salt, Role, RefreshToken, Password)
VALUES ('s2', 'Mafia', 'Queen LVL88', '1945-04-29', 1, null, 'employee', null, null);

INSERT INTO dbo.Student (IndexNumber, FirstName, LastName, BirthDate, IdEnrollment, Salt, Role, RefreshToken, Password)
VALUES ('s13445', 'Zwykly', 'Student', '2000-01-01', 1, null, 'student', null, null);
