using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using StackExchange.Redis;
using Tekla.Structures;
using Tekla.Structures.Model;
using Tekla.Structures.Model.UI;

namespace CheckingManager
{
	public partial class CheckingManagerForm : Form
	{
		private readonly Model _model;
		private readonly Picker _picker;
		private readonly ConnectionMultiplexer _redis;
		private List<BeamCheckResult> _beamCheckResults;


		public CheckingManagerForm()
		{
			try
			{
				_redis = ConnectionMultiplexer.Connect("localhost");

				InitializeComponent();

				_model = new Model();
				_picker = new Picker();

				if (!_model.GetConnectionStatus()) return;

				RefreshConnections();
				RefreshBeams();

				RefreshDatabase();
			}
			catch (Exception ex)
			{
				Trace.WriteLine(ex.Message + ex.InnerException + ex.StackTrace);
			}
		}

		private void RefreshDatabase()
		{
			var db = _redis.GetDatabase();

			var connectionCheckViews = GetConnectionCheckViews();

			connectionCheckViews.ForEach(c =>
			{
				db.StringSetAsync(c.GUID, JsonConvert.SerializeObject(c, Formatting.Indented));
			});
		}

		private void RefreshConnections()
		{
			ModelObjectEnumerator.AutoFetch = true;
			var connections = _model.GetModelObjectSelector()
				.GetAllObjectsWithType(ModelObject.ModelObjectEnum.CONNECTION)
				.ToList();

			var connectionCheckResults = ConnectionCheckResults();

			var summary = connections.Select(c => new
				{
					Connection = c,
					ConnectionCheckPlugin =
						connectionCheckResults.FirstOrDefault(cc => cc?.Connection?.Identifier?.GUID == c?.Identifier?.GUID)
				})
				.ToList();

			textBoxWithPluginCount.Text = summary.Count(c => c.ConnectionCheckPlugin != null).ToString();
			textBoxWithoutPluginCount.Text = summary.Count(c => c.ConnectionCheckPlugin == null).ToString();

			var results = summary
				.Where(c => c.ConnectionCheckPlugin != null)
				.Select(c => c.ConnectionCheckPlugin)
				.OrderByDescending(c => c.Date)
				.ToList();

			this.dataGridViewConnectionCheckingSummary.DataSource 
				= GetConnectionCheckView(results);
		}

		private void RefreshBeams()
		{
			_beamCheckResults = BeamCheckResults();
			var beamCheckViews = JsonConvert.DeserializeObject<List<BeamCheckView>>(JsonConvert.SerializeObject(_beamCheckResults))
				.OrderByDescending(b => b.Date)
				.ToList();

			dataGridViewObjectCheckingSummary.DataSource = beamCheckViews;
		}

		private List<ConnectionCheckResult> ConnectionCheckResults()
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

		private List<ConnectionCheckSummary> GetConnectionCheckViews()
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

		private List<string> GetBeamFilenames()
		{
			var folder = BeamFolderPath();
			return Directory.EnumerateFiles(folder).ToList();
		}

		private string ConnectionFolderPath()
		{
			string modelpath = _model.GetInfo().ModelPath;
			var connectionCheckFolder = @"RazorCX\ConnectionChecker";
			return modelpath + @"\" + connectionCheckFolder;
		}

		private string BeamFolderPath()
		{
			string modelpath = _model.GetInfo().ModelPath;
			var checkFolder = @"RazorCX\BeamChecker";
			return modelpath + @"\" + checkFolder;
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

		private List<ConnectionCheckView> GetConnectionCheckView(List<ConnectionCheckResult> result)
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

		private void btnRefreshConnections_Click(object sender, EventArgs e)
		{
			RefreshConnections();
		}

		private void btnRefreshBeams_Click(object sender, EventArgs e)
		{
			RefreshBeams();
		}


		private void btnAddObject_Click(object sender, EventArgs e)
		{
			try
			{
				var modelObject = _picker.PickObject(Picker.PickObjectEnum.PICK_ONE_OBJECT);
				var checkResult = GetBeamCheck(modelObject);

				var beamCheckResults = new List<BeamCheckResult> {checkResult};

				var json = JsonConvert.SerializeObject(beamCheckResults, Formatting.Indented);

				WriteCheckHistory(json, BeamFilepath(modelObject));

				RefreshBeams();
			}
			catch(Exception ex)
			{
				
			}
		}

		private string BeamFilepath(ModelObject obj)
		{
			string modelpath = _model.GetInfo().ModelPath;
			var checkFolder = @"RazorCX\BeamChecker";
			var checkFile = $"{obj.Identifier.GUID}.json";
			var path = modelpath + @"\" + checkFolder;

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			return path + @"\" + checkFile;
		}

		private void WriteCheckHistory(string json, string filepath)
		{
			using (var writer = new StreamWriter(filepath))
				writer.Write(json);
		}

		private BeamCheckResult GetBeamCheck(ModelObject obj)
		{
			obj.GetPhase(out Phase phase);

			var beam = obj as Beam;
			if (beam == null) return null;

			return new BeamCheckResult()
			{
				Id = obj.Identifier.ID,
				Guid = obj.Identifier.GUID,
				Beam = beam,
				Phase = phase.PhaseNumber,
				Name = beam.Name,
				Profile = beam.Profile.ProfileString,
				Material = beam.Material.MaterialString,
				Date = DateTime.Now,
			};
		}

		private List<BeamCheckResult> ReadBeamCheckHistory(string filepath)
		{
			var beamChecks = new List<BeamCheckResult>();

			try
			{
				var file = string.Empty;

				using (var reader = new StreamReader(filepath))
					file = reader.ReadToEnd();

				if (!string.IsNullOrEmpty(file))
					beamChecks =
						JsonConvert.DeserializeObject<List<BeamCheckResult>>(file);

				return beamChecks;
			}
			catch (Exception ex)
			{
				return beamChecks;
			}
		}

		private List<BeamCheckSummary> GetBeamCheckSummarys()
		{
			var fileNames = GetBeamFilenames();

			return fileNames.AsParallel().Select(f =>
				{
					var guid = f.Split('\\').LastOrDefault()?.Split('.').FirstOrDefault();
					var beamCheckResults = ReadBeamCheckHistory(f);

					return new BeamCheckSummary
					{
						GUID = guid,
						BeamCheckResults = beamCheckResults
					};
				})
				.ToList();
		}

		private List<BeamCheckResult> BeamCheckResults()
		{
			var beamViews = GetBeamCheckSummarys();

			return beamViews
				//.Where(b => b.BeamCheckResults.Any())
				.Select(c =>
				{
					var result = c.BeamCheckResults.LastOrDefault();
					return result ?? new BeamCheckResult();
				})
				.OrderByDescending(c => c.Date)
				.ToList();
		}

		private void dataGridMembers_RowValidated(object sender, DataGridViewCellEventArgs e)
		{
			var dataGridView = sender as DataGridView;
			if (dataGridView == null) return;

			try
			{
				var row = dataGridView.CurrentRow;
				if (row == null) return;

				var beamCheckView = (BeamCheckView) row.DataBoundItem;
				var beamCheckResult = _beamCheckResults
					.FirstOrDefault(b => b.Id == beamCheckView.Id);

				var newResult = JsonConvert.DeserializeObject<BeamCheckResult>(JsonConvert.SerializeObject(beamCheckView));
				newResult.Beam = beamCheckResult?.Beam;
				newResult.Guid = beamCheckResult.Guid;

				//write json to file
				string modelpath = _model.GetInfo().ModelPath;
				var checkFolder = @"RazorCX\BeamChecker";

				var filepath = $"{modelpath}\\{checkFolder}\\{beamCheckResult?.Guid}.json";

				var file = string.Empty;

				try
				{
					using (var reader = new StreamReader(filepath))
						file = reader.ReadToEnd();
				}
				catch (Exception ex)
				{

				}

				List<BeamCheckResult> beamCheckResults = new List<BeamCheckResult>();
				try
				{
					if (!string.IsNullOrEmpty(file))
						beamCheckResults =
							JsonConvert.DeserializeObject<List<BeamCheckResult>>(file);
				}
				catch (Exception ex)
				{

				}

				if (beamCheckResults == null) beamCheckResults = new List<BeamCheckResult>();
				beamCheckResults.Add(newResult);

				var json = JsonConvert.SerializeObject(beamCheckResults, Formatting.Indented);

				using (var writer = new StreamWriter(filepath))
					writer.Write(json);
			}
			catch (Exception ex)
			{

			}
		}

		private void dataGridViewConnectionCheckingSummary_SelectionChanged(object sender, EventArgs e)
		{
			var selectedRows = GetSelectedRows(sender);
			var ids = GetIds(selectedRows);
			SelectModelObjectsInUi(ids);
		}

		private List<DataGridViewRow> GetSelectedRows(object sender)
		{
			var dataGrid = sender as DataGridView;
			return dataGrid?.SelectedRows.OfType<DataGridViewRow>().ToList();
		}

		private List<int> GetIds(List<DataGridViewRow> rows)
		{
			return rows
				.Select(r => (int)((dynamic)r.DataBoundItem).Id)
				.ToList();
		}

		private void SelectModelObjectsInUi(List<int> ids)
		{
			var modelObjects = new ArrayList();

			ids.ForEach(id =>
			{
				var modelObject = _model.SelectModelObject(new Identifier(id));
				if (modelObject == null) return;
				modelObjects.Add(modelObject);
			});

			var selector = new Tekla.Structures.Model.UI.ModelObjectSelector();
			selector.Select(modelObjects);
		}
	}
}
