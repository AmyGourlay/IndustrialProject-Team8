using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;

namespace QuizAPI.Models
{
	public class Lobby
	{
		public int id { get; set; }
		public string easyQuestions { get; set; }
		public string mediumQuestions { get; set; }
		public string hardQuestions { get; set; }
		public string requestURL { get; set; }
		public string date { get; set; }
	}
}