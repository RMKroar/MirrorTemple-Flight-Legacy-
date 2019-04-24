using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage : MonoBehaviour {

    public GameObject[] Enemies;
    public GameObject Boss;
    public Image BlackPanel;

    public Sprite Back1;
    public Sprite Back2;

    public Vector3[] SpawnInfo; // Vector3: (id, posX, posY)
    public float[] DelayInfo;

    public string StageTitle;
    public string StageDesc;

    bool paused = false;
    int it = -2;

	// Use this for initialization
	void Start () {
        BlackPanel = GameObject.Find("BlackPanel").GetComponent<Image>();
        GameObject.Find("Back1Panel").GetComponent<Image>().sprite = Back1;
        GameObject.Find("Back2Panel").GetComponent<Image>().sprite = Back2;
        GameObject.Find("Back1Panel").GetComponent<Image>().color = Color.white;
        GameObject.Find("Back2Panel").GetComponent<Image>().color = Color.white;

        GameObject.Find("BackgroundPanel").GetComponent<RectTransform>().localPosition = new Vector3(440, -100);
        BlackPanel.color = new Color(0f, 0f, 0f, 1f);
        StartCoroutine("StageOpen");
        StartCoroutine("Spawn");

        if (name.Contains("Castle") || name.Contains("Challenge") || name.Contains("Final"))
        {
            GameObject.Find("GameController").GetComponent<GameController>().StopCoroutine("Scroll");
            GameObject.Find("BackgroundPanel").GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);

            GameObject.Find("Back1Panel").GetComponent<RectTransform>().sizeDelta = new Vector2(1280, 900);
            GameObject.Find("Back2Panel").GetComponent<RectTransform>().sizeDelta = new Vector2(1280, 900);
        }
        else {
            GameObject.Find("GameController").GetComponent<GameController>().StopCoroutine("Scroll");
            GameObject.Find("GameController").GetComponent<GameController>().StartCoroutine("Scroll");
            GameObject.Find("Back1Panel").GetComponent<RectTransform>().sizeDelta = new Vector2(2160, 1080);
            GameObject.Find("Back2Panel").GetComponent<RectTransform>().sizeDelta = new Vector2(2160, 1080);
        }

        if (name.Contains("Challenge")) {
            int ind = GameObject.Find("GameController").GetComponent<GameController>().Bosses.Length;
            ArrayList tempArray = new ArrayList();

            for (int i = 0; i < ind; i++) {
                tempArray.Add(i);
            }

            ind = Random.Range(0, tempArray.Count);          
            Enemies[0] = GameObject.Find("GameController").GetComponent<GameController>().Bosses[(int)tempArray[ind]];
            tempArray.RemoveAt(ind);
            ind = Random.Range(0, tempArray.Count);
            Boss = GameObject.Find("GameController").GetComponent<GameController>().Bosses[(int)tempArray[ind]];
        }
	}

    IEnumerator StageOpen() {
        yield return new WaitForSeconds(1 / 60f);
        BlackPanel.color -= new Color(0f, 0f, 0f, 1 / 60f);
        if (BlackPanel.color.a > 0) StartCoroutine("StageOpen");
        else StartCoroutine("StageIntroduction");
    }

    IEnumerator StageIntroduction() {
        GameObject StageIntro = GameObject.Find("StageIntroductionPanel");
        string stageText = "";
        if (name.Contains("Final")) stageText = "FINAL Stage";
        else if (GameController.stageNum == -1)
        {
            stageText = "Episode No." + (MainController.GetGameMode() % 10 + 1);
        }
        else if (GameController.stageNum <= 4)
        {
            stageText = "STAGE " + (GameController.stageNum + 1);
        }
        else if (name.Contains("Challenge")) stageText = "Challenge Stage";
        else stageText = "Last Stage";

        StageIntro.transform.GetChild(0).gameObject.GetComponent<Text>().text = stageText;
        StageIntro.transform.GetChild(1).gameObject.GetComponent<Text>().text = StageTitle;
        StageIntro.transform.GetChild(2).gameObject.GetComponent<Text>().text = StageDesc.Replace("#", System.Environment.NewLine);
        RectTransform SI_Rect = StageIntro.GetComponent<RectTransform>();
        CanvasGroup SI_Canv = StageIntro.GetComponent<CanvasGroup>();

        SI_Rect.localScale = new Vector3(2f, 2f, 1f);
        SI_Canv.alpha = 0;

        for (; SI_Canv.alpha < 1f; SI_Canv.alpha += 1 / 15f) 
        {
            yield return new WaitForSeconds(1 / 60f);
            SI_Rect.localScale -= new Vector3(1 / 15f, 1 / 15f);
        }

        yield return new WaitForSeconds(2f);

        for (; SI_Canv.alpha > 0f; SI_Canv.alpha -= 1 / 60f)
        {
            yield return new WaitForSeconds(1 / 60f);
        }
    }

    IEnumerator Spawn() {
        it++;
        if (it == -1)
        {
            yield return new WaitForSeconds(6f);
            StartCoroutine("Spawn");
        }
        else if (it == SpawnInfo.Length)
        {
            
            if (MainController.GetGameMode() >= 10 && !PlayerPrefs.HasKey("ItemExtend_" + (MainController.GetGameMode() % 10))) { // Sub Episode
                GameObject instTarget = GameObject.Find("GameController").GetComponent<GameController>().BossEncounter;
                GameObject target = Instantiate(instTarget, GameObject.Find("TriggerPanel").transform);
                target.transform.localPosition = new Vector3(450f, 0f);
            }
            else { // [Main Story] OR [Sub Episode that has been already cleared]
                GameObject target = Instantiate(Boss, GameObject.Find("TriggerPanel").transform);
                target.transform.localPosition = new Vector3(450f, 0f);
            }
            yield return null;
        }
        else {
            GameObject target = Instantiate(Enemies[(int)SpawnInfo[it].x], GameObject.Find("TriggerPanel").transform);
            target.transform.localPosition = new Vector3(SpawnInfo[it].y, SpawnInfo[it].z);
            if (DelayInfo[it] == 0f)
            {
                StartCoroutine("Spawn");
                yield return null;
            }
            else if (DelayInfo[it] == -1) {
                paused = true;
                while (paused) {
                    yield return new WaitForSeconds(1 / 30f);
                }
                yield return new WaitForSeconds(3f);
                StartCoroutine("Spawn");
            }
            else
            {
                yield return new WaitForSeconds(DelayInfo[it]);
                StartCoroutine("Spawn");
            }
        }
    }

    public void InvokeChangeBackground(Sprite back1, Sprite back2, GameObject obj) {
        StartCoroutine(JustChangeBackground(back1, back2, obj));
    }

    public void InvokeChangeBackground(Sprite back1, Sprite back2) {
        StartCoroutine(ChangeBackground(back1, back2));
    }

    IEnumerator JustChangeBackground(Sprite back1, Sprite back2, GameObject SpawnExtra) // Stage_Castle
    {
        yield return new WaitForSeconds(1f);
        for (; BlackPanel.color.a < 1; BlackPanel.color += new Color(0f, 0f, 0f, 1 / 60f)) {
            yield return new WaitForSeconds(1 / 60f);         
        }
        GameObject.Find("Back1Panel").GetComponent<Image>().sprite = back1;
        GameObject.Find("Back2Panel").GetComponent<Image>().sprite = back2;
        for (; BlackPanel.color.a > 0; BlackPanel.color -= new Color(0f, 0f, 0f, 1 / 60f))
        {
            yield return new WaitForSeconds(1 / 60f);           
        }
        yield return new WaitForSeconds(2f);

        if (SpawnExtra != null) {
            GameObject target = Instantiate(SpawnExtra, GameObject.Find("TriggerPanel").transform);
            target.transform.localPosition = new Vector3(480f, 0f);
        }        
    }

    IEnumerator ChangeBackground(Sprite back1, Sprite back2) // Last Stages
    {
        yield return new WaitForSeconds(1f);
        for (; BlackPanel.color.a < 1; BlackPanel.color += new Color(0f, 0f, 0f, 1 / 60f))
        {
            yield return new WaitForSeconds(1 / 60f);
        }
        GameObject.Find("Back1Panel").GetComponent<Image>().sprite = back1;
        GameObject.Find("Back2Panel").GetComponent<Image>().sprite = back2;
        GameObject.Find("BackgroundPanel").GetComponent<RectTransform>().localPosition = new Vector3(440, -100);
        for (; BlackPanel.color.a > 0; BlackPanel.color -= new Color(0f, 0f, 0f, 1 / 60f))
        {
            yield return new WaitForSeconds(1 / 60f);
        }
        yield return new WaitForSeconds(1f);

        paused = false;
    }
}
