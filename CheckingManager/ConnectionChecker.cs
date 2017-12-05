using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Tekla.Structures.Model;

namespace CheckingManager
{
	public class ConnectionChecker
	{
		private readonly Model _model;

		public ConnectionChecker()
		{
			_model = new Model();
		}

		public List<ConnectionCheckResult> ConnectionCheckResults()
		{
			var connectionViews = GetConnectionCheckViews();

			return connectionViews.Select(c =>
				{
					var result = c.ConnectionCheckResults.LastOrDefault();
					return result ?? new ConnectionCheckResult();
				})
				.OrderByDescending(c => c.Date)
				.ToList();
		}

		public List<ConnectionCheckSummary> GetConnectionCheckViews()
		{
			var fileNames = GetConnectionFilenames();

			return fileNames.AsParallel().Select(f =>
				{
					var guid = f.Split('\\').LastOrDefault()?.Split('.').FirstOrDefault();
					var connectionCheckResults = ReadConnectionCheckHistory(f);

					return new ConnectionCheckSummary
					{
						GUID = guid,
						ConnectionCheckResults = connectionCheckResults
					};
				})
				.ToList();
		}

		private List<string> GetConnectionFilenames()
		{
			var folder = ConnectionFolderPath();
			return Directory.EnumerateFiles(folder).ToList();
		}

		private string ConnectionFolderPath()
		{
			string modelpath = _model.GetInfo().ModelPath;
			var connectionCheckFolder = @"RazorCX\ConnectionChecker";
			return modelpath + @"\" + connectionCheckFolder;
		}

		private List<ConnectionCheckResult> ReadConnectionCheckHistory(string filepath)
		{
			var checkConnections = new List<ConnectionCheckResult>();

			try
			{
				var file = string.Empty;

				using (var reader = new StreamReader(filepath))
					file = reader.ReadToEnd();

				if (!string.IsNullOrEmpty(file))
					checkConnections =
						JsonConvert.DeserializeObject<List<ConnectionCheckResult>>(file);

				return checkConnections;
			}
			catch (Exception ex)
			{
				return checkConnections;
			}
		}

		public List<ConnectionCheckView> GetConnectionCheckView(List<ConnectionCheckResult> result)
		{
			return
				result
					.Select(c => new ConnectionCheckView
					{
						Id = c?.Id,
						Phase = c?.Phase,
						Name = c?.Name,
						Number = c?.Number,
						DetailedBy = c?.DetailedBy,
						DetailedDate = c?.DetailedDate,
						DetailedComments = c?.DetailedComments,
						DesignedBy = c?.DesignedBy,
						DesignedDate = c?.DesignedDate,
						DesignedComments = c?.DesignedComments,
						CheckedBy = c?.CheckedBy,
						CheckedDate = c?.CheckedDate,
						CheckedComments = c?.CheckedComments,
						ApprovedBy = c?.ApprovedBy,
						ApprovedDate = c?.ApprovedDate,
						ApprovedComments = c?.ApprovedComments,
						Date = c?.Date
					}).ToList();
		}
	}
}