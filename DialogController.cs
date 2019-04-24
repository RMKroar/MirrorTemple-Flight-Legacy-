using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour {

    public Sprite torn_grassland;
    public GameObject[] WorldBosses;
    public AudioClip BossBGM;
    public AudioClip EpisodeBGM;
    CanvasGroup cg;
    Text CharNameText, DialogText;

    private void Start()
    {
        cg = GetComponent<CanvasGroup>();
        CharNameText = transform.GetChild(0).gameObject.GetComponent<Text>();
        DialogText = transform.GetChild(1).gameObject.GetComponent<Text>();
    }

    public void InvokeDialog(int scriptCode) {
        GameController.pausable = false;
        StartCoroutine(OpenDialog(scriptCode));
    }

    IEnumerator OpenDialog(int scriptCode) {
        for (cg.alpha = 0; cg.alpha < 1; cg.alpha += 1 / 30f) {
            yield return new WaitForSeconds(1 / 60f);
        }

        string[] list = GetDialogData(scriptCode);
        if (list != null) {
            for (int k=0; k<list.Length; k++) {
                DialogText.text = "";
                DialogInteraction(scriptCode, k);
                for (int i = 0; i < list[k].Length; i++) {                    
                    DialogText.text += list[k].Substring(i, 1);
                    yield return new WaitForSeconds(1 / 60f);
                }

                while (true) {
                    yield return new WaitForSeconds(1 / 60f);
                    if (!Input.GetKeyDown(KeyCode.Escape) && Input.anyKeyDown) break;
                }
            }

            DialogEndInteraction(scriptCode);
        }
    }

    private string[] GetDialogData(int scriptCode) {
        TextAsset txt = null;
        switch (scriptCode) {
            case -1:
                txt = (TextAsset)Resources.Load("Data/Script_Boss_" + (MainController.GetGameMode() % 10)) as TextAsset;
                break;
            case 0:
                txt = (TextAsset)Resources.Load("Data/Script_Encounter") as TextAsset;
                break;
            case 1:
                txt = (TextAsset)Resources.Load("Data/Script_Ephesus") as TextAsset;
                break;
            case 2:
                txt = (TextAsset)Resources.Load("Data/Script_Laodicea") as TextAsset;
                break;
            case 3:
                txt = (TextAsset)Resources.Load("Data/Script_Thyatera") as TextAsset;
                break;
            case 4:
                txt = (TextAsset)Resources.Load("Data/Script_Luian") as TextAsset;
                break;
            case 5:
                txt = (TextAsset)Resources.Load("Data/Script_UnivernGenesis") as TextAsset;
                break;
            default:
                break;
        }

        // txt Parsing
        if (txt != null)
        {
            return txt.text.Split('\n');
        }
        else return null;
    }

    private void DialogInteraction(int scriptCode, int iter) {
        if (iter == 0) CharNameText.text = "[???]";

        if (scriptCode == -1) {
            if (iter == 0) {
                string[] names = new string[9] { "[리카 엔]", "[리모라 아리둠]", "[트라이아]", "[왕녀 레빌라]", "[알파리아 마리에]", "[알카드로봇 라스트오더]", "[커맨더 니브]", "[라베티카]", "[갑부 아바리]" };
                CharNameText.text = names[MainController.GetGameMode() % 10];
            }
        }
        if (scriptCode == 0) {
            if (iter == 5) CharNameText.text = "[유니번 셀베트]";
        }
        if (scriptCode == 1) {
            if (iter % 2 == 0) CharNameText.text = "[월드 에페서스]";
            else CharNameText.text = "[유니번 셀베트]";
        }
        if (scriptCode == 2)
        {
            if (iter % 2 == 0) CharNameText.text = "[월드 라오디케아]";
            else CharNameText.text = "[유니번 셀베트]";
        }
        if (scriptCode == 3)
        {
            if (iter % 2 == 0) CharNameText.text = "[월드 티아테리아]";
            else CharNameText.text = "[유니번 셀베트]";
        }
        if (scriptCode == 4)
        {
            if (iter % 2 == 0) CharNameText.text = "[월드 루이안]";
            else CharNameText.text = "[유니번 셀베트]";
        }
        if (scriptCode == 5)
        {
            if (iter == 26) DialogText.color = Color.yellow;
            else DialogText.color = Color.white;

            if (iter % 2 == 0) CharNameText.text = "[유니번 셀베트]";
            else CharNameText.text = "[유니번 제네시스]";
        }
    }

    private void DialogEndInteraction(int scriptCode)
    {
        if (scriptCode == 0)
        {
            StartCoroutine(Apocalipse());
        }

        if (scriptCode == -1) {
            GameObject instTarget = GameObject.Find("GameController").GetComponent<GameController>().Bosses[MainController.GetGameMode() % 10];
            GameObject ins = Instantiate(instTarget, GameObject.Find("TriggerPanel").transform);
            ins.transform.localPosition = new Vector3(450f, 0f);
            Destroy(GameObject.Find("Encounter_Boss(Clone)"));

            AudioSource audio = GameObject.FindGameObjectWithTag("Stage").GetComponent<AudioSource>();

            audio.volume = 1f; audio.UnPause();

            StartCoroutine(CloseDialog());
        }
        if (scriptCode == 1) {
            GameObject ins = Instantiate(WorldBosses[0], GameObject.Find("TriggerPanel").transform);
            ins.transform.localPosition = new Vector3(450f, 0f);
            Destroy(GameObject.Find("Encounter_Ephesus(Clone)"));

            AudioSource audio = GameObject.Find("Stage_EphesusKernel(Clone)").GetComponent<AudioSource>();

            audio.clip = BossBGM;
            audio.volume = 1f; audio.Play();

            StartCoroutine(EmergeEphesus());
            StartCoroutine(CloseDialog());
        }
        if (scriptCode == 2)
        {
            GameObject ins = Instantiate(WorldBosses[1], GameObject.Find("TriggerPanel").transform);
            ins.transform.localPosition = new Vector3(450f, 0f);
            Destroy(GameObject.Find("Encounter_Laodicea(Clone)"));

            AudioSource audio = GameObject.Find("Stage_Laodicea(Clone)").GetComponent<AudioSource>();

            audio.clip = BossBGM;
            audio.volume = 1f; audio.Play();

            StartCoroutine(EmergeLaodicea());
            StartCoroutine(CloseDialog());
        }
        if (scriptCode == 3)
        {
            GameObject ins = Instantiate(WorldBosses[2], GameObject.Find("TriggerPanel").transform);
            ins.transform.localPosition = new Vector3(450f, 0f);
            Destroy(GameObject.Find("Encounter_Thyatera(Clone)"));

            AudioSource audio = GameObject.Find("Stage_Thyatera(Clone)").GetComponent<AudioSource>();

            audio.clip = BossBGM;
            audio.volume = 1f; audio.Play();

            StartCoroutine(EmergeThyatera());
            StartCoroutine(CloseDialog());
        }
        if (scriptCode == 4)
        {
            StartCoroutine(CloseDialog());
            StartCoroutine(ToMirrorTemple());
        }
        if (scriptCode == 5) {
            StartCoroutine(CloseDialog());
            GameObject.Find("MainController").GetComponent<MainController>().Invoke_SingleModeStart();
        }
    }

    IEnumerator Apocalipse() { // only for single use
        Image BlackPanel = GameObject.Find("BlackPanel").GetComponent<Image>();

        for (int i = 1; Screen.width > 30; i++) {
            if (BlackPanel.color.a < 1) BlackPanel.color += new Color(0f, 0f, 0f, 1 / 60f);
            int newWidth = Screen.width - (int)(0.3f * i);
            int newHeight = Screen.height - (int)(0.2f * i);
            Screen.SetResolution(newWidth, newHeight, false);
            yield return new WaitForSeconds(1 / 60f);          
        }
        PlayerPrefs.SetInt("Progress", 1);
        PlayerPrefs.Save();
        yield return new WaitForSeconds(1 / 2f);
        Application.Quit();
    }

    IEnumerator ToMirrorTemple()
    { // only for single use
        Transform UIPanel = GameObject.Find("UIPanel").transform;
        Image BlackPanel = GameObject.Find("BlackPanel").GetComponent<Image>();

        float rot = 0;
        for (int i = 0; i < 120; i++) {
            UIPanel.localPosition -= new Vector3(0f, 0.12f * i);
            rot += 0.005f * i;
            UIPanel.localRotation = Quaternion.Euler(new Vector3(0f, 0f, rot));
            yield return new WaitForSeconds(1 / 120f);
        }
        yield return new WaitForSeconds(1f);
        BlackPanel.color = new Color(1f, 1f, 1f, 0f);
        for (; BlackPanel.color.a < 1; BlackPanel.color += new Color(0f, 0f, 0f, 1 / 120f))
        {
            yield return new WaitForSeconds(1 / 120f);
        }
        PlayerPrefs.SetInt("Progress", 5);
        PlayerPrefs.Save();
        yield return new WaitForSeconds(1 / 2f);
        
        Application.Quit();
    }

    IEnumerator CloseDialog()
    {
        for (cg.alpha = 1; cg.alpha > 0; cg.alpha -= 1 / 30f)
        {
            yield return new WaitForSeconds(1 / 60f);
        }
    }

    IEnumerator EmergeEphesus() {
        Image Back1Panel = GameObject.Find("Back1Panel").GetComponent<Image>();
        Image Back2Panel = GameObject.Find("Back2Panel").GetComponent<Image>();

        for (float i = 0; i < 0.5f; i += 1 / 60f)
        {
            Back1Panel.color = new Color(1 - 0.5f * i, 1 - i, 1);
            Back2Panel.color = new Color(1 - 0.5f * i, 1 - i, 1);
            yield return new WaitForSeconds(1 / 60f);
        }
    }

    IEnumerator EmergeLaodicea()
    {
        Image Back1Panel = GameObject.Find("Back1Panel").GetComponent<Image>();
        Image Back2Panel = GameObject.Find("Back2Panel").GetComponent<Image>();

        for (float i = 0; i < 0.5f; i += 1 / 60f)
        {
            Back1Panel.color = new Color(1, 1 - i, 1 - i);
            Back2Panel.color -= new Color(0f, 0f, 0f, 1 - 2 * i);
            yield return new WaitForSeconds(1 / 60f);
        }
    }

    IEnumerator EmergeThyatera()
    {
        Image Back1Panel = GameObject.Find("Back1Panel").GetComponent<Image>();
        Image Back2Panel = GameObject.Find("Back2Panel").GetComponent<Image>();

        for (float i = 0; i < 0.5f; i += 1 / 60f)
        {
            Back1Panel.color = new Color(1, 1 - 0.5f * i, 1 - i);
            Back2Panel.color = new Color(1 - 0.5f * i, 1, 1 - 0.5f * i);
            yield return new WaitForSeconds(1 / 60f);
        }
    }
}