#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThaLeague.Data;
using ThaLeague.Models;
using ThaLeague.ViewModels;

namespace ThaLeague.Controllers
{
    public class AudioController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHost;

        public AudioController(ApplicationDbContext context, IWebHostEnvironment web)
        {
            webHost = web;
            _context = context;
        }

        // GET: Audio
        public async Task<IActionResult> Index()
        {
            return View(await _context.Audio.ToListAsync());
        }

        // GET: Audio/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var audio = await _context.Audio
                .FirstOrDefaultAsync(m => m.AudioId == id);
            if (audio == null)
            {
                return NotFound();
            }

            return View(audio);
        }
        [Route("Audio/Create")]
        // GET: Audio/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Audio/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Audio/Create/{id:int}")]
        public async Task<IActionResult> Create(int id, [Bind("Image,Path,Artist,Title,Spotify")] AudioViewModel audio)
        {
            var artist = _context.Artist.SingleOrDefault(a => a.ArtistId == id);
            if (ModelState.IsValid)
            {
                var fileName = UploadAudio(audio);
                var fileName2 = UploadImage(audio);
                var entity = new Audio
                {
                    ArtistId = id,
                    Artist = artist,
                    StageName = artist.StageName,
                    Image = fileName2,
                    Path = fileName,
                    Title = audio.Title,
                    Featuring = audio.Featuring,
                    Producer = audio.Producer,
                    Credits = audio.Credits,
                    Lyrics = audio.Lyrics,
                    Spotify = audio.Spotify
                };
                _context.Audio.Add(entity);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Artist", new { @id });
            }
            return View(audio);
        }

        // GET: Audio/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var audio = await _context.Audio.FindAsync(id);
            if (audio == null)
            {
                return NotFound();
            }
            return View(audio);
        }

        // POST: Audio/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Audio/Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id, [Bind("AudioId,Image,Title,Path,Spotify")] Audio audio)
        {
            if (id != audio.AudioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(audio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AudioExists(audio.AudioId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(audio);
        }

        // GET: Audio/Delete/5
        /*public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var audio = await _context.Audio
                .FirstOrDefaultAsync(m => m.AudioId == id);
            if (audio == null)
            {
                return NotFound();
            }

            return View(audio);
        }*/

        // POST: Audio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Audio/DeleteConfirmed/{id:int}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var audio = await _context.Audio
                .FirstOrDefaultAsync(m => m.AudioId == id);
            var artist = await _context.Artist.FirstOrDefaultAsync(a => a.Audios.ToList().FirstOrDefault(a => a.AudioId == id).AudioId == id);
            int? artistId = artist.ArtistId;

            _context.Audio.Remove(audio);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Artist", new { artistId });
        }

        private bool AudioExists(int id)
        {
            return _context.Audio.Any(e => e.AudioId == id);
        }

        private string UploadAudio(AudioViewModel model)
        {
            string uniqueFileName = null;
            if (model.Path != null)
            {
                string uploadsFolder = Path.Combine(webHost.WebRootPath, "audios");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Path.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                model.Path.CopyTo(fileStream);
            }
            return uniqueFileName;
        }

        private string UploadImage(AudioViewModel model)
        {
            string uniqueFileName = null;
            if (model.Image != null)
            {
                string uploadsFolder = Path.Combine(webHost.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                model.Image.CopyTo(fileStream);
            }
            return uniqueFileName;
        }
    }
}
