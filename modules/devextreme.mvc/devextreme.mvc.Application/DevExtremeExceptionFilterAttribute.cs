namespace devextreme.mvc.Application
{
    public class DevExtremeExceptionFilterAttribute : IBusinessException 
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is DevExtremeException)
            {
                context.ExceptionHandled = true;
                context.HttpContext.Response.StatusCode = 400;

                context.Result = new JsonResult(context.Exception.MessagesString());

                base.OnException(context);
            }
            else
            {
                base.OnException(context);
            }
        }
    }
}
