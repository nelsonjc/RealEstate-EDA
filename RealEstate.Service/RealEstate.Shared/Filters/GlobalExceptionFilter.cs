﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using RealEstate.Shared.CustomEntities;
using RealEstate.Shared.Exceptions;
using System.Globalization;

namespace RealEstate.Shared.Filters
{
    public class GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger) : IExceptionFilter
    {
        private const string DEFAULT_ERROR_MESSAGE = "Ha ocurrido un error, por favor intentelo de nuevo o contacte con el administrador.";
        private readonly ILogger<GlobalExceptionFilter> _logger = logger;

        public void OnException(ExceptionContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            if (context.Exception is BusinessException exception)
            {
                var response = new Response
                {
                    Status = (int)exception.Status,
                    Message = ReasonPhrases.GetReasonPhrase((int)exception.Status),
                    Description = exception.Message
                };

                context.Result = new ObjectResult(response);
                context.HttpContext.Response.StatusCode = (int)exception.Status;
                context.ExceptionHandled = true;

                RegisterLogInformation(exception);
            }
            else
            {
                var response = new Response
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Message = DEFAULT_ERROR_MESSAGE,
                    Description = DEFAULT_ERROR_MESSAGE
                };

                context.Result = new ObjectResult(response);
                context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.ExceptionHandled = true;

                RegisterLogError(context.Exception);
            }
        }

        private void RegisterLogInformation(Exception exception)
        {
            _logger.LogInformation("Ha ocurrido la siguiente excepción de negocio: {message}", exception.Message);
        }

        private void RegisterLogError(Exception exception)
        {
            _logger.LogError(
                "Ha ocurrido la siguiente excepción: {message}, con el detalle: {detail}, en la hora: {time}",
                exception.Message,
                exception.InnerException?.Message ?? string.Empty,
                DateTime.Now.ToString(CultureInfo.InvariantCulture)
            );
        }
    }
}
