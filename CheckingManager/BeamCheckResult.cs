using System;
using Tekla.Structures.Model;

namespace CheckingManager
{
	public class BeamCheckResult : PartCheckBase
	{
		public Guid Guid { get; set; }
		public Beam Beam { get; set; }
		public DateTime? Date { get; set; }
	}
}