using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizAPI.Models
{
	public class Lobby
	{
		public int id { get; set; }
		public string easyToken { get; set; }
		public string mediumToken { get; set; }
		public string hardToken { get; set; }
		public string requestURL { get; set; }
	}
}