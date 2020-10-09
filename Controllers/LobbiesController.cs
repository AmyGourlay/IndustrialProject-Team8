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
            string connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8;Password=b7zYDzhJ;";
            SqlConnection cnn = new SqlConnection(connetionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM lobby;", cnn);

            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader();

            if(data.HasRows == false)
            {
                data.Close();
                cmd.Dispose();
                cnn.Close();
                return Ok("No records");
            }

            //create lobby object
            List<Lobby> lobbies = new List<Lobby>();
            Lobby tmp;

			while(data.Read())
			{
                tmp = new Lobby();
                tmp.id = data.GetInt32(0);
                tmp.easyToken = data.GetString(1);
                tmp.mediumToken = data.GetString(2);
                tmp.hardToken = data.GetString(3);
                tmp.requestURL = data.GetString(4);

                lobbies.Add(tmp);
            }

            data.Close();
            cmd.Dispose();
            cnn.Close();

            return lobbies;
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

            string connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8;Password=b7zYDzhJ;";
            SqlConnection cnn = new SqlConnection(connetionString);
            SqlCommand cmd = new SqlCommand("UPDATE lobby " +
                                            "SET easyToken = '" + lobby.easyToken + 
                                            "', mediumToken = '" + lobby.mediumToken + 
                                            "', hardToken = '" + lobby.hardToken +
                                            "', requestURL = '" + lobby.requestURL + "' " +
                                            "WHERE Id = " + id + ";", cnn);

            cnn.Open();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            cnn.Close();

            return NoContent();
        }

        // POST: api/Lobbies
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Lobby>> PostLobby(Lobby lobby)
        {
            string connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8;Password=b7zYDzhJ;";
            SqlConnection cnn = new SqlConnection(connetionString);
            SqlCommand cmd = new SqlCommand("INSERT INTO lobby(Id, easyToken, mediumToken, hardToken, requestURL) " +
                                            "VALUES(" + lobby.id + ", '" + lobby.easyToken + "', '" + lobby.mediumToken + "', '" + lobby.hardToken + "', '" + lobby.requestURL + "');", cnn);

            cnn.Open();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            cnn.Close();

            return CreatedAtAction("GetLobby", new { id = lobby.id }, lobby);
        }

        // DELETE: api/Lobbies/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Lobby>> DeleteLobby(int id)
        {
            string connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8;Password=b7zYDzhJ;";
            SqlConnection cnn = new SqlConnection(connetionString);
            SqlCommand cmd = new SqlCommand("SELECT Id FROM lobby WHERE Id=" + id + ";", cnn);

            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader();

            if (data.HasRows == false)
            {
                data.Close();
                cmd.Dispose();
                cnn.Close();
                return NotFound();
            }

            data.Close();
            cmd.Dispose();

            cmd = new SqlCommand("DELETE FROM lobby WHERE Id = " + id + ";", cnn);
            cmd.ExecuteNonQuery();

            cmd.Dispose();
            cnn.Close();

            return Ok("Lobby deleted");
        }

        private bool LobbyExists(int id)
        {
            return _context.LobbyItems.Any(e => e.id == id);
        }
    }
}
