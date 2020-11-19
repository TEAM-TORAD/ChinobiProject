using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class EditorCollapseAll
{
	private static int wait = 0;
	private static int undoIndex;

	[MenuItem( "Assets/Collapse All", priority = 1000 )]
	public static void CollapseFolders()
	{
		FileSystemInfo[] rootItems = new DirectoryInfo( Application.dataPath ).GetFileSystemInfos();
		List<Object> rootItemsList = new List<Object>( rootItems.Length );
		for( int i = 0; i < rootItems.Length; i++ )
		{
			Object asset = AssetDatabase.LoadAssetAtPath<Object>( "Assets/" + rootItems[i].Name );
			if( asset != null )
				rootItemsList.Add( asset );
		}

		if( rootItemsList.Count > 0 )
		{
			Undo.IncrementCurrentGroup();
			Selection.objects = Selection.objects;
			undoIndex = Undo.GetCurrentGroup();

			EditorUtility.FocusProjectWindow();

			Selection.objects = rootItemsList.ToArray();

			EditorApplication.update -= CollapseHelper;
			EditorApplication.update += CollapseHelper;
		}
	}

	public static void CollapseGameObjects()
	{
		EditorApplication.update -= CollapseGameObjects;
		CollapseGameObjects( new MenuCommand( null ) );
	}

	[MenuItem( "GameObject/Collapse All", priority = 40 )]
	private static void CollapseGameObjects( MenuCommand command )
	{
		// This happens when this button is clicked via hierarchy's right click context menu
		// and is called once for each object in the selection. We don't want that, we want
		// the function to be called only once
		if( command.context )
		{
			EditorApplication.update -= CollapseGameObjects;
			EditorApplication.update += CollapseGameObjects;

			return;
		}

		List<GameObject> rootGameObjects = new List<GameObject>();
#if UNITY_2018_3_OR_NEWER
		// Check if a prefab stage is currently open
		var prefabStage = UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
		if( prefabStage != null && prefabStage.stageHandle.IsValid() )
			rootGameObjects.Add( prefabStage.prefabContentsRoot );
		else
#endif
		{
			int sceneCount = SceneManager.sceneCount;
			for( int i = 0; i < sceneCount; i++ )
				rootGameObjects.AddRange( SceneManager.GetSceneAt( i ).GetRootGameObjects() );
		}

		if( rootGameObjects.Count > 0 )
		{
			Undo.IncrementCurrentGroup();
			Selection.objects = Selection.objects;
			undoIndex = Undo.GetCurrentGroup();

			Selection.objects = rootGameObjects.ToArray();

			EditorApplication.update -= CollapseHelper;
			EditorApplication.update += CollapseHelper;
		}
	}

	[MenuItem( "CONTEXT/Component/Collapse All", priority = 1400 )]
	private static void CollapseComponents( MenuCommand command )
	{
		// Credit: https://forum.unity.com/threads/is-it-possible-to-fold-a-component-from-script-inspector-view.296333/#post-2353538
		ActiveEditorTracker tracker = ActiveEditorTracker.sharedTracker;
		for( int i = 0, length = tracker.activeEditors.Length; i < length; i++ )
			tracker.SetVisible( i, 0 );

		EditorWindow.focusedWindow.Repaint();
	}

	private static void CollapseHelper()
	{
		if( wait < 1 ) // Increase the number if script doesn't always work
			wait++;
		else
		{
			EditorApplication.update -= CollapseHelper;
			wait = 0;

			EditorWindow focusedWindow = EditorWindow.focusedWindow;
			if( focusedWindow != null )
				focusedWindow.SendEvent( new Event { keyCode = KeyCode.LeftArrow, type = EventType.KeyDown, alt = true } );

			Undo.RevertAllDownToGroup( undoIndex );
		}
	}
}