using MediatR;
using MF.JwtStore.Core.Contexts.AccountContext.Entities;
using MF.JwtStore.Core.Contexts.AccountContext.UseCases.Authenticate.Contracts;

namespace MF.JwtStore.Core.Contexts.AccountContext.UseCases.Authenticate;

public class Handler : IRequestHandler<Request, Response>
{
    private readonly IRepository _repository;

    public Handler(IRepository repository) => _repository = repository;

    public async Task<Response> Handle(Request request,
                                       CancellationToken cancellationToken)
    {
        #region 01. Validates the request

        try
        {
            var res = Specification.Ensure(request);
            if (!res.IsValid)
                return new Response("Invalid request", 400, res.Notifications);
        }
        catch
        {
            return new Response("Unable to validate your request", 500);
        }

        #endregion

        #region 02. Retrieve the profile

        User? user;
        try
        {
            user = await _repository.GetUserByEmailAsync(request.Email, cancellationToken);
            if (user is null)
                return new Response("Profile not found", 404);
        }
        catch
        {
            return new Response("Unable to retrieve your profile", 500);
        }

        #endregion

        #region 03. Check if the password is valid

        if (!user.Password.Challenge(request.Password))
            return new Response("Username or password is invalid", 400);

        #endregion

        #region 04. Check if the account is verified

        try
        {
            if (!user.Email.Verification.IsActive)
                return new Response("Inactive account", 400);
        }
        catch
        {
            return new Response("Unable to verify your profile", 500);
        }

        #endregion

        #region 05. Returns the data

        try
        {
            var data = new ResponseData
            {
                Id = user.Id.ToString(),
                Name = user.Name,
                Email = user.Email,
                Roles = user.Roles.Select(r => r.Name).ToArray()
            };

            return new Response(string.Empty, data);
        }
        catch
        {
            return new Response("Unable to retrieve profile data", 500);
        }

        #endregion
    }
}

