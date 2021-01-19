using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using Volo.Abp.Data;
using Volo.Abp.Localization;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.BlazoriseUI.Components.ObjectExtending
{
    public partial class SelectExtensionProperty<TEntity, TResourceType> : ComponentBase
        where TEntity : IHasExtraProperties
    {
        [Inject]
        public IStringLocalizerFactory StringLocalizerFactory { get; set; }

        [Parameter]
        public TEntity Entity { get; set; }

        [Parameter]
        public ObjectExtensionPropertyInfo PropertyInfo { get; set; }

        public int SelectedValue
        {
            get
            {
                return Entity.GetProperty<int>(PropertyInfo.Name);
            }
            set
            {
                Entity.SetProperty(PropertyInfo.Name, value);
            }
        }

        protected virtual IEnumerable<SelectItem<int>> GetSelectItemsFromEnum()
        {
            var isNullableType = Nullable.GetUnderlyingType(PropertyInfo.Type) != null;
            var enumType = PropertyInfo.Type;

            if (isNullableType)
            {
                enumType = Nullable.GetUnderlyingType(PropertyInfo.Type);
                yield return new SelectItem<int>();
            }

            foreach (var enumValue in enumType.GetEnumValues())
            {
                var memberName = enumType.GetEnumName(enumValue);
                var localizedMemberName = AbpInternalLocalizationHelper.LocalizeWithFallback(
                    new[]
                    {
                       // containerLocalizer,
                        StringLocalizerFactory.CreateDefaultOrNull()
                    },
                    new[]
                    {
                        $"Enum:{enumType.Name}.{memberName}",
                        $"{enumType.Name}.{memberName}",
                        memberName
                    },
                    memberName
                );

                yield return new SelectItem<int>
                {
                    Value = (int)enumValue,
                    Text = localizedMemberName
                };
            }
        }


        protected virtual void SelectedValueChanged(int value)
        {
            SelectedValue = value;
        }
    }

    public class SelectItem<TValue>
    {
        public string Text { get; set; }
        public TValue Value { get; set; }
    }
}