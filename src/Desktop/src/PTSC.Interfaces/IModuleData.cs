namespace PTSC.Interfaces
{
    public interface IModuleData : IDictionary<string, IModuleDataPoint>
    {
        public IModuleData Clone();
    }
}
