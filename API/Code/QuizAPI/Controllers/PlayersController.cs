using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using QuizAPI.Models;

namespace QuizAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly PlayerContext _context;

        public PlayersController(PlayerContext context)
        {
            _context = context;
        }

        ////**** GET ****////
        // GET: api/Players
        // READ ALL
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayerItems()
        {
            string connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8;Password=b7zYDzhJ;";
            SqlConnection cnn = new SqlConnection(connetionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM player;", cnn);

            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader();

            if(data.HasRows == false)
            {
                data.Close();
                cmd.Dispose();
                cnn.Close();
                return NotFound();
            }

            //create player object
            List<Player> players = new List<Player>();
            Player tmp;

            while(data.Read())
            {
                tmp = new Player();
                tmp.id = data.GetInt32(0);
                tmp.name = data.GetString(1);
                tmp.score = data.GetInt32(2);
                tmp.lobbyId = data.GetInt32(3);

                players.Add(tmp);
            }

            data.Close();
            cmd.Dispose();
            cnn.Close();

            return players;
        }

        // GET: api/Players/[id]
        // READ
        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayer(int id)
        {
            string connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8;Password=b7zYDzhJ;";
            SqlConnection cnn = new SqlConnection(connetionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM player WHERE id=" + id + ";", cnn);

            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader();

            if(data.HasRows == false)
            {
                data.Close();
                cmd.Dispose();
                cnn.Close();
                return NotFound();
            }

            //create player list
            Player player = new Player();

            data.Read();
            player.id = data.GetInt32(0);
            player.name = data.GetString(1);
            player.score = data.GetInt32(2);
            player.lobbyId = data.GetInt32(3);

            data.Close();
            cmd.Dispose();
            cnn.Close();

            return player;
        }

        // GET: api/Players/inlobby/[id]
        // READ
        [HttpGet("inlobby/{lobbyId}")]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayersInLobby(int lobbyId)
        {
            string connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8;Password=b7zYDzhJ;";
            SqlConnection cnn = new SqlConnection(connetionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM player WHERE lobbyId=" + lobbyId + ";", cnn);

            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader();

            if(data.HasRows == false)
            {
                data.Close();
                cmd.Dispose();
                cnn.Close();
                return NotFound("Tried GetPlayersInLobby() with id " + lobbyId);
            }

            //create player list
            List<Player> players = new List<Player>();
            Player tmp;

            while(data.Read())
            {
                tmp = new Player();
                tmp.id = data.GetInt32(0);
                tmp.name = data.GetString(1);
                tmp.score = data.GetInt32(2);
                tmp.lobbyId = data.GetInt32(3);

                players.Add(tmp);
            }

            data.Close();
            cmd.Dispose();
            cnn.Close();

            return players;
        }



        ////**** PUT ****////
        // PUT: api/Players/[id]
        // UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlayer(int id, Player player)
        {
            if(id != player.id)
            {
                return BadRequest();
            }

            string connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8;Password=b7zYDzhJ;";
            SqlConnection cnn = new SqlConnection(connetionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM player WHERE id=" + id + ";", cnn);

            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader();

            if(data.HasRows == false)
            {
                data.Close();
                cmd.Dispose();
                cnn.Close();
                return NotFound();
            }

            data.Close();
            cmd.Dispose();

            connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8;Password=b7zYDzhJ;";
            cmd = new SqlCommand("UPDATE player " +
                                            "SET PlayerName = '" + player.name +
                                            "', Score = " + player.score +
                                            ", lobbyId = " + player.lobbyId +
                                            " WHERE id = " + id + ";", cnn);

            cmd.ExecuteNonQuery();
            cmd.Dispose();
            cnn.Close();

            return NoContent();
        }



        ////**** POST ****////
        // POST: api/Players
        // INSERT
        [HttpPost]
        public async Task<ActionResult<Player>> PostPlayer(Player player)
        {
            string connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8;Password=b7zYDzhJ;";
            SqlConnection cnn = new SqlConnection(connetionString);
            SqlCommand cmd = new SqlCommand("INSERT INTO player(id, PlayerName, Score, lobbyId) " +
                                            "VALUES(" + player.id + ", '" + player.name + "', "  + player.score + ", " + player.lobbyId + ");", cnn);

            cnn.Open();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            cnn.Close();

            return CreatedAtAction("GetPlayer", new { id = player.id }, player);
        }



        ////**** DELETE ****////
        // DELETE: api/Players/[id]
        // DELETE
        [HttpDelete("{id}")]
        public async Task<ActionResult<Player>> DeletePlayer(int id)
        {
            string connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8;Password=b7zYDzhJ;";
            SqlConnection cnn = new SqlConnection(connetionString);
            SqlCommand cmd = new SqlCommand("SELECT Id FROM player WHERE id=" + id + ";", cnn);

            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader();

            if(data.HasRows == false)
            {
                data.Close();
                cmd.Dispose();
                cnn.Close();
                return NotFound();
            }

            data.Close();
            cmd.Dispose();

            cmd = new SqlCommand("DELETE FROM player WHERE id = " + id + ";", cnn);
            cmd.ExecuteNonQuery();

            cmd.Dispose();
            cnn.Close();

            return Ok("Player deleted");
        }

        private bool PlayerExists(int id)
        {
            return _context.LobbyItems.Any(e => e.id == id);
        }
    }
}
