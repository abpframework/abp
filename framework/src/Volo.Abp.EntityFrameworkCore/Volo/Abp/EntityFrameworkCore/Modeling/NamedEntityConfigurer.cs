using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Volo.Abp.EntityFrameworkCore.Modeling;

public class NamedEntityConfigurer
{
    /// <summary>
    /// Name of the configurer.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Action to configure the given <see cref="EntityTypeBuilder"/>.
    /// </summary>
    public Action<EntityTypeBuilder> ConfigureAction { get; }

    public NamedEntityConfigurer(string name, Action<EntityTypeBuilder> configureAction)
    {
        Name = Check.NotNullOrEmpty(name, nameof(name));
        ConfigureAction = Check.NotNull(configureAction, nameof(configureAction));
    }
}