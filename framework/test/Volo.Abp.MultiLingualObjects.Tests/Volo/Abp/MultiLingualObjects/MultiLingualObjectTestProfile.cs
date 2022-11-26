namespace Volo.Abp.MultiLingualObjects;

using System;
using System.Collections.Generic;
using global::AutoMapper;
using Volo.Abp.MultiLingualObjects.TestObjects;

public class MultiLingualObjectTestProfile : Profile
{
    public MultiLingualObjectTestProfile()
    {
        CreateMap<MultiLingualBook, MultiLingualBookDto>()
            .ForMember(x => x.Name,
            x => x.MapFrom((src, target, member, context) =>
            {
                if (context.Items.TryGetValue(nameof(MultiLingualBookTranslation), out var translationsRaw) && translationsRaw is IReadOnlyDictionary<Guid, MultiLingualBookTranslation> translations)
                {
                    return translations.GetValueOrDefault(src.Id)?.Name;
                }
                return null;
            }));
    }
}
