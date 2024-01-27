using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

// TODO: finish, cleanup
namespace Bobsled.Editors
{
public static class Bobsled
{
	private static readonly string API_KEY = "keyleKY9WM7FrM6sW";
	// private static readonly string BASE = "appV18GoKPS2UW4TC";
	// private static readonly string TABLE = "tblip402eQkrS29s1";

	public static async Task Pull()
	{
		Log("starting pull");
		// await Task.Delay(10000);

		var client = new HttpClient();
		client.DefaultRequestHeaders.Authorization =
			new AuthenticationHeaderValue("Bearer", API_KEY);

		var uri = "https://api.airtable.com/v0/appV18GoKPS2UW4TC/tblip402eQkrS29s1?";
		var request = new HttpRequestMessage(HttpMethod.Get, uri);

		var response =
			await client.SendAsync(request);

		client.Dispose();

		var body =
			await response.Content.ReadAsStringAsync();


		Log("pull complete!");
		Debug.Log($"bobsled: pull result \n{body}");
	}


	static void Log(string log)
	{
		Debug.Log($"bobsled:  {log}");
	}


	public static void DrawEditor(VisualElement root)
	{
		var initButton = new Button {
			text = "Init Definitions",
			style = {
				height = 50,
			},
			tooltip =
				$"All Definitions must be in the same directory (or child directory) as the DefinitionLibrary",
		};

		initButton.clickable.clicked += RunInitInEditor;

		_resultLabel = new Label {
			text = "",
		};

		root.Add(initButton);
		root.Add(_resultLabel);
	}


	static Label _resultLabel;

	static void RunInitInEditor()
	{
		_resultLabel.text = "jk this isn't needed anymore";
		// Initialize(log => _resultLabel.text += "\n" + log);
	}

	// public static DefinitionLibrary Initialize(Action<string> logger = null) {
	// 	logger ??= _ => { };
	//
	// 	logger("Initializing");
	//
	// 	var libraryPaths = BobsledUtils.FindAssetPaths<DefinitionLibrary>();
	//
	// 	if (libraryPaths.Length != 1) {
	// 		logger($"Missing exactly 1 ${nameof(DefinitionLibrary)}");
	// 		Debug.LogError($"Bobsled requires exactly 1 {nameof(DefinitionLibrary)}");
	// 		return null;
	// 	}
	//
	// 	var libraryPath = libraryPaths[0];
	// 	var library = BobsledUtils.LoadFromPath<DefinitionLibrary>(libraryPath);
	//
	// 	var directory = Regex.Match(libraryPath, @".*(?=\/)").Value;
	// 	logger($"Loading from {directory}");
	//
	// 	var defsTotal = 0;
	// 	var defTypeToDefs = new Dictionary<Type, List<Definition>>();
	// 	var defAssets = BobsledUtils.FindAssets<Definition>(directory);
	//
	// 	foreach (var def in defAssets) {
	// 		var defType = def.GetDefType();
	//
	// 		if (!defTypeToDefs.TryGetValue(defType, out var typeDefs)) {
	// 			typeDefs = new List<Definition>();
	// 			defTypeToDefs[defType] = typeDefs;
	// 		}
	//
	// 		typeDefs.Add(def);
	// 		defsTotal++;
	// 	}
	// 	
	// 	var collectionAssets = BobsledUtils
	// 		.FindAssets<DefinitionCollection>(directory)
	// 		.OrderBy(col => col.name);
	//
	//
	// 	var collections = new List<DefinitionCollection>();
	// 	collections.Add(null); // 0 id is null
	// 	collections.AddRange(collectionAssets.OrderBy(col => col.name));
	//
	// 	// 0 id is null, so start at 1
	// 	for (var collectionId = 1; collectionId < collections.Count; collectionId++) {
	// 		var collection = collections[collectionId];
	// 		
	// 		collection.CollectionId = collectionId;
	// 		collection.NameToDef = new Dictionary<string, Definition>();
	//
	// 		if (collection.BLANK == null) {
	// 			logger($"Invalid collection {collection.name}: missing BLANK");
	// 			Debug.LogWarning($"Invalid collection {collection.name}: missing BLANK");
	// 			continue;
	// 		}
	//
	// 		collection.BLANK.IsBlank = true;
	// 		var defType = collection.GetDefType();
	// 		var hasDefs = defTypeToDefs.TryGetValue(defType, out var defs);
	//
	// 		if (!hasDefs || defs.Count <= 1) {
	// 			logger($"Collection {collectionId}: {collection.name} has no definitions");
	// 			Debug.LogWarning($"Collection {collectionId}: {collection.name} has no definitions");
	// 			continue;
	// 		}
	// 		
	// 		collection.PreProcessCollection();
	//
	// 		var collDefs = new List<Definition>();
	// 		collDefs.AddRange(defs.OrderBy(def => def.name));
	// 		collDefs.Remove(collection.BLANK);
	// 		collDefs.Insert(0, collection.BLANK);// 0 id is BLANK definition
	//
	// 		var definitionId = 0;
	// 		foreach (var def in collDefs) {
	// 			def.CollectionId = collectionId;
	// 			def.DefinitionId = definitionId++;
	// 			collection.NameToDef[def.name.ToLower()] = def;
	// 			collection.PostProcessDef(def);
	// 			EditorUtility.SetDirty(def);
	// 		}
	// 		
	// 		collection.Defs = collDefs.ToArray();
	// 		collection.PostProcessCollection();
	// 		EditorUtility.SetDirty(collection);
	// 		logger($"Collection {collectionId}: {collection.name}, with {collDefs.Count - 1} {defType.Name}s");
	// 	}
	//
	// 	// TODO: AirTable stuff
	// 	// TODO: take ID from AirTable
	// 	library.Collections = collections.ToArray();
	// 	EditorUtility.SetDirty(library);
	// 	logger($"Library Initialized: {collections.Count - 1} collections, {defsTotal} defs");
	//
	// 	return library;
	// }
}
}


// TODO: reimplement Airtable stuff:

// private Label loadingLabel;
//
// private void OnEnable() {
// 	var root = rootVisualElement;
// 	
// 	var pullButton = new Button {
// 		text = "Pull",
// 		style = {
// 			height = 50,
// 		}
// 	};
//
// 	pullButton.clickable.clicked += Pull;
//
// 	loadingLabel = new Label {
// 		text = "",
// 	};
// 	
// 	root.Add(pullButton);
// 	root.Add(loadingLabel);
// }
//
//
// async void Pull() {
// 	loadingLabel.text = "pulling...";
// 	await Bobsled.Pull();
// 	loadingLabel.text = "Done";
// }