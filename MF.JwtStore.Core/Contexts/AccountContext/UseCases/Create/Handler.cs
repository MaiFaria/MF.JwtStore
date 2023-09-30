using MediatR;
using MF.JwtStore.Core.Contexts.AccountContext.Entities;
using MF.JwtStore.Core.Contexts.AccountContext.UseCases.Create.Contracts;
using MF.JwtStore.Core.Contexts.AccountContext.ValueObjects;

namespace MF.JwtStore.Core.Contexts.AccountContext.UseCases.Create;

public class Handler : IRequestHandler<Request, Response>
{
    private readonly IRepository _repository;
    private readonly IService _service;

    public Handler(IRepository repository,
                   IService service)
    {
        _repository = repository;
        _service = service;
    }

    public async Task<Response> Handle(Request request,
                                       CancellationToken cancellationToken)
    {
        #region Validate request
        try
        {
            var response = Specification.Ensure(request);
            if (!response.IsValid)
                return new Response("Invalid request",
                    400, response.Notifications);
        }
        catch
        {
            return new Response("Unable to validate your request", 500);
        }
        #endregion

        #region Generate Objects
        Email email;
        Password password;
        User user;

        try
        {
            email = new Email(request.Email);
            password = new Password(request.Password);
            user = new User(request.Name, email, password);
        }
        catch (Exception ex)
        {
            return new Response(ex.Message, 400);
        }
        #endregion

        #region Check existing user
        try
        {
            var exists = await _repository.AnyAsync(request.Email, cancellationToken);
            if (exists)
                return new Response("This email is already in use.", 400);
        }
        catch 
        {
            return new Response("Failed to verify registered email", 500);
        }
        #endregion

        #region Persist data
        try
        {
            await _repository.SaveAsync(user, cancellationToken);
        }
        catch
        {
            return new Response("Failed to persist data.", 500);
        }
        #endregion

        #region Send confirmation email
        try
        {
            await _service.SendVerificationEmailAsync(user, cancellationToken);
        }
        catch
        { }

        #endregion

        return new Response("Account created",
            new ResponseData(user.Id, user.Name, user.Email));
    }
}

