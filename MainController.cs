using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainController : MonoBehaviour {

    private static int Gamemode = 0;

    public CanvasGroup CV_BackgroundPanel;
    public GameObject MainTitle;
    public RectTransform RT_AddonMask;
    public GameObject TitleDebris;
    public Transform ModeSelectPanel;
    public Transform CharacterSelectPanel;
    public Transform RankSelectPanel;
    public Transform SubEpisodePanel;
    public Image BlackPanel;
    public AudioSource[] Sounds;
    public GameObject UnivernGenesis;

    public float DebrisCreate;
    public float DebrisFrequency;

    GameObject BackgroundAddon;
    GameObject SubTitle;
    Transform CharacterList;
    Transform EpisodeList;
    Text EpisodeDifficultyText;
    Transform RewardIntroductionPanel;

    int selectMode;
    int selectCharacter;
    int selectRank;
    int selectEpisode;

    int maxSelectMode;
    int maxSelectCharacter;
    int maxSelectEpisode = 0;

    ArrayList ExtendedItems = new ArrayList();

    // Use this for initialization
    void Awake() {
        Time.timeScale = 1;

        Screen.SetResolution(1280, 880, false);
        CV_BackgroundPanel.alpha = 0;
        ModeSelectPanel.gameObject.GetComponent<CanvasGroup>().alpha = 0;
        MainTitle.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        MainTitle.GetComponent<RectTransform>().localScale = new Vector3(3f, 3f, 1f);
        RT_AddonMask.sizeDelta = new Vector2(0f, 880f);
        selectMode = 0;
        selectRank = 0;

        BackgroundAddon = RT_AddonMask.transform.GetChild(0).gameObject;
        SubTitle = RT_AddonMask.transform.GetChild(1).gameObject;
        CharacterList = CharacterSelectPanel.GetChild(0);
        EpisodeList = SubEpisodePanel.GetChild(0);
        EpisodeDifficultyText = SubEpisodePanel.GetChild(1).GetChild(0).gameObject.GetComponent<Text>();
        RewardIntroductionPanel = SubEpisodePanel.GetChild(2).gameObject.transform;

        maxSelectMode = ModeSelectPanel.childCount - 2;
        maxSelectCharacter = CharacterList.childCount - 1;
        maxSelectEpisode = EpisodeList.childCount - 1;

        ItemDataLoad();
        PanelInitialize();

        // If PlayerPrefs.HasKey("Progress") IS NULL, it'll be initialized as 0 by GameController. 
        StartCoroutine(StartAnimation(!(PlayerPrefs.HasKey("Progress") && PlayerPrefs.GetInt("Progress") >= 5)));
    }

    private void ItemDataLoad() {
        // csv Parsing
        TextAsset csv = (TextAsset)Resources.Load("Data/ItemDataExtended") as TextAsset;
        string[] csvBundles = csv.text.Split('\n');
        int cnt = 0;
        foreach (string bundle in csvBundles)
        {
            if (cnt != 0)
            {
                string[] csvPieces = bundle.Split(',');
                if (csvPieces[0] == null || csvPieces[0] == "") break;
                ArrayList csvData = new ArrayList();
                for (int i = 0; ; i++)
                {
                    if (csvPieces[i].ToString().Contains("@")) break;

                    if (i == 4 || i == 5 || i == 2)
                    {
                        int datum = System.Int32.Parse(csvPieces[i]);
                        csvData.Add(datum);
                    }
                    else
                    {
                        string datum = csvPieces[i];
                        if (datum != null)
                        {
                            if (i == 3) datum = datum.Replace("$", System.Environment.NewLine);
                            csvData.Add(datum);
                        }
                        else csvData.Add("");
                    }
                }
                ExtendedItems.Add(csvData);
            }
            cnt++;
        }
    }

    public void PanelInitialize() {
        ModeSelectPanel.localPosition = new Vector3(0f, 0f, 0f);
        CharacterSelectPanel.localPosition = new Vector3(1200f, 0f, 0f);
        RankSelectPanel.localPosition = new Vector3(1200f, 0f, 0f);
        SubEpisodePanel.localPosition = new Vector3(1200f, 0f, 0f);

        Text tempText;
        for (int i = 0; i <= maxSelectMode; i++)
        {
            tempText = ModeSelectPanel.GetChild(i).gameObject.GetComponent<Text>();
            tempText.color = Color.white; tempText.fontSize = 57;
        }

        tempText = ModeSelectPanel.GetChild(0).gameObject.GetComponent<Text>();
        tempText.color = Color.yellow; tempText.fontSize = 75;

        CanvasGroup tempCVG;
        for (int i = 0; i <= maxSelectCharacter; i++)
        {
            tempCVG = CharacterList.GetChild(i).gameObject.GetComponent<CanvasGroup>();
            tempCVG.alpha = 0;
        }

        tempCVG = CharacterList.GetChild(0).gameObject.GetComponent<CanvasGroup>();
        tempCVG.alpha = 1;

        RankSelectPanel.GetChild(0).GetComponent<Text>().text = "0";

        BlackPanel.color = Color.clear;
    }

    void Update()
    {
        if (PlayerPrefs.HasKey("Progress") && PlayerPrefs.GetInt("Progress") >= 5) { // Univern Genesis
            // nothing
        }
        else { // Normal State
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (ModeSelectPanel.localPosition.x == 0 && ModeSelectPanel.gameObject.GetComponent<CanvasGroup>().alpha >= 1)
                {
                    Sounds[0].Play();
                    selectMode = (selectMode >= maxSelectMode) ? 0 : selectMode + 1;

                    Text tempText;
                    for (int i = 0; i <= maxSelectMode; i++)
                    {
                        tempText = ModeSelectPanel.GetChild(i).gameObject.GetComponent<Text>();
                        tempText.color = Color.white; tempText.fontSize = 57;
                    }

                    tempText = ModeSelectPanel.GetChild(selectMode).gameObject.GetComponent<Text>();
                    tempText.color = Color.yellow; tempText.fontSize = 75;
                    if (selectMode == 2) ModeSelectPanel.GetChild(selectMode).GetChild(0).gameObject.SetActive(true);
                    else ModeSelectPanel.GetChild(2).GetChild(0).gameObject.SetActive(false);
                }

                if (CharacterSelectPanel.localPosition.x == 0)
                {
                    Sounds[0].Play();
                    selectCharacter = (selectCharacter >= maxSelectCharacter) ? 0 : selectCharacter + 1;

                    CanvasGroup tempCVG;
                    for (int i = 0; i <= maxSelectCharacter; i++)
                    {
                        tempCVG = CharacterList.GetChild(i).gameObject.GetComponent<CanvasGroup>();
                        tempCVG.alpha = 0;
                    }

                    tempCVG = CharacterList.GetChild(selectCharacter).gameObject.GetComponent<CanvasGroup>();
                    tempCVG.alpha = 1;
                }

                if (SubEpisodePanel.localPosition.x == 0)
                {
                    Sounds[0].Play();
                    selectEpisode = (selectEpisode >= maxSelectEpisode) ? 0 : selectEpisode + 1;

                    CanvasGroup tempCVG;
                    for (int i = 0; i <= maxSelectEpisode; i++)
                    {
                        tempCVG = EpisodeList.GetChild(i).gameObject.GetComponent<CanvasGroup>();
                        tempCVG.alpha = 0;
                    }

                    tempCVG = EpisodeList.GetChild(selectEpisode).gameObject.GetComponent<CanvasGroup>();
                    tempCVG.alpha = 1;

                    DisplayEpisodeReward();
                }
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (ModeSelectPanel.localPosition.x == 0 && ModeSelectPanel.gameObject.GetComponent<CanvasGroup>().alpha >= 1)
                {
                    Sounds[0].Play();
                    selectMode = (selectMode <= 0) ? maxSelectMode : selectMode - 1;

                    Text tempText;
                    for (int i = 0; i <= maxSelectMode; i++)
                    {
                        tempText = ModeSelectPanel.GetChild(i).gameObject.GetComponent<Text>();
                        tempText.color = Color.white; tempText.fontSize = 57;
                    }

                    tempText = ModeSelectPanel.GetChild(selectMode).gameObject.GetComponent<Text>();
                    tempText.color = Color.yellow; tempText.fontSize = 75;
                    if (selectMode == 2) ModeSelectPanel.GetChild(selectMode).GetChild(0).gameObject.SetActive(true);
                    else ModeSelectPanel.GetChild(2).GetChild(0).gameObject.SetActive(false);
                }

                if (CharacterSelectPanel.localPosition.x == 0)
                {
                    Sounds[0].Play();
                    selectCharacter = (selectCharacter <= 0) ? maxSelectCharacter : selectCharacter - 1;

                    CanvasGroup tempCVG;
                    for (int i = 0; i <= maxSelectCharacter; i++)
                    {
                        tempCVG = CharacterList.GetChild(i).gameObject.GetComponent<CanvasGroup>();
                        tempCVG.alpha = 0;
                    }

                    tempCVG = CharacterList.GetChild(selectCharacter).gameObject.GetComponent<CanvasGroup>();
                    tempCVG.alpha = 1;
                }

                if (SubEpisodePanel.localPosition.x == 0)
                {
                    Sounds[0].Play();
                    selectEpisode = (selectEpisode <= 0) ? maxSelectEpisode : selectEpisode - 1;

                    CanvasGroup tempCVG;
                    for (int i = 0; i <= maxSelectEpisode; i++)
                    {
                        tempCVG = EpisodeList.GetChild(i).gameObject.GetComponent<CanvasGroup>();
                        tempCVG.alpha = 0;
                    }

                    tempCVG = EpisodeList.GetChild(selectEpisode).gameObject.GetComponent<CanvasGroup>();
                    tempCVG.alpha = 1;

                    DisplayEpisodeReward();
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {

                if (RankSelectPanel.localPosition.x == 0)
                {
                    Sounds[0].Play();
                    selectRank = (selectRank <= 0) ? 10 : selectRank - 1;
                    RankSelectPanel.GetChild(0).GetComponent<Text>().text = selectRank.ToString();
                }
                if (SubEpisodePanel.localPosition.x == 0)
                {
                    Sounds[0].Play();
                    selectRank = (selectRank == 2) ? 8 : selectRank - 3;
                    DisplayEpisodeDifficulty();
                    RankSelectPanel.GetChild(0).GetComponent<Text>().text = selectRank.ToString();
                }
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (RankSelectPanel.localPosition.x == 0)
                {
                    Sounds[0].Play();
                    selectRank = (selectRank >= 10) ? 0 : selectRank + 1;
                    RankSelectPanel.GetChild(0).GetComponent<Text>().text = selectRank.ToString();
                }
                if (SubEpisodePanel.localPosition.x == 0)
                {
                    Sounds[0].Play();
                    selectRank = (selectRank == 8) ? 2 : selectRank + 3;
                    DisplayEpisodeDifficulty();
                    RankSelectPanel.GetChild(0).GetComponent<Text>().text = selectRank.ToString();
                }
            }

            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
            {
                if (ModeSelectPanel.localPosition.x == 0 && ModeSelectPanel.gameObject.GetComponent<CanvasGroup>().alpha >= 1)
                {
                    if (selectMode == 0 || selectMode == 1)
                    {
                        StartCoroutine(MovePanel(ModeSelectPanel, true));
                        StartCoroutine(MovePanel(CharacterSelectPanel, true));
                    }

                    if (selectMode == 3)
                    {
                        Application.Quit();
                    }
                }

                if (CharacterSelectPanel.localPosition.x == 0)
                {
                    if (selectMode == 0)
                    {
                        StartCoroutine(MovePanel(CharacterSelectPanel, true));
                        StartCoroutine(MovePanel(RankSelectPanel, true));
                    }
                    else if (selectMode == 1)
                    {
                        StartCoroutine(MovePanel(CharacterSelectPanel, true));
                        selectRank = 2;
                        DisplayEpisodeDifficulty();
                        StartCoroutine(MovePanel(SubEpisodePanel, true));
                    }
                }

                if (RankSelectPanel.localPosition.x == 0)
                {
                    Sounds[1].Play();
                    GameController.rank = selectRank;
                    GameController.characterIndex = selectCharacter;

                    Gamemode = 0; // Main Story
                    StartCoroutine(MovePanel(RankSelectPanel, true));
                    StartCoroutine(SingleModeStart());
                }

                if (SubEpisodePanel.localPosition.x == 0)
                {
                    Sounds[1].Play();
                    GameController.rank = selectRank;
                    GameController.characterIndex = selectCharacter;

                    Gamemode = 10 + selectEpisode; // Sub Episode: 10, 11, 12, ---
                    StartCoroutine(MovePanel(SubEpisodePanel, true));
                    StartCoroutine(SingleModeStart());
                }
            }

            if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape))
            {
                if (CharacterSelectPanel.localPosition.x == 0)
                {
                    Sounds[2].Play();
                    StartCoroutine(MovePanel(CharacterSelectPanel, false));
                    StartCoroutine(MovePanel(ModeSelectPanel, false));
                }

                if (RankSelectPanel.localPosition.x == 0)
                {
                    Sounds[2].Play();
                    StartCoroutine(MovePanel(RankSelectPanel, false));
                    StartCoroutine(MovePanel(CharacterSelectPanel, false));
                }

                if (SubEpisodePanel.localPosition.x == 0)
                {
                    Sounds[2].Play();
                    StartCoroutine(MovePanel(SubEpisodePanel, false));
                    StartCoroutine(MovePanel(CharacterSelectPanel, false));
                }
            }
        }
    }

    private void DisplayEpisodeDifficulty() {
        if (selectRank == 2) EpisodeDifficultyText.text = "EASY";
        else if (selectRank == 5) EpisodeDifficultyText.text = "<color=yellow>NORMAL</color>";
        else if (selectRank == 8) EpisodeDifficultyText.text = "<color=red>HARD</color>";
        else {
            selectRank = 2;
            EpisodeDifficultyText.text = "EASY";
        }

        DisplayEpisodeReward();
    }

    private void DisplayEpisodeReward()
    {
        ArrayList ar = (ArrayList)ExtendedItems[3 * selectEpisode + (selectRank / 3)];
        string itemKind = ""; string itemId = (string)ar[0];
        string pic_url = "Items/" + itemId.Remove(0, 1);
        if (itemId.Contains("#")) itemKind = " [사용 아이템]";
        else if (itemId.Contains("*")) itemKind = " [세트 아이템]";
        else if (itemId.Contains("~")) itemKind = " [소비 아이템]";

        RewardIntroductionPanel.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(pic_url) as Sprite;
        RewardIntroductionPanel.GetChild(1).GetComponent<Text>().text = "<color=magenta>보상: </color>" + (string)ar[1] + itemKind;
        RewardIntroductionPanel.GetChild(2).GetComponent<Text>().text = (string)ar[3];

        string[] DifficultyParser = new string[3] { "E", "N", "H" };
        string keyString = "ItemExtend_" + selectEpisode;
        RewardIntroductionPanel.GetChild(3).gameObject.SetActive(
            PlayerPrefs.HasKey(keyString) && PlayerPrefs.GetString(keyString).Contains(DifficultyParser[selectRank / 3]));
    }

    public static int GetGameMode() {
        return Gamemode;
    }

    public IEnumerator StartAnimation(bool value) { 
        for (; CV_BackgroundPanel.alpha < 1; CV_BackgroundPanel.alpha += 1 / 30f)
        {
            yield return new WaitForSeconds(1 / 120f);
        }

        for (; MainTitle.GetComponent<RectTransform>().localScale.x > 1; MainTitle.GetComponent<RectTransform>().localScale -= new Vector3(2 / 15f, 2 / 15f)) {
            MainTitle.GetComponent<Image>().color += new Color(0f, 0f, 0f, 1 / 15f);
            yield return new WaitForSeconds(1 / 120f);
        }
        yield return new WaitForSeconds(1 / 2f);
        for (; RT_AddonMask.sizeDelta.x < 1280; RT_AddonMask.sizeDelta += new Vector2(40f, 0f)) {
            yield return new WaitForSeconds(1 / 120f);
        }     

        yield return new WaitForSeconds(DebrisCreate);

        if (value) StartCoroutine(MainDebris());
        else StartCoroutine(Apocalipse());

        float freq = 0;
        for (; ; ) {
            freq += 0.03f;
            BackgroundAddon.transform.localPosition += new Vector3(0f, 0.3f * Mathf.Sin(freq));
            SubTitle.transform.localPosition += new Vector3(0f, 0.3f * Mathf.Sin(freq));

            yield return new WaitForSeconds(1 / 120f);
        }
    }

    IEnumerator MovePanel(Transform target, bool towardLeft) {
        int dir = (towardLeft) ? -1 : 1;
        Vector3 storePos = target.localPosition;

        for (int i = 0; i < 20; i++) {
            target.localPosition += new Vector3(60f * dir, 0f, 0f);
            yield return new WaitForSeconds(1 / 120f);
        }

        target.localPosition = storePos + new Vector3(1200f * dir, 0f, 0f);

        yield return null;
    }

    IEnumerator MainDebris() {
        TitleDebris.GetComponent<RectTransform>().localScale = Vector3.one;
        TitleDebris.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.75f);
        for (; TitleDebris.GetComponent<Image>().color.a > 0; TitleDebris.GetComponent<Image>().color -= new Color(0f, 0f, 0f, 1 / 90f)) {
            TitleDebris.GetComponent<RectTransform>().localScale += new Vector3(0.05f, 0.05f);
            ModeSelectPanel.GetComponent<CanvasGroup>().alpha += 1 / 70f;
            yield return new WaitForSeconds(1 / 120f);
        }
        ModeSelectPanel.GetComponent<CanvasGroup>().alpha = 1;
        for (; ; ) {
            yield return new WaitForSeconds(DebrisFrequency);
            TitleDebris.GetComponent<RectTransform>().localScale = Vector3.one;
            TitleDebris.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.2f);
            for (; TitleDebris.GetComponent<Image>().color.a > 0; TitleDebris.GetComponent<Image>().color -= new Color(0f, 0f, 0f, 1 / 240f))
            {
                TitleDebris.GetComponent<RectTransform>().localScale += new Vector3(0.01f, 0.01f);
                yield return new WaitForSeconds(1 / 120f);
            }
        }

    }

    public void Invoke_SingleModeStart() {
        StartCoroutine(SingleModeStart());
    }

    IEnumerator SingleModeStart()
    {
        for (; BlackPanel.color.a < 1; BlackPanel.color += new Color(0f, 0f, 0f, 1 / 60f)) {
            yield return new WaitForSeconds(1 / 120f);
        }
        StopCoroutine("StartAnimation");
        StopCoroutine("MainDebris");
        SceneManager.LoadScene("SingleMode");
        yield return null;
    }

    IEnumerator Apocalipse() {
        Image BackgroundPanel = GameObject.Find("BackgroundPanel").GetComponent<Image>();
        Image AddonImage = BackgroundAddon.GetComponent<Image>();
        Transform SubTitleTr = SubTitle.transform;
        Transform MainTitleTr = MainTitle.transform;
        float rot = 0;

        MainTitle.GetComponent<Image>().color = Color.grey;
        StartCoroutine(AudioPitchDown());

        yield return new WaitForSeconds(1 / 2f);
        for (; BackgroundPanel.color.g > 0.5f; BackgroundPanel.color -= new Color(0f, 1 / 120f, 0f, 0f)) {
            yield return new WaitForSeconds(1 / 120f);
        }
        for (; AddonImage.color.g > 0.5f; AddonImage.color -= new Color(0f, 1 / 120f, 1 / 120f, 0f)) {
            yield return new WaitForSeconds(1 / 120f);
        }
        for (int i = 0; i < 180; i++) {
            rot += 0.005f * i;
            SubTitleTr.localPosition -= new Vector3(0f, 0.2f * i, 0f);
            MainTitleTr.localPosition -= new Vector3(0f, 0.25f * i, 0f);

            SubTitleTr.localRotation = Quaternion.Euler(new Vector3(0f, 0f, rot));
            MainTitleTr.localRotation = Quaternion.Euler(new Vector3(0f, 0f, -rot));
            yield return new WaitForSeconds(1 / 120f);
        }

        yield return new WaitForSeconds(2f);

        GameObject ins = Instantiate(UnivernGenesis, GameObject.Find("TriggerPanel").transform);
        ins.transform.localPosition = new Vector3(450f, 0f);
    }

    IEnumerator AudioPitchDown() {
        AudioSource myAudio = GetComponent<AudioSource>();
        for (; myAudio.pitch < 3; myAudio.pitch += 1 / 40f) {
            yield return new WaitForSeconds(1 / 120f);
        }
        yield return new WaitForSeconds(3f);
        for (; myAudio.pitch > 0.3f; myAudio.pitch -= 1 / 40f) {
            yield return new WaitForSeconds(1 / 120f);
        }
    }
}