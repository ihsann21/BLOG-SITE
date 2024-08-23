using blogSitesi.DB;
using blogSitesi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace blogSitesi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EmailService _emailService;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, EmailService emailService, ApplicationDbContext context)
        {
            _logger = logger;
            _emailService = emailService;
            _context = context;
        }

        public IActionResult Index()
        {
            var blogPost = new List<BlogPost>
        {
            
        };
            return View(blogPost);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public  IActionResult iletisim()
        {
            return View();
        }
        public IActionResult Guncel()
        {
            return View();
        }
        public IActionResult Muzik()
        {
            return View();
        }
        public IActionResult AnonimYaz()
        {
            return View();
        } 

        public IActionResult Hakkýmýzda()
        {
            return View();
        }
        // Diðer eylemler
        [HttpPost]
        public async Task<IActionResult> IletisimGonder(string Name, string Email, string Subject, string Message)
        {
            // Form verilerini iþleyin ve gerekirse kaydedin

            // Kullanýcýya otomatik cevap e-postasý gönderme
            string autoReplySubject = "Ýletiþim Formunuza Teþekkürler!";
            string autoReplyMessage = $"Merhaba {Name},<br><br>" +
                                      "Mesajýnýzý aldýk ve en kýsa sürede size geri döneceðiz.<br><br>" +
                                      "Teþekkürler!<br><br>" +
                                      "ihsan";

            await _emailService.SendEmailAsync(Email, autoReplySubject, autoReplyMessage);

            // Baþka bir sayfaya yönlendirme veya bir onay mesajý gösterebilirsiniz
            return View("IletisimOnay"); // Ýletiþim formu gönderildikten sonra gösterilecek sayfa
        }

        [HttpGet]
            public IActionResult Search(string q)
            {
                // Arama sorgusunu iþleyin ve sonuçlarý döndürün
                // Örneðin, arama sonuçlarýný bir görünüme (view) geçirin
                ViewData["SearchQuery"] = q;
                return View(); // Arama sonuçlarýný gösterecek bir görünüm
            }
       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        // GET: /BlogPosts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /BlogPosts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogPost blogPosts)
        {
            if (ModelState.IsValid)
            {
                blogPosts.CreatedDate = DateTime.Now;
                blogPosts.UpdatedDate = DateTime.Now;
                _context.Add(blogPosts);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Blog yazýnýz baþarýyla oluþturuldu. Teþekkürler!";
                return RedirectToAction(nameof(Index));

                
            }
            return View(blogPosts);
            
        }
        public async Task<IActionResult> Anasayfa()
        {
            try
            {
                var blogPosts = await _context.BlogPosts.OrderByDescending(p => p.CreatedDate).ToListAsync();
                return View(blogPosts);
            }
            catch (Exception ex)
            {
                // Hata günlüðü
                Console.WriteLine(ex.Message);
                return View("Error");
            }
        }

    }
}
