using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ItemController : MonoBehaviour {

    public GameObject[] CoolDown;
    public Text[] CoolDownText;
    public Image BlackPanel;

    // [#] Active Item, [*] Set Item, [~] Consume Item
    // in case of Active Item, arraylist consists of { id, name, rare, desc, continuable time(sec), cooltime(sec) }
    public static ArrayList Items = new ArrayList();
    public static ArrayList ExtendedItems = new ArrayList();

    public Image[] Active;
    public Image[] Set;

    public GameObject ItemSelectPanel;
    public GameObject ItemIntroductionPanel;
    public RectTransform ActiveChest;
    public RectTransform SetChest;
    public RectTransform ItemChest;

    public CanvasGroup ChallengeChoicePanel;
    public RectTransform ToLastStage;
    public RectTransform ToChallengeStage;

    public GameObject[] SpecialShots;
    public GameObject[] SpecialActives;
    public GameObject[] SpecialItems;
    public GameObject ItemDebris;

    bool proceedStage;
    bool input_intropanel;
    Stack itemSeq;

    void Awake()
    {
        // csv Parsing (Original Items)
        TextAsset csv = (TextAsset)Resources.Load("Data/ItemData") as TextAsset;
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
                Items.Add(csvData);
            }
            cnt++;
        }

        // csv Parsing (Extended Items)
        // csv Parsing
        TextAsset csv2 = (TextAsset)Resources.Load("Data/ItemDataExtended") as TextAsset;
        string[] csvBundles2 = csv2.text.Split('\n');
        string[] DifficultyParser = new string[3] { "E", "N", "H" };
        cnt = 0;
        foreach (string bundle in csvBundles2)
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

                if (PlayerPrefs.HasKey("ItemExtend_" + ((cnt - 1) / 3)) &&
                    ("" + PlayerPrefs.GetString("ItemExtend_" + ((cnt - 1) / 3))).Contains(DifficultyParser[(cnt - 1) % 3])) {
                    Items.Add(csvData);
                }                
            }
            cnt++;
        }
    }

    public void Start()
    {
        input_intropanel = false;
        MakePool();
    }

    public void Reward() {
        if (MainController.GetGameMode() == 0)
        { // Main Story
            select = 0;
            ItemSelectPanel.SetActive(true);
            StartCoroutine("SelectPanelOpen");
        }
        else
        { // Sub Episode
            UnlockItem();
        }
    }

    public void ChallengeStage()
    {
        itemSeq = new Stack();
        GameController.rank -= (GameController.rank < 3) ? GameController.rank : 3;
        itemSeq.Push(2); itemSeq.Push(1); itemSeq.Push(0);
        SelectItemFromStack();
    }

    int select;

    IEnumerator SelectPanelOpen() {
        for (; ItemSelectPanel.GetComponent<CanvasGroup>().alpha < 1;) {
            ItemSelectPanel.GetComponent<CanvasGroup>().alpha += 0.02f;
            yield return new WaitForSeconds(1 / 50f);
        }
        ItemSelectPanel.GetComponent<CanvasGroup>().alpha = 1;
        _ChestScale();
    }

    IEnumerator SelectPanelClose()
    {
        for (; ItemSelectPanel.GetComponent<CanvasGroup>().alpha > 0;)
        {
            ItemSelectPanel.GetComponent<CanvasGroup>().alpha -= 0.02f;
            yield return new WaitForSeconds(1 / 50f);
        }
    }

    IEnumerator ChallengeChoicePanelOpen() {
        select = 0;
        for (; ChallengeChoicePanel.alpha < 1;)
        {
            ChallengeChoicePanel.alpha += 0.02f;
            yield return new WaitForSeconds(1 / 50f);
        }
        ChallengeChoicePanel.alpha = 1;
        _StageScale();
    }

    IEnumerator ChallengeChoicePanelClose()
    {
        for (; ChallengeChoicePanel.alpha > 0;)
        {
            ChallengeChoicePanel.alpha -= 0.02f;
            yield return new WaitForSeconds(1 / 50f);
        }
    }

    public void Update()
    {
        if (Time.timeScale != 0) {

            if (input_intropanel)
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    input_intropanel = false;
                    StartCoroutine("IntroPanelClose");
                }
            }

            if (ItemSelectPanel.activeInHierarchy && ItemSelectPanel.GetComponent<CanvasGroup>().alpha >= 1f)
            {
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    select = (select == 2) ? 0 : select + 1;
                    _ChestScale();
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    select = (select == 0) ? 2 : select - 1;
                    _ChestScale();
                }
                else if (Input.GetKeyDown(KeyCode.Z))
                {
                    StartCoroutine("SelectPanelClose");
                    SelectTreasure();
                }
            }

            if (ChallengeChoicePanel.alpha >= 1f) {
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)) {
                    select = (select == 0) ? 1 : 0;
                    _StageScale();
                }
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    StartCoroutine("ChallengeChoicePanelClose");
                    if (select == 0) StartCoroutine("GoLastStage");
                    else ChallengeStage();
                }
            }
        }       
    }

    public void _ChestScale() {
        switch (select) {
            case 0:
                ActiveChest.localScale = new Vector3(1.3f, 1.3f, 1f);
                SetChest.localScale = Vector3.one;
                ItemChest.localScale = Vector3.one;
                break;
            case 1:
                SetChest.localScale = new Vector3(1.3f, 1.3f, 1f);
                ActiveChest.localScale = Vector3.one;
                ItemChest.localScale = Vector3.one;
                break;
            case 2:
                ItemChest.localScale = new Vector3(1.3f, 1.3f, 1f);
                ActiveChest.localScale = Vector3.one;
                SetChest.localScale = Vector3.one;
                break;
            default:
                break;
        }
    }

    public void _StageScale()
    {
        switch (select)
        {
            case 0:
                ToLastStage.localScale = new Vector3(1.3f, 1.3f, 1f);
                ToChallengeStage.localScale = Vector3.one;
                break;
            case 1:
                ToChallengeStage.localScale = new Vector3(1.3f, 1.3f, 1f);
                ToLastStage.localScale = Vector3.one;
                break;
            default:
                break;
        }
    }

    public void SelectTreasure() {
        string container;
        if (select == 0) container = "#";
        else if (select == 1) container = "*";
        else container = "~";

        ArrayList sample = new ArrayList();
        foreach (ArrayList ar in Items) {
            if (((string)ar[0]).Contains(container))
            {
                for (int i = 0; i < 3 - ((int)ar[2]); i++) {
                    sample.Add(ar);
                }               
            }
        }
        int randomSeed = Random.Range(0, sample.Count);
        GetItem((ArrayList)sample[randomSeed], true);        
    }

    public void GetItem(ArrayList ar, bool _proceedStage) {
        ItemIntroductionPanel.SetActive(true);
        StartCoroutine("IntroPanelOpen");
        proceedStage = _proceedStage;

        ItemEffect(((string)ar[0]).Remove(0, 1));

        string pic_url = "Items/" + ((string)ar[0]).Remove(0, 1);
        string itemKind = "";
        if (((string)ar[0]).Contains("#"))
        {
            itemKind = " [사용 아이템]";
            for (int i = 0; i <= 3; i++)
            {
                if (Active[i].sprite == null)
                {
                    Active[i].sprite = Resources.Load<Sprite>(pic_url) as Sprite;
                    Active[i].color = Color.white;

                    GetPlayer().GetComponent<Player>().Active.Add(ar);
                    break;
                }
            }
        }
        else if (((string)ar[0]).Contains("*"))
        {
            itemKind = " [세트 아이템]";
            if (!((string)ar[0]).Contains("candle"))
            {
                for (int i = 0; i <= 7; i++)
                {
                    if (Set[i].sprite == null)
                    {
                        Set[i].sprite = Resources.Load<Sprite>(pic_url) as Sprite;
                        Set[i].color = Color.white;
                        GetPlayer().GetComponent<Player>().Set.Add((string)ar[0]);
                        break;
                    }
                }
            }
            else {
                Set[5].sprite = Resources.Load<Sprite>(pic_url) as Sprite;
                Set[5].color = Color.white;
                GetPlayer().GetComponent<Player>().Set.Add((string)ar[0]);
            }
        }
        else if (((string)ar[0]).Contains("~"))
        {
            itemKind = " [소비 아이템]";
        }

        ItemIntroductionPanel.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(pic_url) as Sprite;
        ItemIntroductionPanel.transform.GetChild(1).GetComponent<Text>().text = "<다음의 아이템을 얻습니다.>";
        ItemIntroductionPanel.transform.GetChild(2).GetComponent<Text>().text = (string)ar[1] + itemKind;
        ItemIntroductionPanel.transform.GetChild(3).GetComponent<Text>().text = (string)ar[3];
    }

    public void UnlockItem()
    {
        int episodeNumber = MainController.GetGameMode() % 10;
        ArrayList ar = (ArrayList)ExtendedItems[3 * episodeNumber + (GameController.rank / 3)];
        ItemIntroductionPanel.SetActive(true);
        StartCoroutine("IntroPanelOpen");

        string pic_url = "Items/" + ((string)ar[0]).Remove(0, 1);
        string itemKind = "";
        if (((string)ar[0]).Contains("#"))
        {
            itemKind = " [사용 아이템]";
            for (int i = 0; i <= 3; i++)
            {
                if (Active[i].sprite == null)
                {
                    Active[i].sprite = Resources.Load<Sprite>(pic_url) as Sprite;
                    Active[i].color = Color.white;

                    GetPlayer().GetComponent<Player>().Active.Add(ar);
                    break;
                }
            }
        }
        else if (((string)ar[0]).Contains("*"))
        {
            itemKind = " [세트 아이템]";
            for (int i = 0; i <= 7; i++)
            {
                if (Set[i].sprite == null)
                {
                    Set[i].sprite = Resources.Load<Sprite>(pic_url) as Sprite;
                    Set[i].color = Color.white;
                    GetPlayer().GetComponent<Player>().Set.Add((string)ar[0]);
                    break;
                }
            }
        }
        else if (((string)ar[0]).Contains("~"))
        {
            itemKind = " [소비 아이템]";
        }

        ItemIntroductionPanel.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(pic_url) as Sprite;
        ItemIntroductionPanel.transform.GetChild(1).GetComponent<Text>().text = "<아이템이 해금되었습니다!>";
        ItemIntroductionPanel.transform.GetChild(2).GetComponent<Text>().text = (string)ar[1] + itemKind;
        ItemIntroductionPanel.transform.GetChild(3).GetComponent<Text>().text = (string)ar[3];

        string[] DifficultyParser = new string[3] { "E", "N", "H" };
        if (!PlayerPrefs.HasKey("ItemExtend_" + episodeNumber)) {
            PlayerPrefs.SetString("ItemExtend_" + episodeNumber, DifficultyParser[GameController.rank / 3]);
        }
        else {
            string temp = PlayerPrefs.GetString("ItemExtend_" + episodeNumber);
            if (!temp.Contains(DifficultyParser[GameController.rank / 3])) {
                PlayerPrefs.SetString("ItemExtend_" + episodeNumber, temp + DifficultyParser[GameController.rank / 3]);
            }
            else {
                ItemIntroductionPanel.transform.GetChild(1).GetComponent<Text>().text = "<이미 해금된 아이템입니다.>";
            }
        }

        PlayerPrefs.Save();
        ItemEffect(((string)ar[0]).Remove(0, 1));
    }

    public void GetSpecificItem(string id)
    {
        ArrayList ar = new ArrayList();

        foreach (ArrayList list in Items)
        {
            if (((string)list[0]).Equals(id))
            {
                ar = list;
            }
        }
        foreach (ArrayList list in ExtendedItems)
        {
            if (((string)list[0]).Equals(id))
            {
                ar = list;
            }
        }

        string pic_url = "Items/" + ((string)ar[0]).Remove(0, 1);
        if (((string)ar[0]).Contains("#"))
        {
            for (int i = 0; i <= 3; i++)
            {
                if (Active[i].sprite == null)
                {
                    Active[i].sprite = Resources.Load<Sprite>(pic_url) as Sprite;
                    Active[i].color = Color.white;

                    GetPlayer().GetComponent<Player>().Active.Add(ar);
                    break;
                }
            }
        }
        else if (((string)ar[0]).Contains("*"))
        {
            for (int i = 0; i <= 7; i++)
            {
                if (Set[i].sprite == null)
                {
                    Set[i].sprite = Resources.Load<Sprite>(pic_url) as Sprite;
                    Set[i].color = Color.white;
                    GetPlayer().GetComponent<Player>().Set.Add((string)ar[0]);
                    break;
                }
            }
        }

        ItemEffect(((string)ar[0]).Remove(0, 1));
    }

    public void SelectItemFromStack()
    {
        select = (int)itemSeq.Pop();
        string container;
        if (select == 0) container = "#";
        else if (select == 1) container = "*";
        else container = "~";

        ArrayList sample = new ArrayList();
        foreach (ArrayList ar in Items)
        {
            if (((string)ar[0]).Contains(container))
            {
                for (int i = 0; i < 3 - ((int)ar[2]); i++)
                {
                    sample.Add(ar);
                }
            }
        }
        int randomSeed = Random.Range(0, sample.Count);
        GetItem((ArrayList)sample[randomSeed], false);
    }

    IEnumerator IntroPanelOpen()
    {
        GetComponent<AudioSource>().Play();
        for (; ItemIntroductionPanel.GetComponent<CanvasGroup>().alpha < 1;)
        {
            ItemIntroductionPanel.GetComponent<CanvasGroup>().alpha += 0.02f;
            yield return new WaitForSeconds(1 / 50f);
        }
        input_intropanel = true;
        ItemIntroductionPanel.GetComponent<CanvasGroup>().alpha = 1;

        yield return null;
    }

    IEnumerator IntroPanelClose()
    {
        for (; ItemIntroductionPanel.GetComponent<CanvasGroup>().alpha > 0;)
        {
            ItemIntroductionPanel.GetComponent<CanvasGroup>().alpha -= 0.02f;
            yield return new WaitForSeconds(1 / 50f);
        }
        if (MainController.GetGameMode() == 0)
        {
            // Main Story
            if (proceedStage)
            {
                if (GameController.stageNum < 4)
                {
                    for (; BlackPanel.color.a < 1;)
                    {
                        BlackPanel.color += new Color(0f, 0f, 0f, 1 / 60f);
                        yield return new WaitForSeconds(1 / 60f);
                    }

                    if (GameController.rank < 10) GameController.rank += 1;
                    GameObject.Find("CurrentRank").GetComponent<Text>().text = GameController.rank.ToString();
                    GameObject.Find("GameController").GetComponent<GameController>().SelectStage();
                }
                else if (GameController.stageNum >= 5) StartCoroutine("GoLastStage");
                else
                { // GameController.stageNum = 4
                    StartCoroutine("ChallengeChoicePanelOpen");
                }
            }
            else if (itemSeq.Count != 0)
            {
                SelectItemFromStack();
            }
            else
            { // itemSeq.Count = 0, proceedStage = false
                StartCoroutine("GoChallengeStage");
            }
        }
        else {
            // Sub Episode
            for (; BlackPanel.color.a < 1;)
            {
                BlackPanel.color += new Color(0f, 0f, 0f, 1 / 60f);
                yield return new WaitForSeconds(1 / 60f);
            }
            SceneManager.LoadScene("Main");
        }
    }

    IEnumerator GoChallengeStage() {
        for (; BlackPanel.color.a < 1;)
        {
            BlackPanel.color += new Color(0f, 0f, 0f, 1 / 60f);
            yield return new WaitForSeconds(1 / 60f);
        }

        if (GameController.rank < 10) GameController.rank += 1;
        GameObject.Find("CurrentRank").GetComponent<Text>().text = GameController.rank.ToString();
        GameObject.Find("GameController").GetComponent<GameController>().GoChallengeStage();
    }

    IEnumerator GoLastStage()
    {
        for (; BlackPanel.color.a < 1;)
        {
            BlackPanel.color += new Color(0f, 0f, 0f, 1 / 60f);
            yield return new WaitForSeconds(1 / 60f);
        }

        if (GameController.rank < 10) GameController.rank += 1;
        GameObject.Find("CurrentRank").GetComponent<Text>().text = GameController.rank.ToString();
        GameObject.Find("GameController").GetComponent<GameController>().GoLastStage();
    }

    public void ItemEffect(string id) {
        Player player = GetPlayer().GetComponent<Player>();
        float value; bool isConsume = false;
        GameObject ins;

        // Set Items
        switch (id) {
            case "terial_luz":
                player.mainShotDamage += 2;
                break;
            case "terial_malen":
                player.subShotDamage += 2;
                break;
            case "leather_boot":
                player.moveSpeed += 2;
                player.mainShotDelay *= 0.85f;               
                break;
            case "snow_boot":
                player.shiftSpeed += 2;
                player.subShotDelay *= 0.85f;
                break;
            case "marble_of_life":
                GetPlayer().GetComponent<CircleCollider2D>().radius *= 0.5f;
                value = GetPlayer().GetComponent<CircleCollider2D>().radius;

                GetPlayer().transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(value / 4f, value / 4f, 1f);
                GetPlayer().transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(value / 4f, value / 4f, 1f);
                player.health += 4;
                break;
            case "marble_of_paradox":
                player.mainShotDamage += 2;
                player.subShotDamage += 2;
                GetPlayer().GetComponent<CircleCollider2D>().radius *= 1.5f;
                value = GetPlayer().GetComponent<CircleCollider2D>().radius;

                GetPlayer().transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(value / 4f, value / 4f, 1f);
                GetPlayer().transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(value / 4f, value / 4f, 1f);
                break;
            case "terial_drec":
                player.mainShotDelay *= 0.75f;
                player.subShotDelay *= 0.75f;
                break;
            case "heist_pill":
                player.moveSpeed += 1;
                player.shiftSpeed += 1;
                player.mainShotDelay *= 0.85f;
                player.subShotDelay *= 0.85f;
                break;
            case "leira_clock":
                player.deltaCooldown = (player.deltaCooldown >= 0.5f)? 0.8f : player.deltaCooldown + 0.3f;
                player.mainShotDelay *= 0.85f;
                player.subShotDelay *= 0.85f;
                break;
            case "full_power_ladybug":
                player.moveSpeed = (player.moveSpeed >= 1)? player.moveSpeed - 1 : 0;
                player.shiftSpeed = (player.shiftSpeed >= 1)? player.shiftSpeed - 1 : 0;
                player.mainShotDamage += 2;
                player.subShotDamage += 2;
                break;
            case "feather_pen":
                player.mainShotDelay *= 0.6f;
                player.mainShotDamage -= 1;
                break;
            case "toy_train":
                player.subShotDelay *= 0.6f;
                player.subShotDamage -= 1;
                break;
            case "cute_cat":
                player.mainShotDelay *= 1.2f;
                player.mainShotDamage += 3;
                break;
            case "robotic_dog":
                player.subShotDelay *= 1.2f;
                player.subShotDamage += 3;
                break;
            case "justice_balance":
                player.moveSpeed += 1;
                player.shiftSpeed += 1;
                player.mainShotDelay *= 0.9f;
                player.subShotDelay *= 0.9f;
                player.mainShotDamage += 1;
                player.subShotDamage += 1;
                break;
            case "ninja_symbol":
                player.LaunchSpecial_Invoke("kunaiS");
                break;
            case "music_cd":
                player.LaunchSpecial_Invoke("tonepair");
                break;
            case "silverstar_bow":
                player.LaunchSpecial_Invoke("arrow");
                break;
            case "silverstellar":
                player.LaunchSpecial_Invoke("star");
                break;
            case "snowflake":
                player.LaunchSpecial_Invoke("snowflake");
                break;
            case "poison_devil":
                player.LaunchSpecial_Invoke("poisonSting");
                break;
            case "fire_badge":
                player.LaunchSpecial_Invoke("fire");
                break;
            case "ayla_hone":
                player.mainShotDamage *= 2;
                player.subShotDamage /= 2;
                break;
            case "dexter_hone":
                player.mainShotDamage /= 2;
                player.subShotDamage *= 2;
                break;
            case "toy_fortress":
                player.shiftSpeed = 0;
                player.subShotDamage *= 2;
                break;
            case "alpharia_marie":
                player.subShotDelay *= 0.8f;
                player.mainShotDelay *= 0.8f;
                if (player.Set.Contains("*betaria_pasen")) {
                    player.subShotDelay *= 0.5f;
                    player.mainShotDelay *= 0.5f;
                }
                break;
            case "betaria_pasen":
                player.subShotDamage += 1;
                player.mainShotDamage += 1;
                if (player.Set.Contains("*alpharia_marie"))
                {
                    player.subShotDamage += 3;
                    player.mainShotDamage += 3;
                }
                break;
            case "white_wing":
                player.moveSpeed += 3;
                player.shiftSpeed += 3;
                player.mainShotDelay *= 0.7f;
                player.subShotDelay *= 0.7f;
                player.deltaCooldown = (player.deltaCooldown >= 0.7f) ? 0.8f : player.deltaCooldown + 0.1f;
                break;
            case "lucky_clover":
                player.subShotDamage += 1;
                player.mainShotDamage += 1;
                if (GameController.rank >= 5) {
                    player.subShotDamage += 1;
                    player.mainShotDamage += 1;
                }
                break;
            case "pyramid":
                player.subShotDamage *= 2.5f;
                player.mainShotDamage *= 2.5f;
                player.subShotDelay *= 2;
                player.mainShotDelay *= 2;
                break;
            case "tropical_fish":
                player.subShotDamage += (0.8f * player.Active.Count);
                player.mainShotDamage += (0.8f * player.Active.Count);
                break;
            case "gamble_machine":
                player.subShotDelay *= 0.85f;
                player.mainShotDelay *= 0.85f;

                ArrayList sample = new ArrayList();
                foreach (ArrayList ar in Items)
                {
                    if (((string)ar[0]).Contains("#"))
                    {
                        for (int i = 0; i < 3 - ((int)ar[2]); i++)
                        {
                            sample.Add(ar);
                        }
                    }
                }
                int randomSeed = Random.Range(0, sample.Count);
                GetSpecificItem((string)((ArrayList)sample[randomSeed])[0]);
                break;
            case "blazing_fruit":
                player.subShotDamage += 0.5f;
                player.mainShotDamage += 0.5f;
                break;
            case "zircon":
                player.subShotDamage += 1;
                player.mainShotDamage += 1;
                if (player.health <= 5)
                {
                    player.subShotDamage += 1;
                    player.mainShotDamage += 1;
                }
                break;
            case "red_candle":
                if (player.Set.Contains("*red_candle")) {
                    player.subShotDamage *= 10f; player.mainShotDamage *= 10f;
                }
                if (player.Set.Contains("*green_candle")) {
                    player.transform.localScale = new Vector3(-1, 1, 1);
                }
                if (player.Set.Contains("*blue_candle")) {
                    player.moveSpeed *= 2f;
                    player.shiftSpeed *= 2f;
                }
                player.Set.Clear();
                player.subShotDamage *= 0.1f;
                player.mainShotDamage *= 0.1f;
                break;
            case "green_candle":
                if (player.Set.Contains("*red_candle"))
                {
                    player.subShotDamage *= 10f; player.mainShotDamage *= 10f;
                }
                if (player.Set.Contains("*green_candle"))
                {
                    player.transform.localScale = new Vector3(-1, 1, 1);
                }
                if (player.Set.Contains("*blue_candle"))
                {
                    player.moveSpeed *= 2f;
                    player.shiftSpeed *= 2f;
                }
                player.Set.Clear();
                player.transform.localScale = new Vector3(-1, 1, 1) * 3;
                break;
            case "blue_candle":
                if (player.Set.Contains("*red_candle"))
                {
                    player.subShotDamage *= 10f; player.mainShotDamage *= 10f;
                }
                if (player.Set.Contains("*green_candle"))
                {
                    player.transform.localScale = new Vector3(-1, 1, 1);
                }
                if (player.Set.Contains("*blue_candle"))
                {
                    player.moveSpeed *= 2f;
                    player.shiftSpeed *= 2f;
                }
                player.Set.Clear();
                player.moveSpeed *= 0.5f;
                player.shiftSpeed *= 0.5f;
                break;
            case "grey_candle":
                if (player.Set.Contains("*red_candle"))
                {
                    player.subShotDamage *= 10f; player.mainShotDamage *= 10f;
                }
                if (player.Set.Contains("*green_candle"))
                {
                    player.transform.localScale = new Vector3(-1, 1, 1);
                }
                if (player.Set.Contains("*blue_candle"))
                {
                    player.moveSpeed *= 2f;
                    player.shiftSpeed *= 2f;
                }
                player.Set.Clear();
                break;
            default:
                isConsume = true;
                break;
        }
        // Consume Items 
        switch (id) {
            case "apple":
                player.health += 5;
                break;
            case "candy":
                if (GameController.rank >= 5) GameController.rank -= 1;
                player.health += 4;
                break;
            case "chip_cookie":
                if (player.health < 10) player.health = 10;
                else player.health += 4;
                break;
            case "witch_soup":
                player.health += 10;
                GameController.rank += (GameController.rank < 10) ? 1 : 0;
                break;
            case "sour_lemon":
                player.health += 4;
                player.deltaCooldown = (player.deltaCooldown >= 0.7f) ? 0.8f : player.deltaCooldown + 0.1f;
                break;
            case "croissant":
                player.health += 3;
                GameController.rank -= (GameController.rank > 0) ? 1 : 0;
                break;
            case "soft_cup":
                player.health += 7;
                break;
            case "hard_icecream":
                player.health += 9;
                GameController.rank += (GameController.rank < 10) ? 1 : 0;
                player.deltaCooldown = (player.deltaCooldown >= 0.7f) ? 0.8f : player.deltaCooldown + 0.1f;
                break;
            case "crunchy_tarte":
                player.health += 11;
                GameController.rank = (GameController.rank >= 7) ? 10 : GameController.rank + 2;
                player.deltaCooldown = (player.deltaCooldown >= 0.47f) ? 0.8f : player.deltaCooldown + 0.33f;
                break;
            case "gourmet_cake":
                player.health += 2;
                GameController.rank = (GameController.rank <= 2) ? 0 : GameController.rank - 3;
                break;
            case "mystic_fruit":
                player.health += 10;
                GameController.rank -= (GameController.rank > 0) ? 1 : 0;
                break;
            case "omega_gear":
                player.deltaCooldown = (player.deltaCooldown >= 0.4f) ? 0.8f : player.deltaCooldown + 0.4f;
                player.health += 10;
                break;
            case "lica_lata":
                player.health += 4;
                if (GameController.rank >= 5) player.health += 4;
                break;
            case "scorpion_skin":
                player.health += 3;
                ins = Instantiate(SpecialItems[0], GameObject.Find("TriggerPanel").transform);
                ins.GetComponent<SpecialItem>().SetPlayer(GetPlayer());
                break;
            case "sushi":
                player.health += 3 + (2 * player.Active.Count);
                break;
            case "firstaid_kit":
                player.health += 8;
                player.deltaCooldown -= 0.2f;
                break;
            case "vending_machine":
                player.health += 2;
                ArrayList sample = new ArrayList();
                foreach (ArrayList ar in Items)
                {
                    if (((string)ar[0]).Contains("*"))
                    {
                        for (int i = 0; i < 3 - ((int)ar[2]); i++)
                        {
                            sample.Add(ar);
                        }
                    }
                }
                int randomSeed = Random.Range(0, sample.Count);
                GetSpecificItem((string)((ArrayList)sample[randomSeed])[0]);
                break;
            case "strawberry_ice":
                player.health += 3;
                ins = Instantiate(SpecialItems[1], GameObject.Find("TriggerPanel").transform);
                ins.GetComponent<SpecialItem>().SetPlayer(GetPlayer());
                break;
            case "hamburger":
                if (player.health <= 7) player.health += 8;
                else player.health += 4;
                break;
            default:
                isConsume = false;
                break;
        }
        if (isConsume) {
            if (player.Set.Contains("*blazing_fruit")) { player.health += 3; }
        }

        GameObject.Find("CurrentHealth").GetComponent<Text>().text = player.health.ToString();
        GameObject.Find("CurrentRank").GetComponent<Text>().text = GameController.rank.ToString();
        player.SpeedRefresh();
    }

    public GameObject GetPlayer()
    {
        GameObject returnObj = null;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            returnObj = obj;
        }

        return returnObj;
    }

    public void CooldownDisp(int ind, float value) {
        if (!CoolDown[ind].activeInHierarchy) CoolDown[ind].SetActive(true);
        CoolDownText[ind].text = value.ToString();
    }

    public void MakePool() {
        GameObject ins;
        for (int i = 0; i < 30; i++)
        {
            for (int j = 0; j < SpecialShots.Length; j++) {
                ins = Instantiate(SpecialShots[j], GameController.ShotPool.transform);
                GameController.ShotPool.GetComponent<ShotPool>().poolSpecial.Add(ins);
                ins.SetActive(false);
            }           
        }
    }
}
