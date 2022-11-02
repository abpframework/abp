using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace Volo.CmsKit.Domain.Volo.CmsKit.Pages;

[Serializable]
public class MultipleHomePageException : BusinessException
{
	public MultipleHomePageException()
	{
		Code = CmsKitErrorCodes.Pages.MultipleHomePage;
	}
	
	public MultipleHomePageException(SerializationInfo serializationInfo, StreamingContext context)
		: base(serializationInfo, context)
	{
	}
}
