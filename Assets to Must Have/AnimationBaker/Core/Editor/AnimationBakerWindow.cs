using UnityEngine;
using System.Collections;
using UnityEditor;

public class AnimationBakerWindow : EditorWindow {
	Transform[] charactersList = new Transform[0];
	Transform curCharacter;
	Object[] inputAnimationClips,inputCharacters;
	AnimationClip tempClip;
	AnimationClipProperty[] animationsList = new AnimationClipProperty[0];
	int curCharacterId =-1, curAnimationClipId = -1;
	int i,j,selectedAnimationClip = -1,selectedCharacter = -1;
	Rect areaRect,dropArea;
	Vector2 scrollViewPos = new Vector2(0f,0f),scrollViewPos1 = new Vector2(0f,0f),scrollViewPos2 = new Vector2(0f,30f);
	public static AnimationBakerWindow aBWindow;
	public int sampleRate = 60;
	public bool createDumpForAll= true,recordFullLengthForAll = true,autoBakeDumpForAll = true,executionModeA = true,useRootConstraints = true,
	startRecord = false,previewIsPlaying = false,playButtonPressed = false,useCommonBakingProperties = true,useCommonRootConstraints = true,settingsEnabled = false;
	bool modelNamePrefix = true, frameRatePrefix = true,prefixSubsection = false;
	public string exMode,dumpFileFolderPath = "Assets/AnimationBaker/AnimationDump",animationFileFolderPath = "Assets/AnimationBaker/UnityAnimations",
	recButtonName,playButtonName,preferencesFolderPath = "Assets/AnimationBaker/Core/Preferences";
	Camera mainCamera;
	Animator _animator;
	string lastStateName = "dummy",curCharacterName = "",dumpFilePath = "",info = "";
	AnimationBaker baker;
	float delay = 0.5f;
	RuntimeAnimatorController baseController;
	//======constraints block=======================
	AnimationBaker.Constraint rootPositionConstraints = new AnimationBaker.Constraint(true,true,true);
	AnimationBaker.Constraint rootRotationConstraints = new AnimationBaker.Constraint(true,true,true);
	AnimationBaker.Constraint rootScaleConstraints = new AnimationBaker.Constraint(true,true,true);

	AnimationBaker.Constraint bakingProperties = new AnimationBaker.Constraint(true,false,false);
	//==============================================
	string preferencesFilePath;
	AnimationBakerPreferences preferences;
	Event curEvent;
	bool area1Clicked = false, area2Clicked = false,tempBool = false;
		//searching field section
	//============================================================
	bool showSearchField1 = false,showSearchField2 = false;
	string searchFieldText = "";
	Color tempColor = new Color(Mathf.Clamp01(119f/255f),Mathf.Clamp01(137f/255f),Mathf.Clamp01(163f/255f));
	int highlightUnitID = -1;
	float scrollViewHeight = -1f;
	Rect tempRect;
	//============================================================

	[MenuItem("Window/AnimationBaker/AnimationBaker")]
	public static void OpenAnimationBaker () {
		aBWindow = (AnimationBakerWindow) EditorWindow.GetWindow(typeof(AnimationBakerWindow));
		aBWindow.minSize = new Vector2(600f,300f);
		aBWindow.title = "AnimationBaker";
	}

	//need only for debug
	/*
	[MenuItem("Window/AnimationBaker_close(debug)")]
	public static void CloseAnimationBaker () {
		aBWindow = (AnimationBakerWindow) EditorWindow.GetWindow(typeof(AnimationBakerWindow));
		aBWindow.Close ();
	}
*/
	
	[System.Serializable]
	public class AnimationClipProperty
	{
		public AnimationClip clip;
		public bool[] bakeForID = new bool[0];
		public float recordTime = -1f;
		public bool createDump = true;
		public bool autoBakeDump = true;
		public bool useRootConstraints = false;
		public AnimationBaker.Constraint rootPositionConstraints = new AnimationBaker.Constraint(false,false,false);
		public AnimationBaker.Constraint rootRotationConstraints = new AnimationBaker.Constraint(false,false,false);
		public AnimationBaker.Constraint rootScaleConstraints = new AnimationBaker.Constraint(false,false,false);
		public AnimationBaker.Constraint bakingProperties = new AnimationBaker.Constraint(true,false,false);

		public AnimationClipProperty(AnimationClip aC){
			clip = aC;
		}
	}

	
	[System.Serializable]
	public class AnimationBakerPreferences
	{
		public string[] animationsPaths = new string[0];
		public string[] charactersPaths = new string[0];

	}

	void OnEnable(){
		string prfPath = "";
		baseController = AssetDatabase.LoadAssetAtPath("Assets/AnimationBaker/Core/aB_Animator.controller",typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
		if(!playButtonPressed && !startRecord && charactersList.Length<1 && animationsList.Length<1){
			prfPath = EditorPrefs.GetString("preferencesFolder");
			if(!string.IsNullOrEmpty(prfPath))
				preferencesFolderPath = prfPath;
			preferencesFilePath = preferencesFolderPath+"/preferences.aprf";
		preferences = (AnimationBakerPreferences) BINS.Load (preferencesFilePath);
			if(preferences!=null)
				AssignPreferences();
			}
	}

	void AssignPreferences(){
		int i;
		//assigning animations===================
		if(preferences.animationsPaths.Length>0){
			AnimationClip aC;
			AnimationClipProperty aCP;
			for(i=0;i<preferences.animationsPaths.Length;i++){
				if(!string.IsNullOrEmpty(preferences.animationsPaths[i])){
					aC = AssetDatabase.LoadAssetAtPath(preferences.animationsPaths[i],typeof(AnimationClip)) as AnimationClip;
					if(aC!=null){
						aCP = new AnimationClipProperty(aC);
						ArrayUtility.Add (ref animationsList,aCP);
					}else Debug.Log ("animation clip at path  "+preferences.animationsPaths[i]+"   not found!");
				}
			}
		}
		//assigning characters =====================
		if(preferences.charactersPaths.Length>0){
			Transform character;
			for(i=0;i<preferences.charactersPaths.Length;i++){
				if(!string.IsNullOrEmpty(preferences.charactersPaths[i])){
					character = AssetDatabase.LoadAssetAtPath(preferences.charactersPaths[i],typeof(Transform)) as Transform;
					if(character!=null){
						ArrayUtility.Add (ref charactersList,character);
					}else Debug.Log ("character at path  "+preferences.charactersPaths[i]+"   not found!");
				}
			}
		}
		UpdateAnimationPropertiesCharactersList();
	}

	void SavePreferences(){
		preferences = new AnimationBakerPreferences();
		int i;
		string curPath;
		if(animationsList.Length>0){
			for(i=0;i<animationsList.Length;i++){
				if(animationsList[i].clip!=null){
					curPath = AssetDatabase.GetAssetPath(animationsList[i].clip);
					if(!string.IsNullOrEmpty(curPath))
						ArrayUtility.Add (ref preferences.animationsPaths,curPath);
				}
			}
		}

		if(charactersList.Length>0){
			for(i=0;i<charactersList.Length;i++){
				if(charactersList[i]!=null){
					curPath = AssetDatabase.GetAssetPath(charactersList[i]);
					if(!string.IsNullOrEmpty(curPath))
						ArrayUtility.Add (ref preferences.charactersPaths,curPath);
				}
			}
		}
		preferencesFilePath = preferencesFolderPath+"/preferences.aprf";
		EditorPrefs.SetString("preferencesFolder",preferencesFolderPath);
		BINS.Save (preferences,preferencesFilePath);

	}

	void OnGUI () {
		GUI.color = Color.white;
		GUILayout.BeginHorizontal ();
		if(!startRecord && !playButtonPressed){
		AnimationClipsArea ();
		CharactersArea ();
		}else ProgressInfoArea();
		MainMenuArea();
		GUILayout.EndHorizontal ();
	}
	

		void DrawSearchingField(ref string newSearchFieldText){
				GUILayout.BeginHorizontal ();
				newSearchFieldText = EditorGUILayout.TextField (newSearchFieldText, GUILayout.Width (160));
				if (GUILayout.Button ("X", GUILayout.Width (20), GUILayout.Height (16))) {
						showSearchField1 = false;
						showSearchField2 = false;
						newSearchFieldText = "";
						highlightUnitID = -1;
				}
				GUILayout.EndHorizontal ();
		}


		void UpdateAnimationClipSearchingStatus(){
				showSearchField2 = false;
				showSearchField1 = true;
				searchFieldText = "";
				highlightUnitID = -1;
				inputAnimationClips = Selection.GetFiltered(typeof(AnimationClip),SelectionMode.Unfiltered);
				if(inputAnimationClips != null){
						if(inputAnimationClips.Length>0){
								searchFieldText = inputAnimationClips [0].name;
						}
						inputAnimationClips = null;
				}
		}


		void SearchAnimationClipInList(){
				if (animationsList.Length < 1)
						return;
				if (highlightUnitID > -1)
						return;
				for (int i = 0; i < animationsList.Length; i++) {
						if (animationsList [i] != null) {
								if (animationsList [i].clip.name == searchFieldText) {
										highlightUnitID = i;
										scrollViewHeight = 0f;
										//scrollViewPos.y = 20 * i;
										return;
								}
						}
				}
		}

	void AnimationClipsArea(){
		GUILayout.BeginVertical ();
			GUILayout.BeginHorizontal ();
				GUILayout.Label ("Animations list",GUILayout.Width(110));
				tempBool = false;
				if (GUILayout.Button ("Clear", GUILayout.Width (60)))
						tempBool = true;
				if (tempBool) {
						if (EditorUtility.DisplayDialog ("Clear animations list", "Do you want to remove all animations from list?", "Yes", "No")) {
								ClearAnimationsList ();	
								tempBool = false;
						}
				}
			GUILayout.EndHorizontal ();
		GUI.color = Color.green;
		GUILayout.Box ("Select desired animation clips "+'\n'+" and click to this area");
		dropArea = GUILayoutUtility.GetLastRect();
		GUI.color = Color.white;
		curEvent = Event.current;
		if(curEvent.type == EventType.MouseDown){
			if(dropArea.Contains(curEvent.mousePosition)){
				area1Clicked = true;
			}else area1Clicked = false;
		}else area1Clicked = false;
	//============ animation clips area searching field section =========================
				GUI.color = tempColor;
				if (showSearchField1 == false) {
						GUILayout.Box ("Search field", GUILayout.Width (180f));
						dropArea = GUILayoutUtility.GetLastRect ();
						if (curEvent.type == EventType.MouseDown) {
								if (dropArea.Contains (curEvent.mousePosition)) {
										UpdateAnimationClipSearchingStatus ();
								} 
						}
				} else
						DrawSearchingField (ref searchFieldText);
				GUI.color = Color.white;
	//===================================================================================
		if(animationsList.Length>0){
			scrollViewPos = GUILayout.BeginScrollView(scrollViewPos);
			DrawAnimationClipsFields();
			GUILayout.EndScrollView();
		}
		GUILayout.EndVertical ();
	}

	void DrawAnimationClipsFields(){
		for(i=0;i<animationsList.Length;i++){
			DrawAnimationClipField(i);
		}
	}

	void DrawAnimationClipField(int id){
				// higlight properties of searching field ===================
				if (highlightUnitID == id && showSearchField1) {
						GUI.color = tempColor;
						GUI.contentColor = tempColor;
				}
				//==========================================================
		GUILayout.BeginHorizontal ();
		GUILayout.Label ((id+1).ToString ());
		if(selectedAnimationClip!=id){
			if(GUILayout.Button("Open")){
				selectedAnimationClip = id;
				//Debug.Log ("animation "+(id+1)+" selected");
			}
		}
		//GUILayout.Box(animationsList[id].clip.name,GUILayout.Width(120));
				EditorGUILayout.SelectableLabel(animationsList[id].clip.name,EditorStyles.boldLabel,GUILayout.Width(120));
				//===============================================================
				curEvent = Event.current;
				if (scrollViewHeight > -1f && curEvent.type == EventType.Repaint && highlightUnitID == id) {
						tempRect = GUILayoutUtility.GetLastRect ();
						scrollViewPos.y = tempRect.y;
						scrollViewHeight = -1f;
						//Debug.Log ("cur rect :" + tempRect);
				}
				//===============================================================
		if(GUILayout.Button("X",GUILayout.Width (20))){
			if(animationsList.Length == 1)
				selectedAnimationClip = -1;
			ArrayUtility.RemoveAt(ref animationsList,id);
						if (highlightUnitID == id && showSearchField1)
								highlightUnitID = -1;
		}
		GUILayout.EndHorizontal ();
		if(selectedAnimationClip!=-1 && id == selectedAnimationClip)
			DrawAnimationClipProperty(id);
				GUI.color = Color.white;
	}

	void DrawAnimationClipProperty(int id){
		animationsList[id].recordTime = EditorGUILayout.FloatField("Record time",animationsList[id].recordTime);
		animationsList[id].createDump = EditorGUILayout.ToggleLeft("Create dump file",animationsList[id].createDump);
		animationsList[id].autoBakeDump = EditorGUILayout.ToggleLeft("Auto bake dump to *.anim",animationsList[id].autoBakeDump);
		if(animationsList[id].bakeForID.Length>0){
		GUI.color = Color.green;
		GUILayout.Box ("Select characters for baking");
		GUI.color = Color.white;
		}
		//=============================================
		animationsList[id].useRootConstraints = EditorGUILayout.ToggleLeft ("Use root constraints",animationsList[id].useRootConstraints);
		if(animationsList[id].useRootConstraints){
			EditorGUILayout.LabelField ("Position constraints:",EditorStyles.boldLabel);
		animationsList[id].rootPositionConstraints.x = EditorGUILayout.Toggle ("x",animationsList[id].rootPositionConstraints.x);
		animationsList[id].rootPositionConstraints.y = EditorGUILayout.Toggle ("y",animationsList[id].rootPositionConstraints.y);
		animationsList[id].rootPositionConstraints.z = EditorGUILayout.Toggle ("z",animationsList[id].rootPositionConstraints.z);
			EditorGUILayout.LabelField ("Rotation constraints:",EditorStyles.boldLabel);
		animationsList[id].rootRotationConstraints.x = EditorGUILayout.Toggle ("x",animationsList[id].rootRotationConstraints.x);
		animationsList[id].rootRotationConstraints.y = EditorGUILayout.Toggle ("y",animationsList[id].rootRotationConstraints.y);
		animationsList[id].rootRotationConstraints.z = EditorGUILayout.Toggle ("z",animationsList[id].rootRotationConstraints.z);
			EditorGUILayout.LabelField ("Scale constraints:",EditorStyles.boldLabel);
		animationsList[id].rootScaleConstraints.x = EditorGUILayout.Toggle ("x",animationsList[id].rootScaleConstraints.x);
		animationsList[id].rootScaleConstraints.y = EditorGUILayout.Toggle ("y",animationsList[id].rootScaleConstraints.y);
		animationsList[id].rootScaleConstraints.z = EditorGUILayout.Toggle ("z",animationsList[id].rootScaleConstraints.z);
		}

		EditorGUILayout.LabelField ("Baking components:",EditorStyles.boldLabel);
		animationsList[id].bakingProperties.x = EditorGUILayout.Toggle ("Rotations",animationsList[id].bakingProperties.x);
		animationsList[id].bakingProperties.y = EditorGUILayout.Toggle ("Positions",animationsList[id].bakingProperties.y);
		animationsList[id].bakingProperties.z = EditorGUILayout.Toggle ("Scales",animationsList[id].bakingProperties.z);
		//=============================================
		if(GUILayout.Button ("Close"))
			selectedAnimationClip = -1;

	}

	void ClearAnimationsList(){
		selectedAnimationClip = -1;
		animationsList = new AnimationClipProperty[0];
	}

	void CharactersArea(){
		GUI.color = Color.gray;
		GUILayout.Box("  ",GUILayout.Height(this.position.height-5f));
		GUI.color = Color.white;
		GUILayout.BeginVertical ();
			GUILayout.BeginHorizontal ();
				GUILayout.Label ("Characters list",GUILayout.Width(110));
				tempBool = false;
				if (GUILayout.Button ("Clear", GUILayout.Width (60))) {
						tempBool = true;
				}
				if (tempBool) {
						if (EditorUtility.DisplayDialog ("Clear characters list", "Do you want to remove all characters from list?", "Yes", "No")) {
								ClearCharactersList ();	
								tempBool = false;
						}
				}
			GUILayout.EndHorizontal ();
		GUI.color = Color.green;
		GUILayout.Box ("Select desired characters "+'\n'+" and click to this area",GUILayout.Width (180f));
		GUI.color = Color.white;
		dropArea = GUILayoutUtility.GetLastRect();
		curEvent = Event.current;
		if(curEvent.type == EventType.MouseDown){
			if(dropArea.Contains(curEvent.mousePosition)){
				area2Clicked = true;
			}else area2Clicked = false;
		}else area2Clicked = false;
				//============ characters area searching field section =========================
				GUI.color = tempColor;
				if (showSearchField2 == false) {
						GUILayout.Box ("Search field", GUILayout.Width (180f));
						dropArea = GUILayoutUtility.GetLastRect ();
						if (curEvent.type == EventType.MouseDown) {
								if (dropArea.Contains (curEvent.mousePosition)) {
										UpdateCharactersSearchingStatus ();
								} 
						}
				} else
						DrawSearchingField (ref searchFieldText);
				GUI.color = Color.white;
				//===================================================================================
		if(charactersList!=null){
			if(charactersList.Length>0){
				scrollViewPos1 = GUILayout.BeginScrollView (scrollViewPos1);
				DrawCharactersFields ();
				GUILayout.EndScrollView();
			}
		}
		GUILayout.EndVertical ();
	}

	void DrawCharactersFields(){
		for(i=0;i<charactersList.Length;i++){
			DrawCharacterField(i);
		}
	}

	void DrawCharacterField(int id){
		GUILayout.BeginHorizontal ();
		if(selectedAnimationClip>-1)
			ShowBakingFlags(id);
		else
			ShowIndicator(Color.yellow,id);
				if (selectedCharacter == id)
						GUI.color = Color.yellow;
				else if (highlightUnitID == id && showSearchField2) {
						GUI.color = tempColor;
				}

		GUILayout.Label ((id+1).ToString (),GUILayout.Width (20));
		GUILayout.Box(charactersList[id].name,GUILayout.Width(100));
				curEvent = Event.current;
				if (scrollViewHeight > -1f && curEvent.type == EventType.Repaint && highlightUnitID == id) {
						tempRect = GUILayoutUtility.GetLastRect ();
						scrollViewPos1.y = tempRect.y;
						scrollViewHeight = -1f;
						//Debug.Log ("cur rect :" + tempRect);
				}
		if(GUILayout.Button("X",GUILayout.Width (20))){
			selectedCharacter = -1;
			RemoveCharacterFromAllAnimationProperties(id);
			ArrayUtility.RemoveAt(ref charactersList,id);
			//UpdateAnimationPropertiesCharactersList();
		}
		if(selectedCharacter == id)
			GUI.color = Color.white;

		GUILayout.EndHorizontal ();
	}


		void UpdateCharactersSearchingStatus(){
				showSearchField2 = true;
				showSearchField1 = false;
				searchFieldText = "";
				highlightUnitID = -1;
				inputCharacters = Selection.GetFiltered(typeof(Transform),SelectionMode.Unfiltered);
				if(inputCharacters != null){
						if(inputCharacters.Length>0){
								searchFieldText = inputCharacters [0].name;
						}
						inputCharacters = null;
				}
		}

		void SearchCharacterInList(){
				if (charactersList.Length < 1)
						return;
				if (highlightUnitID > -1)
						return;
				for (int i = 0; i < charactersList.Length; i++) {
						if (charactersList [i] != null) {
								if (charactersList [i].name == searchFieldText) {
										highlightUnitID = i;
										scrollViewHeight = 0f;
										//scrollViewPos1.y = 0f;
										return;
								}
						}
				}
		}


	void ShowIndicator(Color col,int id){
		GUI.color = col;
		if(GUILayout.Button ("",GUILayout.Width(10),GUILayout.Height(10)))
			selectedCharacter = id;
		GUI.color = Color.white;
	}


	void ShowBakingFlags(int id){
		if(animationsList[selectedAnimationClip].bakeForID.Length>id){
			GUI.color = Color.green;
			animationsList[selectedAnimationClip].bakeForID[id] = GUILayout.Toggle(animationsList[selectedAnimationClip].bakeForID[id],"");
			GUI.color = Color.white;
		}
	}

	void SettingsArea(){
		if(!settingsEnabled){
			if(GUILayout.Button ("Settings"))
				settingsEnabled = true;
		}else{
			EditorGUILayout.LabelField ("Dump folder:",EditorStyles.boldLabel);
			GUILayout.Label (dumpFileFolderPath);
			if(GUILayout.Button ("Select"))
				dumpFileFolderPath = EditorUtility.OpenFolderPanel ("Select parent folder for animation dump",dumpFileFolderPath,"");
			//======================================================================
			EditorGUILayout.LabelField ("Animations folder:",EditorStyles.boldLabel);
			GUILayout.Label (animationFileFolderPath);
			if(GUILayout.Button ("Select"))
				animationFileFolderPath = EditorUtility.OpenFolderPanel ("Select parent folder for animations",animationFileFolderPath,"");
			//=================================================================================
			EditorGUILayout.LabelField ("Preferences folder:",EditorStyles.boldLabel);
			GUILayout.Label (preferencesFolderPath);
			if(GUILayout.Button ("Select"))
				preferencesFolderPath = EditorUtility.OpenFolderPanel ("Select parent folder for animation dump",preferencesFolderPath,"");
			//=======================================================================
			GUILayout.Space (20);
			if(GUILayout.Button ("Close settings"))
				settingsEnabled = false;
			GUILayout.Space (20);
		}
	}

	void MainMenuArea(){
		GUI.color = Color.gray;
		GUILayout.Box("  ",GUILayout.Height(this.position.height-5f));
		GUI.color = Color.white;
		GUILayout.BeginVertical();
		scrollViewPos2 = GUILayout.BeginScrollView(scrollViewPos2);
		SettingsArea();
		//========================Recording options==================================
		if(!playButtonPressed)
		MainMenu_RecordingOptionsArea();	
		//==========================================================================
		if(!startRecord){
		MainMenu_PlayingOptionsArea();
		MainMenu_ConstraintsArea();
		}
		if(!playButtonPressed && !startRecord)
		MainMenu_BakingOptionsArea();
		GUILayout.EndScrollView();
		GUILayout.EndVertical ();
	}

	void MainMenu_RecordingOptionsArea(){
		EditorGUILayout.LabelField ("RECORDING OPTIONS",EditorStyles.boldLabel,GUILayout.ExpandWidth(true));
		sampleRate = EditorGUILayout.IntField("Samples",sampleRate);
		createDumpForAll = EditorGUILayout.ToggleLeft("Create dump for all",createDumpForAll);
		recordFullLengthForAll = EditorGUILayout.ToggleLeft("Record full length for all",recordFullLengthForAll);
		autoBakeDumpForAll = EditorGUILayout.ToggleLeft("Auto bake dump for all",autoBakeDumpForAll);
		useCommonBakingProperties = EditorGUILayout.ToggleLeft("Use common baking properties",useCommonBakingProperties);
		useCommonRootConstraints = EditorGUILayout.ToggleLeft("Use common root constraints",useCommonRootConstraints);
		prefixSubsection = EditorGUILayout.Foldout(prefixSubsection,"Clip name prefix");
		if(prefixSubsection){
			modelNamePrefix = EditorGUILayout.ToggleLeft("Use model name prefix",modelNamePrefix);
			frameRatePrefix = EditorGUILayout.ToggleLeft("Use clip frame rate prefix",frameRatePrefix);
		}
		if(executionModeA){
			if(exMode!="A")
				exMode = "A";
		}else if(exMode!="B") exMode = "B";
		
		GUILayout.BeginHorizontal();
		GUILayout.Label ("Execution mode: "+exMode);
		if(GUILayout.Button ("Change"))
			if(executionModeA)
				executionModeA = false;
		else executionModeA = true;
		GUILayout.EndHorizontal();
		if(!startRecord){
			if(recButtonName!="Start recording")
				recButtonName = "Start recording";
		}else if(recButtonName!="Stop recording") recButtonName="Stop recording";
		if(GUILayout.Button (recButtonName)){
			if(startRecord){
				startRecord = false;
				AnimationBaker.cycleProcessed = true;
				EditorApplication.isPlaying = false;
			}
			else{
				RebuildLists();
				if(animationsList.Length>0 && charactersList.Length>0){
				startRecord = true;
				ResetCounters();
				EditorApplication.isPlaying = true;
				}else Debug.Log("animations list or characters list are empty!");
			}
		}
		GUILayout.Space(10);
	}

	void RebuildLists(){
		int i;
		for(i=0;i<animationsList.Length;i++){
			if(animationsList[i].clip ==null)
				ArrayUtility.RemoveAt (ref animationsList,i);
		}

		for(i=0;i<charactersList.Length;i++){
			if(charactersList[i]==null){
				ArrayUtility.RemoveAt (ref charactersList,i);
				RemoveCharacterFromAllAnimationProperties(i);
			}
		}
	}

	void MainMenu_PlayingOptionsArea(){
		EditorGUILayout.LabelField ("PLAYING OPTIONS",EditorStyles.boldLabel,GUILayout.ExpandWidth(true));
		if(!string.IsNullOrEmpty(dumpFilePath)){
			if(!playButtonPressed)
				playButtonName = "Play preview";
			else playButtonName = "Stop playing";
			if(GUILayout.Button (playButtonName)){
				if(playButtonPressed){
					playButtonPressed = false;
					previewIsPlaying = false;
					if(curCharacter)
						Destroy (curCharacter.gameObject);
					EditorApplication.isPlaying = false;
				}else{
					if(selectedCharacter>-1){
						if(charactersList[selectedCharacter] !=null){
							playButtonPressed = true;
							EditorApplication.isPlaying = true;
						}else info = "Selected character's field is empty!";
					}else info = "Select some character before!";
				}
			}
		}
		if(!playButtonPressed)
		if(GUILayout.Button ("Load dump")){
			dumpFilePath = EditorUtility.OpenFilePanel("Select dump file",dumpFileFolderPath,"anm");
		}
		GUILayout.Label (info);
	}

	void MainMenu_PlayPreview(){
		previewIsPlaying = true;
		curCharacter = Instantiate(charactersList[selectedCharacter],Vector3.zero,Quaternion.identity) as Transform;
		curCharacter.name = charactersList[selectedCharacter].name;
		curCharacter.gameObject.AddComponent<AnimationBaker>();
		baker = curCharacter.GetComponent<AnimationBaker>();
		baker.dumpFilePath = dumpFilePath;
		if(useRootConstraints){
			baker.useRootConstraints = true;
			baker.rootPositionConstraint = rootPositionConstraints;
			baker.rootRotationConstraint = rootRotationConstraints;
			baker.rootScaleConstraint = rootScaleConstraints;
		}
		baker.playAnimation = true;

		if(mainCamera == null){
			mainCamera = Camera.main;
			if(mainCamera == null){
				GameObject dummy = new GameObject();
				dummy.AddComponent<Camera>();
				mainCamera = dummy.GetComponent<Camera>();
			}
		}
		mainCamera.transform.position = new Vector3(curCharacter.position.x,curCharacter.position.y+2f,curCharacter.position.z)+curCharacter.forward*100;
		mainCamera.transform.LookAt(curCharacter.position);
		mainCamera.orthographic = true;
		mainCamera.orthographicSize = 2f;
	}

	void MainMenu_ConstraintsArea(){
		EditorGUILayout.LabelField("ROOT CONSTRAINTS",EditorStyles.boldLabel);
		useRootConstraints = EditorGUILayout.ToggleLeft ("Use root constraints",useRootConstraints);
		if(useRootConstraints){
			EditorGUILayout.LabelField ("Position constraints:",EditorStyles.boldLabel);
			rootPositionConstraints.x = EditorGUILayout.Toggle ("x",rootPositionConstraints.x);
			rootPositionConstraints.y = EditorGUILayout.Toggle ("y",rootPositionConstraints.y);
			rootPositionConstraints.z = EditorGUILayout.Toggle ("z",rootPositionConstraints.z);
			EditorGUILayout.LabelField ("Rotation constraints:",EditorStyles.boldLabel);
			rootRotationConstraints.x = EditorGUILayout.Toggle ("x",rootRotationConstraints.x);
			rootRotationConstraints.y = EditorGUILayout.Toggle ("y",rootRotationConstraints.y);
			rootRotationConstraints.z = EditorGUILayout.Toggle ("z",rootRotationConstraints.z);
			EditorGUILayout.LabelField ("Scale constraints:",EditorStyles.boldLabel);
			rootScaleConstraints.x = EditorGUILayout.Toggle ("x",rootScaleConstraints.x);
			rootScaleConstraints.y = EditorGUILayout.Toggle ("y",rootScaleConstraints.y);
			rootScaleConstraints.z = EditorGUILayout.Toggle ("z",rootScaleConstraints.z);
		}
	}

	void MainMenu_BakingOptionsArea(){
		EditorGUILayout.LabelField ("BAKING OPTIONS",EditorStyles.boldLabel,GUILayout.ExpandWidth(true));
		EditorGUILayout.LabelField("Select baking components:");
		bakingProperties.x = EditorGUILayout.Toggle ("Rotations",bakingProperties.x);
		bakingProperties.y = EditorGUILayout.Toggle ("Positions",bakingProperties.y);
		bakingProperties.z = EditorGUILayout.Toggle ("Scales",bakingProperties.z);
		if(GUILayout.Button("Bake")){
			if(!string.IsNullOrEmpty(dumpFilePath)){
				if(selectedCharacter>-1){
					if(charactersList[selectedCharacter]!=null){
						BakeDump();
					}else Debug.Log ("Selected character's field is empty! Assign character before baking!");
				}else Debug.Log ("Select some character before!");
			}else Debug.Log ("Load animation dump before!");
		}
	}


	void BakeDump(){
		curCharacter = Instantiate(charactersList[selectedCharacter],Vector3.zero,Quaternion.identity) as Transform;
		curCharacter.name = charactersList[selectedCharacter].name;
		curCharacter.gameObject.AddComponent<AnimationBaker>();
		baker = curCharacter.GetComponent<AnimationBaker>();
		baker.LoadAnimationDump(dumpFilePath);
		if(useRootConstraints){
			baker.useRootConstraints = true;
			baker.rootPositionConstraint = rootPositionConstraints;
			baker.rootRotationConstraint = rootRotationConstraints;
			baker.rootScaleConstraint = rootScaleConstraints;
		}
		baker.bakingProperties = bakingProperties;
		baker.ConvertDumpToUnityAnimationFile();
		DestroyImmediate(curCharacter.gameObject);
	}

	void  ProgressInfoArea(){
		if(ProgressInfoEnabled()){
			GUILayout.BeginHorizontal ();
				GUILayout.Label("Processing clip '");
			EditorGUILayout.LabelField (lastStateName,EditorStyles.boldLabel);
				GUILayout.Label("' for character: ");
			EditorGUILayout.LabelField (curCharacterName,EditorStyles.boldLabel);
			GUILayout.EndHorizontal ();
			areaRect = GUILayoutUtility.GetLastRect();
			areaRect = new Rect(areaRect.y,areaRect.x+areaRect.height,areaRect.width-5f,areaRect.height);
			EditorGUI.ProgressBar(areaRect,Mathf.Clamp01(baker.passedTime/baker.recordTime),"Progress");
		}

	}
	

	void ClearCharactersList(){
		for(int i =0;i<animationsList.Length;i++){
			animationsList[i].bakeForID = new bool[0];
		}
		charactersList = new Transform[0];
	}


	void UpdateAnimationPropertiesCharactersList(){
		for(int i =0;i<animationsList.Length;i++){
			RebuildFlags (ref animationsList[i].bakeForID);
		}
	}


	void RebuildFlags(ref bool[] charsList){
		int i;
		if(charsList.Length<charactersList.Length){
			i = charactersList.Length - charsList.Length;
			for(int j=0;j<i;j++){
				ArrayUtility.Add(ref charsList,true);
			}
		}else if(charsList.Length>charactersList.Length){
			i = charsList.Length - charactersList.Length;
			for(int j=0;j<i;j++){
				ArrayUtility.RemoveAt(ref charsList,charsList.Length-1);
			}
		}
	}

	//=================================new=======================
	void AddCharacterToAllAnimationProperties(int id){
		int i;
		for(i=0;i<animationsList.Length;i++){
			AddCharacterToAnimationProperty(ref animationsList[i].bakeForID,id);
		}
	}

	void RemoveCharacterFromAllAnimationProperties(int id){
		for(i=0;i<animationsList.Length;i++){
			RemoveCharacterFromAnimationProperty(ref animationsList[i].bakeForID,id);
		}
	}

	void AddCharacterToAnimationProperty(ref bool[] charsList,int id){
		ArrayUtility.Insert (ref charsList,id,true);
	}

	void RemoveCharacterFromAnimationProperty(ref bool[] charList,int id){
		ArrayUtility.RemoveAt(ref charList,id);
	}

	//==============================================================
	

	void ProcessNextCycle(){
		Debug.Log ("next cycle process, time:"+Time.time);
		AnimationBaker.cycleProcessed = false;
		if(curCharacter!=null){
			Destroy(curCharacter.gameObject);
		}
		if((curAnimationClipId== animationsList.Length-1) && (curCharacterId==charactersList.Length-1)){
			Debug.Log ("All stuff processed!");
			startRecord = false;
			EditorApplication.isPlaying = false;
			AssetDatabase.Refresh();
			return;
		}
		if(executionModeA){
			curAnimationClipId++;
			if(curCharacterId<0)
				curCharacterId = 0;
			if(curAnimationClipId>animationsList.Length-1){
				curAnimationClipId = 0;
				curCharacterId++;
			}
		}else{
			curCharacterId++;
			if(curAnimationClipId<0)
				curAnimationClipId = 0;
			if(curCharacterId>charactersList.Length-1){
				curCharacterId = 0;
				curAnimationClipId++;
			}
		}
		Debug.Log ("cur anim id:"+curAnimationClipId+" cur char id:"+curCharacterId);
		if(animationsList[curAnimationClipId].bakeForID[curCharacterId] == false){
			Debug.Log ("baking of '"+animationsList[curAnimationClipId].clip.name+"' skipped for "+charactersList[curCharacterId].transform.name);
			AnimationBaker.cycleProcessed = true;
			return;
		}
			Debug.Log ("create new character");
		lastStateName = "dummy";
			curCharacter = Instantiate(charactersList[curCharacterId],Vector3.zero,Quaternion.identity) as Transform;
			curCharacter.name = charactersList[curCharacterId].name;
				curCharacterName = curCharacter.name;
			_animator = curCharacter.GetComponent<Animator>();
			_animator.cullingMode = AnimatorCullingMode.AlwaysAnimate; 
		if(baseController == null){
			baseController = AssetDatabase.LoadAssetAtPath("Assets/AnimationBaker/Core/aB_Animator.controller",typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
			Debug.Log ("baseController is empty, trying to load again...");
		}
			_animator.runtimeAnimatorController = baseController;
			AnimatorOverrideController oC = new AnimatorOverrideController();
			oC.runtimeAnimatorController = _animator.runtimeAnimatorController;
			oC[lastStateName] = animationsList[curAnimationClipId].clip;
			_animator.runtimeAnimatorController = oC;
			lastStateName = animationsList[curAnimationClipId].clip.name;
			curCharacter.gameObject.AddComponent<AnimationBaker>();
			baker = curCharacter.GetComponent<AnimationBaker>();
			baker.sourceAnimator = _animator;
			baker.samples = sampleRate;
		if(modelNamePrefix)
			baker.clipNamePrefix +=curCharacter.name+"_";
		if(frameRatePrefix)
			baker.clipNamePrefix += sampleRate.ToString()+"_";
		if(!createDumpForAll)
			baker.createDumpFile = animationsList[curAnimationClipId].createDump;
		else
			baker.createDumpFile = createDumpForAll;
		if(!autoBakeDumpForAll)
			baker.dumpAutoBake = animationsList[curAnimationClipId].autoBakeDump;
		else
			baker.dumpAutoBake = autoBakeDumpForAll;
		if(animationsList[curAnimationClipId].recordTime<0)
			baker.recordTime = animationsList[curAnimationClipId].clip.length;
		else
			baker.recordTime = animationsList[curAnimationClipId].recordTime;
			baker.curAnimationName = lastStateName;
		    baker.delayTime = delay;
			baker.delayPassedTime = 0f;
			baker.sourceAnimator.speed = 0f;
		//applying constraints================================
		if(useCommonBakingProperties)
			baker.bakingProperties = bakingProperties;
		else
			baker.bakingProperties = animationsList[curAnimationClipId].bakingProperties;
		if(useCommonRootConstraints){
			if(useRootConstraints){
				baker.rootPositionConstraint = rootPositionConstraints;
				baker.rootRotationConstraint = rootRotationConstraints;
				baker.rootScaleConstraint = rootScaleConstraints;
				baker.useRootConstraints = true;
			}else baker.useRootConstraints = false;
		}else if(animationsList[curAnimationClipId].useRootConstraints){
		baker.rootPositionConstraint = animationsList[curAnimationClipId].rootPositionConstraints;
		baker.rootRotationConstraint = animationsList[curAnimationClipId].rootRotationConstraints;
		baker.rootScaleConstraint = animationsList[curAnimationClipId].rootScaleConstraints;
		baker.useRootConstraints = true;
		}else baker.useRootConstraints = false;
		//====================================================
			baker.startRecord = true;
		
		if(mainCamera == null){
			mainCamera = Camera.main;
			if(mainCamera == null){
				GameObject dummy = new GameObject();
				dummy.AddComponent<Camera>();
				mainCamera = dummy.GetComponent<Camera>();
			}
		}
		mainCamera.transform.position = new Vector3(curCharacter.position.x,curCharacter.position.y+2f,curCharacter.position.z)+curCharacter.forward*100;
		mainCamera.transform.LookAt(curCharacter.position);
		mainCamera.orthographic = true;
		mainCamera.orthographicSize = 2f;
	}

	void AddSelectedAnimationClips(){
		inputAnimationClips = Selection.GetFiltered(typeof(AnimationClip),SelectionMode.Unfiltered);
		if(inputAnimationClips != null){
			if(inputAnimationClips.Length>0){
				for(int i=0;i<inputAnimationClips.Length;i++){
				tempClip = (AnimationClip) inputAnimationClips[i];
					if(tempClip){
						if(!ClipAlreadyAdded(tempClip)){
							ArrayUtility.Add(ref animationsList,new AnimationClipProperty(tempClip));
							UpdateAnimationPropertiesCharactersList();
							tempClip = null;
						}
					}
				}
			}
			inputAnimationClips = null;
		}
	}

	void AddSelectedCharacters(){
		inputCharacters = Selection.GetFiltered (typeof(Transform),SelectionMode.Assets);
		if(inputCharacters != null){
			if(inputCharacters.Length>0){
				for(int i =0;i<inputCharacters.Length;i++){
				curCharacter = (Transform) inputCharacters[i];
					if(curCharacter){
						if(!CharacterAlreadyAdded(curCharacter)){
							ArrayUtility.Add (ref charactersList,curCharacter);
							AddCharacterToAllAnimationProperties(charactersList.Length-1);
							curCharacter = null;
						}
					}
				}
			}
			inputCharacters = null;
		}
	}

	bool ClipAlreadyAdded(AnimationClip aC){
		bool result = false;
		if(animationsList.Length>0){
			for(int i=0;i<animationsList.Length;i++){
				if(animationsList[i]!=null){
					if(animationsList[i].clip!=null){
						if(animationsList[i].clip == aC)
							result = true;
					}
				}
			}
		}
		return result;
	}

	bool CharacterAlreadyAdded(Transform character){
		bool result = false;
		if(charactersList.Length>0){
			for(int i=0;i<charactersList.Length;i++){
				if(charactersList[i]!=null){
					if(charactersList[i] == character)
						result = true;
				}
			}
		}
		return result;
	}

	void ResetCounters(){
		curCharacterId =-1;
		curAnimationClipId = -1;
		lastStateName = "dummy";
	}


	void UpdateSearchingFields(){
		if (showSearchField1) {
			if (string.IsNullOrEmpty (searchFieldText) == false)
				SearchAnimationClipInList ();
			else if (highlightUnitID > -1)
					highlightUnitID = -1;
			}

		if (showSearchField2) {
				if (string.IsNullOrEmpty (searchFieldText) == false)
						SearchCharacterInList ();
				else if (highlightUnitID > -1)
						highlightUnitID = -1;
		}
	}

	void Update(){
		Repaint ();
		if(area1Clicked)
			AddSelectedAnimationClips();
		
		if(area2Clicked)
			AddSelectedCharacters();
		//============= searching fields update ===============================		
				UpdateSearchingFields();
		//=====================================================================
		if(startRecord && EditorApplication.isPlaying){
			if(lastStateName == "dummy"){
				Debug.Log ("enter from position 1, time:"+Time.time);
				ProcessNextCycle();
			}else if(AnimationBaker.cycleProcessed ){
				Debug.Log ("enter from position 2, time;"+Time.time);
			ProcessNextCycle();
		}
		}

		if(!startRecord && playButtonPressed && !previewIsPlaying && EditorApplication.isPlaying){
			MainMenu_PlayPreview();
		}

	}

	bool ProgressInfoEnabled(){
		bool result = true;
		if(lastStateName =="dummy"){
			//Debug.Log ("current state is dummy!");
			result = false;
		}
		if(curAnimationClipId<0){
			//Debug.Log ("current animation clip id<0");
			result = false;
		}
		if(curCharacterId<0){
			//Debug.Log ("current character id<0");
			result = false;
		}
		if(baker == null){
			//Debug.Log ("current AnimationBaker components is null!");
			result = false;
		}
		return result;

	}
	

	void OnDisable(){
		if(!playButtonPressed && !startRecord){
			if(animationsList.Length>0 || charactersList.Length>0){
			bool savePrefs = EditorUtility.DisplayDialog("Save preferences","Do you want to save assigned clips and characters?","Yes","No");
				if(savePrefs)
					SavePreferences();	
				}
		}
	}


}
