CREATE OR ALTER PROCEDURE usp_TaskManagement_InsertTask
    @Id NVARCHAR(50),
    @Name NVARCHAR(255),
    @Description NVARCHAR(MAX),
    @DueDate DATETIME,
    @StartDate DATETIME,
    @EndDate DATETIME = NULL, -- Optional parameter
    @Priority INT, -- Expected values: 0 (HIGH), 1 (MEDIUM), 2 (LOW)
    @Status INT -- Expected values: 0 (NEW), 1 (IN_PROGRESS), 2 (FINISHED)
AS
BEGIN
    SET NOCOUNT ON;

    -- Validate Priority
    IF @Priority NOT IN (0, 1, 2)
    BEGIN
        RAISERROR ('Invalid Priority value. Accepted values: 0 (HIGH), 1 (MEDIUM), 2 (LOW)', 16, 1);
        RETURN;
    END

    -- Validate Status
    IF @Status NOT IN (0, 1, 2)
    BEGIN
        RAISERROR ('Invalid Status value. Accepted values: 0 (NEW), 1 (IN_PROGRESS), 2 (FINISHED)', 16, 1);
        RETURN;
    END

    -- Insert Task
    BEGIN TRY
        INSERT INTO dbo.Tasks (Id, Name, Description, DueDate, StartDate, EndDate, Priority, Status)
        VALUES (@Id, @Name, @Description, @DueDate, @StartDate, @EndDate, @Priority, @Status);

        PRINT 'Task successfully inserted.';
    END TRY
    BEGIN CATCH
        -- Handle errors
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR (@ErrorMessage, 16, 1);
    END CATCH
END;