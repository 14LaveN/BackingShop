using BackingShop.Application.Core.Extensions;
using BackingShop.Domain.Common.Core.Errors;
using FluentValidation;

namespace BackingShop.Micro.Product.Mediatr.Commands.CreateProduct;

/// <summary>
/// Represents the <see cref="IValidator"/> for <see cref="CreateProductCommand"/> class.
/// </summary>
internal sealed class CreateProductCommandValidator
    : AbstractValidator<CreateProductCommand>
{
    /// <summary>
    /// Validate the <see cref="CreateProductCommand"/>
    /// </summary>
    public CreateProductCommandValidator()
    {
        RuleFor(p =>
                p.Title).NotEqual(string.Empty)
            .WithError(DomainErrors.StringErrors.IsNull)
            .MaximumLength(256)
            .WithMessage("Your title too big");
        
        RuleFor(p =>
                p.Description).NotEqual(string.Empty)
            .WithError(DomainErrors.StringErrors.IsNull)
            .MaximumLength(1024)
            .WithMessage("Your description too big");
        
        RuleFor(p =>
                p.Tag).NotEqual(string.Empty)
            .WithError(DomainErrors.StringErrors.IsNull)
            .MaximumLength(256)
            .WithMessage("Your tag too big");
        
        RuleFor(p =>
                p.CompanyName).NotEqual(string.Empty)
            .WithError(DomainErrors.StringErrors.IsNull)
            .MaximumLength(128)
            .WithMessage("Your company name too big");
    }
}