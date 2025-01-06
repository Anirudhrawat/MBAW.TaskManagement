CREATE OR ALTER PROCEDURE usp_TaskManagement_UpdateTask
    @Id NVARCHAR(50),
    @Name NVARCHAR(255) = NULL,          -- Optional parameter
    @Description NVARCHAR(MAX) = NULL,   -- Optional parameter
    @DueDate DATETIME = NULL,            -- Optional parameter
    @StartDate DATETIME = NULL,          -- Optional parameter
    @EndDate DATETIME = NULL,            -- Optional parameter
    @Priority INT = NULL,                -- Optional parameter
    @Status INT = NULL                   -- Optional parameter
AS
BEGIN
    SET NOCOUNT ON;

    -- Validate Priority if provided
    IF @Priority IS NOT NULL AND @Priority NOT IN (0, 1, 2)
    BEGIN
        RAISERROR ('Invalid Priority value. Accepted values: 0 (HIGH), 1 (MEDIUM), 2 (LOW)', 16, 1);
        RETURN;
    END

    -- Validate Status if provided
    IF @Status IS NOT NULL AND @Status NOT IN (0, 1, 2)
    BEGIN
        RAISERROR ('Invalid Status value. Accepted values: 0 (NEW), 1 (IN_PROGRESS), 2 (FINISHED)', 16, 1);
        RETURN;
    END

    -- Check if Task exists
    IF NOT EXISTS (SELECT 1 FROM dbo.Tasks WHERE Id = @Id)
    BEGIN
        RAISERROR ('Task with the specified Id does not exist.', 16, 1);
        RETURN;
    END

    -- Update Task
    BEGIN TRY
        UPDATE dbo.Tasks
        SET 
            Name = COALESCE(@Name, Name),
            Description = COALESCE(@Description, Description),
            DueDate = COALESCE(@DueDate, DueDate),
            StartDate = COALESCE(@StartDate, StartDate),
            EndDate = COALESCE(@EndDate, EndDate),
            Priority = COALESCE(@Priority, Priority),
            Status = COALESCE(@Status, Status)
        WHERE Id = @Id;

        PRINT 'Task successfully updated.';
    END TRY
    BEGIN CATCH
        -- Handle errors
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR (@ErrorMessage, 16, 1);
    END CATCH
END;
