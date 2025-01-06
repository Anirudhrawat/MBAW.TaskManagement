namespace MBAW.TaskManagement.Infrastructure
{
    public static class Constants
    {
        public static class Misc
        {
            public const string CONNECTION_STRING_KEY = "DefaultConnection";
            public const string STORED_PROCEDURE_DIRECTORY = "StoredProcedures";
        }
        public static class ValidationMessages
        {
            public const string EMPTY_CONNECTION_STRING = "DefaultConnection string is not configured in the appsettings.";
            public const string STORED_PROCEDURE_ALREADY_EXIST = "Stored procedure already registered in the registry.";
            public const string INVALID_REGISTRY_LOCATION = "Could not determine registry location.";
            public const string NO_REGISTRY_DIRECTORY = "StoredProcedures folder not found in {0}.";
            public const string STORED_PROCEDURE_NOT_FOUND = "File for stored procedure '{0}' not found: {1}.";
            public const string EMPTY_STORED_PROCEDURE_NAME = "Stored procedure name cannot be null or empty.";
        }
    }
}
