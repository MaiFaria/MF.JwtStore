﻿using MF.JwtStore.Core.Contexts.AccountContext.Entities;

namespace MF.JwtStore.Core.Contexts.AccountContext.UseCases.Authenticate.Contracts;

public interface IRepository
{

    Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
}

