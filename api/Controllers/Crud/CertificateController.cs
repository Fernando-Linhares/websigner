using System.Security.Claims;
using Api.Controllers.Crud.Requests;
using Api.Database;
using Api.Models;
using Api.Models.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers.Crud;

[Authorize]
[ApiController]
[Route("[controller]/")]
public class CertificateController: BaseController
{
    private readonly DataContext _context;

    public CertificateController(DataContext context): base(context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index(
        [FromQuery(Name = "page")] int page = 1,
        [FromQuery(Name = "skipPagination")] bool skipPagination = false,
        [FromQuery(Name = "perPage")] int perPage = 20
        )
    {

        var query = _context.Certificates
            .Where(c => c.DeletedAt == null && c.UserId == CurrentUser().Id)
            .OrderByDescending(c => c.CreatedAt);
        
        if (skipPagination)
        {
            return Ok(new
            {
                data = await query.ToListAsync()
            });
        }
        
        var total = _context.Certificates.Count(c => c.DeletedAt == null && c.UserId == CurrentUser().Id);

        var input = new PaginationInput<Certificate>(page, perPage, total, query);
        var model = new Certificate();
        return AnswerPagination(await model.Paginate(input));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(long id)
    {
        Certificate? cert =  await _context.Certificates
            .Where(c => c.Id == id && c.DeletedAt == null && c.UserId == CurrentUser().Id)
            .FirstOrDefaultAsync();

        if (cert is null)
        {
            return NotFound();
        }
        
        return AnswerSuccess(cert);
    }

    private bool UserHasCertificate()
    {
        return _context
            .Certificates
            .Any(c => c.UserId == CurrentUser().Id
                      && c.DeletedAt == null);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CertificateRegisterRequest certificate)
    {
       var path = Path.Combine(Directory.GetCurrentDirectory(), "Storage", "Certs");
       var file = certificate.file;
       var fileName = Path.GetRandomFileName() + Path.GetExtension(file.FileName);
       var filePath = Path.Combine(path, fileName);
       await using var stream = new FileStream(filePath, FileMode.Create);
       await file.CopyToAsync(stream);
       stream.Close();

       string email = User?.FindFirst(ClaimTypes.Email).Value;
       User? user = CurrentUser();

       var cert = new Certificate
       {
           FileName = fileName,
           Alias = certificate.Name,
           UserId = user.Id,
           CreatedAt = DateTime.Now,
           ExpiresAt = DateTime.Now,
           IsActive = !UserHasCertificate()
       };
       _context.Certificates.Add(cert);
       await _context.SaveChangesAsync();

       return AnswerSuccess(cert);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var cert = await _context.Certificates.FindAsync(id);

        if (cert is null)
        {
            return NotFound();
        }
        
        cert.DeletedAt = DateTime.Now;
        _context.Certificates.Update(cert);
        await _context.SaveChangesAsync();
        return AnswerSuccess(cert);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Select(long id)
    {
        if (!UserHasCertificate())
        {
            return NotFound();
        }
        
        var cert = await _context.Certificates.FindAsync(id);
        cert.IsActive = true;
        _context.Certificates.Update(cert);
        await _context.SaveChangesAsync();
        var user = CurrentUser();
        
        var certs = await _context.Certificates.Where(c => c.DeletedAt == null 
                                         && c.UserId == user.Id
                                         && c.Id != cert.Id)
            .ToListAsync();

        var certsUnselected = new List<Certificate>();
        foreach (var c in certs)
        {
            c.IsActive = false;
            certsUnselected.Add(c);
        }
        
        _context.Certificates.UpdateRange(certsUnselected);
        await _context.SaveChangesAsync();
        
        return AnswerSuccess(cert);
    }
}