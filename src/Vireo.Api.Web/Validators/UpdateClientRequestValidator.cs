using FluentValidation;
using Vireo.Api.Core.Domain.Dtos.Requests;

namespace Vireo.Api.Web.Validators;

public class UpdateClientRequestValidator : AbstractValidator<UpdateClientRequest>
{
    public UpdateClientRequestValidator()
    {
        RuleFor(_ => _.Name)
            .NotEmpty()
            .WithMessage("O campo 'Nome' é obrigatório.")
            .MaximumLength(100)
            .WithMessage("O campo 'Nome' não pode ser maior do que 100 caracteres.");

        RuleFor(_ => _.LastName)
            .NotEmpty()
            .WithMessage("O campo 'Apelido' é obrigatório.")
            .MaximumLength(100)
            .WithMessage("O campo 'Apelido' não pode ser maior do que 100 caracteres.");

        RuleFor(_ => _.Phone)
            .NotEmpty()
            .WithMessage("O campo 'Telefone' é obrigatório.")
            .MaximumLength(13)
            .WithMessage("O campo 'Telefone' não pode ser maior do que 13 caracteres.");

        RuleFor(_ => _.Email)
            .NotEmpty()
            .WithMessage("O campo 'Email' é obrigatório.")
            .MaximumLength(50)
            .WithMessage("O campo 'Email' não pode ser maior do que 50 caracteres.")
            .EmailAddress()
            .WithMessage("O campo 'Email' não é um email válido.");
    }
}
