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

        public IActionResult Hakk�m�zda()
        {
            return View();
        }
        // Di�er eylemler
        [HttpPost]
        public async Task<IActionResult> IletisimGonder(string Name, string Email, string Subject, string Message)
        {
            // Form verilerini i�leyin ve gerekirse kaydedin

            // Kullan�c�ya otomatik cevap e-postas� g�nderme
            string autoReplySubject = "�leti�im Formunuza Te�ekk�rler!";
            string autoReplyMessage = $"Merhaba {Name},<br><br>" +
                                      "Mesaj�n�z� ald�k ve en k�sa s�rede size geri d�nece�iz.<br><br>" +
                                      "Te�ekk�rler!<br><br>" +
                                      "ihsan";

            await _emailService.SendEmailAsync(Email, autoReplySubject, autoReplyMessage);

            // Ba�ka bir sayfaya y�nlendirme veya bir onay mesaj� g�sterebilirsiniz
            return View("IletisimOnay"); // �leti�im formu g�nderildikten sonra g�sterilecek sayfa
        }

        [HttpGet]
            public IActionResult Search(string q)
            {
                // Arama sorgusunu i�leyin ve sonu�lar� d�nd�r�n
                // �rne�in, arama sonu�lar�n� bir g�r�n�me (view) ge�irin
                ViewData["SearchQuery"] = q;
                return View(); // Arama sonu�lar�n� g�sterecek bir g�r�n�m
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
                TempData["SuccessMessage"] = "Blog yaz�n�z ba�ar�yla olu�turuldu. Te�ekk�rler!";
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
                // Hata g�nl���
                Console.WriteLine(ex.Message);
                return View("Error");
            }
        }

    }
}
