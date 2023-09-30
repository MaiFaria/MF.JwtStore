using MF.JwtStore.Core.Contexts.SharedContext.Entities;

namespace MF.JwtStore.Core.Contexts.AccountContext.Entities;

public class Role : Entity
{
    public string Name { get; set; } = string.Empty;

    public List<User> Users { get; set; } = new();
}