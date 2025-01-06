namespace MBAW.TaskManagement.Shared
{
    public static class Constants
    {
        public static class StoredProcedures
        {
            public const string CREATE_DATABASE = "create_tasks_table";
            public const string INSERT_TASK = "usp_TaskManagement_InsertTask";
            public const string UPDATE_TASK = "usp_TaskManagement_UpdateTask";
            public const string GET_HIGH_PRIORITY_TASK = "usp_TaskManagement_GetHighPriorityCount";
            public const string GET_BY_ID = "usp_TaskManagement_GetTaskById";
        }
        public static class ValidationMessages
        {
            public const string INVALID_DUE_DATE = "Due date is can not be a holiday.";
            public const string PAST_DUE_DATE = "Due date is can not be in past.";
            public const string DUE_TASKS_THRESHOLD = "Tasks of high priority with the same due date more than 100.";
            public const string EXCEPTION_EXECUTING_QUERY = "An exception occured executing the query.";
            public const string NO_TASK_FOUND_WITH_ID = "No task found with this id.";
        }
    }
}
