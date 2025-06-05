using CashFlow.Communication.Responses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CashFlow.Api.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if(context.Exception is CashFlowException cashFlowException)
            HandleProjectException(context, cashFlowException);
        else
            ThrowUnknowError(context);
    }

    private static void HandleProjectException(ExceptionContext context, CashFlowException cashFlowException)
    {
        context.HttpContext.Response.StatusCode = cashFlowException!.StatusCode;
        context.Result = new ObjectResult(new ResponseErrorJson(cashFlowException.GetErrors()));
    }

    private static void ThrowUnknowError(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode =  StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(new ResponseErrorJson(ResourceErrorMessages.UNKNOWN_ERROR));
    }
}
