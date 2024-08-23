using blogSitesi.DB;
using blogSitesi.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[Authorize(AuthenticationSchemes = "AdminCookie")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly AdminDbContext _adminDbContext;

    public AdminController(AdminDbContext adminDbContext, ApplicationDbContext context)
    {
        _adminDbContext = adminDbContext;
        _context = context;
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var admin = _adminDbContext.Admins
            .FirstOrDefault(a => a.Email == email && a.Password == password);

        if (admin != null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, admin.Email)
            };

            var claimsIdentity = new ClaimsIdentity(claims, "AdminCookie");
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };

            await HttpContext.SignInAsync("AdminCookie", new ClaimsPrincipal(claimsIdentity), authProperties);

            return RedirectToAction("Anasayfa");
        }

        ViewBag.Error = "Geçersiz email veya şifre";
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("AdminCookie");
        return RedirectToAction("Anasayfa","Home");
    }

    // GET: Admin
    public async Task<IActionResult> Anasayfa()
    {
        return View(await _context.BlogPosts.ToListAsync());
    }
    // GET: Admin/Create
    public IActionResult Create()
    {
        if (HttpContext.Session.GetString("Admin") == null)
        {
            return RedirectToAction("Login");
        }
        return View();
    }

    // POST: Admin/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,Content,ImagePath")] BlogPost blogPost)
    {
        if (ModelState.IsValid)
        {
            _context.Add(blogPost);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(blogPost);
    }


    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var blogPost = await _context.BlogPosts.FindAsync(id);
        if (blogPost == null)
        {
            return NotFound();
        }

        return View(blogPost);
    }

    // POST: Admin/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,ImagePath")] BlogPost blogPost)
    {
        if (id != blogPost.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(blogPost);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogPostExists(blogPost.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Anasayfa","Admin");
        }
        return View(blogPost);
    }

    // GET: Admin/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var blogPost = await _context.BlogPosts
            .FirstOrDefaultAsync(m => m.Id == id);
        if (blogPost == null)
        {
            return NotFound();
        }

        return View(blogPost);
    }

    // POST: Admin/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var blogPost = await _context.BlogPosts.FindAsync(id);
        if (blogPost == null)
        {
            return NotFound();
        }
        _context.BlogPosts.Remove(blogPost);
        await _context.SaveChangesAsync();
        return RedirectToAction("Anasayfa","Admin");
    }

    private bool BlogPostExists(int id)
    {
        return _context.BlogPosts.Any(e => e.Id == id);
    }
    public async Task<IActionResult> Index()
    {
        return View(await _context.BlogPosts.ToListAsync());
    }
    public async Task<IActionResult> Detail(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var blogPost = await _context.BlogPosts
            .FirstOrDefaultAsync(m => m.Id == id);

        if (blogPost == null)
        {
            return NotFound();
        }

        return View(blogPost);
    }
}
