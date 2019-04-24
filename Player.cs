using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public string ID;
    public int health;

    public GameObject MainShot;
    public GameObject SubShot;
    public GameObject SupportShot;
    public GameObject Shadow;

    public Sprite[] img;
    public int imgIndex = 0;

    public float moveSpeed = 0;
    public float shiftSpeed = 0;
    float speed = 0;
    bool shift = false;

    public float mainShotDamage;
    public float subShotDamage;

    public float mainShotDelay = 0;
    public float subShotDelay = 0;

    public float deltaCooldown = 0;

    public ArrayList Active = new ArrayList();
    public ArrayList Set = new ArrayList();

    float time = 0;
    float shotTime = 0;

    bool[] ActiveEnable;
    bool recentDamaged = false;

    bool immortal = false;
    bool moveable = true;

    int lune = 3;
    int shadowAttack = 0;

    void Start()
    {
        MakePool();
        speed = moveSpeed;
        ActiveEnable = new bool[4] { true, true, true, true };
        StartCoroutine("Animate");
        StartCoroutine("Move");
        GameObject.Find("CurrentHealth").GetComponent<Text>().text = health.ToString();

        PlayerOriginalItem();
        if (ID == "Lune") {
            LuneCreation();
        }
        if (ID == "Shadower") {
            StartCoroutine(ShadowAttackDecline());
        }
    }

    public void PlayerOriginalItem() {
        switch (ID) {
            case "Silverstar":
                GameObject.Find("InventoryPanel").GetComponent<ItemController>().GetSpecificItem("#markon");
                break;
            case "Simeka":
                GameObject.Find("InventoryPanel").GetComponent<ItemController>().GetSpecificItem("#nubilum");
                break;
            case "Ephesus":
                GameObject.Find("InventoryPanel").GetComponent<ItemController>().GetSpecificItem("#master_piano");
                break;
            case "Lune":
                GameObject.Find("InventoryPanel").GetComponent<ItemController>().GetSpecificItem("#enceladus");
                break;
            case "Kamen":
                GameObject.Find("InventoryPanel").GetComponent<ItemController>().GetSpecificItem("#mars_bless");
                break;
            case "Eclaire":
                GameObject.Find("InventoryPanel").GetComponent<ItemController>().GetSpecificItem("#caster_enacra");
                break;
            case "Nemen":
                GameObject.Find("InventoryPanel").GetComponent<ItemController>().GetSpecificItem("#spider_world");
                break;
            case "Noxier":
                GameObject.Find("InventoryPanel").GetComponent<ItemController>().GetSpecificItem("#nox_orb");
                break;
            case "Shadower":
                GameObject.Find("InventoryPanel").GetComponent<ItemController>().GetSpecificItem("#shadow_swap");
                break;
            case "Eleutherios":
                GameObject.Find("InventoryPanel").GetComponent<ItemController>().GetSpecificItem("*ninja_symbol");
                GameObject.Find("InventoryPanel").GetComponent<ItemController>().GetSpecificItem("*music_cd");
                GameObject.Find("InventoryPanel").GetComponent<ItemController>().GetSpecificItem("*silverstar_bow");
                GameObject.Find("InventoryPanel").GetComponent<ItemController>().GetSpecificItem("*poison_devil");
                GameObject.Find("InventoryPanel").GetComponent<ItemController>().GetSpecificItem("*fire_badge");
                GameObject.Find("InventoryPanel").GetComponent<ItemController>().GetSpecificItem("#kamen_sword");
                GameObject.Find("InventoryPanel").GetComponent<ItemController>().GetSpecificItem("#holy_spill");
                GameObject.Find("InventoryPanel").GetComponent<ItemController>().GetSpecificItem("#enceladus");
                GameObject.Find("InventoryPanel").GetComponent<ItemController>().GetSpecificItem("#snowflakes");
                break;
            default:
                break;
        }
    }

    IEnumerator Animate()
    {
        gameObject.GetComponent<Image>().sprite = img[imgIndex];
        yield return new WaitForSeconds(1 / 4f);
        imgIndex = (imgIndex == img.Length - 1) ? 0 : imgIndex + 1;
        StartCoroutine("Animate");
    }

    int luneCreateCnt = 0;
    IEnumerator Move()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (transform.localPosition.y < 325f) transform.localPosition += new Vector3(0f, speed, 0f);
            else
            {
                Vector3 pos = transform.localPosition;
                transform.localPosition = new Vector3(pos.x, 325f, 0f);
            }
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (transform.localPosition.y > -325f) transform.localPosition += new Vector3(0f, -speed, 0f);
            else
            {
                Vector3 pos = transform.localPosition;
                transform.localPosition = new Vector3(pos.x, -325f, 0f);
            }
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (transform.localPosition.x > -610f) transform.localPosition += new Vector3(-speed, 0f, 0f);
            else
            {
                Vector3 pos = transform.localPosition;
                transform.localPosition = new Vector3(-610f, pos.y, 0f);
            }
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (transform.localPosition.x < 610f) transform.localPosition += new Vector3(speed, 0f, 0f);
            else
            {
                Vector3 pos = transform.localPosition;
                transform.localPosition = new Vector3(610f, pos.y, 0f);
            }
        }
        yield return new WaitForSeconds(1 / 120f);

        if (ID == "Lune") {
            if (luneCreateCnt++ >= 400)
            {
                luneCreateCnt = 0;
                if (lune <= 5)
                {
                    lune++;
                    LuneCreation();
                }
            }
        }      

        StartCoroutine("Move");
    }

    void Update() {
        if (Time.timeScale != 0 && moveable) {
            time++;

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {             
                if (ID == "Kamen")
                {
                    GameObject shield = null;
                    if (transform.childCount == 3) shield = transform.GetChild(2).gameObject;
                    if (shield != null)
                    {
                        shield.SetActive(true);
                        shield.GetComponent<KamenShield>().Initialize();
                    }
                }

                if (!"Noxier".Equals(ID) && !"Shadower".Equals(ID)) speed = shiftSpeed;
                else speed = 0;
                shift = true;
                if ("Shadower".Equals(ID)) {
                    shadowAttack = 30; // 10 frame per second
                    transform.GetChild(0).gameObject.GetComponent<CanvasGroup>().alpha = 0;
                    Shadow.transform.GetChild(0).gameObject.GetComponent<CanvasGroup>().alpha = 1;
                }
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                speed = moveSpeed;
                shift = false;
                if ("Shadower".Equals(ID)) {
                    shadowAttack = 30; // 10 frame per second
                    transform.GetChild(0).gameObject.GetComponent<CanvasGroup>().alpha = 1;
                    Shadow.transform.GetChild(0).gameObject.GetComponent<CanvasGroup>().alpha = 0;
                }
                        
            }

            if (Input.GetKey(KeyCode.Z))
            {
                if (!shift)
                {
                    if (time >= shotTime)
                    {
                        LaunchMain();
                        shotTime = time + mainShotDelay;
                    }
                }
                else
                {
                    if (time >= shotTime && ID != "Kamen")
                    {
                        LaunchSub();
                        if (ID != "Nemen" && ID != "Noxier") shotTime = time + subShotDelay;
                        else shotTime = time + mainShotDelay;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Z)) {
                if (ID == "Kamen" && speed == shiftSpeed) {
                    LaunchSub();
                }
            }

            if (Input.GetKey(KeyCode.X))
            {
                if (Active.Count > 0 && Active[0] != null && ActiveEnable[0])
                {
                    StartCoroutine(UseActive(0, System.Convert.ToSingle(((ArrayList)Active[0])[4])));
                }
            }

            if (Input.GetKey(KeyCode.C))
            {
                if (Active.Count > 1 && Active[1] != null && ActiveEnable[1])
                {
                    StartCoroutine(UseActive(1, System.Convert.ToSingle(((ArrayList)Active[1])[4])));  
                }
            }

            if (Input.GetKey(KeyCode.A))
            {
                if (Active.Count > 2 && Active[2] != null && ActiveEnable[2])
                {
                    StartCoroutine(UseActive(2, System.Convert.ToSingle(((ArrayList)Active[2])[4])));
                }
            }

            if (Input.GetKey(KeyCode.S))
            {
                if (Active.Count > 3 && Active[3] != null && ActiveEnable[3])
                {
                    StartCoroutine(UseActive(3, System.Convert.ToSingle(((ArrayList)Active[3])[4])));
                }
            }
        }       
    }

    void MakePool() {
        GameObject ins;

        if (MainShot != null) {
            for (int i = 0; i < 50; i++)
            {
                ins = Instantiate(MainShot, GameController.ShotPool.transform);
                GameController.ShotPool.GetComponent<ShotPool>().poolMain.Add(ins);
                ins.SetActive(false);
            }
        }

        if (SubShot != null)
        {
            for (int i = 0; i < 50; i++)
            {
                ins = Instantiate(SubShot, GameController.ShotPool.transform);
                GameController.ShotPool.GetComponent<ShotPool>().poolSub.Add(ins);
                ins.SetActive(false);
            }
        }
        
        if (SupportShot != null) {
            for (int i = 0; i < 50; i++)
            {
                ins = Instantiate(SupportShot, GameController.ShotPool.transform);
                GameController.ShotPool.GetComponent<ShotPool>().poolSupport.Add(ins);
                ins.SetActive(false);
            }
        }
    }

    void LaunchMain() {
        GameObject ins;
        switch (ID)
        {
            case "Silverstar":
                ins = GetBullet("MAIN");
                if (ins != null) ins.GetComponent<Shot>().SetIdentity(transform.localPosition, 15f, 0, 0.75f, 0.75f, mainShotDamage);
                ins = GetBullet("MAIN");
                if (ins != null) ins.GetComponent<Shot>().SetIdentity(transform.localPosition, 15f, -12f, 0.75f, 0.75f, mainShotDamage);
                ins = GetBullet("MAIN");
                if (ins != null) ins.GetComponent<Shot>().SetIdentity(transform.localPosition, 15f, 12f, 0.75f, 0.75f, mainShotDamage);
                break;
            case "Simeka":
                ins = GetBullet("MAIN");
                if (ins != null) ins.GetComponent<Shot>().SetIdentity(transform.localPosition, 30f, 0, 1f, 1f, mainShotDamage);
                break;
            case "Ephesus":
                ins = GetBullet("MAIN");
                if (ins != null)
                {
                    ins.GetComponent<Shot>().SetIdentity(transform.localPosition, 10f, 0, 1f, 1f, mainShotDamage);
                    if (masterPiano) ins.GetComponent<Shot>().additionalCode = "MASTERPIANO";
                }
                break;
            case "Lune":
                foreach (GameObject obj in LuneArray) {
                    float _direction = Mathf.Sin(obj.GetComponent<Moon>().direction * Mathf.PI / 180f);
                    ins = GetBullet("MAIN");
                    if (ins != null) ins.GetComponent<Shot>().SetIdentity(obj.transform.localPosition, 20f, 5f * _direction, 1f, 1f, mainShotDamage);
                }
                break;
            case "Kamen":
                ins = GetBullet("MAIN");
                if (ins != null) ins.GetComponent<Shot>().SetIdentity(transform.localPosition, 20f, 0, 1f, 1f, mainShotDamage);
                break;
            case "Nemen":
                ins = GetBullet("MAIN");
                if (ins != null) ins.GetComponent<Shot>().SetIdentity(transform.localPosition, 30f, 0, 1f, 1f, mainShotDamage);
                ins = GetBullet("MAIN");
                if (ins != null) ins.GetComponent<Shot>().SetIdentity(transform.localPosition, 30f, 20f, 1f, 1f, mainShotDamage);
                ins = GetBullet("MAIN");
                if (ins != null) ins.GetComponent<Shot>().SetIdentity(transform.localPosition, 30f, -20f, 1f, 1f, mainShotDamage);
                break;
            case "Noxier":
                ins = GetBullet("MAIN");
                if (ins != null)
                {
                    Shot shotData = ins.GetComponent<Shot>();
                    shotData.SetIdentity(transform.localPosition, 10f, 0, 1f, 1f, mainShotDamage / 1.5f);
                }
                break;
            case "Shadower":
                for (int i = -10; i <= 10; i += 10) {
                    ins = GetBullet("MAIN");
                    if (ins != null) ins.GetComponent<Shot>().SetIdentity(transform.localPosition, 40f, i, 1f, 1f, mainShotDamage);

                    if (shadowAttack > 0) {
                        ins = GetBullet("SUB");
                        if (ins != null) ins.GetComponent<Shot>().SetIdentity(Shadow.transform.localPosition, 40f, i, 1f, 1f, subShotDamage);
                    }
                }
                break;
            default:
                break;
        }
    }

    void LaunchSub()
    {
        GameObject ins;
        switch (ID)
        {
            case "Silverstar":
                ins = GetBullet("SUB");
                if (ins != null)
                {
                    ins.GetComponent<Shot>().SetIdentity(transform.localPosition, 30f, Random.Range(-1.5f, 1.5f), 1f, 1f, subShotDamage);
                    if (markon) ins.GetComponent<Shot>().additionalCode = "MARKON";
                }
                break;
            case "Simeka":
                ins = GetBullet("SUB");
                if (ins != null) ins.GetComponent<Shot>().SetIdentity(transform.localPosition, 20f, 0f, 1f, 1f, subShotDamage);
                ins = GetBullet("SUB");
                if (ins != null) ins.GetComponent<Shot>().SetIdentity(transform.localPosition, 20f, 15f, 1f, 1f, subShotDamage);
                ins = GetBullet("SUB");
                if (ins != null) ins.GetComponent<Shot>().SetIdentity(transform.localPosition, 20f, -15f, 1f, 1f, subShotDamage);
                break;
            case "Ephesus":
                ins = GetBullet("SUB");
                if (ins != null)
                {
                    ins.GetComponent<Shot>().SetIdentity(transform.localPosition, 25f, 0, 1f, 1f, subShotDamage);
                    if (masterPiano) ins.GetComponent<Shot>().additionalCode = "MASTERPIANO";
                }
                break;
            case "Lune":
                foreach (GameObject obj in LuneArray)
                {
                    float _direction = Mathf.Sin(obj.GetComponent<Moon>().direction * Mathf.PI / 180f);
                    ins = GetBullet("SUB");
                    if (ins != null) ins.GetComponent<Shot>().SetIdentity(obj.transform.localPosition, 20f, -5f * _direction, 1f, 1f, subShotDamage);
                }
                break;
            case "Nemen":
                ins = GetBullet("MAIN");
                if (ins != null) ins.GetComponent<Shot>().SetIdentity(transform.localPosition, 30f, 3, 1f, 1f, mainShotDamage);
                ins = GetBullet("MAIN");
                if (ins != null) ins.GetComponent<Shot>().SetIdentity(transform.localPosition, 30f, -3f, 1f, 1f, mainShotDamage);
                break;
            case "Noxier":
                ins = GetBullet("MAIN");
                if (ins != null) {
                    Shot shotData = ins.GetComponent<Shot>();
                    shotData.SetIdentity(transform.localPosition, 10f, 0, 1f, 1f, mainShotDamage / 3f);
                }
                break;
            case "Shadower":
                ins = GetBullet("SUB");
                if (ins != null) ins.GetComponent<Shot>().SetIdentity(Shadow.transform.localPosition, 40f, 0f, 1f, 1f, subShotDamage);

                if (shadowAttack > 0) {
                    ins = GetBullet("MAIN");
                    if (ins != null) ins.GetComponent<Shot>().SetIdentity(transform.localPosition, 40f, 0f, 1f, 1f, mainShotDamage);
                }
                break;
            default:
                break;
        }
    }

    public void LaunchSpecial_Invoke(string shotName) {
        StartCoroutine(LaunchSpecial(shotName));
    }

    IEnumerator LaunchSpecial(string shotName)
    {
        GameObject ins;
        switch (shotName)
        {
            case "star":
                for (int i = -20; i <= 20; i += 10) {
                    ins = GetBullet("SPECIAL_star");
                    if (ins != null) ins.GetComponent<Shot>().SetIdentity(transform.localPosition, 15f, i, 1f, 1f, mainShotDamage * 3f);
                }
                yield return new WaitForSeconds(2f);
                break;
            case "arrow":
                ins = GetBullet("SPECIAL_arrow");
                if (ins != null) ins.GetComponent<Shot>().SetIdentity(transform.localPosition, 30f, 0f, 1f, 1f, subShotDamage);
                yield return new WaitForSeconds(0.25f);
                break;
            case "kunaiS":
                ins = GetBullet("SPECIAL_kunai");
                if (ins != null) ins.GetComponent<Shot>().SetIdentity(transform.localPosition, 25f, 0, 1f, 1f, mainShotDamage * 3f);
                yield return new WaitForSeconds(0.8f);
                break;
            case "tonepair":
                ins = GetBullet("SPECIAL_tonepair");
                if (ins != null) ins.GetComponent<Shot>().SetIdentity(transform.localPosition, 20f, 0, 1f, 1f, subShotDamage);
                yield return new WaitForSeconds(0.4f);
                break;
            case "snowflake":
                ins = GetBullet("SPECIAL_snowflake");
                if (ins != null) ins.GetComponent<Shot>().SetIdentity(transform.localPosition, 15f, 0, 1f, 1f, 0);
                yield return new WaitForSeconds(8f);
                break;
            case "poisonSting":
                ins = GetBullet("SPECIAL_poisonSting");
                if (ins != null) ins.GetComponent<Shot>().SetIdentity(transform.localPosition, 25f, 0, 1f, 1f, 0);
                yield return new WaitForSeconds(4f);
                break;
            case "fire":
                Vector3 savedPos = transform.localPosition;
                for (int i = 200; i <= 800; i += 200)
                {
                    ins = Instantiate(GameObject.Find("InventoryPanel").GetComponent<ItemController>().SpecialActives[3], GameObject.Find("TriggerPanel").transform);
                    if (ins != null)
                    {
                        ins.GetComponent<Shot>().SetIdentity(savedPos + new Vector3(i, 0), 0.01f, 0f, 1f, 1f, mainShotDamage / 3);
                    }
                    yield return new WaitForSeconds(1 / 4f);
                }
                yield return new WaitForSeconds(5f);
                break;
            default:
                break;
        }
        StartCoroutine(LaunchSpecial(shotName));
    }

    public GameObject GetBullet(string code)
    {
        return GameController.ShotPool.GetComponent<ShotPool>().GetChildMin(code);
    }

    public void SpeedRefresh() {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) speed = shiftSpeed;
        else speed = moveSpeed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Bullet") {
            int dam = 1;
            if ("EPHESUS".Equals(collision.GetComponent<Bullet>().moveCode)) dam++;

            if (collision.GetComponent<Bullet>().moveCode != "DEBRIS" && !immortal) {
                if (Set.Contains("*divine_mirror"))
                {
                    int rand = Random.Range(0, 2);
                    if (rand == 0) health -= dam;
                }
                else
                {
                    ParticleController Par = GameObject.Find("ParticleController").GetComponent<ParticleController>();
                    GameObject ins = Instantiate(Par.ParticleHit, GameObject.Find("ParticlePanel").transform);
                    ins.transform.localPosition = transform.localPosition;
                    ins.GetComponent<Image>().color = Color.red;
                    health -= dam;
                }
                GameController.EraseBullet();
                GameObject.Find("CurrentHealth").GetComponent<Text>().text = health.ToString();
                StartCoroutine("Damaged");
                if (Set.Contains("*zealot_axe")) {
                    mainShotDamage += 0.2f;
                    subShotDamage += 0.2f;
                }
            }

            if (health < 0 && moveable) {
                moveable = false;
                immortal = true;
                transform.localScale = Vector3.zero;
                GameController.pausable = false;
                StartCoroutine(GameOver());
            }
        }    
    }

    IEnumerator Cooldown(int index, float cooltimeReduce) {
        float RealCooltime = (int)((ArrayList)Active[index])[5] * (1 - cooltimeReduce);

        for (int cnt = 0; cnt < (int)RealCooltime; cnt++) {
            GameObject.Find("InventoryPanel").GetComponent<ItemController>().CooldownDisp(index, (int)RealCooltime - cnt);
            yield return new WaitForSeconds(1f - deltaCooldown);
        }
        ActiveEnable[index] = true;

        if (index == 0) GameObject.Find("CoolDownX").SetActive(false);
        else if (index == 1) GameObject.Find("CoolDownC").SetActive(false);
        else if (index == 2) GameObject.Find("CoolDownA").SetActive(false);
        else if (index == 3) GameObject.Find("CoolDownS").SetActive(false);
    }

    IEnumerator Damaged() {      
        if (ID == "Lune")
        {
            lune = 3;
            LuneCreation();
        }
        recentDamaged = true; immortal = true;
        GetComponent<Image>().color = Color.red;
        yield return new WaitForSeconds(0.5f);
        recentDamaged = false; immortal = false;
        GetComponent<Image>().color = Color.white;
    }

    /* original item status variables */
    bool markon = false, masterPiano = false;
    public bool mars = false;

    IEnumerator UseActive(int index, float continueTime)
    {
        GameObject ins;

        ActiveEnable[index] = false;
        float cooltimeReduce = 0;  // Real cooltime: cooltime * (1 - cooltimeReduce)
        string itemId = (string)((ArrayList)Active[index])[0];

        if (continueTime != 0) {
            GameObject target = GameObject.Find("ActivePanel_" + index);
            target.GetComponent<Image>().color = Color.green;

            GameObject targetText = target.transform.GetChild(0).gameObject;
            targetText.GetComponent<Text>().color = Color.green;
            targetText.GetComponent<Text>().text = "ON";
        }

        ins = Instantiate(GameObject.Find("InventoryPanel").GetComponent<ItemController>().ItemDebris,
            GameObject.Find("ActivePanel_" + index).transform);
        if (ins != null) ins.GetComponent<ItemDebris>().SetIdentity(itemId.Remove(0, 1));
        
        switch (itemId)
        {
            case "#book_of_extinction":
                GameController.EraseBullet();
                yield return null;
                break;
            case "#book_of_equalibrium":
                if(GameController.EraseBullet_ReturnAmount() >= 200) health += 1;
                yield return null;
                break;
            case "#mana_boot":
                float speedTemp1 = moveSpeed * 0.5f, speedTemp2 = shiftSpeed * 0.5f;
                moveSpeed += speedTemp1; shiftSpeed += speedTemp2;
                SpeedRefresh();
                yield return new WaitForSeconds(continueTime);
                moveSpeed -= speedTemp1; shiftSpeed -= speedTemp2;
                SpeedRefresh();
                break;
            case "#steroid":
                float atkTemp1 = mainShotDamage, atkTemp2 = subShotDamage;
                mainShotDamage += atkTemp1; subShotDamage += atkTemp2;
                yield return new WaitForSeconds(continueTime);
                mainShotDamage -= atkTemp1; subShotDamage -= atkTemp2;
                break;
            case "#morphin":
                float delTemp1 = mainShotDelay * 0.5f, delTemp2 = subShotDelay * 0.5f;
                mainShotDelay -= delTemp1; subShotDelay -= delTemp2;
                yield return new WaitForSeconds(continueTime);
                mainShotDelay += delTemp1; subShotDelay += delTemp2;
                break;
            case "#feather":
                immortal = true; GetComponent<Image>().color = Color.yellow;
                yield return new WaitForSeconds(continueTime);
                immortal = false; GetComponent<Image>().color = Color.white;
                break;
            case "#parring_shield":
                if (recentDamaged)
                {
                    health += 1;
                    mainShotDamage += 0.5f; subShotDamage += 0.5f;
                }
                else cooltimeReduce = 0.9f;
                yield return null;
                break;
            case "#kunais":
                for (int i = -45; i <= 45; i += 15) {
                    ins = GetBullet("SPECIAL_kunai");
                    ins.GetComponent<Shot>().SetIdentity(transform.localPosition, 30, i, 1f, 1f, mainShotDamage * 3f);
                }           
                yield return null;
                break;
            case "#little_tone":
                for (int i = 0; i < 15; i++)
                {
                    ins = GetBullet("SPECIAL_tonesingle");
                    ins.GetComponent<Shot>().SetIdentity(transform.localPosition, Random.Range(5f, 8f), Random.Range(0f, 360f), 1f, 1f, subShotDamage);
                    ins = GetBullet("SPECIAL_tonepair");
                    ins.GetComponent<Shot>().SetIdentity(transform.localPosition, Random.Range(11f, 14f), Random.Range(0f, 360f), 1f, 1f, subShotDamage);
                    yield return new WaitForSeconds(1 / 30f);
                }
                yield return null;
                break;
            case "#snowflakes":
                for (float i = Random.Range(0f, 60f); i < 360; i += 60)
                {
                    ins = GetBullet("SPECIAL_snowflake");
                    ins.GetComponent<Shot>().SetIdentity(transform.localPosition, 10, i, 1f, 1f, 0);
                    ins = GetBullet("SPECIAL_snowflake");
                    ins.GetComponent<Shot>().SetIdentity(transform.localPosition, 20, i + 30f, 1f, 1f, 0);
                }
                yield return null;
                break;
            case "#suicide_bomb":
                ins = Instantiate(GameObject.Find("InventoryPanel").GetComponent<ItemController>().SpecialActives[0], GameObject.Find("TriggerPanel").transform);
                ins.GetComponent<Shot>().SetIdentity(transform.localPosition, 0, 0, 1f, 1f, (mainShotDamage / 3) + (subShotDamage / 3));
                yield return null; break;
            case "#revolution_torch":
                ins = Instantiate(GameObject.Find("InventoryPanel").GetComponent<ItemController>().SpecialActives[3], GameObject.Find("TriggerPanel").transform);
                if (ins != null) ins.GetComponent<Shot>().SetIdentity(new Vector3(-400, 0), 0.01f, 0f, 8f, 8f, mainShotDamage / 2);
                ins = Instantiate(GameObject.Find("InventoryPanel").GetComponent<ItemController>().SpecialActives[3], GameObject.Find("TriggerPanel").transform);
                if (ins != null) ins.GetComponent<Shot>().SetIdentity(new Vector3(400, 0), 0.01f, 0f, 8f, 8f, mainShotDamage / 2);
                break;
            case "#holy_spill":
                foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    ins = Instantiate(GameObject.Find("InventoryPanel").GetComponent<ItemController>().SpecialActives[4], GameObject.Find("TriggerPanel").transform);
                    if (ins != null) ins.GetComponent<Shot>().SetIdentity(enemy.transform.localPosition, 0.01f, 0f, 1f, 1f, subShotDamage / 2);
                }
                break;
            case "#kamen_sword":
                ins = Instantiate(GameObject.Find("InventoryPanel").GetComponent<ItemController>().SpecialActives[5], GameObject.Find("TriggerPanel").transform);
                ins.GetComponent<Shot>().SetIdentity(transform.localPosition, 0.01f, 60f, 1f, 1f, subShotDamage * 2);
                break;
            case "#illusion_cube":
                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Bullet"))
                {
                    obj.GetComponent<Bullet>().speed = Random.Range(2f, 10f);
                    obj.GetComponent<Bullet>().direction = Random.Range(0f, 360f);
                }
                yield return null; break;
            case "#golden_apple":
                if (health < 10) health += 1;
                yield return null; break;
            case "#salamander_egg":
                if (GameController.rank < 10) {
                    GameController.rank += 1;
                    mainShotDamage *= 1.5f; subShotDamage *= 1.5f;
                }
                yield return null; break;
            case "#green_maple":
                ins = Instantiate(GameObject.Find("InventoryPanel").GetComponent<ItemController>().SpecialItems[2], GameObject.Find("TriggerPanel").transform);
                ins.transform.localPosition = transform.localPosition + new Vector3(200f, 0f);
                ins.GetComponent<SpecialItem>().SetPlayer(gameObject);
                yield return null; break;
            case "#sun_badge":
                ins = Instantiate(GameObject.Find("InventoryPanel").GetComponent<ItemController>().SpecialActives[7], GameObject.Find("TriggerPanel").transform);
                ins.GetComponent<Shot>().SetIdentity(Vector3.zero, 0.01f, 0, 1f, 1f, (mainShotDamage + subShotDamage) / 20f);
                yield return null; break;
            case "#steel_anchor":
                for (int i = -400; i <= 400; i += 400) {
                    ins = Instantiate(GameObject.Find("InventoryPanel").GetComponent<ItemController>().SpecialActives[8], GameObject.Find("TriggerPanel").transform);
                    ins.GetComponent<Shot>().SetIdentity(new Vector3(i, 600), 10f, 270, 1f, 1f, (mainShotDamage + subShotDamage) / 3f);
                    ins.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, -28f));

                    yield return new WaitForSeconds(1 / 3f);
                }      
                break;
            case "#brick_wall":
                for (int i = 0; i < 30; i++) {
                    ins = Instantiate(GameObject.Find("InventoryPanel").GetComponent<ItemController>().SpecialItems[3], GameObject.Find("TriggerPanel").transform);
                    ins.transform.localPosition = transform.localPosition + new Vector3(Random.Range(100f, 200f), Random.Range(-100f, 100f));
                    ins.GetComponent<SpecialItem>().SetPlayer(gameObject);
                    yield return new WaitForSeconds(1 / 15f);
                }
                break;
            case "#cosmo_kateric":
                float atkTemp3 = mainShotDamage * 3, atkTemp4 = subShotDamage * 3;
                mainShotDamage += atkTemp3; subShotDamage += atkTemp4;
                yield return new WaitForSeconds(continueTime);
                mainShotDamage -= atkTemp3; subShotDamage -= atkTemp4;
                if(Set.Contains("*alpharia_marie") || Set.Contains("*betaria_pasen")) cooltimeReduce = 0.5f;
                break;
            case "#jackpot_slot":
                ItemController _ItemController = GameObject.Find("InventoryPanel").GetComponent<ItemController>();
                mainShotDamage = (mainShotDamage > 1) ? mainShotDamage - 1 : 0;
                subShotDamage = (subShotDamage > 1) ? subShotDamage - 1 : 0;
                ArrayList sample = new ArrayList();

                foreach (ArrayList ar in ItemController.Items)
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
                _ItemController.GetSpecificItem((string)((ArrayList)sample[randomSeed])[0]);
                break;
            case "#bloody_gemstone":
                if (health > 3) {
                    health -= 3;
                    mainShotDamage += 1;
                    subShotDamage += 1;
                }
                break;
            case "#markon":
                markon = true;
                yield return new WaitForSeconds(continueTime);
                markon = false;
                break;
            case "#nubilum":
                atkTemp1 = mainShotDamage; atkTemp2 = subShotDamage;
                mainShotDamage += atkTemp1; subShotDamage += atkTemp2;
                delTemp1 = mainShotDelay * 0.5f; delTemp2 = subShotDelay * 0.5f;
                mainShotDelay -= delTemp1; subShotDelay -= delTemp2;
                yield return new WaitForSeconds(continueTime);
                mainShotDamage -= atkTemp1; subShotDamage -= atkTemp2;
                mainShotDelay += delTemp1; subShotDelay += delTemp2;
                break;
            case "#enceladus":
                ins = Instantiate(GameObject.Find("InventoryPanel").GetComponent<ItemController>().SpecialActives[1], GameObject.Find("TriggerPanel").transform);
                ins.GetComponent<Shot>().SetIdentity(new Vector3(-1000f, 0f), 12, 0, 1f, 1f, (mainShotDamage + subShotDamage) / 2);
                break;
            case "#master_piano":
                masterPiano = true;
                yield return new WaitForSeconds(continueTime);
                masterPiano = false;
                break;
            case "#mars_bless":
                mars = true;
                yield return new WaitForSeconds(continueTime);
                mars = false;
                break;
            case "#caster_enacra":
                ArcanePanel ArcanePanel = transform.GetChild(2).gameObject.GetComponent<ArcanePanel>(); 
                ArcanePanel.enacra = true;
                delTemp1 = mainShotDelay * 0.5f; delTemp2 = subShotDelay * 0.5f;
                mainShotDelay -= delTemp1; subShotDelay -= delTemp2;
                yield return new WaitForSeconds(continueTime);
                ArcanePanel.enacra = false;
                mainShotDelay += delTemp1; subShotDelay += delTemp2;
                break;
            case "#spider_world":
                foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
                    Transform enemyTr = enemy.transform;
                    for (int i = enemyTr.childCount - 1; i >= 0; i--)
                    {
                        if ("PoisonGenerator(Clone)".Equals(enemyTr.GetChild(i).name))
                        {
                            ins = Instantiate(GameObject.Find("InventoryPanel").GetComponent<ItemController>().SpecialActives[2], GameObject.Find("TriggerPanel").transform);
                            ins.GetComponent<Shot>().SetIdentity(enemyTr.localPosition, 0.01f, 0f, 1f, 1f, subShotDamage / 3f);
                            break;
                        }
                    }
                }
                break;
            case "#nox_orb":
                GameObject orb = GameObject.Find("NoxOrb");
                if (orb != null) {
                    orb.GetComponent<NoxOrb>().DarkInstantiate();
                }
                break;
            case "#shadow_swap": 
                foreach (GameObject target in GameObject.FindGameObjectsWithTag("Bullet")) {
                    if (Vector3.Magnitude(target.transform.localPosition - transform.localPosition) <= 300 ||
                       Vector3.Magnitude(target.transform.localPosition - Shadow.transform.localPosition) <= 300) {
                        target.GetComponent<Bullet>().UnableWithDebris();
                    }
                }
                Vector3 tempPos = Shadow.transform.localPosition;
                Shadow.transform.localPosition = transform.localPosition;
                transform.localPosition = tempPos;
                break;
            default:
                break;
        }

        if (continueTime != 0)
        {
            string[] tempString = new string[4] { "X", "C", "A", "S" };

            GameObject target = GameObject.Find("ActivePanel_" + index);
            target.GetComponent<Image>().color = Color.white;

            GameObject targetText = target.transform.GetChild(0).gameObject;
            targetText.GetComponent<Text>().color = Color.white;
            targetText.GetComponent<Text>().text = "Act [" + tempString[index] + "]";
        }

        StartCoroutine(Cooldown(index, cooltimeReduce));
        GameObject.Find("CurrentHealth").GetComponent<Text>().text = health.ToString();
        GameObject.Find("CurrentRank").GetComponent<Text>().text = GameController.rank.ToString();
    }

    // for player 'Lune'
    ArrayList LuneArray;
    private void LuneCreation() {
        LuneArray = new ArrayList();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("SpecialShot")) {
            Destroy(obj);
        }

        float _direction = Random.Range(0f, 360f);
        for (int i = 0; i < lune; i++) {
            GameObject ins = Instantiate(SupportShot, GameObject.Find("TriggerPanel").transform);
            ins.GetComponent<Moon>().direction = _direction;
            LuneArray.Add(ins);

            _direction += (360f / lune);
        }
    }

    private IEnumerator ShadowAttackDecline() {
        while (true) {
            if (shadowAttack > 0) shadowAttack -= 1;
            yield return new WaitForSeconds(1 / 10f);
        }
    }

    private IEnumerator GameOver() {
        RectTransform GameoverPanel = GameObject.Find("GameoverPanel").GetComponent<RectTransform>();
        GameoverPanel.sizeDelta = new Vector2(0f, 900f);

        for (int i = 1; GameoverPanel.sizeDelta.x < 1300; GameoverPanel.sizeDelta += new Vector2(0.2f * i, 0f)) {          
            moveable = false;
            transform.localScale = Vector3.zero;
            immortal = true;
            GameController.pausable = false;
            i += 2;
            yield return new WaitForSeconds(1 / 120f);
        }

        Time.timeScale = 0;
        yield return null;
    }

    public void Reincarnate() {
        StartCoroutine(Reincarnation());
    }

    private IEnumerator Reincarnation()
    {
        RectTransform GameoverPanel = GameObject.Find("GameoverPanel").GetComponent<RectTransform>();
        Text GameoverText = GameObject.Find("GameoverDesc").GetComponent<Text>();
        health = 25;
        Time.timeScale = 1;

        GameoverText.color = Color.yellow;
        GameoverText.text = "아니, 이제는 시간이 없어.";

        GameoverPanel.sizeDelta = new Vector2(1280f, 900f);

        for (int i = 1; GameoverPanel.sizeDelta.x > 0; GameoverPanel.sizeDelta -= new Vector2(0.2f * i, 0f))
        {
            moveable = true;
            transform.localScale = new Vector3(-1, 1, 1);
            immortal = false;
            GameController.pausable = false;
            i += 2;
            yield return new WaitForSeconds(1 / 120f);
        }

        GameoverText.color = Color.red;
        GameoverText.text = "걱정하지마. 이 시간대는 분해되어 공허 속으로 사라질 테니까.\n처음부터, 다시 시작하면 되는 거야.";
        yield return null;
    }
}
