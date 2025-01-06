CREATE OR ALTER PROCEDURE usp_TaskManagement_GetTaskById
    @Id NVARCHAR(50) -- Required parameter for Task ID
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Check if the Task exists
        IF NOT EXISTS (SELECT 1 FROM dbo.Tasks WHERE Id = @Id)
        BEGIN
            RAISERROR ('Task with the specified Id does not exist.', 16, 1);
            RETURN;
        END

        -- Retrieve the Task details
        SELECT 
            Id,
            Name,
            Description,
            DueDate,
            StartDate,
            EndDate,
            Priority,
            Status
        FROM dbo.Tasks
        WHERE Id = @Id;
    END TRY
    BEGIN CATCH
        -- Handle errors
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR (@ErrorMessage, 16, 1);
    END CATCH
END;
