using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QuizAPI.Models;

namespace QuizAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LobbiesController : ControllerBase
    {
        private readonly LobbyContext _context;

        public LobbiesController(LobbyContext context)
        {
            _context = context;
        }

        // GET: api/Lobbies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lobby>>> GetLobbyItems()
        {
            return await _context.LobbyItems.ToListAsync();
        }

        // GET: api/Lobbies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Lobby>> GetLobby(int id)
        {
            string connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8;Password=b7zYDzhJ;";
            SqlConnection cnn = new SqlConnection(connetionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM lobby WHERE Id="+id+";", cnn);

            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader();

            if (data.HasRows == false)
            {
                data.Close();
                cmd.Dispose();
                cnn.Close();
                return Ok(id);
            }

            //create lobby object
            Lobby lobby = new Lobby();

            data.Read();
            lobby.id = data.GetInt32(0);
            lobby.easyToken = data.GetString(1);
            lobby.mediumToken = data.GetString(2);
            lobby.hardToken = data.GetString(3);
            lobby.requestURL = data.GetString(4);

            data.Close();
            cmd.Dispose();
            cnn.Close();

            return lobby;
        }

        // PUT: api/Lobbies/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLobby(int id, Lobby lobby)
        {
            if (id != lobby.id)
            {
                return BadRequest();
            }

            _context.Entry(lobby).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LobbyExists(id))
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

        // POST: api/Lobbies
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Lobby>> PostLobby(Lobby lobby)
        {
            _context.LobbyItems.Add(lobby);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLobby", new { id = lobby.id }, lobby);
        }

        // DELETE: api/Lobbies/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Lobby>> DeleteLobby(int id)
        {
            var lobby = await _context.LobbyItems.FindAsync(id);
            if (lobby == null)
            {
                return NotFound();
            }

            _context.LobbyItems.Remove(lobby);
            await _context.SaveChangesAsync();

            return lobby;
        }

        private bool LobbyExists(int id)
        {
            return _context.LobbyItems.Any(e => e.id == id);
        }
    }
}
