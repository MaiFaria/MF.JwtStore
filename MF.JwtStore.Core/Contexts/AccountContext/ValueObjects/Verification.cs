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
            throw new Exception("This item is already active.");

        if (ExpiresAt < DateTime.Now)
            throw new Exception("This code has already expired");

        if (!string.Equals(code.Trim(), code.Trim(), StringComparison.CurrentCultureIgnoreCase))
            throw new Exception("Invalid verification code");

        ExpiresAt = null;
        VerifiedAt = DateTime.Now;
    }
}