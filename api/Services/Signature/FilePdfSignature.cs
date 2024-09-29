using System.IO;
using iTextSharp.text;
using BouncyCert = Org.BouncyCastle.X509.X509Certificate;
using CertificateX509 = System.Security.Cryptography.X509Certificates.X509Certificate2;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;
using X509Parser = Org.BouncyCastle.X509.X509CertificateParser;
using CryptoException = System.Security.Cryptography.CryptographicException;
using PdfWriter = iText.Kernel.Pdf.PdfWriter;

namespace Api.Services.Signature;

public class FilePdfSignature: ISignature
{
    public async Task<SignatureOutput> Signature(SignatureInput input)
    {
        var cert = new CertificateX509(GetCertPath(input.Cert.FileName), input.Pin);
        string certName = cert.Subject;
        var rawData = GetCertRowData(cert);
        string fileNameOut = input.File.FileName;
        await using var file = InputFileEncapsulate(input.File, fileNameOut);
        using var reader = new PdfReader(file.Name);
        using var stamper = CreateStamperPdf(reader, file);
        var datetime = DateTime.Now;

        var signature = GetSignaturePdf(input.Cert.Alias, datetime);
        var extraText = $"Signed by {certName} at {datetime:dd/MM/yyyy}";

        var appearance = GetSignatureAppearance(stamper, rawData[0], signature, extraText);
        var external = new ExternalSignature(cert);

        MakeSignature.SignDetached(appearance, external, rawData, null, null, null, 0, CryptoStandard.CMS);

        stamper.Close();
        reader.Close();
        file.Close();

        return new SignatureOutput(datetime.ToBinary(), fileNameOut);
    }

    private PdfSignatureAppearance GetSignatureAppearance(
        PdfStamper stamper,
        BouncyCert cert,
        PdfSignature signature,
        string extraData)
    {
        var apparence = stamper.SignatureAppearance;
        apparence.Certificate = cert;
        apparence.CryptoDictionary = signature;
        apparence.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.DESCRIPTION;
        apparence.SetVisibleSignature(new Rectangle(100, 100, 300, 200), 1, "Signature");
        apparence.Reason = "Personal";
        apparence.Location = "Brazil";
        apparence.Layer2Text = extraData;
        apparence.Acro6Layers = true;

        return apparence;
    }

    private PdfStamper CreateStamperPdf(PdfReader reader, FileStream file)
    {
        return PdfStamper.CreateSignature(reader, file, '\0', null, true);
    }

    private PdfSignature GetSignaturePdf(string certAlias, DateTime timestamp)
    {
        var signature = new PdfSignature(PdfName.ADOBE_PPKLITE, PdfName.ADBE_PKCS7_DETACHED);
        signature.Name = certAlias;
        signature.Date = new PdfDate(timestamp);
        return signature;
    }

    private FileStream InputFileEncapsulate(IFormFile inputFile, string fileNameOut)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pdfs");
        var filepath = Path.Combine(path, fileNameOut);
        var file = new FileStream(filepath, FileMode.Create);
        inputFile.CopyTo(file);
        return file;
    }

    private string GetCertPath(string certFileName)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Storage", "Certs");
        return Path.Combine(path, certFileName);
    }

    private BouncyCert[] GetCertRowData(CertificateX509 x509)
    {
        var parser = new X509Parser();
        BouncyCert[] cert = new BouncyCert[1];
        cert[0] = parser.ReadCertificate(x509.RawData);
        return cert;
    }
}