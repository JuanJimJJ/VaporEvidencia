using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using VaporApp.Application.Requests.Orders;

namespace VaporApp.Application.Validators
{
    public class UpdateOrdersValidator : AbstractValidator<UpdateOrdersRequest>
    {
        public UpdateOrdersValidator()
        {
            RuleFor(x => x.OrderShippingAddress)
                .NotEmpty()
                .MinimumLength(15)
                .WithMessage("Shipping order is required for update and it must be more than 15 characters long");
        }
    }
}
