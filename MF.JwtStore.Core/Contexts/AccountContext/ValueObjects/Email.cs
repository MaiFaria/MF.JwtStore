﻿using System.Text.RegularExpressions;
using MF.JwtStore.Core.Contexts.SharedContext.Extensions;
using MF.JwtStore.Core.Contexts.SharedContext.ValueObjects;

namespace MF.JwtStore.Core.Contexts.AccountContext.ValueObjects;

public partial class Email : ValueObject
{
    private const string Pattern
        = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

    protected Email() { }

    public Email(string address)
    {
        if (string.IsNullOrEmpty(address))
            throw new Exception("Invalid email");

        Address = address.Trim().ToLower();

        if (Address.Length < 5 ||
            !EmailRegex().IsMatch(Address))
            throw new Exception("Invalid email");
    }

    public string Address { get; }
    public string Hash => Address.ToBase64();
    public Verification Verification { get; private set; } = new();

    public void ResendVerification()
        => Verification = new Verification();

    public static implicit operator string(Email email)
        => email.ToString();

    public static implicit operator Email(string address)
        => new(address);

    public override string ToString()
        => Address;

    [GeneratedRegex(Pattern)]
    private static partial Regex EmailRegex();
}