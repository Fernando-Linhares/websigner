using Api.Database;

namespace Api.Controllers.Crud;

public abstract class CrudController : BaseController
{
    private readonly DataContext _context;

    protected CrudController(DataContext context): base(context)
    {
        
    }
}