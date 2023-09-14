namespace OnlineShop.Domain.Entities;

public class ConfirmationCode : IEntity
{
    public Guid Id { get; init; }
    public Guid AccountId { get; set; }
    public string Code { get; set; }
    public DateTimeOffset CreateAt { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }

    public ConfirmationCode(Guid id, Guid accountId, DateTimeOffset createdAt, TimeSpan codeLifetime)
    {
        Id = id;
        AccountId = accountId;
        Code = GenerateCode();
        CreateAt = createdAt;
        ExpiresAt = CreateAt.Add(codeLifetime);
    }

    private string GenerateCode()
    {
        return Random.Shared.Next(100_000, 999_999).ToString();
    }
}