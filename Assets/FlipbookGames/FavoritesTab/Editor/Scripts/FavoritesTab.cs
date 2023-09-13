﻿/* Favorites Tab[s] for Unity
 * version 1.3.1, September 2023
 * Copyright © 2012-2023, by Flipbook Games
 * 
 * Your personalized list of favorite assets and scene objects
 * 
 * Version 1.3.1:
 * - Fixed color of stars changing by itself on reloading script assemblies - thanks @stylophone and @sampenguin
 * - Fixed item drag starting too soon in a favorites tab when using a hi-res pointing device - thanks @ismailkoroglu
 *
 * Version 1.3.0:
 * - Works in multi-scene editing.
 * - Significant performance optimizations.
 * - Several minor bug fixes.
 * - Updated for compatibility with the latest Unity versions.
 *
 * Version 1.2.20:
 * - Updated for combatibility with latest Unity versions
 * 
 * Version 1.2.19:
 * - New option to filter by star color
 * - New option to show stars in favorites tabs
 * - Fixed NRE when no Favorites tab is open
 * 
 * Version 1.2.18:
 * - Upgraded for compatibility with Unity 5.4
 * 
 * Version 1.2.17:
 * - Upgraded for compatibility with Unity 4.7 and Unity 5.3
 * 
 * Version 1.2.16:
 * - New option to show bookmark ribbon icons instead of stars
 * 
 * Version 1.2.15:
 * - New option to fix positions of other icons in the Hierarchy view
 * - New sorting option - Most recently favorited on top
 * - New - Add to Favorites & select star color by right-clicking a hollow star
 * - Bug fixes
 * 
 * Version 1.2.14:
 * - A quick update for Unity 5.1
 * 
 * Version 1.2.13:
 * - Selected favorite folder will automatically expand in a single-column Project view
 * 
 * Version 1.2.12:
 * - Fixed: Delete and Backspace in the search field were executing "Remove from Favorites"
 * - Fixed: Selecting another favorite item with keyboard after changing filters
 * - Added Enter/Return key moves focus from search box to favorites list
 * 
 * Version 1.2.11:
 * - Added "Show in New Inspector" context menu on items in Project View
 * - Fixed "Show in New Inspector" right after favoriting a scene object (thanks to Yann Papouin for finding the issue)
 * - Fixed not showing hollow stars sometimes
 * - Fixed rare NullReferenceException when accessing mouseOverWindow
 * - Fixed keyboard shortcut shown on "Remove from Favorites" context menu
 * 
 * Version 1.2.10:
 * - Added option to show assets' locations
 * - Added "Show in New Inspector" feature
 * - Delete and Backspace keys now remove selected items from favorites list
 * 
 * Version 1.2.9:
 * - Compatible with Hierarchy2 v1.3
 * 
 * Version 1.2.8:
 * - Compatible with Hierarchy2 (thanks to Jesse Werner's idea)
 * 
 * Version 1.2.7:
 * - Fixed showing content of favorite folders in Unity 4.3 (thanks to Maurizio for discovering the issue)
 * 
 * Version 1.2.6:
 * - Fixed (again) performance issue with no FG_GameObjectGUIDs (re-introduced with v1.2.5).
 * 
 * Version 1.2.5:
 * - Fixed lost references to scene objects after entering game mode on modified scenes (thanks to Jimww).
 * - Fixed stars blinking in Hierarchy view when entering game mode with a game object selected (thanks to Jimww again).
 * 
 * Version 1.2.4:
 * - Fixed positioning of Antares Universe icons (thanks to Nezabyte).
 * 
 * Version 1.2.3:
 * - Shows the content of bookmarked folders in the second column of Project view in Unity 4.
 * 
 * Version 1.2.2:
 * - Fixed performance issues in scenes with no FG_GameObjectGUIDs (thanks to Jim Young).
 * 
 * Version 1.2.1:
 * - Fixed title initialization on Favorites tabs hidden behind another tab.
 * 
 * New Features in 1.2:
 * - Support for showing multiple favorites tabs, each with different filtering to show diffent sets of favorites.
 * - Filtering and search setting for each favorites tab are persistent between Unity sessions.
 * - Many new filtering options added to filter by asset type.
 * - Favorites tabs filtered by type show the selected type in the title.
 * - Star icons for each favorite item can optionally be set to colors other than the default yellow star, independently for each user.
 * - FG_GameObjectGUIDs game object gets created only when its needed and can optionally be deleted if user wants that.
 * - A custom inspector appears on FG_GameObjectGUIDs, explaining the function of this game object.
 * - Added context menu items on favorite assets to reimport them and to show them in Explorer (reveal in Finder on Mac).
 * - Editor/Resources folder renamed to Editor/Textures to avoid inclusion of those in final builds.
 * - Unity 4 support.
 * 
 * New in version 1.1:
 * - Multiple selected favorite items can be removed from the Favorites Tab at once.
 * - FG_GameObjectGUIDs game object is not hidden anymore.
 *
 * Features:
 * - Native look and feel, very similar to Project and Hierarchy views!
 * - No learning required! Just use your common Unity Editor knowledge and see it working as you would expect.
 * - Easy to mark or unmark favorite assets and scene object with just a single mouse-click.
 * - Easy to spot your favorite assets or scene objects in the Project and Hierarchy window, even when the Favorites Tab is closed!
 * - Favorites Tab displays all favorites sorted by name or type.
 * - Search by name functionality.
 * - Filters to show only assets or scene objects.
 * - Keyboard and mouse are fully supported.
 * - Selection synchronization. Select an item in the Favorites Tab to easily find it in the Hierarchy or Project views.
 * - Multiple favorite items selections.
 * - Dragging items from the Favorites Tab to any other Unity view is fully supported.
 * - Double click or press F key (or use context menu) to Frame the selected scene object in the Scene View, same as from the Hierarchy view.
 * - Double click or press Enter key (or use context menu) to open the selected asset, same as from the Project view.
 * - Works with teams! All team members have their own list of favorites even if they share the same project!
 * - GUID based asset references, so that assets exported and imported into another project remain in your list of favorites.
 * - Full source code provided for your reference or modification! :-)
 * 
 * Follow @FlipbookGames on http://twitter.com/FlipbookGames
 * Like Flipbook Games on Facebook http://facebook.com/FlipbookGames
 * Join Unity forum discusion http://forum.unity3d.com/threads/149856
 * Contact info@flipbookgames.com for feedback, bug reports, or suggestions.
 * Visit http://flipbookgames.com/ for more info.
 */

namespace FavoritesTabs
{

using UnityEngine;
using UnityEditor;
using System.Text;
using System.Reflection;
using System;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;


[InitializeOnLoad]
public class FavoritesTab : EditorWindow
{
	public static string GetVersionString()
	{
		return "1.3.1, September 2023";
	}

	private static Favorites favorites;

	[SerializeField]
	private Vector2 scrollPosition = new Vector2();
	private Rect scrollViewRect;
	private float contentHeight;
	
	[SerializeField]
	private bool showPaths = false;
	private float itemsHeight { get { return showPaths ? 16f + 11f : 16f; } }
	
	[SerializeField]
	private bool showStarsInFavTabs = false;

	private ListViewItem[] listViewItems = new ListViewItem[0];
	private bool recreateListViewItems = true;
	[SerializeField]
	private int focusedItemIndex = -1;
	[SerializeField]
	private int anchorItemIndex = -1;
	private int itemToSelect = -1;
	private int itemMouseDown = -1;
	private Vector2 mouseDownPosition = new Vector2();
	private int draggedItem = -1;
	private GenericMenu itemPopupMenu = null;
	private static bool paintingFavoritesTab = false;
	private static bool paintFavsIcons = false;

	[SerializeField]
	private string searchString = "";
	[SerializeField]
	private int searchMode = 0;
	[SerializeField]
	private int starColorFilter = -1;
	[SerializeField]
	private bool sortByType = true;
	[SerializeField]
	private bool sortByName = false;
	private static GUIContent[] searchModesMenuItems = null;
	private static GUIContent[] SearchModesMenuItems {
		get {
			if (searchModesMenuItems == null)
			{
				searchModesMenuItems = new GUIContent[searchModes.Length];
				for (int i = 0; i < searchModes.Length; ++i)
				{
					int modeIndex = searchModesOrder[i];
					searchModesMenuItems[i] = new GUIContent(searchModes[modeIndex]);
				}
			}
			return searchModesMenuItems;
		}
	}

	private bool focusSearchBox = false;
	private bool hasSearchBoxFocus = false;
	private bool focusListView = true;

	private static bool initialized = false;

	private static Texture2D gameObjectIcon = null;
	private static Texture2D windowIcon = null;
	private static Texture2D emptyStar = null;
	private static Texture2D[] filledStars = null;
	private static Texture2D emptyRibbon = null;
	private static Texture2D[] filledRibbons = null;
	private static GUIContent[] starColorNames = new GUIContent[]
	{
		new GUIContent("Red"),
		new GUIContent("Orange"),
		new GUIContent("Yellow"),
		new GUIContent("Green"),
		new GUIContent("Cyan"),
		new GUIContent("Blue"),
		new GUIContent("Magenta"),
	};
	[NonSerialized]
	public static Texture2D createdByFlipbookGames = null;

	private static GUIStyle scrollViewStyle = null;
	private static GUIStyle lineStyle = null;
	private static GUIStyle labelStyle = null;
	private static GUIStyle toolbarSearchField = null;
	private static GUIStyle toolbarSearchFieldCancelButton = null;
	private static GUIStyle toolbarSearchFieldCancelButtonEmpty = null;

	private static int[] searchModesOrder = new int[] { 0, 1, 2, 3,
		4, //"Project Folders",
		5, //"Animations",
		6, //"Audio Assets",
		7, //"Custom Assets",
		16, //"Data Assets",
		8, //"Fonts",
		9, //"Materials",
		10, //"Models",
		11, //"Prefabs",
		12, //"Preference Assets",
		13, //"Scenes",
		14, //"Scripts",
		15, //"Shaders",
		18, //"Text Assets",
		19, //"Textures",
		17, //"UI",
		20, //"Video Assets",
		21, //"Data Assets"
	};
	
	private static string[] searchModes = new string[] { "All", "Scene Objects", "Assets", "",
		"Project Folders",
		"Animations",
		"Audio Assets",
		"Custom Assets",
		"Fonts",
		"Materials",
		"Models",
		"Prefabs",
		"Preference Assets",
		"Scenes",
		"Scripts",
		"Shaders",
		"Data Assets",
		"UI",
		"Text Assets",
		"Textures",
		"Video Assets",
		"Data Assets"
	};

	private static int[] filterTypeIds = { 100, 100, 100, 100,
		-1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

	private static string[] windowTitles = new string[] { "Favorites", "Fav. Objects", "Fav. Assets", "",
		"Fav. Folders",
		"Fav. Anims",
		"Fav. Audio",
		"Custom Favs",
		"Fav. Fonts",
		"Fav. Mat.",
		"Fav. Models",
		"Fav. Prefabs",
		"Fav. Prefs",
		"Fav. Scenes",
		"Fav. Scripts",
		"Fav. Shaders",
		"Fav. Data",
		"Fav. UI",
		"Fav. Texts",
		"Fav.Textures",
		"Fav. Videos",
		"Fav. XMLs"
	};

	private static Color[] lightSkinColors = new Color[] { Color.black, new Color(0f, 0.15f, 0.51f, 1f), new Color(0.25f, 0.05f, 0.05f, 1f), Color.black, Color.white, new Color(0.67f, 0.76f, 1f), new Color(1f, 0.71f, 0.71f, 1f), Color.white };
	private static Color[] darkSkinColors = new Color[] { new Color(0.705f, 0.705f, 0.705f, 1f), new Color(0.3f, 0.5f, 0.85f, 1f), new Color(0.7f, 0.4f, 0.4f, 1f), new Color(0.705f, 0.705f, 0.705f, 1f), Color.white, new Color(0.67f, 0.76f, 1f), new Color(1f, 0.71f, 0.71f, 1f), Color.white };

	private static bool _pushOtherIcons;
	private static bool PushOtherIcons {
		get {
			return _pushOtherIcons;
		}
		set {
			_pushOtherIcons = value;
			EditorPrefs.SetBool("FlipbookGames.FavoritesTab.PushOtherIcons", value);
		}
	}
	
	private static bool _showStars;
	private static bool ShowStars {
		get {
			return _showStars;
		}
		set {
			_showStars = value;
			EditorPrefs.SetBool("FlipbookGames.FavoritesTab.ShowStars", value);
		}
	}


	// A custom asset postprocessor to track deleted object so they can get removed from the list of favorites
	private class FavoritesTabAssetPostprocessor : AssetPostprocessor
	{
		static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
		{
			foreach (string path in deletedAssets)
				favorites.OnAssetDeleted(path);
		}
	}


	private class Favorites
	{
		public static int addOrderCounter = 0;

		public class Entry : IComparable<Entry>, IEquatable<Entry>
		{
			public string guid;
			public int starColor = 2;
			public int addOrder;

			public override int GetHashCode()
			{
				return guid != null ? guid.GetHashCode() : 0;
			}

			public Entry() {}
			public Entry(string guid) { this.guid = guid; }

			public int CompareTo(Entry other)
			{
				if (guid == null)
					return other.guid == null ? 0 : 1;
				else
					return other.guid == null ? -1 : guid.CompareTo(other.guid);
			}

			public override string ToString()
			{
				return starColor != 2 ? guid + ':' + (char)('0' + starColor) : guid; // (path != null ? path + "/" : string.Empty) + guid;
			}

			static public Entry FromString(string registryRecord)
			{
				throw new System.NotImplementedException();
			}

            public bool Equals(Entry other)
            {
				if (guid != null)
					return guid == other.guid;
	            return other.guid == null;
            }
        }

		private Dictionary<Entry, Entry> favoriteObjectsSet;
		private Dictionary<Entry, Entry> favoriteAssetsSet;

		public bool isLoaded;

		public Favorites()
		{
			FG_GameObjectGUIDs._dirty = true;
			EditorApplication.hierarchyChanged += OnHierarchyChanged;
		}
		
		private static void OnHierarchyChanged()
		{
			FG_GameObjectGUIDs._dirty = true;
			//Debug.Log("OnHierarchyChanged");
		}

		public void Load()
		{
			isLoaded = true;

			_pushOtherIcons = EditorPrefs.GetBool("FlipbookGames.FavoritesTab.PushOtherIcons", true);
			_showStars = EditorPrefs.GetBool("FlipbookGames.FavoritesTab.ShowStars", true);

			string allFavorites;
			if (EditorPrefs.HasKey("FlipbookGames.AllFavorites"))
			{
				allFavorites = EditorPrefs.GetString("FlipbookGames.AllFavorites", "");
			}
			else
			{
				string allFavoriteAssets = EditorPrefs.GetString("FlipbookGames.FavoriteAssets", string.Empty);
				string allFavoriteObjects = EditorPrefs.GetString("FlipbookGames.FavoriteObjects", string.Empty);
				allFavorites = allFavoriteAssets + '|' + allFavoriteObjects;
			}

			string[] entries = allFavorites.Split(new[]{'|'}, StringSplitOptions.RemoveEmptyEntries);
			int numEntries = entries.Length;
			var favoriteObjectsList = new List<Entry>(numEntries);
			var favoriteAssetsList = new List<Entry>(numEntries);

			for (int i = 0; i < numEntries; ++i)
			{
				Entry current = new Entry();

				string entry = entries[i];
				int lastColon = entry.LastIndexOf(':');
				if (lastColon >= 0)
				{
					current.starColor = entry[lastColon + 1] - '0';
					entry = entry.Substring(0, lastColon);
				}
				
				current.guid = entry;
				current.addOrder = ++addOrderCounter;
				
				if (entry.IndexOf('-') >= 0)
					favoriteObjectsList.Add(current);
				else
					favoriteAssetsList.Add(current);
			}

			favoriteObjectsSet = new Dictionary<Entry, Entry>(favoriteObjectsList.Count);
			foreach (var item in favoriteObjectsList)
				favoriteObjectsSet.Add(item, item);

			favoriteAssetsSet = new Dictionary<Entry, Entry>(favoriteAssetsList.Count);
			foreach (var item in favoriteAssetsList)
				favoriteAssetsSet.Add(item, item);
		}

		private void SaveFavorites()
		{
			StringBuilder sb = new StringBuilder();
			var favorites = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(favoriteAssetsSet.Values, favoriteObjectsSet.Values));
			Array.Sort(favorites, (a, b) => a.addOrder - b.addOrder);
			if (favorites.Length > 0)
				sb.Append(favorites[0].ToString());
			for (int i = 1; i < favorites.Length; ++i)
			{
				sb.Append('|');
				sb.Append(favorites[i].ToString());
			}
			EditorPrefs.SetString("FlipbookGames.AllFavorites", sb.ToString());
		}

		List<GameObject> tempMovedObjects = new List<GameObject>();
		List<string> tempMovedGuids = new List<string>();
		
		private void UpdateGoGUIDs()
		{
			if (!FG_GameObjectGUIDs._dirty)
				return;
			
			FG_GameObjectGUIDs._dirty = false;
			
			bool refresh = false;
			
			tempMovedObjects.Clear();
			tempMovedGuids.Clear();

			foreach (var instance in FG_GameObjectGUIDs.allInstances)
			{
				if (instance == null)
					continue;
				
				var objects = instance.objects;
				var guids = instance.guids;
				var instanceScene = instance.gameObject.scene;
				
				for (int i = objects.Count; i --> 0; )
				{
					if (objects[i] == null)
					{
						tempEntry.guid = guids[i];
						if (favoriteObjectsSet.Remove(tempEntry))
							refresh = true;
						
						objects[i] = objects[objects.Count - 1];
						objects.RemoveAt(objects.Count - 1);
						
						guids[i] = guids[guids.Count - 1];
						guids.RemoveAt(guids.Count - 1);
					
						continue;
					}
					
					GameObject go = objects[i] as GameObject;
					if (go == null)
						continue;
					
					if (go.scene == instanceScene)
						continue;
					refresh = true;
					//Debug.Log("GameObject " + go.name + " moved from scene " + instanceScene.name + " to " + go.scene.name);
					
					tempMovedObjects.Add(go);
					tempMovedGuids.Add(guids[i]);
					
					objects[i] = objects[objects.Count - 1];
					objects.RemoveAt(objects.Count - 1);
						
					guids[i] = guids[guids.Count - 1];
					guids.RemoveAt(guids.Count - 1);
				}
			}
			
			for (int i = tempMovedObjects.Count; i --> 0; )
			{
				var go = tempMovedObjects[i];
				
				var guids = GetOrCreateGuidsForScene(go.scene.handle);
				if (guids == null)
				{
					tempEntry.guid = tempMovedGuids[i];
					favoriteObjectsSet.Remove(tempEntry);
					
					continue;
				}
				
				guids.objects.Add(go);
				guids.guids.Add(tempMovedGuids[i]);
				EditorUtility.SetDirty(guids);
			}
			
			tempMovedObjects.Clear();
			tempMovedGuids.Clear();
			
			if (refresh)
			{
				SaveFavorites();
				RefreshAllTabs();
			}
		}
		
		private FG_GameObjectGUIDs GetOrCreateGuidsForScene(int sceneHandle)
		{
			FG_GameObjectGUIDs guids = GetGuidsForScene(sceneHandle);
			if (guids != null)
				return guids;
			
			int numScenes = EditorSceneManager.sceneCount;
			for (int i = numScenes; i --> 0; )
			{
				Scene scene = EditorSceneManager.GetSceneAt(i);
				if (scene.handle == sceneHandle)
				{
					GameObject go = new GameObject();
					if (go.scene != scene)
						EditorSceneManager.MoveGameObjectToScene(go, scene);
					go.name = "FG_GameObjectGUIDs";
					go.tag = "EditorOnly";
					return go.AddComponent<FG_GameObjectGUIDs>();
				}
			}
			
			return null;
		}
		
		private FG_GameObjectGUIDs GetGuidsForScene(int sceneHandle)
		{
			foreach (FG_GameObjectGUIDs instance in FG_GameObjectGUIDs.allInstances)
			{
				if (instance.gameObject != null && instance.gameObject.scene.handle == sceneHandle)
					return instance;
			}
			
			return null;
		}

		public string GetOrCreateGuidForGameObject(GameObject obj)
		{
			UpdateGoGUIDs();
			
			FG_GameObjectGUIDs guids = GetOrCreateGuidsForScene(obj.scene.handle);
			if (guids == null)
				return null;
			
			int index = guids.objects.IndexOf(obj);
			if (index >= 0)
				return guids.guids[index];
			
			var newGuid = System.Guid.NewGuid().ToString();

			guids.objects.Add(obj);
			guids.guids.Add(newGuid);

			EditorUtility.SetDirty(guids);

			return newGuid;
		}

		public string GetGuidForGameObject(UnityEngine.Object obj)
		{
			UpdateGoGUIDs();
			
			GameObject go = obj as GameObject;
			if (go == null)
				return string.Empty;
			
			FG_GameObjectGUIDs guids = GetGuidsForScene(go.scene.handle);
			if (guids == null)
				return string.Empty;

			int index = guids.objects.IndexOf(obj);
			if (index >= 0)
				return guids.guids[index];
			else
				return string.Empty;
		}

		public GameObject FindGameObject(string guid)
		{
			UpdateGoGUIDs();

			foreach (FG_GameObjectGUIDs instance in FG_GameObjectGUIDs.allInstances)
			{
				int index = instance.guids.IndexOf(guid);
				if (index >= 0)
					return instance.objects[index] as GameObject;
			}
			
			return null;
		}

		private bool CheckFilter(string name, ref string[] words)
		{
			foreach (string word in words)
				if (name.IndexOf(word, StringComparison.InvariantCultureIgnoreCase) < 0)
					return false;
			return true;
		}

		// Known file types in Unity. Feel free to add support for non-native file types.
		private static Dictionary<string, int> typeIndexMap = new Dictionary<string, int>()
		{
			// animation
			{"anim", 0}, {"controller", 0}, {"overridecontroller", 0}, {"mask", 0},

			// audio
			{"mixer", 1},
			{"aac", 1}, {"aif", 1}, {"aiff", 1}, {"au", 1}, {"mid", 1}, {"midi", 1}, {"mp3", 1}, {"mpa", 1},
			{"ra", 1}, {"ram", 1}, {"wma", 1}, {"wav", 1}, {"wave", 1}, {"ogg", 1}, {"flac", 1},
			{"mod", 1}, {"it", 1}, {"s3m", 1}, {"xm", 1},

			// custom
			{"asset", 2}, {"bytes", 2},

			// fonts
			{"ttf", 3}, {"otf", 3}, {"fon", 3}, {"fnt", 3},
			{"fontsettings", 3},
			
			// material
			{"mat", 4},
			{"flare", 4}, {"giparams", 4}, {"rendertexture", 4}, {"physicsmaterial", 4}, {"physicsmaterial2d", 4},

			// meshes
			{"3df", 5}, {"3dm", 5}, {"3dmf", 5}, {"3ds", 5}, {"3dv", 5}, {"3dx", 5}, {"blend", 5},
			{"c4d", 5}, {"c5d", 5}, {"lwo", 5}, {"lws", 5}, {"ma", 5}, {"max", 5}, {"mb", 5}, {"mesh", 5},
			{"obj", 5}, {"vrl", 5}, {"wrl", 5}, {"wrz", 5},
			{"fbx", 5}, {"jas", 5}, {"dae", 5}, {"dxf", 5}, {"lxo", 5},
			
			// prefabs
			{"prefab", 6},

			// preferences
			{"prefs", 7},

			// scenes
			{"unity", 8}, {"scenetemplate", 8},

			// scripts
			{"asmdef", 9}, {"asmref", 9}, {"boo", 9}, {"cs", 9}, {"js", 9},

			// shaders
			{"compute", 10}, {"raytrace", 10}, {"shader", 10}, {"shadervariants", 10},
			{"cginc", 10}, {"cg", 10}, {"glslinc", 10}, {"hlsl", 10},

			// data
			{"csv", 11}, {"json", 11}, {"sql", 11}, {"xml", 11}, {"yaml", 11},

			// ui
			{"guiskin", 12}, {"uss", 12}, {"uxml", 12},

			// text
			{"txt", 13}, {"doc", 13}, {"docx", 13}, {"pdf", 13}, {"htm", 13}, {"html", 13},

			// textures
			{"ai", 14}, {"apng", 14}, {"png", 14}, {"bmp", 14}, {"cdr", 14}, {"dib", 14}, {"eps", 14},
			{"exif", 14}, {"gif", 14}, {"ico", 14}, {"icon", 14}, {"j", 14}, {"j2c", 14}, {"j2k", 14},
			{"jiff", 14}, {"jng", 14}, {"jp2", 14}, {"jpc", 14}, {"jpe", 14}, {"jpeg", 14},
			{"jpf", 14}, {"jpg", 14}, {"jpw", 14}, {"jpx", 14}, {"jtf", 14}, {"mac", 14}, {"omf", 14},
			{"qif", 14}, {"qti", 14}, {"qtif", 14}, {"tex", 14}, {"tfw", 14}, {"tga", 14}, {"tif", 14},
			{"tiff", 14}, {"wmf", 14}, {"psd", 14}, {"exr", 14}, {"hdr", 14}, {"cubemap", 14},
			{"astc", 14}, {"dds", 14}, {"ktx", 14}, {"pvr", 14},

			// video
			{"asf", 15}, {"asx", 15}, {"avi", 15}, {"dat", 15}, {"divx", 15}, {"dv", 15}, {"dvx", 15},
			{"mlv", 15}, {"m2l", 15}, {"m2t", 15}, {"m2ts", 15}, {"m2v", 15}, {"m4e", 15}, {"m4v", 15},
			{"mjp", 15}, {"mov", 15}, {"movie", 15}, {"mp21", 15}, {"mp4", 15}, {"mpe", 15}, {"mpeg", 15},
			{"mpg", 15}, {"mpv2", 15}, {"ogm", 15}, {"ogv", 15}, {"qt", 15}, {"rm", 15}, {"rmvb", 15},
			{"vp8", 15}, {"webm", 15}, {"wmw", 15}, {"xvid", 15},
		};

		public static int GuessAssetType(ListViewItem item)
		{
#if UNITY_3_5
			return item.guiContent.image != null && item.guiContent.image.name == "_Folder" ? -1 : GetTypeIndexForFile(item.assetPath);
#else
			var iconName = item.guiContent.image != null ? item.guiContent.image.name : null;
			var isFolder = iconName != null && (iconName == "Folder Icon" || iconName == "d_Folder Icon");
			return isFolder ? -1 : GetTypeIndexForFile(item.assetPath);
#endif
		}

		private static int GetTypeIndexForFile(string fileName)
		{
			int typeIndex;
            string extension = System.IO.Path.GetExtension(fileName).ToLowerInvariant();
			if (extension.StartsWith(".", StringComparison.Ordinal))
				extension = extension.Remove(0, 1);
            if (typeIndexMap.TryGetValue(extension, out typeIndex))
				return typeIndex;
			else
				return 100; // unknown types go at the end
		}

		public ListViewItem[] CreateListViewItems(string filter, int searchMode, int starColorFilter, bool sortByType, bool sortByName)
		{
			UpdateGoGUIDs();

			string[] words = filter.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

			List<ListViewItem> items = new List<ListViewItem>();

			int numGameObjects = 0;
			if (searchMode < 2)
			{
				foreach (var favoriteObject in favoriteObjectsSet)
				{
					ListViewItem item = ListViewItem.FromGameObjectGUID(favoriteObject.Value.guid);
					if (item != null && CheckFilter(item.guiContent.text, ref words))
					{
						if (starColorFilter == -1 || starColorFilter == favoriteObject.Value.starColor)
						{
							item.addOrder = favoriteObject.Value.addOrder;
							item.starColor = favoriteObject.Value.starColor;
							items.Add(item);
						}
					}
				}
				numGameObjects = items.Count;
			}

			if (searchMode != 1)
			{
				int typeFilter = filterTypeIds[searchMode];

				foreach (var favoriteAsset in favoriteAssetsSet)
				{
					ListViewItem item = ListViewItem.FromAssetGUID(favoriteAsset.Value.guid);
					if (item != null && (typeFilter == 100 || typeFilter == GuessAssetType(item)))
					{
						if (CheckFilter(item.guiContent.text, ref words))
						{
							if (starColorFilter == -1 || starColorFilter == favoriteAsset.Value.starColor)
							{
								item.addOrder = favoriteAsset.Value.addOrder;
								item.starColor = favoriteAsset.Value.starColor;
								items.Add(item);
							}
						}
					}
				}
			}

			ListViewItem[] all = items.ToArray();
			if (sortByName)
			{
				Array.Sort(all);
			}
			else if (sortByType)
			{
				if (searchMode < 2)
					Array.Sort(all, 0, numGameObjects);
				if (searchMode != 1)
					Array.Sort(all, numGameObjects, all.Length - numGameObjects, new CompareAssetsByType());
			}
			else
			{
				Array.Sort(all, (a, b) => b.addOrder - a.addOrder);
			}
			return all;
		}

		public class CompareAssetsByType : IComparer<ListViewItem>
		{
			public int Compare(ListViewItem a, ListViewItem b)
			{
				int typeOfA = GuessAssetType(a);
				int typeOfB = GuessAssetType(b);

				int r = typeOfA.CompareTo(typeOfB);
				if (r == 0 && (typeOfA == 100 || typeOfA == 2) && a.guiContent.image != null && b.guiContent.image != null)
					r = a.guiContent.image.name.CompareTo(b.guiContent.image.name);
				if (r == 0)
					r = a.guiContent.text.CompareTo(b.guiContent.text);
				return r;
			}
		}
		
		private Entry tempEntry = new Entry();

		public int Contains(string guid)
		{
			if (!isLoaded)
				Load();
			
			tempEntry.guid = guid;

			Entry entry;
			if (favoriteAssetsSet.TryGetValue(tempEntry, out entry))
				return entry.starColor;
			else
				return -1;
		}

		public int Contains(int instanceID)
		{
			if (!isLoaded)
				Load();
			
			UpdateGoGUIDs();

			string guid = GetGuidForGameObject(EditorUtility.InstanceIDToObject(instanceID));
			if (guid == string.Empty)
				return -1;
			
			tempEntry.guid = guid;
			
			Entry entry;
			if (favoriteObjectsSet.TryGetValue(tempEntry, out entry))
				return entry.starColor;
			else
				return -1;
		}

		public void ToggleGameObject(int instanceID)
		{
			var obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
			if (!obj)
			{
				//Debug.Log("GameObject not found for instance ID: " + instanceID);
				var o = EditorUtility.InstanceIDToObject(instanceID);
				//Debug.Log("Object for instance ID: " + (o == null ? "null" : obj.GetType().ToString()));
				return;
			}
			string guid = GetOrCreateGuidForGameObject(obj);
			ToggleGameObject(guid);
		}

		public void ToggleAsset(string guid)
		{
			tempEntry.guid = guid;
			ToggleAsset(tempEntry);
		}

		public void ToggleAsset(Entry guid)
		{
			if (!favoriteAssetsSet.Remove(guid))
			{
				Entry newEntry = new Entry(guid.guid);
				newEntry.starColor = guid.starColor;
				newEntry.addOrder = ++addOrderCounter;
				favoriteAssetsSet.Add(newEntry, newEntry);
			}

			EditorApplication.RepaintProjectWindow();
#if UNITY_2018_1_OR_NEWER
			if (MissingAPI.callProjectHasChangedMethod != null)
				MissingAPI.callProjectHasChangedMethod.Invoke(null, null);
#else
			if (EditorApplication.projectWindowChanged != null)
				EditorApplication.projectWindowChanged();
#endif
			SaveFavorites();
		}

		public void ToggleGameObject(string guid)
		{
			tempEntry.guid = guid;
			if (!favoriteObjectsSet.Remove(tempEntry))
			{
				Entry newEntry = new Entry(guid);
				newEntry.addOrder = ++addOrderCounter;
				favoriteObjectsSet.Add(newEntry, newEntry);
			}

			EditorApplication.RepaintHierarchyWindow();
#if UNITY_2018_1_OR_NEWER
			//EditorApplication.hierarchyChanged.Invoke();
#else
			if (EditorApplication.hierarchyWindowChanged != null)
				EditorApplication.hierarchyWindowChanged();
#endif
			SaveFavorites();
		}

		public void OnAssetDeleted(string path)
		{
			if (!isLoaded)
				Load();
			
			string guid = AssetDatabase.AssetPathToGUID(path);
			tempEntry.guid = guid;
			if (favoriteAssetsSet.Remove(tempEntry))
			{
				RefreshAllTabs();
				SaveFavorites();
			}
		}

		public void SetStarColor(object userData, string[] options, int colorIndex)
		{
			if (userData is int)
			{
				int instanceID = (int) userData;
				GameObject go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
				
				string guid = GetOrCreateGuidForGameObject(go);
				if (string.IsNullOrEmpty(guid))
					return;
				
				Entry entry;
				
				tempEntry.guid = guid;
				
				if (!favoriteObjectsSet.TryGetValue(tempEntry, out entry))
				{
					entry = new Entry(guid);
					entry.addOrder = ++addOrderCounter;
					favoriteObjectsSet.Add(entry, entry);
				}
				
				entry.starColor = colorIndex;
				
				EditorApplication.RepaintHierarchyWindow();
#if UNITY_2018_1_OR_NEWER
				//EditorApplication.hierarchyChanged();
#else
				if (EditorApplication.hierarchyWindowChanged != null)
					EditorApplication.hierarchyWindowChanged();
#endif

				RefreshAllTabs();
				SaveFavorites();
			}
			else
			{
				string guid = (string) userData;
				
				Entry entry;
				
				tempEntry.guid = guid;
				if (!favoriteAssetsSet.TryGetValue(tempEntry, out entry))
				{
					entry = new Entry(guid);
					entry.addOrder = ++addOrderCounter;
					favoriteAssetsSet.Add(entry, entry);
				}

				entry.starColor = colorIndex;
				
				EditorApplication.RepaintProjectWindow();
#if UNITY_2018_1_OR_NEWER
				if (MissingAPI.callProjectHasChangedMethod != null)
					MissingAPI.callProjectHasChangedMethod.Invoke(null, null);
#else
				if (EditorApplication.projectWindowChanged != null)
					EditorApplication.projectWindowChanged();
#endif

				RefreshAllTabs();
				SaveFavorites();
			}
		}
	}


	[UnityEditor.MenuItem("Window/Favorites", false, 500)]
	public static void OpenWindow()
	{
		FavoritesTab wnd;
		if (EditorWindow.focusedWindow is FavoritesTab)
		{
			wnd = ScriptableObject.CreateInstance<FavoritesTab>();
		}
		else
		{
			wnd = EditorWindow.GetWindow<FavoritesTab>();
		}
		wnd.sortByType = EditorPrefs.GetBool("FlipbookGames.FavoritesTab.SortByType", true);
		wnd.sortByName = EditorPrefs.GetBool("FlipbookGames.FavoritesTab.SortByName", !wnd.sortByType);
		wnd.Show();
		wnd.Focus();
		wnd.minSize = new Vector2(130f, 64f);
	}

	FavoritesTab()
	{
//		sortByType = EditorPrefs.GetBool("FlipbookGames.FavoritesTab.SortByType", true);
//		sortByName = EditorPrefs.GetBool("FlipbookGames.FavoritesTab.SortByName", !sortByType);
	}

	private void OnDestroy()
	{
	}
	
	private static new EditorWindow mouseOverWindow;
	private static void CacheMouseOverWindow()
	{
		mouseOverWindow = EditorWindow.mouseOverWindow;
	}

	public void OnEnable()
	{
#if UNITY_2018_1_OR_NEWER
		EditorApplication.hierarchyChanged -= OnHierarchyOrProjectWindowChanged;
		EditorApplication.hierarchyChanged += OnHierarchyOrProjectWindowChanged;
		EditorApplication.projectChanged -= OnHierarchyOrProjectWindowChanged;
		EditorApplication.projectChanged += OnHierarchyOrProjectWindowChanged;
#else
		EditorApplication.hierarchyWindowChanged -= OnHierarchyOrProjectWindowChanged;
		EditorApplication.hierarchyWindowChanged += OnHierarchyOrProjectWindowChanged;
		EditorApplication.projectWindowChanged -= OnHierarchyOrProjectWindowChanged;
		EditorApplication.projectWindowChanged += OnHierarchyOrProjectWindowChanged;
#endif

		focusSearchBox = false;
		focusListView = true;
		recreateListViewItems = true;
		
#if UNITY_3_5 || UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_5_0
		if (cachedTitleContent == null)
		{
			cachedTitleContent = base.GetType().GetProperty("cachedTitleContent", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField);
		}
		if (cachedTitleContent != null)
		{
			GUIContent titleContent = cachedTitleContent.GetValue(this, null) as GUIContent;
			if (titleContent != null)
			{
				if (windowIcon == null)
					windowIcon = LoadEditorTexture2D("fgFavoritesTabIcon", true);
				titleContent.image = windowIcon;
				titleContent.text = windowTitles[searchMode];
			}
		}
#else
		if (windowIcon == null)
			windowIcon = LoadEditorTexture2D("fgFavoritesTabIcon", true);
		titleContent.image = windowIcon;
		titleContent.text = windowTitles[searchMode];
#endif
	}

	public void OnDisable()
	{
#if UNITY_2018_1_OR_NEWER
		EditorApplication.hierarchyChanged -= OnHierarchyOrProjectWindowChanged;
		EditorApplication.projectChanged -= OnHierarchyOrProjectWindowChanged;
#else
		EditorApplication.hierarchyWindowChanged -= OnHierarchyOrProjectWindowChanged;
		EditorApplication.projectWindowChanged -= OnHierarchyOrProjectWindowChanged;
#endif

		//mouseOverWindow = null;
	}

	private void OnHierarchyChange()
	{
		if (!EditorApplication.isPlayingOrWillChangePlaymode)
		{
			Repaint();
		}
	}

	private void OnHierarchyOrProjectWindowChanged()
	{
		recreateListViewItems = true;
		//Repaint();
	}

	private static string GetObjectName(UnityEngine.Object assetObject)
	{
		if (assetObject.name != string.Empty)
			return assetObject.name;
		else
			return System.IO.Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(assetObject));
	}

	private static string thisAssetPath = "Assets/FlipbookGames/FavoritesTab/Editor";

	static void Initialize()
	{
		if (initialized)
			return;
		initialized = true;

		_pushOtherIcons = EditorPrefs.GetBool("FlipbookGames.FavoritesTab.PushOtherIcons", true);
		_showStars = EditorPrefs.GetBool("FlipbookGames.FavoritesTab.ShowStars", true);

		AboutFavoritesTab tempInstance = ScriptableObject.CreateInstance<AboutFavoritesTab>();
		MonoScript thisScript = MonoScript.FromScriptableObject(tempInstance);
		ScriptableObject.DestroyImmediate(tempInstance);
		tempInstance = null;
		thisAssetPath = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(AssetDatabase.GetAssetPath(thisScript)));

		scrollViewStyle = new GUIStyle();
		scrollViewStyle.overflow.left = 2;
		scrollViewStyle.stretchWidth = true;
		scrollViewStyle.stretchHeight = true;

		lineStyle = new GUIStyle("PR Label");
		lineStyle.onNormal.textColor = lineStyle.normal.textColor;
		
		labelStyle = new GUIStyle(lineStyle);
		
		//Styles.Log(Styles.disabledLabel);
		//Styles.Log(Styles.disabledPrefabLabel);
		//Styles.Log(Styles.disabledBrokenPrefabLabel);
		//Styles.Log(labelStyle);
		//Styles.Log(Styles.prefabLabel);
		//Styles.Log(Styles.brokenPrefabLabel);
		
		labelStyle.padding.left = 2;
		labelStyle.margin.right = 0;
		labelStyle.fixedHeight = 0;

		gameObjectIcon = (Texture2D) EditorGUIUtility.ObjectContent(null, typeof(GameObject)).image;
		windowIcon = LoadEditorTexture2D("fgFavoritesTabIcon", true);
		emptyStar = LoadEditorTexture2D("fgEmptyStar", true);
		filledStars = new Texture2D[]
		{
			LoadEditorTexture2D("fgRedStar", true),
			LoadEditorTexture2D("fgOrangeStar", true),
			LoadEditorTexture2D("fgYellowStar", true),
			LoadEditorTexture2D("fgGreenStar", true),
			LoadEditorTexture2D("fgCyanStar", true),
			LoadEditorTexture2D("fgBlueStar", true),
			LoadEditorTexture2D("fgMagentaStar", true),
		};
		emptyRibbon = LoadEditorTexture2D("fgEmptyRibbon", true);
		filledRibbons = new Texture2D[]
		{
			LoadEditorTexture2D("fgRedRibbon", true),
			LoadEditorTexture2D("fgOrangeRibbon", true),
			LoadEditorTexture2D("fgYellowRibbon", true),
			LoadEditorTexture2D("fgGreenRibbon", true),
			LoadEditorTexture2D("fgCyanRibbon", true),
			LoadEditorTexture2D("fgBlueRibbon", true),
			LoadEditorTexture2D("fgMagentaRibbon", true),
		};
		createdByFlipbookGames = LoadEditorTexture2D("CreatedByFlipbookGames", false);
	}

	static class Styles
	{
		public static GUIStyle disabledLabel = new GUIStyle("PR DisabledLabel");
		public static GUIStyle prefabLabel = new GUIStyle("PR PrefabLabel");
		public static GUIStyle disabledPrefabLabel = new GUIStyle("PR DisabledPrefabLabel");
		public static GUIStyle brokenPrefabLabel = new GUIStyle("PR BrokenPrefabLabel");
		public static GUIStyle disabledBrokenPrefabLabel = new GUIStyle("PR DisabledBrokenPrefabLabel");
		
		//public static void Log(GUIStyle style)
		//{
		//	Debug.Log(style);
		//	if (style.name == "StyleNotFoundError")
		//		return;
		//	Debug.Log("normal: " + style.normal.textColor);
		//	Debug.Log("active: " + style.active.textColor);
		//	Debug.Log("focused: " + style.focused.textColor);
		//	Debug.Log("hover: " + style.hover.textColor);
		//	Debug.Log("onNormal: " + style.onNormal.textColor);
		//	Debug.Log("onActive: " + style.onActive.textColor);
		//	Debug.Log("onFocused: " + style.onFocused.textColor);
		//	Debug.Log("onHover: " + style.onHover.textColor);
		//}
	}

	private static Texture2D LoadEditorTexture2D(string name, bool indieAndPro)
	{
		string skin = indieAndPro ? (EditorGUIUtility.isProSkin ? "Pro.png" : "Indie.png") : ".png";

		string path = System.IO.Path.Combine(thisAssetPath, "Textures");
		path = System.IO.Path.Combine(path, name);
		Texture2D texture = AssetDatabase.LoadMainAssetAtPath(path + skin) as Texture2D;
		if (texture != null)
			return texture;

		string oldPath = System.IO.Path.Combine(thisAssetPath, "Resources");
		oldPath = System.IO.Path.Combine(oldPath, name);
		texture = AssetDatabase.LoadMainAssetAtPath(oldPath + skin) as Texture2D;
		if (texture != null)
		{
			AssetDatabase.MoveAsset(oldPath + skin, path + skin);
			if (indieAndPro)
			{
				skin = !EditorGUIUtility.isProSkin ? "Pro.png" : "Indie.png";
				AssetDatabase.MoveAsset(oldPath + skin, path + skin);
			}
			return texture;
		}

		oldPath = System.IO.Path.Combine("FlipbookGames/FavoritesTab", name);
		texture = EditorGUIUtility.Load(oldPath + skin) as Texture2D;
		if (texture != null)
		{
			oldPath = System.IO.Path.Combine("Assets/Editor Default Resources", oldPath);
			AssetDatabase.MoveAsset(oldPath + skin, path + skin);
			if (indieAndPro)
			{
				skin = !EditorGUIUtility.isProSkin ? "Pro.png" : "Indie.png";
				AssetDatabase.MoveAsset(oldPath + skin, path + skin);
			}
		}
		return texture;
	}

	private void ScrollToItem(int index)
	{
		int count = listViewItems.Length;
		if (count > (int)((scrollViewRect.height) / itemsHeight))
		{
			float maxScrollPos = itemsHeight * index;
			float minScrollPos = maxScrollPos - scrollViewRect.height + itemsHeight;
			if (scrollPosition.y < minScrollPos)
			{
				scrollPosition.y = minScrollPos;
				Repaint();
			}
			else if (scrollPosition.y > maxScrollPos)
			{
				scrollPosition.y = maxScrollPos;
				Repaint();
			}
		}
	}

	private void SelectItem(int index)
	{
		ListViewItem item = listViewItems[index];

		bool ctrl = EditorGUI.actionKey;
		bool shift = (Event.current.modifiers & EventModifiers.Shift) != 0 && focusedItemIndex >= 0;
		if (!shift)
			anchorItemIndex = -1;

		if (shift || ctrl)
		{
			List<UnityEngine.Object> newSelection = new List<UnityEngine.Object>();
			if (ctrl)
			{
				newSelection.AddRange(Selection.objects);

				UnityEngine.Object obj;
				if (item.instanceID != 0)
					obj = EditorUtility.InstanceIDToObject(item.instanceID);
				else
					obj = AssetDatabase.LoadAssetAtPath(item.assetPath, typeof(UnityEngine.Object));

				if (newSelection.Contains(obj))
					newSelection.Remove(obj);
				else
					newSelection.Add(obj);
			}
			else
			{
				if (anchorItemIndex < index)
				{
					for (int i = anchorItemIndex; i <= index; ++i)
					{
						UnityEngine.Object obj;
						if (listViewItems[i].instanceID != 0)
							obj = EditorUtility.InstanceIDToObject(listViewItems[i].instanceID);
						else
							obj = AssetDatabase.LoadAssetAtPath(listViewItems[i].assetPath, typeof(UnityEngine.Object));
						if (!newSelection.Contains(obj))
							newSelection.Add(obj);
					}
				}
				else
				{
					for (int i = index; i <= anchorItemIndex; ++i)
					{
						UnityEngine.Object obj;
						if (listViewItems[i].instanceID != 0)
							obj = EditorUtility.InstanceIDToObject(listViewItems[i].instanceID);
						else
							obj = AssetDatabase.LoadAssetAtPath(listViewItems[i].assetPath, typeof(UnityEngine.Object));
						if (!newSelection.Contains(obj))
							newSelection.Add(obj);
					}
				}
			}
			Selection.objects = newSelection.ToArray();
		}
		else if (item.instanceID != 0)
		{
			Selection.instanceIDs = new int[] { item.instanceID };
			EditorGUIUtility.PingObject(item.instanceID);
		}
		else
		{
			bool ping = true;
			UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(item.assetPath, typeof(UnityEngine.Object));

			if (obj != null)
			{
#if !UNITY_3_5
				if (MissingAPI.lastInteractedObjectBrowser != null)
				{
					bool isFolder = IsFolder(obj.GetInstanceID());
					if (isFolder)
					{
						EditorWindow objectBrowser = MissingAPI.lastInteractedObjectBrowser.GetValue(null) as EditorWindow;
						if (objectBrowser != null)
						{
							bool isLocked = (bool)(
								MissingAPI.isLocked != null
								? MissingAPI.isLocked.GetValue(objectBrowser)
								: MissingAPI.isLocked2.GetValue(objectBrowser, new object[]{})
							);
							if (!isLocked)
							{
								if (MissingAPI.viewModeField.GetValue(objectBrowser).ToString() == "TwoColumns")
								{
									ping = false;
									try
									{
										MissingAPI.showFolderContentsMethod.Invoke(objectBrowser, new object[] { obj.GetInstanceID(), false });
									}
									catch {}

									if (MissingAPI.isLocked != null)
										MissingAPI.isLocked.SetValue(objectBrowser, true);
									else
										MissingAPI.isLocked2.SetValue(objectBrowser, true, new object[]{});
									Selection.objects = new UnityEngine.Object[] { obj };
									EditorApplication.update -= UnlockObjectBrowser;
									EditorApplication.update += UnlockObjectBrowser;
								}
								else
								{
									var expandedIndex = Array.BinarySearch(UnityEditorInternal.InternalEditorUtility.expandedProjectWindowItems, obj.GetInstanceID());
									if (expandedIndex < 0)
									{
										EditorApplication.delayCall += () =>
										{
											if (focusedWindow == this)
											{
												objectBrowser.Focus();
												var key = Event.KeyboardEvent("escape");
												key.keyCode = KeyCode.RightArrow;
												objectBrowser.SendEvent(key);
												Focus();
											}
										};
									}
								}
							}
						}
					}
				}
#else
				if (item.IsFolder)
				{
					if (projectWindowType == null)
						projectWindowType = typeof(Editor).Assembly.GetType("UnityEditor.ProjectWindow");
					if (projectWindowType != null)
					{
						var projectWindows = Resources.FindObjectsOfTypeAll(projectWindowType);
						if (projectWindows.Length > 0)
						{
							var projectWindow = projectWindows[0] as EditorWindow;
							if (projectWindow)
							{
								var expandedIndex = Array.BinarySearch(UnityEditorInternal.InternalEditorUtility.expandedProjectWindowItems, obj.GetInstanceID());
								if (expandedIndex < 0)
								{
									EditorApplication.delayCall += () =>
									{
										if (focusedWindow == this)
										{
											projectWindow.Focus();
											var key = Event.KeyboardEvent("escape");
											key.keyCode = KeyCode.RightArrow;
											projectWindow.SendEvent(key);
											Focus();
										}
									};
								}
							}
						}
					}
				}
#endif
				Selection.objects = new UnityEngine.Object[] { obj };
				if (ping)
					EditorGUIUtility.PingObject(obj);
			}
		}

		if (Selection.objects.Length > 0)
		{
			focusedItemIndex = index;
			if (!shift)
				anchorItemIndex = index;
		}

		ScrollToItem(index);
	}

	private static Type projectWindowType;

#if !UNITY_3_5
	private static void UnlockObjectBrowser()
	{
		EditorApplication.update -= UnlockObjectBrowser;
		EditorApplication.update -= UnlockObjectBrowserDelayed;
		EditorApplication.update += UnlockObjectBrowserDelayed;
	}

	private static void UnlockObjectBrowserDelayed()
	{
		EditorApplication.update -= UnlockObjectBrowserDelayed;

		EditorWindow objectBrowser = MissingAPI.lastInteractedObjectBrowser.GetValue(null) as EditorWindow;
		if (objectBrowser != null)
		{
			if (MissingAPI.isLocked != null)
				MissingAPI.isLocked.SetValue(objectBrowser, false);
			else
				MissingAPI.isLocked2.SetValue(objectBrowser, false, new object[]{});
		}
	}

	private static void ShowFolderContent()
	{
		EditorApplication.update -= ShowFolderContent;
		//EditorApplication.update -= ShowFolderContentDelayed;
		ShowFolderContentDelayed();
		EditorApplication.update += ShowFolderContentDelayed;
	}

	private static void ShowFolderContentDelayed()
	{
		EditorApplication.update -= ShowFolderContentDelayed;

		if (Selection.objects != null && Selection.objects.Length == 1)
		{
			EditorWindow objectBrowser = MissingAPI.lastInteractedObjectBrowser.GetValue(null) as EditorWindow;
			if (objectBrowser != null)
			{
				objectBrowser.Repaint();
				MissingAPI.showFolderContentsMethod.Invoke(objectBrowser, new object[] { Selection.objects[0].GetInstanceID(), false });
			}
		}
	}
	
	private static bool IsFolder(int instanceID)
	{
		return System.IO.Directory.Exists(AssetDatabase.GetAssetPath(instanceID));
	}

	private static class MissingAPI
	{
		public static Type objectBrowserType;
		public static FieldInfo lastInteractedObjectBrowser;
		public static FieldInfo viewModeField;
		public static FieldInfo isLocked;
		public static PropertyInfo isLocked2;
		public static MethodInfo showFolderContentsMethod;
#if UNITY_2018_1_OR_NEWER
		public static MethodInfo callProjectHasChangedMethod;
#endif

		static MissingAPI()
		{
#if UNITY_2018_1_OR_NEWER
			callProjectHasChangedMethod = typeof(EditorApplication).GetMethod("Internal_CallProjectHasChanged", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
#endif

			objectBrowserType = typeof(Editor).Assembly.GetType("UnityEditor.ObjectBrowser");
			if (objectBrowserType == null)
			{
				objectBrowserType = typeof(Editor).Assembly.GetType("UnityEditor.ProjectBrowser");
				if (objectBrowserType == null)
					return;
			}

			lastInteractedObjectBrowser = objectBrowserType.GetField("s_LastInteractedObjectBrowser", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			if (lastInteractedObjectBrowser == null)
			{
				lastInteractedObjectBrowser = objectBrowserType.GetField("s_LastInteractedProjectBrowser", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				if (lastInteractedObjectBrowser == null)
					return;
			}

			viewModeField = objectBrowserType.GetField("m_ViewMode", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			isLocked = objectBrowserType.GetField("m_IsLocked", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (isLocked == null)
				isLocked2 = objectBrowserType.GetProperty("isLocked", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (viewModeField == null || isLocked == null && isLocked2 == null)
			{
				lastInteractedObjectBrowser = null;
				return;
			}

			showFolderContentsMethod = objectBrowserType.GetMethod("ShowFolderContents",
				BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null,
				new Type[] { typeof(int), typeof(bool) }, null);
			if (showFolderContentsMethod == null)
			{
				lastInteractedObjectBrowser = null;
				return;
			}
		}
	}
#endif

#if UNITY_3_5 || UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_5_0
	PropertyInfo cachedTitleContent = null;
#endif
	
	private static void RefreshAllTabs()
	{
		FavoritesTab[] favsTabs = Resources.FindObjectsOfTypeAll(typeof(FavoritesTab)) as FavoritesTab[];
		foreach (FavoritesTab tab in favsTabs)
		{
			if (tab != null)
			{
				tab.recreateListViewItems = true;
				tab.Repaint();
			}
		}
	}

	public void OnGUI()
	{
		if (!favorites.isLoaded)
		{
			favorites.Load();
		}

		if (!initialized)
		{
			Initialize();
		}

#if UNITY_3_5 || UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_5_0
		if (cachedTitleContent != null)
		{
			title = windowTitles[searchMode];

			GUIContent titleContent = cachedTitleContent.GetValue(this, null) as GUIContent;
			if (titleContent != null)
				titleContent.image = windowIcon;
		}
#else
		titleContent.text = windowTitles[searchMode];
		titleContent.image = windowIcon;
#endif
		if (recreateListViewItems && Event.current.type == EventType.Layout)
		{
			EditorPrefs.SetBool("FlipbookGames.FavoritesTab.SortByType", sortByType);
			EditorPrefs.SetBool("FlipbookGames.FavoritesTab.SortByName", sortByName);

			recreateListViewItems = false;
			listViewItems = favorites.CreateListViewItems(searchString, searchMode, starColorFilter, sortByType, sortByName);
			
			focusedItemIndex = -1;
			anchorItemIndex = -1;
			foreach (var selectedObject in Selection.objects)
			{
				if (!selectedObject)
					continue;
				
				if (AssetDatabase.IsMainAsset(selectedObject))
				{
					var assetPath = AssetDatabase.GetAssetPath(selectedObject);
					var index = Array.FindIndex<ListViewItem>(listViewItems, x => assetPath == x.assetPath);
					if (index >= 0)
					{
						focusedItemIndex = anchorItemIndex = index;
						break;
					}
				}
				else
				{
					var instanceId = selectedObject.GetInstanceID();
					var index = Array.FindIndex<ListViewItem>(listViewItems, x => instanceId == x.instanceID);
					if (index >= 0)
					{
						focusedItemIndex = anchorItemIndex = index;
						break;
					}
				}
			}
		}

		if (Event.current.type == EventType.ValidateCommand && Event.current.commandName == "FrameSelected")
		{
			Event.current.Use();
			return;
		}
		else if (Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "FrameSelected")
		{
			Event.current.Use();
			SceneView.FrameLastActiveSceneView();
			return;
		}
		else if (Event.current.type == EventType.KeyDown)
		{
			switch (Event.current.keyCode)
			{
				case KeyCode.UpArrow:
					if (listViewItems.Length > 0)
						itemToSelect = Math.Max(0, focusedItemIndex - 1);
					Event.current.Use();
					break;

				case KeyCode.DownArrow:
					itemToSelect = Math.Min(listViewItems.Length - 1, focusedItemIndex + 1);
					Event.current.Use();
					break;

				case KeyCode.Home:
					if (hasSearchBoxFocus)
						break;
					if (listViewItems.Length > 0)
						itemToSelect = 0;
					Event.current.Use();
					break;

				case KeyCode.End:
					if (hasSearchBoxFocus)
						break;
					itemToSelect = listViewItems.Length - 1;
					Event.current.Use();
					break;

				case KeyCode.PageUp:
					if (listViewItems.Length > 0)
						itemToSelect = Math.Max(0, focusedItemIndex - ((int)scrollViewRect.height) / 16);
					Event.current.Use();
					break;

				case KeyCode.PageDown:
					itemToSelect = Math.Min(listViewItems.Length - 1, focusedItemIndex + ((int)scrollViewRect.height) / 16);
					Event.current.Use();
					break;
				case KeyCode.Delete:
				case KeyCode.Backspace:
					if (hasSearchBoxFocus)
						break;
					Event.current.Use();
					selectedOnRightClick = false;
					RemoveFavoritesMenuHandler(null);
					return;
			}

			if (EditorGUI.actionKey && Event.current.keyCode == KeyCode.T && !Event.current.alt && !Event.current.shift)
			{
				Event.current.Use();
				OpenWindow();
				GUIUtility.ExitGUI();
			}
			else if (!hasSearchBoxFocus && Event.current.modifiers == 0 && Event.current.character == '\n')
			{
				Event.current.Use();
				EditorApplication.ExecuteMenuItem("Assets/Open");
				GUIUtility.ExitGUI();
			}
		}
		
#if UNITY_3_5 || UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
		EditorGUIUtility.LookLikeControls();
#endif

		DoToolbar();

		if (Event.current.type == EventType.ValidateCommand && Event.current.commandName == "SelectAll")
		{
			Event.current.Use();
			return;
		}
		else if (Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "SelectAll")
		{
			Event.current.Use();

			UnityEngine.Object[] sel = new UnityEngine.Object[listViewItems.Length];
			for (int i = 0; i < listViewItems.Length; ++i)
			{
				if (listViewItems[i].instanceID != 0)
					sel[i] = EditorUtility.InstanceIDToObject(listViewItems[i].instanceID);
				else
					sel[i] = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(listViewItems[i].guid), typeof(UnityEngine.Object));
			}
			Selection.objects = sel;
			return;
		}
		
		GUI.SetNextControlName("ListView");
		if (focusListView)
		{
			GUI.FocusControl("ListView");
			if (Event.current.type == EventType.Repaint)
			{
				focusListView = false;
			}
		}

		if (Event.current.type == EventType.Repaint)
			scrollViewRect = GUILayoutUtility.GetRect(1f, Screen.width, 16f, Screen.height, scrollViewStyle);
		else
			GUILayoutUtility.GetRect(1f, Screen.width, 16f, Screen.height, scrollViewStyle);

		if (Event.current.type != EventType.Layout)
		{
			contentHeight = itemsHeight * listViewItems.Length;
			scrollPosition = GUI.BeginScrollView(scrollViewRect, scrollPosition, new Rect(0f, 0f, 1f, contentHeight));

			paintingFavoritesTab = true;
			paintFavsIcons = showStarsInFavTabs;
			for (int i = 0; i < listViewItems.Length; ++i)
			{
				DoListViewItem(listViewItems[i], i, !hasSearchBoxFocus && EditorWindow.focusedWindow == this);
			}
			paintFavsIcons = false;
			paintingFavoritesTab = false;

			if (itemToSelect >= 0)
			{
				SelectItem(itemToSelect);
				itemToSelect = -1;
			}

			GUI.EndScrollView();
		}

		if (itemPopupMenu != null)
		{
			itemPopupMenu.ShowAsContext();
			itemPopupMenu = null;
		}
	}

	static FavoritesTab()
	{
		favorites = new Favorites();
		//	AutoFavorite.onSpawnAutoFavorite = OnSpawnAutoFavorite;
	}
	
	static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		//Debug.Log("Scene Loaded (" + mode + "): " + scene.path);
	}
	
	static void OnSceneUnloaded(Scene scene)
	{
		//Debug.Log("Scene Unloaded: " + scene.path);
	}
	
	static void OnSpawnAutoFavorite(GameObject go)
	{
		if (!EditorUtility.IsPersistent(go) && favorites.Contains(go.GetInstanceID()) < 0)
			favorites.ToggleGameObject(go.GetInstanceID());
	}

	[InitializeOnLoad]
	private static class TreeViewTracker
	{
		private static EditorWindow treeViewEditorWindow = null;
		private static Rect highlightedItemRect = new Rect();
		
		private static EditorApplication.HierarchyWindowItemCallback hierarchyItemCallbacks;
		private static EditorApplication.ProjectWindowItemCallback projectItemCallbacks;
		
		private static Type typeOfHierarchy2;

		static TreeViewTracker()
		{
			typeOfHierarchy2 =
				typeof(TreeViewTracker).Assembly.GetType("vietlabs.Hierarchy2")
				?? typeof(TreeViewTracker).Assembly.GetType("Hierarchy2");
			
			EditorApplication.update += OnFirstUpdate;
			EditorApplication.update += TrackMouseOnWindow;
			EditorApplication.hierarchyWindowItemOnGUI -= HierarchyItemOnGuiCallback;
			EditorApplication.hierarchyWindowItemOnGUI += HierarchyItemOnGuiCallback;
		}

		private static void OnFirstUpdate()
		{
			EditorApplication.update -= OnFirstUpdate;

			EditorApplication.update -= CacheMouseOverWindow;
			EditorApplication.update += CacheMouseOverWindow;
			
			SceneManager.sceneLoaded += OnSceneLoaded;
			SceneManager.sceneUnloaded += OnSceneUnloaded;

			EditorApplication.hierarchyWindowItemOnGUI -= HierarchyItemOnGuiCallback;
			hierarchyItemCallbacks = EditorApplication.hierarchyWindowItemOnGUI;
			EditorApplication.hierarchyWindowItemOnGUI = HierarchyItemOnGuiCallback;

			EditorApplication.projectWindowItemOnGUI -= ProjectItemOnGuiCallback;
			projectItemCallbacks = EditorApplication.projectWindowItemOnGUI;
			EditorApplication.projectWindowItemOnGUI = ProjectItemOnGuiCallback;

			EditorApplication.RepaintHierarchyWindow();
			EditorApplication.RepaintProjectWindow();
		}

		private static void ProjectItemOnGuiCallback(string item, Rect selectionRect)
		{
			if (string.IsNullOrEmpty(item))
				return;

			float iconWidth = ShowStars ? 16f : 8f;

			bool showFavIcons = !paintingFavoritesTab || paintFavsIcons;
			
			if (showFavIcons)
			{
				if (!initialized)
					Initialize();

				if (treeViewEditorWindow && !treeViewEditorWindow.wantsMouseMove)
					treeViewEditorWindow.wantsMouseMove = true;

				Rect rc = selectionRect;
				if (IsSameRect())
				{
					int isFavorite = IsFavorite(item);

					rc.x = rc.xMax - iconWidth - 1f;
					rc.width = 17f;
					rc.height = 16f;
					if (GUI.Button(rc, isFavorite < 0 ? null : (ShowStars ? filledStars : filledRibbons)[isFavorite], GUIStyle.none))
					{
						if (Event.current.button == 0 && !paintingFavoritesTab)
						{
							favorites.ToggleAsset(item);
							RefreshAllTabs();
						}
						else if (Event.current.button == 1 || paintingFavoritesTab)
						{
							EditorUtility.DisplayCustomMenu(rc, starColorNames, isFavorite, favorites.SetStarColor, item);
							var popupMenu = new GenericMenu();
							for (var i = 0; i < starColorNames.Length; ++i)
							{
								var index = i;
								popupMenu.AddItem(starColorNames[i], i == isFavorite, (x) => favorites.SetStarColor(x, null, index), item);
							}
							popupMenu.AddSeparator("");
							popupMenu.AddItem(new GUIContent("Stars"), ShowStars, () => ShowStars = true);
							popupMenu.AddItem(new GUIContent("Ribbons"), !ShowStars, () => ShowStars = false);
							popupMenu.DropDown(rc);
						}
					}
	
					var hoverWindow = mouseOverWindow;
					if (hoverWindow && selectionRect.Contains(Event.current.mousePosition))
					{
						if (!hoverWindow.wantsMouseMove)
							hoverWindow.wantsMouseMove = true;
						
						treeViewEditorWindow = hoverWindow;
						highlightedItemRect.Set(Mathf.Max(selectionRect.x, 4f), selectionRect.y, selectionRect.width, selectionRect.height);
						enableTrackMouseOnWindow = true;
	
						if (isFavorite < 0 && DragAndDrop.objectReferences.Length == 0 && DragAndDrop.paths.Length == 0)
						{
							if (GUI.Button(rc, ShowStars ? emptyStar : emptyRibbon, GUIStyle.none))
							{
								if (Event.current.button == 0)
								{
									favorites.ToggleAsset(item);
									RefreshAllTabs();
								}
							}
						}
					}
				}
			}

			if (projectItemCallbacks != null)
			{
				if (!paintingFavoritesTab || paintFavsIcons)
					selectionRect.xMax -= iconWidth + 2f;
				projectItemCallbacks(item, selectionRect);
			}
		}
		
		private static object[] tempObjectArray2 = new object[2];

		private static void HierarchyItemOnGuiCallback(int item, Rect selectionRect)
		{
			//bool shrink = false;
			float iconWidth = ShowStars ? 16f : 8f;

			bool isMainStage =
#if UNITY_2018_3_OR_NEWER
				UnityEditor.SceneManagement.StageUtility.GetMainStageHandle() == UnityEditor.SceneManagement.StageUtility.GetCurrentStageHandle();
#else
				false;
#endif
			
			bool showFavIcons = paintingFavoritesTab ? paintFavsIcons : isMainStage;

			if (showFavIcons && Event.current.type != EventType.Layout)
			{
				if (!initialized)
					Initialize();

				if (treeViewEditorWindow && !treeViewEditorWindow.wantsMouseMove)
					treeViewEditorWindow.wantsMouseMove = true;

				if (IsSameRect())
				{
					//selectionRect.xMin = 0;
					Rect rc = selectionRect;

					int isFavorite = IsFavorite(item);

					rc.x = rc.xMax - iconWidth - 1f;
					rc.width = 17f;
					rc.height = 16f;
					if (GUI.Button(rc, isFavorite < 0 ? null : (ShowStars ? filledStars : filledRibbons)[isFavorite], GUIStyle.none))
					{
						if (Event.current.button == 0 && !paintingFavoritesTab)
						{
							favorites.ToggleGameObject(item);
							RefreshAllTabs();
						}
						else if (Event.current.button == 1 || paintingFavoritesTab)
						{
							var popupMenu = new GenericMenu();
							for (var i = 0; i < starColorNames.Length; ++i)
							{
								var index = i;
								popupMenu.AddItem(starColorNames[i], i == isFavorite, (x) => favorites.SetStarColor(x, null, index), item);
							}
							popupMenu.AddSeparator("");
							popupMenu.AddItem(new GUIContent("Stars"), ShowStars, () => ShowStars = true);
							popupMenu.AddItem(new GUIContent("Ribbons"), !ShowStars, () => ShowStars = false);
							popupMenu.AddSeparator("");
							popupMenu.AddItem(new GUIContent("Push other icons"), PushOtherIcons, () => PushOtherIcons = !PushOtherIcons);
							popupMenu.DropDown(rc);
						}
					}

					var hoverWindow = mouseOverWindow;
					if (hoverWindow != null && selectionRect.Contains(Event.current.mousePosition))
					{
						if (!hoverWindow.wantsMouseMove)
							hoverWindow.wantsMouseMove = true;
						
						treeViewEditorWindow = hoverWindow;
						highlightedItemRect = selectionRect;
						enableTrackMouseOnWindow = true;

						if (isFavorite < 0 && DragAndDrop.objectReferences.Length == 0 && DragAndDrop.paths.Length == 0)
						{
							//shrink = true;
							if (GUI.Button(rc, ShowStars ? emptyStar : emptyRibbon, GUIStyle.none))
							{
								if (Event.current.button == 0)
								{
									favorites.ToggleGameObject(item);
									RefreshAllTabs();
								}
							}
						}
					}
				}
			}

			if (hierarchyItemCallbacks != null)
			{
				if (showFavIcons)
					selectionRect.xMax -= iconWidth + 2f;
				hierarchyItemCallbacks(item, selectionRect);
			}
		}

		static bool enableTrackMouseOnWindow = false;
		static void TrackMouseOnWindow()
		{
			if (!enableTrackMouseOnWindow)
				return;
			
			if (treeViewEditorWindow != null && mouseOverWindow != treeViewEditorWindow)
			{
				treeViewEditorWindow.Repaint();
				highlightedItemRect.Set(0, 0, 0, 0);
				enableTrackMouseOnWindow = false;
			}
		}

		static int IsFavorite(string item)
		{
			return favorites.Contains(item);
		}

		static int IsFavorite(int item)
		{
			return favorites.Contains(item);
		}

		static bool IsSameRect()
		{
			if (treeViewEditorWindow == null)
				return true;
			
			if (Event.current.type != EventType.MouseMove ||
				mouseOverWindow == treeViewEditorWindow && highlightedItemRect.Contains(Event.current.mousePosition))
			{
				return true;
			}

			if (highlightedItemRect.width != 0)
			{
				treeViewEditorWindow.Repaint();
				treeViewEditorWindow = null;
				highlightedItemRect.Set(0, 0, 0, 0);
				enableTrackMouseOnWindow = false;
			}
			return false;
		}
	}

	protected class ListViewItem : IComparable<ListViewItem>
	{
		public GUIContent guiContent;
		public string guid;
		public string assetPath;
		public int instanceID;
		public int addOrder;
		public int starColor;
		public bool selected;
		
		public static ListViewItem FromGameObjectGUID(string guid)
		{
			ListViewItem item = new ListViewItem { guid = guid };
			GameObject obj = favorites.FindGameObject(guid);
			if (obj == null)
				return null;

			item.instanceID = obj.GetInstanceID();
			item.guiContent = new GUIContent(EditorGUIUtility.ObjectContent(obj, null));
			if (item.guiContent.image == null && obj is GameObject)
#if UNITY_2018_3_OR_NEWER
				item.guiContent.image = PrefabUtility.GetIconForGameObject(obj);
#else
				item.guiContent.image = gameObjectIcon;
#endif

			return item;
		}

		public static ListViewItem FromAssetGUID(string guid)
		{
			ListViewItem item = new ListViewItem { guid = guid };
			item.assetPath = AssetDatabase.GUIDToAssetPath(guid);
			if (item.assetPath == string.Empty || item.assetPath.StartsWith("Assets/__DELETED_GUID_Trash/", StringComparison.OrdinalIgnoreCase))
				return null;

			item.guiContent = new GUIContent(System.IO.Path.GetFileNameWithoutExtension(item.assetPath), AssetDatabase.GetCachedIcon(item.assetPath));
			
			if (item.assetPath.StartsWith("Packages/", StringComparison.OrdinalIgnoreCase) && item.IsFolder && item.assetPath.IndexOf('/', "Packages/".Length) == -1)
			{
				try
				{
					var jsonFilename = item.assetPath + "/package.json";
					if (System.IO.File.Exists(jsonFilename))
					{
						var json = System.IO.File.ReadAllText(jsonFilename);
						if (json != null)
						{
							var start = json.IndexOf("\"displayName\"", StringComparison.Ordinal);
							if (start > 0)
							{
								start += "\"displayName\"".Length;
								start = json.IndexOf('"', start);
								if (start > 0)
								{
									++start;
									var end = json.IndexOf('"', start);
									if (end > start)
									{
										item.guiContent.text = json.Substring(start, end - start);
									}
								}
							}
						}
					}
				}
				catch {}
			}
			
			return item;
		}

		public int CompareTo(ListViewItem other)
		{
			return guiContent.text.CompareTo(other.guiContent.text);
		}
		
		public bool IsFolder
		{
			get {
#if UNITY_3_5
				return guiContent.image != null && guiContent.image.name == "_Folder";
#else
				return guiContent.image != null &&
					(guiContent.image.name == "Folder Icon" || guiContent.image.name == "d_Folder Icon");
#endif
			}
		}
	}

	private void OnSelectionChange()
	{
		Repaint();
	}

	private void DoListViewItem(ListViewItem item, int index, bool focused)
	{
		bool isMainStage =
#if UNITY_2018_3_OR_NEWER
			UnityEditor.SceneManagement.StageUtility.GetMainStageHandle() == UnityEditor.SceneManagement.StageUtility.GetCurrentStageHandle();
#else
			true;
#endif

		int selectedAssetInstanceID = 0;

		bool on = false;
		if (draggedItem != -1)
		{
			on = draggedItem == index;
		}
		else if (item.instanceID != 0)
		{
			on = Selection.Contains(item.instanceID);
		}
		else
		{
			foreach (int instanceID in Selection.instanceIDs)
			{
				if (AssetDatabase.GetAssetPath(instanceID) == item.assetPath)
				{
					selectedAssetInstanceID = instanceID;
					on = true;
					break;
				}
			}
		}

		if (item == null || Event.current == null)
			return;
		
		var wasEnabled = GUI.enabled;
		var enable = isMainStage || !string.IsNullOrEmpty(item.assetPath);			;

		Rect rcContent = new Rect(0f, 0f, scrollViewRect.width, scrollViewRect.height);
		if (contentHeight > rcContent.height)
		{
			rcContent.height = contentHeight;
			rcContent.width -= 15f;
		}		
		
		Rect rcItem = new Rect(0f, itemsHeight * index, rcContent.width, 16f);
		
		//if (showStarsInFavTabs)
		//	rcItem.width -= ShowStars ? 16f : 8f;
		
		GameObject go = item.instanceID != 0 ? EditorUtility.InstanceIDToObject(item.instanceID) as GameObject : null;
		
		if (Event.current.type == EventType.Repaint)
		{
			int colorCode = 0;
			
			if (go)
			{
#if UNITY_2018_3_OR_NEWER
				PrefabAssetType prefabAssetType = PrefabUtility.GetPrefabAssetType(go);
				switch (prefabAssetType)
				{
				case PrefabAssetType.NotAPrefab:
					break;
					
				case PrefabAssetType.Regular:
				case PrefabAssetType.Model:
				case PrefabAssetType.Variant:
					colorCode = 1;
					break;
					
				case PrefabAssetType.MissingAsset:
					colorCode = 2;
					break;
				}
#else
				PrefabType prefabType = PrefabUtility.GetPrefabType(go);
				switch (prefabType)
				{
				case PrefabType.PrefabInstance:
				case PrefabType.ModelPrefabInstance:
				//case PrefabType.DisconnectedPrefabInstance:
				//case PrefabType.DisconnectedModelPrefabInstance:
					colorCode |= 1;
					break;
					
				case PrefabType.MissingPrefabInstance:
				//case PrefabType.DisconnectedPrefabInstance:
				//case PrefabType.DisconnectedModelPrefabInstance:
					colorCode |= 2;
					break;
					
				case PrefabType.Prefab:
				case PrefabType.ModelPrefab:
					go = null;
					break;
				}
#endif
				
#if UNITY_3_5
				if (go != null && !go.active)
#else
				if (go != null && !go.activeInHierarchy)
#endif
					colorCode |= 4;
			}
			
			Color[] colorArray = EditorGUIUtility.isProSkin ? darkSkinColors : lightSkinColors;
			Color color = colorArray[colorCode & 3];
			Color onColor = colorArray[(colorCode & 3) + 4];
			
			GUIStyle style;
			switch (colorCode)
			{
				case 0: style = lineStyle; break;
				case 1: style = Styles.prefabLabel; break;
				case 2: style = Styles.brokenPrefabLabel; break;
				case 4: style = Styles.disabledLabel; break;
				case 5: style = Styles.disabledPrefabLabel; break;
				case 6: style = Styles.disabledBrokenPrefabLabel; break;
				default: style = lineStyle; break;
			};
			
			labelStyle.normal.textColor = style.normal.textColor;
			labelStyle.hover.textColor = style.hover.textColor;
			labelStyle.focused.textColor = style.focused.textColor;
			labelStyle.active.textColor = style.active.textColor;
			labelStyle.onNormal.textColor = style.onNormal.textColor;
			labelStyle.onHover.textColor = style.onHover.textColor;
			labelStyle.onFocused.textColor = style.onFocused.textColor;
			labelStyle.onActive.textColor = style.onActive.textColor;
			
			if (showPaths)
			{
				var rcPath = rcItem;
#if UNITY_3_5
				rcPath.y += 15f;
				rcPath.height = 12f;
#else
				rcPath.y += 14f;
				rcPath.height = 13f;
#endif

				var path = item.assetPath;
				if (!string.IsNullOrEmpty(path))
				{
					if (path.StartsWith("Assets/", StringComparison.OrdinalIgnoreCase))
						path = item.assetPath.Substring("Assets/".Length);
					var lastSlash = path.LastIndexOf('/');
					if (lastSlash >= 0)
						path = path.Remove(lastSlash);
					else
						path = "";
					if (path == "")
						path = "/";
				}
				
				if (path != null)
				{
					labelStyle.contentOffset = new Vector2(17f, 0f);
					labelStyle.fontSize = 9;
				
					GUI.enabled = enable;
					labelStyle.Draw(rcPath, path, false, false, on, focused);
					GUI.enabled = wasEnabled;
					labelStyle.fontSize = 0;
					labelStyle.contentOffset = Vector2.zero;
				}
				else
				{
					rcItem.yMax = rcPath.yMax;
				}
			}
			
			EditorGUIUtility.SetIconSize(new Vector2(16f, 16f));
			GUI.enabled = enable;
			labelStyle.Draw(rcItem, item.guiContent, false, false, on, focused);
			GUI.enabled = wasEnabled;
		}
		else
		{
			EditorGUIUtility.SetIconSize(new Vector2(16f, 16f));
		}
		
		if (go != null)
		{
			if (EditorApplication.hierarchyWindowItemOnGUI != null)
				EditorApplication.hierarchyWindowItemOnGUI(item.instanceID, new Rect(19f, rcItem.y, rcContent.width - 19f, 16f));
		}
		else
		{
			if (EditorApplication.projectWindowItemOnGUI != null)
				EditorApplication.projectWindowItemOnGUI(item.guid, rcItem);
		}
		EditorGUIUtility.SetIconSize(Vector2.zero);
		
		GUI.enabled = wasEnabled;
		
		rcItem.height = itemsHeight;

		if (Event.current.type == EventType.DragExited || Event.current.type == EventType.DragPerform)
		{
			draggedItem = -1;
			Repaint();
		}
		else if (Event.current.isMouse && rcItem.Contains(Event.current.mousePosition))
		{
			if (Event.current.button == 0 && Event.current.type == EventType.MouseDown)
			{
				mouseDownPosition = Event.current.mousePosition;
				
				if (Event.current.clickCount == 2)
				{
					Event.current.Use();
					if (item.instanceID == 0 && selectedAssetInstanceID != 0)
						AssetDatabase.OpenAsset(selectedAssetInstanceID);
					else if (enable)
						SceneView.FrameLastActiveSceneView();
					GUIUtility.ExitGUI();
					return;
				}
			}

			if (Event.current.type == EventType.MouseDrag)
			{
				if (enable && itemMouseDown != -1 &&
					(mouseDownPosition - Event.current.mousePosition).SqrMagnitude() >= 64f)
				{
					DragAndDrop.PrepareStartDrag();
					if (selectedAssetInstanceID != 0)
					{
						DragAndDrop.objectReferences = Selection.objects;
						DragAndDrop.StartDrag(Selection.objects.Length > 1 ? Selection.objects.Length + " objects" : item.guiContent.text);
					}
					else
					{
						draggedItem = itemMouseDown;
						if (item.instanceID != 0)
							DragAndDrop.objectReferences = new UnityEngine.Object[] { EditorUtility.InstanceIDToObject(item.instanceID) };
						else
							DragAndDrop.objectReferences = new UnityEngine.Object[]
								{ AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(item.guid), typeof(UnityEngine.Object)) };
						DragAndDrop.StartDrag(item.guiContent.text);
					}
					Repaint();
					Event.current.Use();
					itemMouseDown = -1;
				}
			}
			else if (Event.current.type == EventType.MouseDown)
			{
				if (Event.current.button == 0)
				{
					itemMouseDown = index;
				}
				else
				{
					savedSelection = Selection.objects.Clone() as UnityEngine.Object[];
					
					selectedOnRightClick = !on;
					if (!on)
						itemToSelect = index;
				}
				focusListView = true;
				Event.current.Use();
			}
			else if (Event.current.type == EventType.MouseUp)
			{
				if (Event.current.button == 0)
				{
					if (itemMouseDown >= 0)
						itemToSelect = index;
					Event.current.Use();
				}
				else if (Event.current.button == 1)
				{
					itemPopupMenu = new GenericMenu();
					if (item.instanceID == 0)
					{
						if (selectedAssetInstanceID != 0)
						{
							if (Application.platform == RuntimePlatform.OSXEditor)
							{
								itemPopupMenu.AddItem(new GUIContent("Open _\n"), false, () => EditorApplication.ExecuteMenuItem("Assets/Open"));
								itemPopupMenu.AddItem(new GUIContent("Show in new Inspector"), false, () => ShowInNewInspector(item));
								itemPopupMenu.AddItem(new GUIContent("Reveal in Finder"), false, () => EditorApplication.ExecuteMenuItem("Assets/Reveal in Finder"));
							}
							else
							{
								itemPopupMenu.AddItem(new GUIContent("Open _ENTER"), false, () => EditorApplication.ExecuteMenuItem("Assets/Open"));
								itemPopupMenu.AddItem(new GUIContent("Show in new Inspector"), false, () => ShowInNewInspector(item));
								itemPopupMenu.AddItem(new GUIContent("Show in Explorer"), false, () => EditorApplication.ExecuteMenuItem("Assets/Show in Explorer"));
							}
							itemPopupMenu.AddItem(new GUIContent("Reimport"), false, () => EditorApplication.ExecuteMenuItem("Assets/Reimport"));
							itemPopupMenu.AddSeparator(string.Empty);
						}
						if (Application.platform == RuntimePlatform.OSXEditor)
							itemPopupMenu.AddItem(new GUIContent("Remove from Favorites _\b"), false, () => RemoveFavoritesMenuHandler(selectedOnRightClick ? item : null));
						else
							itemPopupMenu.AddItem(new GUIContent("Remove from Favorites _delete"), false, () => RemoveFavoritesMenuHandler(selectedOnRightClick ? item : null));
					}
					else
					{
						if (enable)
							itemPopupMenu.AddItem(new GUIContent("Frame in Scene _f"), false, () => SceneView.FrameLastActiveSceneView());
						else
							itemPopupMenu.AddDisabledItem(new GUIContent("Frame in Scene _f"));
						itemPopupMenu.AddItem(new GUIContent("Show in new Inspector"), false, () => ShowInNewInspector(item));
						itemPopupMenu.AddSeparator(string.Empty);
						if (Application.platform == RuntimePlatform.OSXEditor)
							itemPopupMenu.AddItem(new GUIContent("Remove from Favorites _\b"), false, () => RemoveFavoritesMenuHandler(selectedOnRightClick ? item : null));
						else
							itemPopupMenu.AddItem(new GUIContent("Remove from Favorites _delete"), false, () => RemoveFavoritesMenuHandler(selectedOnRightClick ? item : null));
					}
					Event.current.Use();
					Focus();
				}
			}
		}
		
		GUI.enabled = wasEnabled;
	}

	[MenuItem("Assets/Show in New Inspector", true, 10)]
	private static bool ValidateShowAssetInNewInspector()
	{
		return Selection.activeObject;
	}

	[MenuItem("Assets/Show in New Inspector", false, 10)]
	private static void ShowAssetInNewInspector(MenuCommand command)
	{
		if (Selection.activeObject)
			ShowActiveObjectInNewInspector();
	}

	private UnityEngine.Object[] savedSelection;
	private void ShowInNewInspector(ListViewItem item)
	{
		if (item == null)
			return;
		selectedOnRightClick = false;
		itemToSelect = -1;
		
		Selection.objects = new UnityEngine.Object[0];
		if (item.instanceID != 0)
			Selection.activeInstanceID = item.instanceID;
		else
			Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(item.guid));

		EditorApplication.update += ShowAfterLoad;
	}

	private void ShowAfterLoad()
	{
		EditorApplication.update -= ShowAfterLoad;
		ShowActiveObjectInNewInspector();
		Selection.objects = savedSelection;
	}

	private static void ShowActiveObjectInNewInspector()
	{
		if (!Selection.activeObject)
			return;

		var inspectorWindowType = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
		if (inspectorWindowType == null)
			return;
		var newInspector = (EditorWindow) CreateInstance(inspectorWindowType);
		newInspector.Show(true);
		newInspector.Repaint();
#if UNITY_2018_OR_NEWER
		var flipLockedMethod = inspectorWindowType.GetMethod("FlipLocked", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
		if (flipLockedMethod != null)
			flipLockedMethod.Invoke(newInspector, null);
#else
		var isLockedMethod = inspectorWindowType == null ? null :  inspectorWindowType.GetMethod("set_isLocked", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
		if (isLockedMethod != null)
			isLockedMethod.Invoke(newInspector, new object[]{true});
#endif
	}

	private bool selectedOnRightClick = true;
	private void RemoveFavoritesMenuHandler(ListViewItem item)
	{
		if (item != null)
		{
			if (item.instanceID != 0)
				favorites.ToggleGameObject(item.instanceID);
			else
				favorites.ToggleAsset(item.guid);
		}
		else
		{
			// Removing all selected favorite items
			ListViewItem[] itemsCopy = (ListViewItem[]) listViewItems.Clone();
			foreach (ListViewItem selectedItem in itemsCopy)
			{
				if (selectedItem.instanceID != 0)
				{
					if (Selection.Contains(selectedItem.instanceID))
						favorites.ToggleGameObject(selectedItem.instanceID);
				}
				else if (Array.Find(Selection.objects, (a) => AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(a)) == selectedItem.guid) != null)
				{
					favorites.ToggleAsset(selectedItem.guid);
				}
			}
		}
		recreateListViewItems = true;
		//focusedItemIndex = -1;
		//anchorItemIndex = -1;
		RefreshAllTabs();
	}

	private void DoToolbar()
	{
		GUI.enabled = true;

		//GUILayout.Space(1f);
		GUILayout.BeginHorizontal(EditorStyles.toolbar);
		
		if (GUILayout.Button("View", EditorStyles.toolbarDropDown))
		{
			GenericMenu menu = new GenericMenu();
			menu.AddItem(new GUIContent("Add Tab %t"), false, OpenWindow);
			menu.AddSeparator(String.Empty);
			menu.AddItem(new GUIContent("Order by Name"), sortByName, () => { sortByType = false; sortByName = true; recreateListViewItems = true; Repaint(); });
			menu.AddItem(new GUIContent("Order by Type"), sortByType, () => { sortByType = true; sortByName = false; recreateListViewItems = true; Repaint(); });
			menu.AddItem(new GUIContent("Order by Recently Added"), !sortByType && !sortByName, () => { sortByType = sortByName = false; recreateListViewItems = true; Repaint(); });
			menu.AddSeparator(String.Empty);
			
			for (var i = 0; i < searchModes.Length; ++i)
			{
				var index = i;
				int modeIndex = searchModesOrder[i];
				if (string.IsNullOrEmpty(searchModes[modeIndex]))
					menu.AddSeparator("Filter by Type/");
				else
					menu.AddItem(new GUIContent("Filter by Type/" + searchModes[modeIndex]), searchMode == modeIndex, () => SelectSearchMode(null, null, index));
			}
			menu.AddItem(new GUIContent("Filter by Star Color/Show All"), starColorFilter == -1, () => SetStarColorFilter(-1));
			menu.AddSeparator("Filter by Star Color/");
			for (var i = 0; i < starColorNames.Length; ++i)
			{
				var index = i;
				menu.AddItem(new GUIContent("Filter by Star Color/" + starColorNames[i].text), starColorFilter == i, () => SetStarColorFilter(index));
			}
			
			menu.AddSeparator(String.Empty);
			menu.AddItem(new GUIContent(ShowStars ? "Show Stars" : "Show Ribbons"), showStarsInFavTabs, () => { showStarsInFavTabs = !showStarsInFavTabs; Repaint(); });
			menu.AddItem(new GUIContent("Show Asset Location"), showPaths, () => { scrollPosition.y /= itemsHeight; showPaths = !showPaths; scrollPosition.y *= itemsHeight; Repaint(); });
			menu.AddSeparator(String.Empty);
			menu.AddItem(new GUIContent("About..."), false, ShowAboutWindow);

			Vector2 size = EditorStyles.toolbarDropDown.CalcSize(new GUIContent("View"));
			menu.DropDown(new Rect(5f, -1f, size.x, size.y));
		}

		GUILayout.Space(5f);
		GUILayout.FlexibleSpace();

		if (toolbarSearchField == null)
			InitToolbarSearchFieldStyles();
		Rect position = GUILayoutUtility.GetRect(1f, 200f, 16f, 16f, toolbarSearchField);
		DoSearchBox(position);

		GUILayout.EndHorizontal();
	}

	private void ShowAboutWindow()
	{
		EditorWindow.GetWindow<AboutFavoritesTab>();
	}

	private void DoSearchBox(Rect position)
	{
		if (Event.current.type == EventType.MouseDown && position.Contains(Event.current.mousePosition))
		{
			focusSearchBox = true;
		}

		if (Event.current.type == EventType.ValidateCommand && Event.current.commandName == "Find")
		{
			Event.current.Use();
			return;
		}
		else if (Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "Find")
		{
			focusSearchBox = true;
		}

		GUI.SetNextControlName("FavTabsSearchFilter");
		if (focusSearchBox)
		{
			GUI.FocusControl("FavTabsSearchFilter");
			if (Event.current.type == EventType.Repaint)
			{
				focusSearchBox = false;
			}
		}
		//var hadFocus = hasSearchBoxFocus;
		hasSearchBoxFocus = GUI.GetNameOfFocusedControl() == "FavTabsSearchFilter";
		//if (hadFocus && !hasSearchBoxFocus)
		//	Debug.Log("Focus lost on " + Event.current);

		if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
		{
			if (hasSearchBoxFocus)
			{
				SetSearchFilter(string.Empty);
				hasSearchBoxFocus = false;
				focusListView = true;
				Event.current.Use();
			}
			else
			{
				if (string.IsNullOrEmpty(searchString))
					searchMode = 0;
				SetSearchFilter(string.Empty);
				hasSearchBoxFocus = false;
				focusListView = true;
				Event.current.Use();
			}
		}

		if (hasSearchBoxFocus && Event.current.type == EventType.KeyDown && Event.current.character == '\n')
		{
			hasSearchBoxFocus = false;
			focusListView = true;
			Event.current.Use();
		}

		string text = ToolbarSearchField(position, searchModes, ref searchMode, searchString);
		if (searchString != text)
		{
			SetSearchFilter(text);
			Repaint();
		}
	}
	
	private void InitToolbarSearchFieldStyles()
	{
		var editorSkin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);
		var searchFieldStyle = editorSkin.FindStyle("ToolbarSeachTextField");
		if (searchFieldStyle != null)
		{
			toolbarSearchField = "ToolbarSeachTextFieldPopup";
			toolbarSearchFieldCancelButton = "ToolbarSeachCancelButton";
			toolbarSearchFieldCancelButtonEmpty = "ToolbarSeachCancelButtonEmpty";
		}
		else
		{
			toolbarSearchField = "ToolbarSearchTextFieldPopup";
			toolbarSearchFieldCancelButton = "ToolbarSearchCancelButton";
			toolbarSearchFieldCancelButtonEmpty = "ToolbarSearchCancelButtonEmpty";
		}
	}

	private string ToolbarSearchField(Rect position, string[] searchModes, ref int searchMode, string text)
	{
		if (toolbarSearchField == null || toolbarSearchFieldCancelButton == null || toolbarSearchFieldCancelButtonEmpty == null)
		{
			InitToolbarSearchFieldStyles();
		}

		Rect rcDropDown = position;
		rcDropDown.width = 20f;
		if ((Event.current.type == EventType.MouseDown) && rcDropDown.Contains(Event.current.mousePosition))
		{
			var selected = Array.IndexOf(searchModesOrder, searchMode);
			EditorUtility.DisplayCustomMenu(position, SearchModesMenuItems, selected, SelectSearchMode, null);
			Event.current.Use();
		}

		Rect rc;
		bool isEmpty = text == string.Empty;
		
		rc = position;
		rc.x += position.width - 14f;
		rc.width = 14f;
		if (!isEmpty || searchMode != 0)
		{
			if (rc.Contains(Event.current.mousePosition))
			{
				EditorGUIUtility.AddCursorRect(rc, MouseCursor.Arrow);
				
				if (Event.current.type == EventType.MouseDown)
				{
					Event.current.Use();
				
					recreateListViewItems = true;
					if (isEmpty)
						searchMode = 0;
					else
						text = string.Empty;
					focusListView = true;
				}
			}
		}

		rc = position;

		text = EditorGUI.TextField(rc, text, toolbarSearchField);

		isEmpty = text == string.Empty;
		if (isEmpty && Event.current.type == EventType.Repaint &&
			(!hasSearchBoxFocus || searchMode != 0 || EditorWindow.focusedWindow != this))
		{
			bool enabled = GUI.enabled;
			GUI.enabled = false;
			Color color = GUI.backgroundColor;
			GUI.backgroundColor = Color.clear;
			if (!hasSearchBoxFocus || EditorWindow.focusedWindow != this)
			{
				toolbarSearchField.alignment = TextAnchor.MiddleLeft;
				toolbarSearchField.Draw(rc, searchModes[searchMode], false, false, false, false);
			}
			else if (searchMode > 0)
			{
				toolbarSearchField.alignment = TextAnchor.MiddleRight;
				toolbarSearchField.Draw(rc, searchModes[searchMode] + '\xa0', false, false, false, false);
			}
			toolbarSearchField.alignment = TextAnchor.MiddleLeft;
			GUI.enabled = enabled;
			GUI.backgroundColor = color;
		}

		if (Event.current.type == EventType.Repaint)
		{
			rc = position;
			rc.x += position.width - 14f;
			rc.width = 14f;
			if (!isEmpty || searchMode != 0)
				toolbarSearchFieldCancelButton.Draw(rc, false, false, false, false);
			else
				toolbarSearchFieldCancelButtonEmpty.Draw(rc, false, false, false, false);
		}

		return text;
	}

	private void SelectSearchMode(object userData, string[] options, int selected)
	{
		searchMode = searchModesOrder[selected];
		recreateListViewItems = true;
		Repaint();
	}

	private void SetSearchFilter(string searchFilter)
	{
		searchString = searchFilter;
		recreateListViewItems = true;
		Repaint();
	}
	
	private void SetStarColorFilter(int index)
	{
		starColorFilter = index;
		recreateListViewItems = true;
		Repaint();
	}
}

}
