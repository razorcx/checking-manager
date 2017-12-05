using System.Collections.Generic;

namespace CheckingManager
{
	public class BeamCheckSummary
	{
		public string GUID { get; set; }
		public List<BeamCheckResult> BeamCheckResults { get; set; }
	}
}