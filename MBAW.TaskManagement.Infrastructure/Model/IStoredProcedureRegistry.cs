namespace MBAW.TaskManagement.Infrastructure.Model
{
    public interface IStoredProcedureRegistry
    {
        public void RegisterStoredProcedure(string name);
        public List<string> GetRegisterStoredProcedures();
    }
}
