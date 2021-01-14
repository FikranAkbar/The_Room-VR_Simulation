using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GazeAtRadio : MonoBehaviour
{
    // images for icon
    private Image whiteIcon;
    private Image redIcon;

    // coroutine for triggering sound
    private Coroutine lastGazingCoroutine;

    // times property for gazing
    private float gazeTime;
    [SerializeField]
    private float clickDuration;

    // sounds
    public List<GameObject> studioEventsEmitter;
    private int randomNum;
    private GameObject eventPlaying;

    // transforms
    public Transform playerVR;
    public float invisibleAfter;

    // booleans property for gazing
    private bool isGazing = false;

    // Start is called before the first frame update
    void Start()
    {
        whiteIcon = transform.GetChild(0).GetComponent<Image>();
        redIcon = transform.GetChild(1).GetComponent<Image>();
        eventPlaying = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGazing)
        {
            gazeTime += Time.deltaTime;
            redIcon.fillAmount = gazeTime / clickDuration;
        }
        else
        {
            if (lastGazingCoroutine != null)
            {
                StopCoroutine(lastGazingCoroutine);
                lastGazingCoroutine = null;
            }
            gazeTime = 0f;
            redIcon.fillAmount = gazeTime;
        }

        if (redIcon.fillAmount >= 1f)
        {
            whiteIcon.enabled = false;
            redIcon.enabled = false;
        }
        else
        {
            whiteIcon.enabled = true;
            redIcon.enabled = true;
        }
    }

    public void StartGazing()
    {
        if (Vector3.Distance(playerVR.position, transform.position) <= invisibleAfter)
        {
            isGazing = true;
            if (lastGazingCoroutine == null)
            {
                lastGazingCoroutine = StartCoroutine(WaitGazingTillFinish());
            }
        }
    }

    public void StopGazing()
    {
        isGazing = false;
    }

    private IEnumerator WaitGazingTillFinish()
    {
        Debug.Log("Start Coroutine");
        yield return new WaitUntil(() => redIcon.fillAmount >= 1f);
        if (eventPlaying == null)
        {
            randomNum = Random.Range(0, studioEventsEmitter.Count);
            eventPlaying = studioEventsEmitter[randomNum];
            eventPlaying.SetActive(true);
        }
        else if (eventPlaying != null)
        {
            eventPlaying.SetActive(false);
            eventPlaying = null;
        }
    }
}
