using MBAW.TaskManagement.Infrastructure.Model;
using MBAW.TaskManagement.Shared;

namespace MBAW.TaskManagement.Domain
{
    public class Registry: StoredProcedureRegistry
    {
        public Registry()
        {
            RegisterStoredProcedure(Constants.StoredProcedures.CREATE_DATABASE);
            RegisterStoredProcedure(Constants.StoredProcedures.INSERT_TASK);
            RegisterStoredProcedure(Constants.StoredProcedures.UPDATE_TASK);
            RegisterStoredProcedure(Constants.StoredProcedures.GET_HIGH_PRIORITY_TASK);
            RegisterStoredProcedure(Constants.StoredProcedures.GET_BY_ID);
        }
    }
}
