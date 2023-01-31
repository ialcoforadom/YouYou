using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using YouYou.Business.ErrorNotifications;
using YouYou.Business.Interfaces;

namespace YouYou.Api.Controllers
{
    [ApiController]
    public class MainController<T> : ControllerBase where T : ControllerBase
    {
        private readonly IErrorNotifier _errorNotifier;
        public readonly IUser AppUser;
        private readonly IStringLocalizer<T> _localizer;

        protected Guid UserId { get; set; }
        protected bool IsAuthenticated { get; set; }

        public MainController(IErrorNotifier errorNotifier, IUser appUser, IStringLocalizer<T> localizer)
        {
            _errorNotifier = errorNotifier;
            AppUser = appUser;

            if (appUser.IsAuthenticated())
            {
                UserId = appUser.GetUserId();
                IsAuthenticated = true;
            }

            _localizer = localizer;
        }

        protected bool validOperation()
        {
            return !_errorNotifier.HasErrorNotification();
        }

        protected ActionResult CustomResponse(object result = null)
        {
            if (validOperation())
            {
                return Ok(new
                {
                    success = true,
                    data = result
                });
            }

            return BadRequest(new
            {
                success = false,
                errors = GetTranslateMessages()
            });
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid) NotifyErrorModelInvalid(modelState);
            return CustomResponse();
        }

        protected void NotifyErrorModelInvalid(ModelStateDictionary modelState)
        {
            var errors = modelState.Values.SelectMany(s => s.Errors);

            foreach (var erro in errors)
            {
                string message = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                NotifyError(message);
            }
        }

        protected void NotifyError(string message)
        {
            _errorNotifier.Handle(new ErrorNotification(message));
        }

        protected IEnumerable<string> GetTranslateMessages()
        {
            List<string> messages = new List<string>();

            foreach (string message in _errorNotifier.GetErrorNotifications().Select(s => s.Message))
            {
                messages.Add(_localizer[message].Value);
            }

            return messages;
        }
    }
}
