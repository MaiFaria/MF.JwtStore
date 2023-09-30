using MF.JwtStore.Core;
using MF.JwtStore.Core.Contexts.AccountContext.Entities;
using MF.JwtStore.Core.Contexts.AccountContext.UseCases.Create.Contracts;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MF.JwtStore.Infra.Contexts.AccountContext.UseCases.Create;

public class Service : IService
{
    public async Task SendVerificationEmailAsync(User user,
                                                 CancellationToken cancellationToken)
    {
        const string subject = "Check your account";

        var client = new SendGridClient(Configuration.SendGrid.ApiKey);
        var from = new EmailAddress(Configuration.Email.DefaultFromEmail,
                                    Configuration.Email.DefaultFromName);

        var to = new EmailAddress(user.Email, user.Name);
        var content = $"Code {user.Email.Verification.Code}";

        var msg = MailHelper.CreateSingleEmail(from,
                                               to,
                                               subject,
                                               content,
                                               content);

        await client.SendEmailAsync(msg, cancellationToken);
    }
}

