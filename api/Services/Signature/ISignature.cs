namespace Api.Services.Signature;

public interface ISignature
{
    public Task<SignatureOutput> Signature(SignatureInput input);
}