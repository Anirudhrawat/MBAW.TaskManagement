CREATE OR ALTER PROCEDURE usp_TaskManagement_GetHighPriorityCount
    @DueDate DATETIME,          -- Required parameter
    @Id NVARCHAR(50) = NULL     -- Optional parameter
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Query to count tasks based on the provided criteria
        SELECT COUNT(*) AS TaskCount
        FROM dbo.Tasks
        WHERE DueDate = @DueDate
          AND Priority = 0 -- HIGH
          AND Status <> 2 -- NOT FINISHED
          AND (@Id IS NULL OR Id <> @Id); -- Exclude the specified Id if provided
    END TRY
    BEGIN CATCH
        -- Handle errors
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR (@ErrorMessage, 16, 1);
    END CATCH
END;
