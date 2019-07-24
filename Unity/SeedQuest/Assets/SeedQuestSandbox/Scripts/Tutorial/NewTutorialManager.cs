using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using SeedQuest.Interactables;

public class NewTutorialManager : MonoBehaviour
{
    private Canvas[] canvas;
    private int currentCanvasIndex;
    private Vector3 playerStartPosition;
    private bool coroutineStarted = false;
    private bool recall = false;

    static private NewTutorialManager instance = null;
    static private NewTutorialManager setInstance() { instance = GameObject.FindObjectOfType<NewTutorialManager>(); return instance; }
    static public NewTutorialManager Instance { get { return instance == null ? setInstance() : instance; } }

    void Awake()
    {
        GameManager.Mode = GameMode.Rehearsal;
        GameManager.TutorialMode = true;
        canvas = GetComponentsInChildren<Canvas>(true);
        currentCanvasIndex = 0;
        playerStartPosition = GameObject.FindGameObjectWithTag("Player").transform.localPosition;
        InteractablePathManager.SeedString = "A09206A09206000000000000000000000";
        InteractablePathManager.SetupInteractablePathIDs();
        InteractablePathManager.Initalize();
    }

    private void Update()
    {
        if (currentCanvasIndex == 5)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (Vector3.Distance(playerStartPosition, player.transform.localPosition) > 0.037)
            {
                if (!coroutineStarted)
                {
                    coroutineStarted = true;
                    StartCoroutine(WaitNextCanvas(1.5f));
                }
            }
        }

        else if (currentCanvasIndex == 10)
        {
            if (InteractableLog.Count > 0)
            {
                NextCanvas();
            }
        }

        else if (currentCanvasIndex == 11)
        {
            if (InteractableLog.Count == 2)
            {
                GoToCanvas(13);
            }
        }

        else if (currentCanvasIndex == 12)
        {
            if (InteractableLog.Count == 0)
            {
                GoToCanvas(11);
            }
        }

        else if (currentCanvasIndex == 13)
        {
            if (InteractableLog.Count == 3)
            {
                InteractablePathManager.Reset();
                InteractablePreviewUI.ToggleShow();
                InteractableManager.UnHighlightAllInteractables();
                InteractableManager.UnTrackAllInteractables();
                ParticleSystem[] particles = FindObjectsOfType<ParticleSystem>();
                foreach (ParticleSystem particle in particles)
                    particle.Stop();
                GoToCanvas(14);
            }
        }

        else if (currentCanvasIndex == 14)
        {
            if (recall)
            {
                if (InteractableLog.Count == 3)
                {
                    GoToCanvas(15);
                }
            }
        }
    }

    public void NextCanvas()
    {
        canvas[currentCanvasIndex].gameObject.SetActive(false);
        currentCanvasIndex++;
        canvas[currentCanvasIndex].gameObject.SetActive(true);
    }

    public void GoToCanvas(int canvasIndex)
    {
        canvas[currentCanvasIndex].gameObject.SetActive(false);
        currentCanvasIndex = canvasIndex;
        canvas[currentCanvasIndex].gameObject.SetActive(true);
    }

    public IEnumerator WaitNextCanvas(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        NextCanvas();
    }

    public void StartRecall()
    {
        GameManager.Mode = GameMode.Recall;
        recall = true;
        canvas[currentCanvasIndex].gameObject.SetActive(false);
    }

    public void Skip()
    {
        SaveSkip();
        GameManager.TutorialMode = false;
        SceneManager.LoadScene("PrototypeSelect");
    }

    public void SaveSkip()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/tutorial.sq";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(true);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public bool LoadSkip()
    {
        string path = Application.persistentDataPath + "/tutorial.sq";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();
            return data.skip;
        }
        else
        {
            return false;
        }
    }

}
