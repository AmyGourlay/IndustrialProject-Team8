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

            //create list of player objects
            List<Player> players = new List<Player>();
            Player tmp;

            while(data.Read())
            {
                tmp = new Player();
                tmp.id = data.GetInt32(0);
                tmp.name = data.GetString(1);
                tmp.score = data.GetInt32(2);
                tmp.lobbyId = data.GetInt32(3);
                tmp.questionIndex = data.GetInt32(4);
                tmp.lifeline5050 = data.GetBoolean(5);
                tmp.lifelineSkip = data.GetBoolean(6);

                players.Add(tmp);
            }

            data.Close();
            cmd.Dispose();
            cnn.Close();

            return players;
        }

        // GET: api/Players/getinfo
        // READ
        [HttpGet("getinfo")]
        public async Task<ActionResult<Player>> GetPlayer(Player curPlayer)
        {
            string connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8;Password=b7zYDzhJ;";
            SqlConnection cnn = new SqlConnection(connetionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM player WHERE playerName = '" + curPlayer.name + "' AND lobbyId = " + curPlayer.lobbyId, cnn);
            //SqlCommand cmd = new SqlCommand("SELECT * FROM player WHERE playerName = '@name' AND lobbyId = @lobbyId", cnn);
            //cmd.Parameters.AddWithValue("@name", curPlayer.name);
            //cmd.Parameters.AddWithValue("@lobbyId", curPlayer.lobbyId);

            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader();

            if(data.HasRows == false)
            {
                data.Close();
                cmd.Dispose();
                cnn.Close();
                return NotFound(curPlayer.name + " " + curPlayer.lobbyId);
            }

            //create player list
            Player player = new Player();

            data.Read();
            player.id = data.GetInt32(0);
            player.name = data.GetString(1);
            player.score = data.GetInt32(2);
            player.lobbyId = data.GetInt32(3);
            player.questionIndex = data.GetInt32(4);
            player.lifeline5050 = data.GetBoolean(5);
            player.lifelineSkip = data.GetBoolean(6);

            data.Close();
            cmd.Dispose();
            cnn.Close();

            return player;
        }

        // GET: api/Players/inlobby/[id]
        // READ ALL FROM A LOBBY
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
                tmp.questionIndex = data.GetInt32(4);
                tmp.lifeline5050 = data.GetBoolean(5);
                tmp.lifelineSkip = data.GetBoolean(6);

                players.Add(tmp);
            }

            data.Close();
            cmd.Dispose();
            cnn.Close();

            return players;
        }

        // GET: api/Players/lifeline5050
        // READ LIFELINE5050
        [HttpGet("lifeline5050")]
        public async Task<ActionResult<bool>> GetPlayerLifeline5050(Player curPlayer)
        {
            string connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8;Password=b7zYDzhJ;";
            SqlConnection cnn = new SqlConnection(connetionString);
            SqlCommand cmd = new SqlCommand("SELECT lifeline5050 FROM player WHERE playerName = '" + curPlayer.name + "' AND lobbyId = " + curPlayer.lobbyId, cnn);

            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader();

            if(data.HasRows == false)
            {
                data.Close();
                cmd.Dispose();
                cnn.Close();
                return NotFound("Could not find any information regarding GetPlayerLifeline5050() with name " + curPlayer.name + " and lobbyID " + curPlayer.lobbyId);
            }

            data.Read();
            bool result = data.GetBoolean(0);

            data.Close();
            cmd.Dispose();
            cnn.Close();

            return result;
        }

        // GET: api/Players/lifelineSkip
        // READ LIFELINESKIP
        [HttpGet("lifelineSkip")]
        public async Task<ActionResult<bool>> GetPlayerLifelineSkip(Player curPlayer)
        {
            string connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8;Password=b7zYDzhJ;";
            SqlConnection cnn = new SqlConnection(connetionString);
            SqlCommand cmd = new SqlCommand("SELECT lifelineSkip FROM player WHERE playerName = '" + curPlayer.name + "' AND lobbyId = " + curPlayer.lobbyId, cnn);

            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader();

            if(data.HasRows == false)
            {
                data.Close();
                cmd.Dispose();
                cnn.Close();
                return NotFound("Could not find any information regarding GetPlayerLifelineSkip() with name " + curPlayer.name + "and lobbyID " + curPlayer.lobbyId);
            }

            data.Read();
            bool result = data.GetBoolean(0);

            data.Close();
            cmd.Dispose();
            cnn.Close();

            return result;
        }



        ////**** PUT ****////
        // PUT: api/Players
        // UPDATE
        [HttpPut]
        public async Task<IActionResult> PutPlayer(Player curPlayer)
        {
            string connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8;Password=b7zYDzhJ;";
            SqlConnection cnn = new SqlConnection(connetionString);
            SqlCommand cmd = new SqlCommand("SELECT id FROM player WHERE playerName = '" + curPlayer.name + "' AND lobbyId = " + curPlayer.lobbyId, cnn);

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
                                            "SET playerName = '" + curPlayer.name +
                                            "', score = " + curPlayer.score +
                                            ", lobbyId = " + curPlayer.lobbyId +
                                            ", questionIndex = " + curPlayer.questionIndex +
                                            ", lifeline5050 = " + curPlayer.lifeline5050 +
                                            ", lifelineSkip = " + curPlayer.lifelineSkip +
                                            " WHERE playerName = '" + curPlayer.name +
                                            "' AND lobbyId = " + curPlayer.lobbyId, cnn);

            cmd.ExecuteNonQuery();
            cmd.Dispose();
            cnn.Close();

            return NoContent();
        }

        // PUT: api/Players/lifelines
        // UPDATE LIFELINES
        [HttpPut("lifelines")]
        public async Task<IActionResult> PutLifeline(Player curPlayer)
        {
            //Check of the record exists
            string connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8;Password=b7zYDzhJ;";
            SqlConnection cnn = new SqlConnection(connetionString);
            SqlCommand cmd = new SqlCommand("SELECT id FROM player WHERE playerName = '" + curPlayer.name + "' AND lobbyId = " + curPlayer.lobbyId, cnn);

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

            //Update the lifeline
            connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8 ;Password=b7zYDzhJ;";
            cmd = new SqlCommand("UPDATE player + SET lifeline5050 = '" + curPlayer.lifeline5050.ToString() +
                                            "', lifelineSkip = '" + curPlayer.lifelineSkip.ToString() +
                                            "WHERE playerName = '" + curPlayer.name + "' AND lobbyId = " + curPlayer.lobbyId, cnn);

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
            SqlCommand cmd = new SqlCommand("INSERT INTO player(playerName, score, lobbyId, questionIndex, lifeline5050, lifelineSkip) " +
                                            "VALUES('" + player.name + "', "  + player.score + ", " + player.lobbyId + ", " + player.questionIndex + ", '" + player.lifeline5050.ToString() + "', '" + player.lifelineSkip.ToString() + "');", cnn);

            cnn.Open();
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            cmd = new SqlCommand("SELECT id FROM player WHERE playerName = '" + player.name + "' AND lobbyId = " + player.lobbyId + ";", cnn);

            SqlDataReader data = cmd.ExecuteReader();
            data.Read();
            int id = data.GetInt32(0);

            data.Close();
            cmd.Dispose();
            cnn.Close();

            return Ok(id);
        }



        ////**** DELETE ****////
        // DELETE: api/Players
        // DELETE
        [HttpDelete]
        public async Task<ActionResult<Player>> DeletePlayer(Player curPlayer)
        {
            string connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8;Password=b7zYDzhJ;";
            SqlConnection cnn = new SqlConnection(connetionString);
            SqlCommand cmd = new SqlCommand("SELECT id FROM player WHERE playerName = '" + curPlayer.name + "' AND lobbyId = " + curPlayer.lobbyId, cnn);

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

            cmd = new SqlCommand("DELETE FROM player WHERE playerName = '" + curPlayer.name + "' AND lobbyId = " + curPlayer.lobbyId, cnn);
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