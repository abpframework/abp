using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace Volo.Abp.IdentityServer
{
    public class AllowedSigningAlgorithmsConverter :
        IValueConverter<ICollection<string>, string>,
        IValueConverter<string, ICollection<string>>
    {
        public static AllowedSigningAlgorithmsConverter Converter = new AllowedSigningAlgorithmsConverter();

        public string Convert(ICollection<string> sourceMember, ResolutionContext context)
        {
            if (sourceMember == null || !sourceMember.Any())
            {
                return null;
            }
            return sourceMember.Aggregate((x, y) => $"{x},{y}");
        }

        public ICollection<string> Convert(string sourceMember, ResolutionContext context)
        {
            var list = new HashSet<string>();
            if (!String.IsNullOrWhiteSpace(sourceMember))
            {
                sourceMember = sourceMember.Trim();
                foreach (var item in sourceMember.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct())
                {
                    list.Add(item);
                }
            }
            return list;
        }
    }
}
