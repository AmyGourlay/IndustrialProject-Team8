using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizAPI.Models
{
	public class Player
	{
		public int id { get; set; }
		public string name { get; set; }
		public int score { get; set; }
		public int lobbyId { get; set; }
		public int questionIndex { get; set; }
	}
}
