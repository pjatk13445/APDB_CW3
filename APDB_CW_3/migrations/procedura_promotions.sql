CREATE PROCEDURE dbo.Promote @Studies NVARCHAR(5), @Semester INT
AS
BEGIN
	DECLARE
	@PreviousEnrollmentId INT = (
		SELECT e.IdEnrollment FROM dbo.Enrollment e JOIN dbo.Studies s ON s.IdStudy = e.IdStudy
		WHERE s.Name = @Studies AND e.Semester = @Semester
	),
	@ReturnValue INT = 0

	IF(@PreviousEnrollmentId IS NULL)
	BEGIN
		RETURN @ReturnValue;
	END

	DECLARE
	@NewEnrollmentId INT = (
		SELECT e.IdEnrollment FROM dbo.Enrollment e JOIN dbo.Studies s ON s.IdStudy = e.IdStudy
		WHERE s.Name = @Studies AND e.Semester = @Semester + 1
	),
	@IdStudy INT = (
		SELECT IdStudy FROM dbo.Studies WHERE Name = @Studies
	)

	IF(@NewEnrollmentId IS NULL)
	BEGIN
		SELECT @NewEnrollmentId = MAX(IdEnrollment) + 1 FROM dbo.Enrollment;
		INSERT INTO dbo.Enrollment (IdEnrollment, Semester, IdStudy, StartDate)
		VALUES (@NewEnrollmentId, @Semester + 1, @IdStudy, CURRENT_TIMESTAMP);
	END

	UPDATE dbo.Student SET IdEnrollment = @NewEnrollmentId WHERE IdEnrollment = @PreviousEnrollmentId
	SET @ReturnValue = @NewEnrollmentId
	RETURN @ReturnValue;
END;