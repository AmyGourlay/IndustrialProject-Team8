using Microsoft.EntityFrameworkCore;

namespace QuizAPI.Models
{
	public class PlayerContext : DbContext
	{
		public PlayerContext()
		{
		}

		public PlayerContext(DbContextOptions<PlayerContext> options)
			: base(options)
		{
		}

		public DbSet<Player> PlayerItems { get; set; }
	}
}
