using MF.JwtStore.Core.Contexts.SharedContext.ValueObjects;

namespace MF.JwtStore.Core.Contexts.AccountContext.ValueObjects;

public class Verification : ValueObject
{
    public Verification() { }

    public string Code { get; }
        = Guid.NewGuid().ToString("N")[..6].ToUpper();

    public DateTime? ExpiresAt { get; private set; }
        = DateTime.UtcNow.AddMinutes(5);

    public DateTime? VerifiedAt { get; private set; }
        = null;

    public bool IsActive
        => VerifiedAt != null && ExpiresAt == null;

    public void Verify(string code)
    {
        if (IsActive)
            throw new Exception("Este item já esta ativo.");

        if (ExpiresAt < DateTime.Now)
            throw new Exception("Este código já expirou");

        if (!string.Equals(code.Trim(), code.Trim(), StringComparison.CurrentCultureIgnoreCase))
            throw new Exception("Código de verificação inválido");

        ExpiresAt = null;
        VerifiedAt = DateTime.Now;
    }
}

