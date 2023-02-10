using UnityEngine;
using System.Collections;
using System.Reflection;

public class AnimationBaker : MonoBehaviour {
	public int samples = 60;
	public float recordTime = 2f;
	public Transform sourceSkeleton;
	float nextSampleTime =0f;
	float sampleStep = 1f;
	int curBoneId = 0,curSample=-1,totalSamples;
	public AnimationFile animClip = new AnimationFile("",-1,-1,new BoneState[0]);
	bool countersReseted = false;
	public bool animateAtPlace = true;
	public bool rotateAtPlace = true;
	public Constraint rootPositionConstraint = new Constraint(false,false,false);
	public Constraint rootRotationConstraint = new Constraint(false,false,false);
	public Constraint rootScaleConstraint = new Constraint(false,false,false);
	//what kind of properties should bake(rotations,positions,scale values)
	public Constraint bakingProperties = new Constraint(true,false,false);
	public bool loop = true;
	public bool startRecord = false,playAnimation = false,isRecording = false;
	public bool dumpAutoBake = false,useRootConstraints = false;
	public bool createDumpFile = true;
	public static bool cycleProcessed = false;
	public bool createUnityAnimation = false;
	public string animationFileFolderPath = "Assets/AnimationBaker/UnityAnimations/",dumpFileFolderPath = "Assets/AnimationBaker/AnimationDump/";
	public string curAnimationName,dumpFilePath="",clipNamePrefix = "";
	Quaternion tempRot;
	Vector3 tempV3;
	SkeletonInfo skeletonInfo = new SkeletonInfo(new string[0],null,0);
	public Animator sourceAnimator;
	Animation sourceAnimation;
	public float passedTime  = 0f;
	public float delayTime = 0.5f,delayPassedTime = 0f;
	bool canPlay = false;
	MethodInfo methodInfo;
	System.Type animClipType;
	

	[System.Serializable]
	public struct BoneState
	{
		public BoneStateSample[] samples;

		public BoneState(BoneStateSample[] s){
			samples = s;
		}

		public void AddSample(BoneStateSample keyframe){
			if(samples == null)
			samples = new BoneStateSample[0];
#if UNITY_EDITOR
			UnityEditor.ArrayUtility.Add(ref samples,keyframe);
#endif
		}
	}


	[System.Serializable]
	public struct BoneStateSample
	{
		public Vector3S pos;
		public QuaternionS rot;
		public Vector3S scale;

		public BoneStateSample(Vector3 p,Quaternion r , Vector3 s){
			pos = new Vector3S(p);
			rot = new QuaternionS(r);
			scale = new Vector3S(s);
		}

		public BoneStateSample(Transform obj){
			pos = new Vector3S(obj.localPosition);
			rot = new QuaternionS(obj.localRotation);
			scale = new Vector3S(obj.localScale);
		}

		[System.Serializable]
		public struct Vector3S
		{
			public float x,y,z;

			public Vector3S(Vector3 v3){
				x = v3.x;
				y = v3.y;
				z = v3.z;
			}

			public Vector3 Read(){
				return new Vector3(x,y,z);
			}
		}

		[System.Serializable]
		public struct QuaternionS
		{
			public float x,y,z,w;

			
			public QuaternionS(Quaternion rot){
				x = rot.x;
				y = rot.y;
				z = rot.z;
				w = rot.w;

			}
			
			public Quaternion Read(){
				return new Quaternion(x,y,z,w);
			}
		}
	}

	[System.Serializable]
	public struct AnimationFile
	{
		public string name;
		public int samples,totalSamples;
		public BoneState[] bonesStates;

		public AnimationFile(string n,int s,int tS,BoneState[] bS){
			name = n;
			samples = s;
			totalSamples = tS;
			bonesStates = bS;
		}

		public bool IsEmpty(){
			bool result = false;
			if(string.IsNullOrEmpty(name))
				result = true;
			if(samples<1 || totalSamples<1)
				result = true;
			return result;
		}
	}

	[System.Serializable]
	public class Constraint
	{
		public bool x,y,z;

		public Constraint(bool newX,bool newY, bool newZ){
			x = newX;
			y = newY;
			z = newZ;
		}

		public bool HasConstraint(){
			if(x || y||z)
				return true;
			else return false;
		}
	}

	public struct SkeletonInfo
	{
		public string[] bonesPaths;
		public Transform skeleton;
		public int bonesCount;

		public SkeletonInfo(string[] bP,Transform s,int bC){
			bonesPaths = bP;
			skeleton = s;
			bonesCount = bC;
		}

		public void UpdateSkeletonInfo(bool debug){
			ReadSkeletonHierarchy(skeleton,debug);
			Debug.Log ("skeleton name:"+skeleton.name);
			Debug.Log ("bones count:"+bonesCount);
		}

		void ReadSkeletonHierarchy(Transform h, bool debug){
			if(!h){
				Debug.Log ("Empty bone detected!");
				return;
			}
			string bonePath = "";
#if UNITY_EDITOR
			if(bonesCount>0)
				bonePath = UnityEditor.AnimationUtility.CalculateTransformPath(h,skeleton);
					UnityEditor.ArrayUtility.Add(ref bonesPaths,bonePath);
#endif
			if(debug)
			Debug.Log ("bone "+h.name+" path:"+bonePath);
			bonesCount++;
			foreach(Transform bone in h){
				ReadSkeletonHierarchy(bone,debug);
			}
		}
	}

	//void Awake(){
		//Time.fixedDeltaTime = (float)1/samples;
	//}

	// Use this for initialization
	void Start () {
		if(!sourceSkeleton)
			sourceSkeleton = transform;
		sampleStep = (float)1/samples;
		Time.fixedDeltaTime = sampleStep;
		skeletonInfo.skeleton = sourceSkeleton;
		skeletonInfo.UpdateSkeletonInfo(false);
		animClip.bonesStates = new BoneState[skeletonInfo.bonesCount];
		//sourceAnimator = GetComponent<Animator>();
		if(sourceAnimator){
		//sourceAnimator.enabled = false;
			Debug.Log ("cur character:"+transform.name+"; animation name:"+curAnimationName+"; delayPassedTime:"+delayPassedTime+";" +
			           "; delay:"+delayTime+"; Time.time:"+Time.time+"sample rate:"+samples+"(sample step = "+sampleStep+")");
		}
		//else Debug.Log ("Can't read animation: animator not found!");
		sourceAnimation = GetComponent<Animation>();
	}
	

	void Update(){
		delayPassedTime+=Time.deltaTime;
		if(delayPassedTime<delayTime){
			if(canPlay) {
				sourceAnimator.Play("dummy",0,0.0f);
		}
		}

	}

	void FixedUpdate(){
		if(playAnimation){
			if(!animClip.IsEmpty())
				PlayAnimation();
			else if(!string.IsNullOrEmpty(dumpFilePath)) 
				LoadAnimationDump(dumpFilePath);
		}
		if(!sourceAnimator)
			return;
		if(canPlay==false){
			recordTime+=Time.fixedDeltaTime;
			canPlay = true;
		}

		if(startRecord && passedTime> recordTime && recordTime>-1f){
			Debug.Log ("passed time:"+ passedTime+"; recordTime:"+ recordTime);
			passedTime = 0f;
			recordTime = -1f;
			startRecord = false;
		}

		if(startRecord){
			if(sourceAnimator.enabled==false)
			sourceAnimator.enabled = true;
			if(delayPassedTime>=delayTime){
				if(sourceAnimator.speed<1f)
					sourceAnimator.speed = 1f;
				passedTime+=Time.fixedDeltaTime;
				ReadAnimation ();
				isRecording = true;
			}
		}else if(isRecording){
			sourceAnimator.enabled = false;
			if(countersReseted == false)
				ResetFlags();
		}

	}

	public void LoadAnimationDump(string filePath){
		animClip = (AnimationFile) BINS.Load (filePath);
	}

	public void ResetAnimationDump(){
		animClip = new AnimationFile("dump",-1,-1,new BoneState[0]);
	}
	

	 void ReadWriteHierarchy(Transform h,bool record){
		if(!h){
			Debug.Log ("Empty skeleton detected!");
			return;
		}
		if(!record){
			if(curBoneId ==0 && rootPositionConstraint.HasConstraint()){
				tempV3 = h.localPosition;
				if(!rootPositionConstraint.x)
					tempV3.x = animClip.bonesStates[curBoneId].samples[curSample].pos.Read().x;
				if(!rootPositionConstraint.y)
					tempV3.y = animClip.bonesStates[curBoneId].samples[curSample].pos.Read().y;
				if(!rootPositionConstraint.z)
					tempV3.z = animClip.bonesStates[curBoneId].samples[curSample].pos.Read().z;
				h.localPosition = tempV3;
			}else{
				//Debug.Log ("cur sample:"+curSample+"curBone:"+curBoneId+"source skeleton bone name:"+h.name);
				//Debug.Log ( "bone position"+animClip.bonesStates[curBoneId].samples[curSample].pos.Read ().ToString());
				h.localPosition = animClip.bonesStates[curBoneId].samples[curSample].pos.Read ();
			}
			if(curBoneId == 0 && rootRotationConstraint.HasConstraint()){
				tempV3 = h.localEulerAngles;
				if(!rootRotationConstraint.x)
					tempV3.x = animClip.bonesStates[curBoneId].samples[curSample].rot.Read().eulerAngles.x;
				if(!rootRotationConstraint.y)
					tempV3.y = animClip.bonesStates[curBoneId].samples[curSample].rot.Read().eulerAngles.y;
				if(!rootRotationConstraint.z)
					tempV3.z = animClip.bonesStates[curBoneId].samples[curSample].rot.Read().eulerAngles.z;
				h.localEulerAngles = tempV3;
			}else h.localRotation = animClip.bonesStates[curBoneId].samples[curSample].rot.Read();

			h.localScale = animClip.bonesStates[curBoneId].samples[curSample].scale.Read ();
		}else{
			tempRot = h.localRotation;
			animClip.bonesStates[curBoneId].AddSample (new BoneStateSample(h.localPosition,tempRot,h.localScale));
		}
			//Debug.Log ("bone '"+h.name+"' local angles recorded :"+bonesAngles[curBoneId,curSample]);
		curBoneId++;
		foreach(Transform bone in h){
			ReadWriteHierarchy(bone,record);
		}
	}

	void ReadAnimation(){
		curBoneId = 0;
		curSample++;
		ReadWriteHierarchy(sourceSkeleton,true);
	}

	void PlayAnimation(){
		if(!sourceSkeleton){
			Debug.Log ("Source skeleton not found, can't play animation!");
			return;
		}
		if(animClip.totalSamples<1){
			Debug.Log ("Can't play animation, animation file is empty!");
			return;
		}
		if(sourceAnimator)
			if(sourceAnimator.enabled)
				sourceAnimator.enabled = false;
		if(sourceAnimation)
			if(sourceAnimation.enabled)
				sourceAnimation.enabled = false;
	
		if(curSample >= animClip.totalSamples){
			if(loop){
				curSample = -1;
			}
			return;
		}
		curBoneId = 0;
		curSample++;
		ReadWriteHierarchy(sourceSkeleton,false);
	}

	void ResetFlags(){
		totalSamples = curSample;
		curSample = -1;
		nextSampleTime = 0f;
		Debug.Log ("flags resetted,total samples:"+totalSamples);
		countersReseted = true;
		animClip.samples = samples;
		animClip.totalSamples = totalSamples;
		animClip.name = curAnimationName;
		isRecording = false;
		if(createDumpFile){
			string newDumpFileFolderPath = dumpFileFolderPath+sourceSkeleton.name+"/";
			BINS.CreateFolder(newDumpFileFolderPath);
			BINS.Save(animClip,newDumpFileFolderPath,clipNamePrefix+animClip.name+".anm");
			//AssetDatabase.Refresh();
			//EditorApplication.isPlaying = false;
		}

		if(dumpAutoBake){
			ConvertDumpToUnityAnimationFile();
			//if(EditorApplication.isPaused || EditorApplication.isPlaying)
				//EditorApplication.isPlaying = false;
		}
		AnimationBaker.cycleProcessed = true;
		string status = "cycle status:";
		if(AnimationBaker.cycleProcessed)
			status+=" processed";
		else status+=" in progress";
		Debug.Log (status);
	}

	public void ConvertDumpToUnityAnimationFile(){
		if(animClip.IsEmpty()){
			Debug.Log ("Current dump is empty, nothing to bake!");
			return;
		}
		if(!bakingProperties.HasConstraint()){
			Debug.Log ("Can't bake animation file: no one property selected!");
			return;
		}
		if(!sourceSkeleton)
			sourceSkeleton = transform;
		curAnimationName = animClip.name;
		AnimationClip uAnimClip = new AnimationClip();
		uAnimClip.frameRate = animClip.samples;
		uAnimClip.name = curAnimationName;
		skeletonInfo = new SkeletonInfo(new string[0],sourceSkeleton,0); 
		skeletonInfo.UpdateSkeletonInfo(true);
		sampleStep = (float)1/animClip.samples;
		for(int i = 0;i<(skeletonInfo.bonesCount);i++){
			uAnimClip = WriteCurveToClip(uAnimClip,i);	
		}
		animClipType = typeof(AnimationClip);
		methodInfo = animClipType.GetMethod ("set_legacy");
		//if (methodInfo != null) {
			//methodInfo.Invoke(uAnimClip,new object[]{true});
		//}
		string newAnimationFileFolderPath = animationFileFolderPath+sourceSkeleton.name+"/";
		BINS.CreateFolder(newAnimationFileFolderPath);
		//Debug.Log ("new animation folder path:"+newAnimationFolderPath);
#if UNITY_EDITOR
			UnityEditor.AssetDatabase.CreateAsset(uAnimClip,newAnimationFileFolderPath+clipNamePrefix+uAnimClip.name+".anim");
			UnityEditor.AssetDatabase.SaveAssets();
#endif
		//AssetDatabase.Refresh();
		Debug.Log ("animation file was created:"+uAnimClip.name+".anim");
	}

	AnimationClip WriteCurveToClip(AnimationClip aC,int boneIndex){
		AnimationClip result = aC;
		AnimationCurve aCurve;
		Keyframe[] keysX = new Keyframe[animClip.totalSamples],keysY = new Keyframe[animClip.totalSamples],keysZ = new Keyframe[animClip.totalSamples];
		nextSampleTime = 0f;
		bool rootConstraint = false;
		#if UNITY_EDITOR
		if(boneIndex ==0 && useRootConstraints)
			rootConstraint = true;
		//write position of this bone for all samples
		//===================================================================================
		if(bakingProperties.y || rootConstraint){
		for(int i=0;i<keysX.Length;i++){
			keysX[i] = new Keyframe(nextSampleTime,animClip.bonesStates[boneIndex].samples[i].pos.x);
				keysY[i] = new Keyframe(nextSampleTime,animClip.bonesStates[boneIndex].samples[i].pos.y);
				keysZ[i] = new Keyframe(nextSampleTime,animClip.bonesStates[boneIndex].samples[i].pos.z);
			nextSampleTime+=sampleStep;
		}
	
			if(rootConstraint && rootPositionConstraint.x ==true)
				keysX = new Keyframe[0];
			if(rootConstraint && rootPositionConstraint.y ==true)
				keysY = new Keyframe[0];
			if(rootConstraint && rootPositionConstraint.z ==true)
				keysZ = new Keyframe[0];
			if(keysX.Length>0){
				aCurve = new AnimationCurve(keysX);
				//result.SetCurve(skeletonInfo.bonesPaths[boneIndex],typeof(Transform),"localPosition.x",aCurve);
				UnityEditor.AnimationUtility.SetEditorCurve(result,UnityEditor.EditorCurveBinding.FloatCurve(skeletonInfo.bonesPaths[boneIndex],
				                                                                                             typeof(Transform),"m_LocalPosition.x"),aCurve);
			}
			if(keysY.Length>0){
				aCurve = new AnimationCurve(keysY);
				//result.SetCurve(skeletonInfo.bonesPaths[boneIndex],typeof(Transform),"localPosition.y",aCurve);
				UnityEditor.AnimationUtility.SetEditorCurve(result,UnityEditor.EditorCurveBinding.FloatCurve(skeletonInfo.bonesPaths[boneIndex],
				                                                                                             typeof(Transform),"m_LocalPosition.y"),aCurve);
			}

			if(keysZ.Length>0){
				aCurve = new AnimationCurve(keysZ);
				//result.SetCurve(skeletonInfo.bonesPaths[boneIndex],typeof(Transform),"localPosition.z",aCurve);
				UnityEditor.AnimationUtility.SetEditorCurve(result,UnityEditor.EditorCurveBinding.FloatCurve(skeletonInfo.bonesPaths[boneIndex],
				                                                                                             typeof(Transform),"m_LocalPosition.z"),aCurve);
			}
			
		}
		//=====================================================================================
		//write rotation of this bone for all samples
		if(bakingProperties.x || rootConstraint){
			keysX = new Keyframe[animClip.totalSamples];
			keysY = new Keyframe[animClip.totalSamples];
			keysZ = new Keyframe[animClip.totalSamples];
			//Keyframe[] keysW = new Keyframe[animClip.totalSamples];
			nextSampleTime = 0f;
			Vector3 testAxis = Vector3.zero;
			float angle = 0f;

			for(int i=0;i<keysX.Length;i++){
				tempRot.Set (animClip.bonesStates[boneIndex].samples[i].rot.x,animClip.bonesStates[boneIndex].samples[i].rot.y,
				             animClip.bonesStates[boneIndex].samples[i].rot.z,animClip.bonesStates[boneIndex].samples[i].rot.w);
				/*
				tempRot.ToAngleAxis(out angle,out testAxis);
				if(angle>180f){
					angle = (360f-angle)*-1f;
					tempRot = Quaternion.AngleAxis(angle,testAxis);
				}
				*/
				testAxis = tempRot.eulerAngles;

				if(testAxis.x>180f){
					testAxis.x = (360f-testAxis.x)*-1;
				}
				if(testAxis.y>180f){
					testAxis.y = (360f-testAxis.y)*-1;
				}
				if(testAxis.z>180f){
					testAxis.z = (360f-testAxis.z)*-1;
				}


				keysX[i] = new Keyframe(nextSampleTime,testAxis.x);
				keysY[i] = new Keyframe(nextSampleTime,testAxis.y);
				keysZ[i] = new Keyframe(nextSampleTime,testAxis.z);

			nextSampleTime+=sampleStep;
			}

			if(rootConstraint && rootRotationConstraint.x == true){
				keysX = new Keyframe[0];
			}
			if(keysX.Length>0){
			aCurve = new AnimationCurve(keysX);
			UnityEditor.AnimationUtility.SetEditorCurve(result,UnityEditor.EditorCurveBinding.FloatCurve(skeletonInfo.bonesPaths[boneIndex],
			                                                                                             typeof(Transform),"localEulerAnglesBaked.x"),aCurve);
			}
			
			if(rootConstraint && rootRotationConstraint.y != true){
				keysY = new Keyframe[0];
			}
			if(keysY.Length>0){
			aCurve = new AnimationCurve(keysY);
			UnityEditor.AnimationUtility.SetEditorCurve(result,UnityEditor.EditorCurveBinding.FloatCurve(skeletonInfo.bonesPaths[boneIndex],
			                                                                                             typeof(Transform),"localEulerAnglesBaked.y"),aCurve);
			}
			
			if(rootConstraint && rootRotationConstraint.z == true){
				keysZ = new Keyframe[0];
			}
			if(keysZ.Length>0){
			aCurve = new AnimationCurve(keysZ);
			UnityEditor.AnimationUtility.SetEditorCurve(result,UnityEditor.EditorCurveBinding.FloatCurve(skeletonInfo.bonesPaths[boneIndex],
			                                                                                             typeof(Transform),"localEulerAnglesBaked.z"),aCurve);
			}
		}
		//======================================================================================
		//=====================================================================================
		//write scale of this bone for all samples
			if(bakingProperties.z || rootConstraint){
		keysX = new Keyframe[animClip.totalSamples];
		keysY = new Keyframe[animClip.totalSamples];
		keysZ = new Keyframe[animClip.totalSamples];
		nextSampleTime = 0f;
		for(int i=0;i<keysX.Length;i++){
				keysX[i] = new Keyframe(nextSampleTime,animClip.bonesStates[boneIndex].samples[i].scale.x);
				keysY[i] = new Keyframe(nextSampleTime,animClip.bonesStates[boneIndex].samples[i].scale.y);
				keysZ[i] = new Keyframe(nextSampleTime,animClip.bonesStates[boneIndex].samples[i].scale.z);
			nextSampleTime+=sampleStep;
		}
				if(!rootConstraint ||(rootConstraint && rootScaleConstraint.x ==false)){
			aCurve = new AnimationCurve(keysX);
	UnityEditor.AnimationUtility.SetEditorCurve(result,UnityEditor.EditorCurveBinding.FloatCurve(skeletonInfo.bonesPaths[boneIndex],
				                                                                                             typeof(Transform),"m_LocalScale.x"),aCurve);
		}
				if(!rootConstraint ||(rootConstraint && rootScaleConstraint.y ==false)){
			aCurve = new AnimationCurve(keysY);
			//result.SetCurve(skeletonInfo.bonesPaths[boneIndex],typeof(Transform),"localScale.y",aCurve);
	UnityEditor.AnimationUtility.SetEditorCurve(result,UnityEditor.EditorCurveBinding.FloatCurve(skeletonInfo.bonesPaths[boneIndex],
				                                                                                             typeof(Transform),"m_LocalScale.y"),aCurve);
		}
				if(!rootConstraint ||(rootConstraint && rootScaleConstraint.z ==false)){
			aCurve = new AnimationCurve(keysZ);
			//result.SetCurve(skeletonInfo.bonesPaths[boneIndex],typeof(Transform),"localScale.z",aCurve);
	UnityEditor.AnimationUtility.SetEditorCurve(result,UnityEditor.EditorCurveBinding.FloatCurve(skeletonInfo.bonesPaths[boneIndex],
				                                                                                             typeof(Transform),"m_LocalScale.z"),aCurve);
		}
		}
		//======================================================================================
#endif
		return result;
	}
				
}
