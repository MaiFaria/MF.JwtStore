using MF.JwtStore.Core.Contexts.AccountContext.Entities;
using MF.JwtStore.Core.Contexts.AccountContext.UseCases.Create.Contracts;
using MF.JwtStore.Core.Contexts.AccountContext.ValueObjects;

namespace MF.JwtStore.Core.Contexts.AccountContext.UseCases.Create;

public class Handler
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
        #region Validar requisição
        try
        {
            var response = Specification.Ensure(request);
            if (!response.IsValid)
                return new Response("Requisição inválida",
                    400, response.Notifications);
        }
        catch
        {
            return new Response("Não foi possível validar sua requisição", 500);
        }
        #endregion

        #region Gerar Objetos
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

        #region Verifica usuário existente
        try
        {
            var exists = await _repository.AnyAsync(request.Email, cancellationToken);
            if (exists)
                return new Response("Este E-mail já está em uso.", 400);
        }
        catch 
        {
            return new Response("Falha ao verificar E-mail cadastrado", 500);
        }
        #endregion

        #region Persistir os dados
        try
        {
            await _repository.SaveAsync(user, cancellationToken);
        }
        catch
        {
            return new Response("Falha ao persistir dados.", 500);
        }
        #endregion

        #region Enviar e-mail de confirmação
        try
        {
            await _service.SendVerificationEmailAsync(user, cancellationToken);
        }
        catch
        { }

        #endregion

        return new Response("Conta criada",
            new ResponseData(user.Id, user.Name, user.Email));
    }
}

