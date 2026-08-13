using ConnectSphere.Application.Common.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Common.Validators
{
    public class PaginationDtoValidator : AbstractValidator<PaginationParameters>
    {
        public PaginationDtoValidator()
        {
            RuleFor(x => x.PageNumber)
                .InclusiveBetween(1, int.MaxValue);

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 50);
        }
    }
}
