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
		private readonly ConnectionMultiplexer _redis;
		private readonly ConnectionChecker _connectionChecker;
		private readonly BeamChecker _beamChecker;


		public CheckingManagerForm()
		{
			try
			{
				_connectionChecker = new ConnectionChecker();
				_beamChecker = new BeamChecker();

				_redis = ConnectionMultiplexer.Connect("localhost");

				InitializeComponent();

				_model = new Model();

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

			var connectionCheckViews = _connectionChecker.GetConnectionCheckViews();

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

			var connectionCheckResults = _connectionChecker.ConnectionCheckResults();

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
				= _connectionChecker.GetConnectionCheckView(results);
		}

		private void RefreshBeams()
		{
			var beamCheckResults = _beamChecker.BeamCheckResults();
			var beamCheckViews = JsonConvert.DeserializeObject<List<BeamCheckView>>(JsonConvert.SerializeObject(beamCheckResults))
				.OrderByDescending(b => b.Date)
				.ToList();

			dataGridViewObjectCheckingSummary.DataSource = beamCheckViews;
		}

		//common methods

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


		//ui events

		private void dataGridViewConnectionCheckingSummary_SelectionChanged(object sender, EventArgs e)
		{
			var selectedRows = GetSelectedRows(sender);
			var ids = GetIds(selectedRows);
			SelectModelObjectsInUi(ids);
		}

		private void dataGridBeams_RowValidated(object sender, DataGridViewCellEventArgs e)
		{
			var dataGridView = sender as DataGridView;
			if (dataGridView == null) return;

			_beamChecker.UpdateJsonFile(dataGridView);
		}

		private void btnRefreshConnections_Click(object sender, EventArgs e)
		{
			RefreshConnections();
		}

		private void btnRefreshBeams_Click(object sender, EventArgs e)
		{
			RefreshBeams();
		}

		private void btnAddBeam_Click(object sender, EventArgs e)
		{
			_beamChecker.AddBeam();
			RefreshBeams();
		}
	}
}
