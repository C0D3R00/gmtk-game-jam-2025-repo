//using System.Collections.Generic;
//using UnityEngine;

//public class TransformRecorder : MonoBehaviour
//{
//    public List<TransformFrame> frames = new();
//    private float startTime;
//    private bool isRecording;

//    public void StartRecording()
//    {
//        frames.Clear();
//        startTime = Time.time;
//        isRecording = true;
//    }

//    public void StopRecording()
//    {
//        isRecording = false;
//    }

//    void Update()
//    {
//        if (!isRecording) return;

//        float timestamp = Time.time - startTime;
//        frames.Add(new TransformFrame(timestamp, transform.position, transform.rotation));
//    }
//}
