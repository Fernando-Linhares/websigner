using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using iTextSharp.text.pdf.security;
using Certificate = System.Security.Cryptography.X509Certificates.X509Certificate2;


namespace Api.Services;

public class ExternalSignature: IExternalSignature
{
    private Certificate _cert;

    public ExternalSignature(Certificate cert)
    {
        _cert = cert;
    }

    public virtual byte[] Sign(byte[] message)
    {
        byte[] signedData = SignatureComplex(message);
        return signedData;
    }

    public byte[] SignatureComplex(byte[]  message)
    {
        return _cert
                   .GetRSAPrivateKey()?
                   .SignData(
                       message,
                       HashAlgorithmName.SHA256,
                       RSASignaturePadding.Pkcs1
                   )
               ??
               new byte[0];
    }

    public virtual string GetHashAlgorithm()
    {
        return "SHA-256";
    }

    public virtual string GetEncryptionAlgorithm()
    {
        return "RSA";
    }
}