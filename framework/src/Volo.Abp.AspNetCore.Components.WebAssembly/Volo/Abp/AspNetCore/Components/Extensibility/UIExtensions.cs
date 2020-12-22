using Volo.Abp.AspNetCore.Components.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Extensibility.TableColumns;

namespace Volo.Abp.AspNetCore.Components.Extensibility
{
    public class UIExtensions
    {
        public static UIExtensions Instance { get; protected set; } = new UIExtensions();

        public EntityActionsConfiguration EntityActions { get; }

        public TableColumnsConfiguration TableColumns { get; }

        private UIExtensions()
        {
            EntityActions = new EntityActionsConfiguration();
            TableColumns = new TableColumnsConfiguration();
        }
    }
}
