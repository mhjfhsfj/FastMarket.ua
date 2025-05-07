using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using FastMarketBackEnd.DataTypes;
using Microsoft.Extensions.Options;

namespace FastMarketBackEnd.services;

public class LiqPayService
{
    private readonly string _publicKey;
    private readonly string _privateKey;

    public LiqPayService(IOptions<LiqPayOptions> options)
    {
        _publicKey = options.Value.PublicKey;
        _privateKey = options.Value.PrivateKey;
    }

    public (string data, string signature) CreatePayment(decimal amount, string orderId, string description)
    {
        var payload = new
        {
            public_key = _publicKey,
            version = 3,
            action = "pay",
            amount = amount,
            currency = "UAH",
            description = description,
            order_id = orderId,
            sandbox = 1, // Важливо!
            result_url = "https://example.com/success",
            server_url = "https://example.com/payment-callback"
        };

        var json = JsonSerializer.Serialize(payload);
        var data = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));

        var signature = GetSignature(data);

        return (data, signature);
    }

    private string GetSignature(string data)
    {
        var str = _privateKey + data + _privateKey;
        using var sha1 = SHA1.Create();
        var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(str));
        return Convert.ToBase64String(hash);
    }
}