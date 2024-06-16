//#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

using System.IO;

public class UnityAnimationRecorder : MonoBehaviour {

	// save file path
	public string savePath;
	public string fileName;

	// use it when save multiple files
	int fileIndex = 0;

	public KeyCode startRecordKey = KeyCode.Q;
	public KeyCode stopRecordKey = KeyCode.W;

	// options
	public bool showLogGUI = false;
	string logMessage = "";

	public bool recordLimitedFrames = false;
	public int recordFrames = 1000;
	int frameIndex = 0;

	public bool changeTimeScale = false;
	public float timeScaleOnStart = 0.0f;
	public float timeScaleOnRecord = 1.0f;

	public bool recordBlendShape = false;


	Transform[] recordObjs;
	SkinnedMeshRenderer[] blendShapeObjs;
	UnityObjectAnimation[] objRecorders;
	List<UnityBlendShapeAnimation> blendShapeRecorders;

	bool isStart = false;
	float nowTime = 0.0f;

	// Use this for initialization
	void Start () {
		SetupRecorders ();

	}

	void SetupRecorders () {
		recordObjs = gameObject.GetComponentsInChildren<Transform> ();
		objRecorders = new UnityObjectAnimation[recordObjs.Length];
		blendShapeRecorders = new List<UnityBlendShapeAnimation> ();

		frameIndex = 0;
		nowTime = 0.0f;

		for (int i = 0; i < recordObjs.Length; i++) {
			string path = AnimationRecorderHelper.GetTransformPathName (transform, recordObjs [i]);
			objRecorders [i] = new UnityObjectAnimation ( path, recordObjs [i]);

			// check if theres blendShape
			if (recordBlendShape) {
				if (recordObjs [i].GetComponent<SkinnedMeshRenderer> ()) {
					SkinnedMeshRenderer tempSkinMeshRenderer = recordObjs [i].GetComponent<SkinnedMeshRenderer> ();

					// there is blendShape exist
					if (tempSkinMeshRenderer.sharedMesh.blendShapeCount > 0) {
						blendShapeRecorders.Add (new UnityBlendShapeAnimation (path, tempSkinMeshRenderer));
					}
				}
			}
		}

		if (changeTimeScale)
			Time.timeScale = timeScaleOnStart;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKeyDown (startRecordKey)) {
			StartRecording ();
		}

		if (Input.GetKeyDown (stopRecordKey)) {
			StopRecording ();
		}

		if (isStart) {
			nowTime += Time.deltaTime;

			for (int i = 0; i < objRecorders.Length; i++) {
				objRecorders [i].AddFrame (nowTime);
			}

			if (recordBlendShape) {
				for (int i = 0; i < blendShapeRecorders.Count; i++) {
					blendShapeRecorders [i].AddFrame (nowTime);
				}
			}
		}

	}

	public void StartRecording () {
		CustomDebug ("Start Recorder");
		isStart = true;
		Time.timeScale = timeScaleOnRecord;
	}


	public void StopRecording () {
		CustomDebug ("End Record, generating .anim file");
		isStart = false;

		ExportAnimationClip ();
		ResetRecorder ();
	}

	void ResetRecorder () {
		SetupRecorders ();
	}


	void FixedUpdate () {

		if (isStart) {

			if (recordLimitedFrames) {
				if (frameIndex < recordFrames) {
					for (int i = 0; i < objRecorders.Length; i++) {
						objRecorders [i].AddFrame (nowTime);
					}

					++frameIndex;
				}
				else {
					isStart = false;
					ExportAnimationClip ();
					CustomDebug ("Recording Finish, generating .anim file");
				}
			}

		}
	}

	void OnGUI () {
		if (showLogGUI)
			GUILayout.Label (logMessage);
	}

	void ExportAnimationClip () {

        string savePath = Application.persistentDataPath + "/Animations";
        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);
		string fileName = string.Format("{0:F0}", Time.realtimeSinceStartup * 10f) + ".anim";
        string exportFilePath = savePath + "/" + fileName;
        //string exportFilePath = savePath + fileName;

		// if record multiple files when run
		//if (fileIndex != 0)
		//	exportFilePath += "-" + fileIndex + ".anim";
		//else
		//	exportFilePath += ".anim";


		AnimationClip clip = new AnimationClip ();
		clip.name = fileName;

		for (int i = 0; i < objRecorders.Length; i++) {
			UnityCurveContainer[] curves = objRecorders [i].curves;

			for (int x = 0; x < curves.Length; x++) {
				clip.SetCurve (objRecorders [i].pathName, typeof(Transform), curves [x].propertyName, curves [x].animCurve);
			}
		}

		if (recordBlendShape) {
			for (int i = 0; i < blendShapeRecorders.Count; i++) {

				UnityCurveContainer[] curves = blendShapeRecorders [i].curves;

				for (int x = 0; x < curves.Length; x++) {
					clip.SetCurve (blendShapeRecorders [i].pathName, typeof(SkinnedMeshRenderer), curves [x].propertyName, curves [x].animCurve);
				}
				
			}
		}

		clip.EnsureQuaternionContinuity ();

		//string clipString = SerializeAnimationClip (clip);
		//File.WriteAllText(exportFilePath, clipString);
        //AssetDatabase.CreateAsset ( clip, exportFilePath );
        //File.WriteAllBytes(exportFilePath, clip.);
        CustomDebug (".anim file generated to " + exportFilePath);
		fileIndex++;
	}

	void CustomDebug ( string message ) {
		if (showLogGUI)
			logMessage = message;
		else
			Debug.Log (message);
	}

    //static string SerializeAnimationClip(AnimationClip clip)
    //{
    //    // Using JSON as an example format
    //    AnimationClipData clipData = new AnimationClipData(clip);

    //    return JsonUtility.ToJson(clipData, true);
    //}
}

//[System.Serializable]
//public class AnimationClipData
//{
//    public string name;
//    public float length;
//    public List<AnimationCurveData> curves = new List<AnimationCurveData>();

//    public AnimationClipData(AnimationClip clip)
//    {
//        name = clip.name;
//        length = clip.length;

//        foreach (var binding in GetCurveBindings(clip))
//        {
//            AnimationCurve curve = GetCurve(clip, binding);
//            if (curve != null)
//            {
//                curves.Add(new AnimationCurveData(binding.path, binding.propertyName, binding.type, curve));
//            }
//        }
//    }

//    private IEnumerable<EditorCurveBinding> GetCurveBindings(AnimationClip clip)
//    {
//        // Use reflection to get private method GetAllBindings
//        var method = typeof(AnimationClip).GetMethod("GetAllBindings", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
//        return (IEnumerable<EditorCurveBinding>)method.Invoke(clip, null);
//    }

//    private AnimationCurve GetCurve(AnimationClip clip, EditorCurveBinding binding)
//    {
//        // Use reflection to get private method GetEditorCurve
//        var method = typeof(AnimationClip).GetMethod("GetEditorCurve", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
//        return (AnimationCurve)method.Invoke(clip, new object[] { binding });
//    }
//}

//[System.Serializable]
//public class AnimationCurveData
//{
//    public string path;
//    public string propertyName;
//    public string type;
//    public List<KeyframeData> keyframes = new List<KeyframeData>();

//    public AnimationCurveData(string path, string propertyName, System.Type type, AnimationCurve curve)
//    {
//        this.path = path;
//        this.propertyName = propertyName;
//        this.type = type.FullName;

//        foreach (Keyframe key in curve.keys)
//        {
//            keyframes.Add(new KeyframeData(key));
//        }
//    }
//}

//[System.Serializable]
//public class KeyframeData
//{
//    public float time;
//    public float value;
//    public float inTangent;
//    public float outTangent;

//    public KeyframeData(Keyframe key)
//    {
//        time = key.time;
//        value = key.value;
//        inTangent = key.inTangent;
//        outTangent = key.outTangent;
//    }
//}
//#endif