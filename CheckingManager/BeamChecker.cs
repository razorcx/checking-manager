using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using Tekla.Structures.Model;
using Tekla.Structures.Model.UI;

namespace CheckingManager
{
	public class BeamChecker
	{
		private readonly Model _model;
		private readonly Picker _picker;

		public BeamChecker()
		{
			_model = new Model();
			_picker = new Picker();
		}

		public void AddBeam()
		{
			try
			{
				var modelObject = _picker.PickObject(Picker.PickObjectEnum.PICK_ONE_OBJECT);
				var checkResult = GetBeamCheck(modelObject);

				var beamCheckResults = new List<BeamCheckResult> { checkResult };

				var json = JsonConvert.SerializeObject(beamCheckResults, Formatting.Indented);

				WriteCheckHistory(json, BeamFilepath(modelObject));
			}
			catch (Exception ex)
			{

			}
		}

		private List<string> GetBeamFilenames()
		{
			var folder = BeamFolderPath();
			return Directory.EnumerateFiles(folder).ToList();
		}

		private string BeamFolderPath()
		{
			string modelpath = _model.GetInfo().ModelPath;
			var checkFolder = @"RazorCX\BeamChecker";
			return modelpath + @"\" + checkFolder;
		}

		public string BeamFilepath(ModelObject obj)
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

		public BeamCheckResult GetBeamCheck(ModelObject obj)
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

		private List<BeamCheckSummary> GetBeamCheckSummaries()
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

		public List<BeamCheckResult> BeamCheckResults()
		{
			var beamViews = GetBeamCheckSummaries();

			return beamViews
				.Select(c =>
				{
					var result = c.BeamCheckResults.LastOrDefault();
					return result ?? new BeamCheckResult();
				})
				.OrderByDescending(c => c.Date)
				.ToList();
		}

		private void WriteCheckHistory(string json, string filepath)
		{
			using (var writer = new StreamWriter(filepath))
				writer.Write(json);
		}

		public void UpdateJsonFile(DataGridView dataGridView)
		{
			try
			{
				var row = dataGridView.CurrentRow;
				if (row == null) return;


				var beamCheckView = (BeamCheckView)row.DataBoundItem;
				var beamCheckResult = BeamCheckResults()
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
	}
}