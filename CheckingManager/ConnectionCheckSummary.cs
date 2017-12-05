using System.Collections.Generic;

namespace CheckingManager
{
	public class ConnectionCheckSummary
	{
		public string GUID { get; set; }
		public List<ConnectionCheckResult> ConnectionCheckResults { get; set; }
	}
}