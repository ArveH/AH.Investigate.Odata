namespace Pg.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientsController : ControllerBase
{
    private readonly IODataService _oDataService;
    private readonly IDbFactory _dbFactory;

    public ClientsController(
        IODataService oDataService,
        IDbFactory dbFactory)
    {
        _oDataService = oDataService;
        _dbFactory = dbFactory;
    }

    [HttpGet]
    public async Task<IActionResult> GetClients()
    {
        await using var context = _dbFactory.CreateContext();
        IQueryable<Client> query = context.Clients;

        var result = await _oDataService.ApplyTo(query, Request)
            .ToListAsync();

        return Ok(result);
    }

    // GET: api/Clients/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Client>> GetClient(int id)
    {
        await using var context = _dbFactory.CreateContext();
        var client = await context.Clients.FindAsync(id);

        if (client == null)
        {
            return NotFound();
        }

        return client;
    }

    // PUT: api/Clients/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutClient(int id, Client client)
    {
        if (id != client.Id)
        {
            return BadRequest();
        }

        await using var context = _dbFactory.CreateContext();
        context.Entry(client).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ClientExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Clients
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Client>> PostClient(Client client)
    {
        await using var context = _dbFactory.CreateContext();
        context.Clients.Add(client);
        await context.SaveChangesAsync();

        return CreatedAtAction("GetClient", new { id = client.Id }, client);
    }

    // DELETE: api/Clients/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClient(int id)
    {
        await using var context = _dbFactory.CreateContext();
        var client = await context.Clients.FindAsync(id);
        if (client == null)
        {
            return NotFound();
        }

        context.Clients.Remove(client);
        await context.SaveChangesAsync();

        return NoContent();
    }

    private bool ClientExists(int id)
    {
        using var context = _dbFactory.CreateContext();
        return context.Clients.Any(e => e.Id == id);
    }
}