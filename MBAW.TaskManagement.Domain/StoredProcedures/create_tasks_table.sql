IF NOT EXISTS (
    SELECT 1
    FROM INFORMATION_SCHEMA.TABLES
    WHERE TABLE_NAME = 'Tasks' AND TABLE_SCHEMA = 'dbo'
)
BEGIN
    CREATE TABLE dbo.Tasks (
        Id NVARCHAR(50) NOT NULL PRIMARY KEY,
        Name NVARCHAR(255) NOT NULL,
        Description NVARCHAR(MAX) NULL,
        DueDate DATETIME NOT NULL,
        StartDate DATETIME NOT NULL,
        EndDate DATETIME NULL,
        Priority INT NOT NULL CHECK (Priority IN (0, 1, 2)), -- 0 = HIGH, 1 = MEDIUM, 2 = LOW
        Status INT NOT NULL CHECK (Status IN (0, 1, 2)) -- 0 = NEW, 1 = IN_PROGRESS, 2 = FINISHED
    );

    -- Optional: Add Indexes
    CREATE INDEX IX_Tasks_Status ON dbo.Tasks(Status);
    CREATE INDEX IX_Tasks_Priority ON dbo.Tasks(Priority);
END;
