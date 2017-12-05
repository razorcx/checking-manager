using System;
using System.Collections.Generic;
using CheckingManager;
using Tekla.Structures.Model;

namespace CheckingManager
{
	public class ConnectionCheckResult : ConnectionCheckBase
	{
		public Connection Connection { get; set; }
		public Beam PrimaryPart { get; set; }
		public List<Beam> SecondaryParts { get; set; }
		public List<Beam> ChildrenParts { get; set; }
		public DateTime? Date { get; set; }
	}
}