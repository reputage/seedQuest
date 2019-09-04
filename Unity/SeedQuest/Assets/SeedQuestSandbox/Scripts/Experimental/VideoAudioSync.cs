using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

public class VideoAudioSync : MonoBehaviour
{
    public VideoClip videoSource;
    public RenderTexture videoTexture;
    public bool loop;
    public float minDistance;
    public float maxDistance;
    public float volume;

    private VideoPlayer videoComponent;
    private AudioSource audioComponent;
    private bool videoStarted;

    private void Awake()
    {
        videoStarted = false;
        videoComponent = gameObject.AddComponent<VideoPlayer>();
        audioComponent = gameObject.AddComponent<AudioSource>();

        videoComponent.playOnAwake = false;
        audioComponent.playOnAwake = false;

        videoComponent.source = VideoSource.VideoClip;
        videoComponent.clip = videoSource;
        videoComponent.isLooping = loop;
        videoComponent.targetTexture = videoTexture;
        videoComponent.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoComponent.controlledAudioTrackCount = 1;

        //audioComponent.clip = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/SeedQuestPrototype/Sounds/empty.mp3", typeof(AudioClip));
        audioComponent.loop = loop;
        audioComponent.volume = volume;
        audioComponent.spatialBlend = 1;
        audioComponent.rolloffMode = AudioRolloffMode.Logarithmic;
        audioComponent.minDistance = minDistance;
        audioComponent.maxDistance = maxDistance;

        videoComponent.EnableAudioTrack(0, true);
        videoComponent.SetTargetAudioSource(0, audioComponent);

    }

    private void Update()
    {
        if(!videoComponent.isPlaying && !videoStarted)
        {
            videoStarted = true;
            StartCoroutine(StartVideo());
        }
    }

    private IEnumerator StartVideo()
    {
        videoComponent.Stop();
        videoComponent.Prepare();

        while (!videoComponent.isPrepared)
        {
            yield return new WaitForEndOfFrame();
        }

        videoComponent.Play();
    }
}

/*[CustomEditor(typeof(VideoAudioSync))]
public class VideoAudioSyncEditor : Editor
{

    public override void OnInspectorGUI()
    {
        VideoAudioSync vas = (VideoAudioSync)target;

        vas.videoSource = (VideoClip)EditorGUILayout.ObjectField ("Video Source", vas.videoSource, typeof(VideoClip));
        vas.videoTexture = (RenderTexture)EditorGUILayout.ObjectField ("Video Render Texture", vas.videoTexture, typeof(RenderTexture));
        vas.audioSource = (AudioClip)EditorGUILayout.ObjectField ("Audio Source", vas.audioSource, typeof(AudioClip));
        vas.loop = EditorGUILayout.Toggle("Loop", vas.loop);
        vas.minDistance = EditorGUILayout.FloatField("Min Distance", vas.minDistance);
        vas.maxDistance = EditorGUILayout.FloatField("Max Distance", vas.maxDistance);
        vas.volume = EditorGUILayout.Slider("Volume", vas.volume, 0, 1);
    }
}*/