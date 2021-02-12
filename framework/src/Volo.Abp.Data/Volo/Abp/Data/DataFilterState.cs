namespace Volo.Abp.Data
{
    public class DataFilterState
    {
        public bool IsActive { get; set; }
        public bool IsEnabled { get; set; }

        public DataFilterState(bool isEnabled, bool isActive = false)
        {
            IsEnabled = isEnabled;
            IsActive = isActive;
        }

        public DataFilterState Clone()
        {
            return new DataFilterState(IsEnabled, IsActive);
        }
    }
}