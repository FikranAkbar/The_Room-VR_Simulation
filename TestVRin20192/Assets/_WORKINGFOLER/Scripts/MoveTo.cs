using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveTo : MonoBehaviour
{
    // images for icon
    private Image whiteIcon;
    private Image redIcon;

    // coroutine for triggering anim
    private Coroutine lastGazingCoroutine;

    // times property for gazing
    private float gazeTime;
    [SerializeField]
    private float clickDuration;

    // transforms
    public Transform playerVR;
    public Transform defaultTransform;
    public Transform targetTransform;
    private Vector3 initialPos;
    private Vector3 bedPos;

    // sounds
    public GameObject moveSound;

    // booleans property for gazing
    private bool isGazing = false;

    // game object to activate
    public List<GameObject> gameObjectsToDeactivate;
    public List<GameObject> gameObjectsToActivate;

    

    // Start is called before the first frame update
    void Start()
    {
        whiteIcon = transform.GetChild(0).GetComponent<Image>();
        redIcon = transform.GetChild(1).GetComponent<Image>();

        initialPos = defaultTransform.transform.position;
        bedPos = targetTransform.transform.position;
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

        //if (playerVR.position == targetTransform.position)
        //{
        //    whiteIcon.enabled = false;
        //    redIcon.enabled = false;
        //}
        //else if (playerVR.position == defaultTransform.position)
        //{
        //    whiteIcon.enabled = true;
        //    redIcon.enabled = true;
        //}
    }

    public void StartGazing()
    {
        isGazing = true;
        if (lastGazingCoroutine == null)
        {
            lastGazingCoroutine = StartCoroutine(WaitGazingTillFinish());
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
        playerVR.position = targetTransform.position;
        
        for (int i = 0; i<gameObjectsToActivate.Count; i++)
        {
            gameObjectsToActivate[i].SetActive(true);
        }

        for (int i = 0; i < gameObjectsToDeactivate.Count; i++)
        {
            gameObjectsToDeactivate[i].SetActive(false);
        }

        moveSound.SetActive(false);
        moveSound.SetActive(true);
        isGazing = false;
        gazeTime = 0f;
        redIcon.fillAmount = gazeTime;
        gameObject.SetActive(false);
    }
}
