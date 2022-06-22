using FluentValidation;
using VaporApp.Application.Requests;


namespace VaporApp.Application.Validators
{
    public class UpdateUsersValidator : AbstractValidator<UpdateUsersRequest>
    {
        public UpdateUsersValidator()
        {
            RuleFor(x => x.UserFirstName)
                .NotEmpty()
                .MinimumLength(1)
                .WithMessage("Full name must exist for update");

            RuleFor(x => x.UserLastName)
                .NotEmpty()
                .MinimumLength(1)
                .WithMessage("Full name must exist for update");
        }
    }
}
