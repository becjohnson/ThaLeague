#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThaLeague.Data;
using ThaLeague.Models;

namespace ThaLeague.ViewModels
{
    public class VideoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VideoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Video
        public async Task<IActionResult> Index()
        {
            return View(await _context.Video.ToListAsync());
        }

        public PartialViewResult RenderVideos(int id)
        {
            var videos = _context.Video.Where(e => e.ArtistId == id).ToListAsync();
            return PartialView(videos);

        }

        // GET: Video/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var video = await _context.Video
                .FirstOrDefaultAsync(m => m.VideoId == id);
            if (video == null)
            {
                return NotFound();
            }

            return View(video);
        }

        // GET: Video/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Video/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Video/Create/{id:int}")]
        public async Task<IActionResult> Create(int id, [Bind("VideoId,Url,Description,Title,Artist")] Video video)
        {
            var videoId = "https://www.youtube.com/embed/" + Youtuber(video.Url);
            var artist = _context.Artist.SingleOrDefault(a => a.ArtistId == id);
            if (ModelState.IsValid)
            {
                var entity = new Video
                {
                    ArtistId = id,
                    Artist = artist,
                    StageName = artist.StageName,
                    Title = video.Title,
                    Description = video.Description,
                    Url = videoId
                };
                _context.Video.Add(entity);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Artist", new { @id });
            }
            return View(video);
        }

        // GET: Video/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var video = await _context.Video.FindAsync(id);
            if (video == null)
            {
                return NotFound();
            }
            return View(video);
        }

        // POST: Video/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VideoId,Url,Description,Title")] Video video)
        {
            if (id != video.VideoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(video);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VideoExists(video.VideoId))
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
            return View(video);
        }

        // GET: Video/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var video = await _context.Video
                .FirstOrDefaultAsync(m => m.VideoId == id);
            if (video == null)
            {
                return NotFound();
            }

            return View(video);
        }

        // POST: Video/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Video/DeleteConfirmed/{id:int}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var video = await _context.Video.FindAsync(id);
            _context.Video.Remove(video);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VideoExists(int id)
        {
            return _context.Video.Any(e => e.VideoId == id);
        }

        public string Youtuber(string url)
        {
            if (url != null)
            {
                var videoId = url.Remove(0, 17);
                return videoId;
            }
            return null;
        }
    }
}
