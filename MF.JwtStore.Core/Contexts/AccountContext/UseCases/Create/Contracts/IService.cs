using MF.JwtStore.Core.Contexts.AccountContext.Entities;

namespace MF.JwtStore.Core.Contexts.AccountContext.UseCases.Create.Contracts;

public interface IService
{
    Task SendVerificationEmailAsync(User user,
                                    CancellationToken cancellationToken);
}

