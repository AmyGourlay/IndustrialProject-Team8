using Microsoft.EntityFrameworkCore;

namespace QuizAPI.Models
{
	public class LobbyContext : DbContext
	{
		public LobbyContext()
		{
		}

		public LobbyContext(DbContextOptions<LobbyContext> options)
			: base(options)
		{
		}

		public DbSet<Lobby> LobbyItems { get; set; }
	}
}
