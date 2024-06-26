using FluentValidation;
using Internships.Core.Interfaces.Repositories;

namespace Internships.Core.Features.Companies.Commands.CreateCompany
{
    public class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
    {
        private readonly ICompanyRepositoryAsync _companyRepository;

        public CreateCompanyCommandValidator(ICompanyRepositoryAsync companyRepository)
        {
            this._companyRepository = companyRepository;

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{Name} is required.")
                .NotNull()
                .MaximumLength(100).WithMessage("{Name} must not exceed 100 characters");
            RuleFor(p => p.Address)
                .NotEmpty().WithMessage("{Address} is required.")
                .NotNull()
                .MaximumLength(100).WithMessage("{Address} must not exceed 100 characters");
            RuleFor(p => p.Phone)
                .NotEmpty().WithMessage("{Phone} is required.")
                .NotNull()
                .MinimumLength(3).WithMessage("{Phone} must be at least 3 characters.")
                .MaximumLength(13).WithMessage("{Phone} must not exceed 13 characters.")
                .Matches(@"^\+?[0-9]{3,13}$").WithMessage("Invalid {Phone} number.");
            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("{Email} is required.")
                .NotNull()
                .MaximumLength(100).WithMessage("{Email} must not exceed 100 characters")
                .EmailAddress().WithMessage("Invalid {Email} address.");
            RuleFor(p => p.Website)
                .NotEmpty().WithMessage("{Website} is required.")
                .NotNull()
                .MaximumLength(100).WithMessage("{Website} must not exceed 100 characters");
            RuleFor(p => p.TaxNumber)
                .NotEmpty().WithMessage("{TaxNumber} is required.")
                .NotNull()
                .MaximumLength(100).WithMessage("{TaxNumber} must not exceed 100 characters");
        }
    }
}
