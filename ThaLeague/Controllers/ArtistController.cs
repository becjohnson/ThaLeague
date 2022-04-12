#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThaLeague.Data;
using ThaLeague.ViewModels;

namespace ThaLeague.Models
{
    public class ArtistController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHost;

        public ArtistController(ApplicationDbContext context, IWebHostEnvironment web)
        {
            webHost = web;
            _context = context;
        }

        // GET: Artist
        public async Task<IActionResult> Index()
        {
            return View(await _context.Artist.ToListAsync());
        }

        // GET: Artist/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _context.Artist
                .Include(a => a.Audios)
                .Include(a => a.Videos)
                .FirstOrDefaultAsync(a => a.ArtistId == id);
            if (artist == null)
            {
                return NotFound();
            }

            return View(artist);
        }

        // GET: Artist/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Artist/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,StageName,Bio,Instagram,Twitter,Facebook,Spotify,Youtube,Soundcloud,Image,State,City,DisplayName,ColorPicker")] ArtistViewModel artist)
        {
            string spotifyUri = GetSpotifyPlayer(artist.Spotify);
            string fileName = UploadImage(artist);
            if (ModelState.IsValid)
            {
                var entity = new Artist
                {
                    ArtistId = artist.ArtistId,
                    FirstName = artist.FirstName,
                    LastName = artist.LastName,
                    Instagram = artist.Instagram,
                    Facebook = artist.Facebook,
                    Spotify = artist.Spotify,
                    SpotifyUrl = spotifyUri,
                    Twitter = artist.Twitter,
                    Soundcloud = artist.Soundcloud,
                    Youtube = artist.Youtube,
                    State = artist.State,
                    City = artist.City,
                    DisplayName = artist.DisplayName,
                    ColorPicker = artist.ColorPicker,
                    Bio = artist.Bio,
                    Image = fileName,
                    StageName = artist.StageName
                };
                _context.Add(entity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(artist);
        }

        // GET: Artist/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _context.Artist
                .Include(a => a.Audios)
                .Include(a => a.Videos)
                .FirstOrDefaultAsync(a => a.ArtistId == id);

            if (artist == null)
            {
                return NotFound();
            }
            return View(artist);
        }

        // POST: Artist/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Artist/Edit/{id:int}")]
        public async Task<IActionResult> Edit(int? id, [Bind("ArtistId,FirstName,LastName,FullName,StageName,Youtube,Bio,Twitter,Image,Instagram,Facebook,Spotify,Soundcloud,Audios,Videos")] ArtistViewModel artistView)
        {
            var artist = await _context.Artist
                                       .Include(a => a.Audios)
                                       .Include(a => a.Videos)
                                       .FirstOrDefaultAsync(a => a.ArtistId == id);
            if (id != artist.ArtistId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string fileName = "";
                    var entity = _context.Artist
                                .Include(a => a.Audios)
                                .Include(a => a.Videos)
                                .Single(a => a.ArtistId == id);
                    entity.Audios = artist.Audios;
                    entity.Videos = artist.Videos;
                    if (artistView.Image == null)
                    {
                        entity.Image = artist.Image;
                    }
                    else
                        fileName = UploadImage(artistView);
                    entity.Bio = artistView.Bio;
                    entity.City = artistView.City;
                    entity.State = artistView.State;
                    entity.FirstName = artistView.FirstName;
                    entity.StageName = artistView.StageName;
                    entity.Facebook = artistView.Facebook;
                    entity.Instagram = artistView.Instagram;
                    entity.Spotify = artistView.Spotify;
                    entity.Twitter = artistView.Twitter;
                    entity.Soundcloud = artistView.Soundcloud;
                    entity.Youtube = artistView.Youtube;
                    _context.Update(entity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtistExists(artist.ArtistId))
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
            return View(artist);
        }

        // GET: Artist/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var artist = await _context.Artist
                .FirstOrDefaultAsync(m => m.ArtistId == id);
            if (artist == null)
            {
                return NotFound();
            }

            return View(artist);
        }

        // POST: Artist/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Artist/DeleteConfirmed/{id:int}")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var artist = await _context.Artist.Include(a => a.Audios)
                .FirstOrDefaultAsync(a => a.ArtistId == id);
            _context.Artist.Remove(artist);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArtistExists(int? id)
        {
            return _context.Artist.Any(e => e.ArtistId == id);
        }

        private string UploadImage(ArtistViewModel model)
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

        string GetSpotifyPlayer(string url)
        {
            if (url != null)
            {
                string result = url.Remove(0, 32);
                var strings = result.Split("?", StringSplitOptions.TrimEntries);
                var artistId = strings.First();
                return artistId;
            }
            return null;
        }
    }
}
