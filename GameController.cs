using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public GameObject[] Players;
    public GameObject BackgroundPanel;
    public GameObject PausePanel;
    public Text RankText;
    public static GameObject BulletPool;
    public static GameObject ShotPool;
    public RectTransform BackgroundRect;

    public float scrollSpeed;
    public GameObject bullet_circle;
    public GameObject bullet_petal;
    public GameObject bullet_thorn;

    public GameObject[] Stages;
    public GameObject[] LastStages;
    public GameObject[] Bosses;
    public ArrayList StageSelector = new ArrayList() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

    public GameObject ChallengeStage;
    public GameObject LastStage;

    public Image BlackPanel;

    public GameObject BossEncounter;
    public GameObject CollapsedEnemy;

    public static bool pausable = true;
    public static int characterIndex = 0;
    public static int bulletInPool = 600;
    public static int rank = 5;
    public static int stageNum = -1;

    void Awake()
    {
        CheckGameData();

        BulletPool = GameObject.Find("BulletPool");
        ShotPool = GameObject.Find("ShotPool");
        pausable = true;

        if (MainController.GetGameMode() == 0) {  // Main Story
            stageNum = -1;
            if (PlayerPrefs.HasKey("Progress") && PlayerPrefs.GetInt("Progress") >= 5)
            {
                GameObject pl = Instantiate(Players[9], GameObject.Find("TriggerPanel").transform);
                pl.transform.SetAsFirstSibling();
                pl.transform.localPosition = new Vector3(-580f, 0f, 0f);
                pl.GetComponent<Player>().health = 25;

                GoLastStage(); // Univern Genesis
            }
            else
            {
                GameObject pl = Instantiate(Players[characterIndex], GameObject.Find("TriggerPanel").transform);
                pl.transform.SetAsFirstSibling();
                pl.transform.localPosition = new Vector3(-580f, 0f, 0f);
                pl.GetComponent<Player>().health = 10;

                SelectStage();
            }
            //stageNum = 0;
        }
        else {  // Sub Episode
            GameObject pl = Instantiate(Players[characterIndex], GameObject.Find("TriggerPanel").transform);
            pl.transform.SetAsFirstSibling();
            pl.transform.localPosition = new Vector3(-580f, 0f, 0f);
            pl.GetComponent<Player>().health = 5;
            stageNum = -1;
            Instantiate(Stages[MainController.GetGameMode() % 10]); 
        }

        BackgroundRect = BackgroundPanel.GetComponent<RectTransform>();

        for (int i = 0; i < bulletInPool; i++)
        {
            GameObject ins = Instantiate(bullet_circle, BulletPool.transform);
            BulletPool.GetComponent<BulletPool>().poolCircle.Add(ins);
            ins.SetActive(false);

            ins = Instantiate(bullet_petal, BulletPool.transform);
            BulletPool.GetComponent<BulletPool>().poolPetal.Add(ins);
            ins.SetActive(false);

            ins = Instantiate(bullet_thorn, BulletPool.transform);
            BulletPool.GetComponent<BulletPool>().poolThorn.Add(ins);
            ins.SetActive(false);
        }
        //GameObject.Find("InventoryPanel").GetComponent<ItemController>().Reward();
        //GameObject.Find("InventoryPanel").GetComponent<ItemController>().ChallengeStage();
        UpdateUI();
    }

    // Use this for initialization
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && pausable) {
            if (Time.timeScale != 0) PauseGame();
            else ResumeGame();
        }

        if (Input.GetKeyDown(KeyCode.F12)) {
            if (Time.timeScale == 0) {
                if (PlayerPrefs.HasKey("Progress") && PlayerPrefs.GetInt("Progress") >= 5)
                {
                    // Univern Genesis
                    GetPlayer().GetComponent<Player>().Reincarnate();
                }
                else
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
                }
            }
        }

        if (Time.timeScale == 0) {
            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                if (exItem.Count != 0) {
                    itIndex = (itIndex == exItem.Count - 1) ? 0 : itIndex + 1;
                    PauseItemExplanation(itIndex);
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                if (exItem.Count != 0)
                {
                    itIndex = (itIndex == 0) ? exItem.Count - 1 : itIndex - 1;
                    PauseItemExplanation(itIndex);
                }
            }
        }
    }

    public void SelectStage() {
        stageNum++;
        foreach (GameObject st in GameObject.FindGameObjectsWithTag("Stage")) {
            Destroy(st);
        }

        int rand = Random.Range(0, StageSelector.Count);
        Instantiate(Stages[(int)StageSelector[rand]]);
        StageSelector.RemoveAt(rand);
    }

    public void GoChallengeStage()
    {
        stageNum++;
        foreach (GameObject st in GameObject.FindGameObjectsWithTag("Stage"))
        {
            Destroy(st);
        }

        Instantiate(ChallengeStage);
    }

    public void GoLastStage()
    {
        if (stageNum < 6)
        {
            stageNum = 6;
            foreach (GameObject st in GameObject.FindGameObjectsWithTag("Stage"))
            {
                Destroy(st);
            }

            Instantiate(LastStage);
        }
        else {
            if(PlayerPrefs.HasKey("Progress")) PlayerPrefs.SetInt("Progress", PlayerPrefs.GetInt("Progress") + 1);
            PlayerPrefs.Save();

            UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
        }
    }

    IEnumerator Scroll() {
        if (BackgroundRect.localPosition.x > -440)
        { 
            BackgroundRect.localPosition += new Vector3(-scrollSpeed, 0f, 0f);
        }
        yield return new WaitForSeconds(1 / 100f);
        StartCoroutine("Scroll");
    }

    public void UpdateUI() {
        RankText.text = "" + rank;
    }

    public static void EraseBullet() {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Bullet")) {
            obj.GetComponent<Bullet>().UnableWithDebris();
        }
    }

    public static int EraseBullet_ReturnAmount()
    {
        int cnt = 0;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Bullet"))
        {
            obj.GetComponent<Bullet>().UnableWithDebris();
            cnt++;
        }

        return cnt;
    }

    ArrayList exItem;
    int itIndex;

    private void PauseGame() {
        Time.timeScale = 0;
        PausePanel.SetActive(true);

        Player playerData = GetPlayer().GetComponent<Player>();

        // Status Title
        GameObject.Find("HighSpeedAtkLabel").GetComponent<Text>().text = "[메인] " + ProcessLabel(true) + " |";
        GameObject.Find("LowSpeedAtkLabel").GetComponent<Text>().text = "[서브] " + ProcessLabel(false) + " |";
        if (playerData.ID == "Noxier") GameObject.Find("LowSpeedLabel").GetComponent<Text>().text = "구체 이동 속도 |";
        else if (playerData.ID == "Shadower") GameObject.Find("LowSpeedLabel").GetComponent<Text>().text = "그림자 이동 속도 |";
        else GameObject.Find("LowSpeedLabel").GetComponent<Text>().text = "저속 이동 속도 |";

        // Status Value
        GameObject.Find("HighSpeedAtkText").GetComponent<Text>().text = ProcessAtkData(true, ((int)(playerData.mainShotDamage * 120f / playerData.mainShotDelay)));
        GameObject.Find("LowSpeedAtkText").GetComponent<Text>().text = ProcessAtkData(false, ((int)(playerData.subShotDamage * 120f / playerData.subShotDelay)));
        GameObject.Find("HighSpeedText").GetComponent<Text>().text = ((int)(playerData.moveSpeed * 10f) / 10.0f).ToString();
        GameObject.Find("LowSpeedText").GetComponent<Text>().text = ((int)(playerData.shiftSpeed * 10f) / 10.0f).ToString();
        GameObject.Find("CoolDownText").GetComponent<Text>().text = ((int)(playerData.deltaCooldown * 10f) / 10.0f).ToString();

        Transform ItemList = GameObject.Find("ExItemList").transform;
        for (int i = 0; i <= 5; i++) {
            ItemList.GetChild(i).gameObject.SetActive(false);
        } exItem = new ArrayList(); itIndex = 0;
        foreach (ArrayList itemId in playerData.Active) {
            string pic_url = "Items/" + ((string)itemId[0]).Remove(0, 1);
            ItemList.GetChild(exItem.Count).gameObject.SetActive(true);
            ItemList.GetChild(exItem.Count).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(pic_url) as Sprite;
            exItem.Add((string)itemId[0]);
        }
        foreach (string itemId in playerData.Set) {
            string pic_url = "Items/" + itemId.Remove(0, 1);
            ItemList.GetChild(exItem.Count).gameObject.SetActive(true);
            ItemList.GetChild(exItem.Count).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(pic_url) as Sprite;
            exItem.Add(itemId);
        }
        if (exItem.Count == 0) GameObject.Find("ExItemIterator").transform.localPosition = new Vector3(0f, -200f);
        else PauseItemExplanation(0);
    }

    private void PauseItemExplanation(int a) {
        if (exItem.Count == 0)
        {
            GameObject.Find("ExItemImage").GetComponent<Image>().color = Color.clear;
            return;
        }
        else
        {
            ArrayList itemData = new ArrayList();

            foreach (ArrayList datum in ItemController.Items)
            {
                if (((string)datum[0]).Contains((string)exItem[a]))
                {
                    itemData = datum;
                    break;
                }
            }

            string pic_url = "Items/" + ((string)itemData[0]).Remove(0, 1);
            GameObject.Find("ExItemImage").GetComponent<Image>().sprite = Resources.Load<Sprite>(pic_url) as Sprite;
            GameObject.Find("ExItemImage").GetComponent<Image>().color = Color.white;
            GameObject.Find("ExNameText").GetComponent<Text>().text = (string)itemData[1];
            GameObject.Find("ExDescriptionText").GetComponent<Text>().text = (string)itemData[3];

            GameObject.Find("ExItemIterator").transform.SetParent(GameObject.Find("ExItemList").transform.GetChild(a));
            GameObject.Find("ExItemIterator").transform.localPosition = Vector3.zero;
        }
    }

    private string ProcessAtkData(bool isHigh, int raw) {
        Player playerData = GetPlayer().GetComponent<Player>();
        string playerId = playerData.ID;
        string resText = "";

        if (isHigh) {
            switch (playerId) {
                case "Silverstar":
                    resText += raw.ToString() + " x 3";
                    break;
                case "Simeka":
                    resText += raw.ToString() + " ~ " + (raw * 2).ToString();
                    break;
                case "Lune":
                    resText += raw.ToString() + " ~ " + (raw * 3).ToString();
                    break;
                case "Eclaire":
                    resText += raw.ToString() + " ~ " + (raw * 2).ToString();
                    break;
                case "Noxier":
                    resText += (raw * 5).ToString() + " ~ " + (raw * 7).ToString();
                    break;
                case "Shadower":
                    resText += "(" + raw.ToString() + "~" + (raw + (int)(playerData.subShotDamage * 120f / playerData.mainShotDelay)).ToString() + ")x3".ToString();
                    break;
                default:
                    resText += raw.ToString();
                    break;
            }
        }
        else // isLow
        {
            switch (playerId)
            {
                case "Simeka":
                    resText += raw.ToString() + " x 3";
                    break;
                case "Lune":
                    resText += (raw * 3).ToString() + " ~ " + (raw * 6).ToString();
                    break;
                case "Kamen":
                    resText += (raw * 5).ToString() + " ~ " + (raw * 20).ToString();
                    break;
                case "Nemen":
                    resText = (raw / 7).ToString();
                    break;
                case "Noxier":
                    resText = ((int)(raw / (120f / playerData.subShotDelay)) * 30).ToString();
                    break;
                case "Shadower":
                    resText += raw.ToString() + " ~ " + (raw + (int)(playerData.mainShotDamage * 120f / playerData.subShotDelay)).ToString();
                    break;
                default:
                    resText += raw.ToString();
                    break;
            }
        }

        return resText;
    }

    private string ProcessLabel(bool isHigh)
    {
        Player playerData = GetPlayer().GetComponent<Player>();
        string playerId = playerData.ID;

        if (isHigh)
        {
            switch (playerId)
            {
                case "Silverstar":
                    return "별조각 공격력";
                case "Simeka":
                    return "수리검 공격력";
                case "Ephesus":
                    return "음표 공격력";
                case "Lune":
                    return "달조각 공격력";
                case "Kamen":
                    return "단검 공격력";
                case "Eclaire":
                    return "화염 공격력";
                case "Nemen":
                    return "독침 공격력";
                case "Noxier":
                    return "박쥐 공격력";
                case "Shadower":
                    return "본체 공격력";
                default:
                    return "무기 공격력";
            }
        }
        else // isLow
        {
            switch (playerId)
            {
                case "Silverstar":
                    return "화살 공격력";
                case "Simeka":
                    return "표창 공격력";
                case "Ephesus":
                    return "음표 공격력";
                case "Lune":
                    return "달조각 공격력";
                case "Kamen":
                    return "베기 공격력";
                case "Eclaire":
                    return "물구슬 공격력";
                case "Nemen":
                    return "독 공격력";
                case "Noxier":
                    return "흑암 공격력";
                case "Shadower":
                    return "그림자 공격력";
                default:
                    return "무기 공격력";
            }
        }
    }

    private void ResumeGame() {
        Time.timeScale = 1;
        PausePanel.SetActive(false);
    }

    private GameObject GetPlayer() {
        GameObject player = null;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player")) {
            player = obj;
            break;
        }
        return player;
    }

    private void CheckGameData() {
        if (!PlayerPrefs.HasKey("Progress"))
        {
            PlayerPrefs.SetInt("Progress", 0);
            LastStage = LastStages[0];
        }
        else {
            LastStage = LastStages[PlayerPrefs.GetInt("Progress")];
        }

        Debug.Log("Game Progress Value: " + PlayerPrefs.GetInt("Progress"));
        PlayerPrefs.Save();
    }
}
