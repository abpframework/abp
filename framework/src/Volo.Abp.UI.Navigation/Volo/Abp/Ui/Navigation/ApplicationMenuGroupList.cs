using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.UI.Navigation;

public class ApplicationMenuGroupList: List<ApplicationMenuGroup>
{
    public ApplicationMenuGroupList()
    {

    }

    public ApplicationMenuGroupList(int capacity)
        : base(capacity)
    {

    }

    public ApplicationMenuGroupList(IEnumerable<ApplicationMenuGroup> collection)
        : base(collection)
    {

    }

    public void Normalize()
    {
        Order();
    }

    private void Order()
    {
        var orderedItems = this.OrderBy(item => item.Order).ToArray();
        Clear();
        AddRange(orderedItems);
    }
}
