using Firmeza.Domain.Entities;

namespace Firmeza.Application.Interfaces;

public interface IPdfGenerator
{
    string GeneratePdf(Sale sale);
}