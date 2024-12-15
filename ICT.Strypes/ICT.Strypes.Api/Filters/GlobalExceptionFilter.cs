using ICT.Strypes.Business.Exceptions;
using ICT.Strypes.Business.Models;
using ICT.Strypes.Business.Resources;
using ICT.Strypes.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;

namespace ICT.Strypes.Api.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var statusCode = context.Exception switch
            {
                BadRequestException => StatusCodes.Status400BadRequest,

                NotFoundException => StatusCodes.Status404NotFound,

                ConflictException => StatusCodes.Status409Conflict,

                DatabaseException => StatusCodes.Status500InternalServerError,

                _ => StatusCodes.Status500InternalServerError
            };

            var errorTitle = context.Exception switch
            {
                BadRequestException => ErrorTitles.BadRequestErrorTitle,

                NotFoundException => ErrorTitles.ResourceNotFoundErrorTitle,

                ConflictException => ErrorTitles.ConflictErrorTitle,

                DatabaseException => ErrorTitles.DatabaseExceptionErrorTitle,

                _ => ErrorTitles.InternalExceptionErrorTitle
            };


            var errorModel = new ErrorModel
            {
                Details = context.Exception.Message,
                StatusCode = statusCode,
                Title = errorTitle
            };

            context.Result = new ObjectResult(errorModel)
            {
                StatusCode = statusCode
            };
        }
    }
}
