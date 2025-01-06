namespace MBAW.TaskManagement.Infrastructure.Model
{
    public class StoredProcedureRegistry: IStoredProcedureRegistry
    {
        private readonly List<string> _storedProcedures;
        
        public StoredProcedureRegistry()
        {
            _storedProcedures = new List<string>();
        }

        public List<string> GetRegisterStoredProcedures()
        {
            return _storedProcedures;
        }

        public void RegisterStoredProcedure(string storedProcedureName)
        {
            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentNullException(nameof(storedProcedureName));
            }
            if (_storedProcedures.Contains(storedProcedureName))
            {
                throw new Exception(Constants.ValidationMessages.STORED_PROCEDURE_ALREADY_EXIST);
            }
            _storedProcedures.Add(storedProcedureName);
        }
    }
}
