using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.UI.Navigation
{
    public class ApplicationMenu : IHasMenuItems
    {
        /// <summary>
        /// Unique name of the menu in the application.
        /// </summary>
        [NotNull]
        public string Name { get; }

        /// <summary>
        /// Display name of the menu.
        /// Default value is the <see cref="Name"/>.
        /// </summary>
        [NotNull]
        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                Check.NotNullOrWhiteSpace(value, nameof(value));
                _displayName = value;
            }
        }
        private string _displayName;

        /// <inheritdoc cref="IHasMenuItems.Items"/>
        [NotNull]
        public IList<ApplicationMenuItem> Items { get; } //TODO: Create a specialized collection (that can contain AddAfter for example)

        /// <summary>
        /// Can be used to store a custom object related to this menu.
        /// </summary>
        [CanBeNull]
        public object CustomData { get; set; }

        public ApplicationMenu(
            [NotNull] string name,
            string displayName = null)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            Name = name;
            DisplayName = displayName ?? Name;

            Items = new List<ApplicationMenuItem>();
        }

        /// <summary>
        /// Adds a <see cref="ApplicationMenuItem"/> to <see cref="Items"/>.
        /// </summary>
        /// <param name="menuItem"><see cref="ApplicationMenuItem"/> to be added</param>
        /// <returns>This <see cref="ApplicationMenu"/> object</returns>
        public ApplicationMenu AddItem([NotNull] ApplicationMenuItem menuItem)
        {
            Items.Add(menuItem);
            return this;
        }
    }
}