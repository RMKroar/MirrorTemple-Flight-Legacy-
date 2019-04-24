using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnivernGenesis : Boss {

    float _speed = 0;
    float direction = 0;
    float radDir;

    IEnumerator PatternSupport;

    void Start()
    {
        immortal = true;
        pattern = maxPattern;

        maxHealth = health;

        ParticleController Par = GameObject.Find("ParticleController").GetComponent<ParticleController>();
        GameObject ins = Instantiate(Par.ParticleAscend, GameObject.Find("ParticlePanel").transform);
        ins.transform.localPosition = transform.localPosition;
        ins.GetComponent<Image>().color = Color.white;

        StartCoroutine(Chaos());
        StartCoroutine("Animate");
        StartCoroutine("StartLaunch");
        StartCoroutine("Move");
    }

    IEnumerator Chaos() {
        Text rankText = GameObject.Find("CurrentRank").GetComponent<Text>();
        while (true) {
            rankText.text = Random.Range(0, 1000).ToString();

            yield return new WaitForSeconds(1 / 30f);
        }
    }

    IEnumerator Move()
    {
        yield return new WaitForSeconds(moveFrequency);
        _speed = moveSpeed;
        float judgeDirection = transform.localPosition.y + Random.Range(-90f, 90f);

        direction = (judgeDirection >= 0) ? 270 : 90;
        if (transform.localPosition.x >= 450)
        {
            direction = (direction == 270) ? Random.Range(260f, 270f) : Random.Range(90f, 100f);
        }
        else
        {
            direction = (direction == 270) ? Random.Range(270f, 280f) : Random.Range(80f, 90f);
        }
        radDir = direction * Mathf.PI / 180f;

        StartCoroutine("MoveSupport");
        StopCoroutine("Animate");
        StartCoroutine("Move");
    }

    IEnumerator MoveSupport()
    {
        GetComponent<Image>().sprite = MoveImage;
        transform.localPosition += new Vector3(Mathf.Cos(radDir) * _speed, Mathf.Sin(radDir) * _speed, 0);
        yield return new WaitForSeconds(1 / 60f);
        _speed -= moveFriction;
        if (_speed <= 0)
        {
            StartCoroutine("Animate");
        }
        else StartCoroutine("MoveSupport");
    }

    IEnumerator Launch()
    {
        StartCoroutine("Pattern_" + (maxPattern - pattern));
        immortal = false;
        yield return null;
    }

    IEnumerator Pattern_0() {
        GameObject ins;
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();       
        for (int i = 0; i < 100; i++) {
            cct.Play();
            ins = GetBullet("CIRCLE");
            if (ins != null) {
                ins.GetComponent<Bullet>().SetIdentityEx("UNIVERN_0", transform.localPosition + new Vector3(-100f * Mathf.Cos((float)i / 3), 100f * Mathf.Sin((float)i / 3)), 9f, ToPlayerAngle() + Random.Range(-50f, 50f), 0.7f, 0.7f,
                    Color.white, new Color(Random.Range(0.7f, 1f), Random.Range(0.7f, 1f), Random.Range(0.7f, 1f)));
            }
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("UNIVERN_0", transform.localPosition + new Vector3(-100f * Mathf.Cos((float)i / 3), -100f * Mathf.Sin((float)i / 3)), 7f, ToPlayerAngle() + Random.Range(-50f, 50f), 0.7f, 0.7f,
                    Color.white, new Color(Random.Range(0.4f, 0.7f), Random.Range(0.4f, 0.7f), Random.Range(0.4f, 0.7f)));
            }
            yield return new WaitForSeconds(1 / 20f);
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(Launch());
    }

    IEnumerator Pattern_1() {
        GameObject ins;
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();

        string[] items = new string[3] { "red_candle", "blue_candle", "green_candle" };
        Color[] colors = new Color[3] { Color.red, Color.blue, Color.green };
        int randomSeed = Random.Range(0, 3);
        GiveItem(items[randomSeed]);
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < 100; i++)
        {
            cct.Play();
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("UNIVERN_0", new Vector3(Random.Range(-700f, 700f), 400f), Random.Range(4f, 9f), Random.Range(260f, 280f), 0.5f, 0.5f,
                    Color.white, colors[randomSeed]);
            }
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("UNIVERN_0", new Vector3(Random.Range(-700f, 700f), -400f), Random.Range(4f, 9f), Random.Range(80f, 100f), 0.5f, 0.5f,
                    Color.white, colors[randomSeed]);
            }
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("UNIVERN_0", new Vector3(-700f, Random.Range(-400f, 400f)), Random.Range(4f, 9f), Random.Range(-10f, 10f), 0.5f, 0.5f,
                    Color.white, colors[randomSeed]);
            }
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("UNIVERN_0", new Vector3(700f, Random.Range(-400f, 400f)), Random.Range(4f, 9f), Random.Range(170f, 190f), 0.5f, 0.5f,
                    Color.white, colors[randomSeed]);
            }
            yield return new WaitForSeconds(1 / 10f);
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(Launch());
    }

    bool flag_pat2 = false;
    IEnumerator Pattern_2()
    {
        if (!flag_pat2) {
            GiveItem("grey_candle");
            flag_pat2 = true;
        }
        GameObject ins;
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();
        AudioSource pct = GameObject.Find("Sound_EnemyPetal").GetComponent<AudioSource>();
        Image Back1Panel = GameObject.Find("Back1Panel").GetComponent<Image>();
        Image Back2Panel = GameObject.Find("Back2Panel").GetComponent<Image>();

        for (int i = 1; i < 120; i++)
        {
            if (i % 20 == 0) {
                pct.volume = 0.35f; pct.Play();
                Time.timeScale += 0.6f;
                Back1Panel.color -= new Color(0f, 0.15f, 0.15f, 0f);
                Back2Panel.color -= new Color(0f, 0.15f, 0.15f, 0f);
            }

            cct.Play();
            for (float randDir = ToPlayerAngle() + Random.Range(-50f, 50f), j = -30; j <= 30; j += 30) {
                ins = GetBullet("CIRCLE");
                if (ins != null)
                {
                    ins.GetComponent<Bullet>().SetIdentityEx("UNIVERN_0", transform.localPosition + new Vector3(-100f * Mathf.Cos((float)i / 3), 100f * Mathf.Sin((float)i / 3)), 9f, randDir + j, 0.5f, 0.5f,
                        Color.white, new Color(Random.Range(0.7f, 1f), Random.Range(0.7f, 1f), Random.Range(0.7f, 1f)));
                }
            }
            for (float randDir = ToPlayerAngle() + Random.Range(-50f, 50f), j = -30; j <= 30; j += 30)
            {
                ins = GetBullet("CIRCLE");
                if (ins != null)
                {
                    ins.GetComponent<Bullet>().SetIdentityEx("UNIVERN_0", transform.localPosition + new Vector3(-100f * Mathf.Cos((float)i / 3), -100f * Mathf.Sin((float)i / 3)), 7f, randDir + j, 0.5f, 0.5f,
                        Color.white, new Color(Random.Range(0.4f, 0.7f), Random.Range(0.4f, 0.7f), Random.Range(0.4f, 0.7f)));
                }
            }
            yield return new WaitForSeconds(1 / 10f);
        }
        Time.timeScale = 1;
        Back1Panel.color = Color.white;
        Back2Panel.color = Color.white;
        yield return new WaitForSeconds(1f);
        StartCoroutine(Launch());
    }

    IEnumerator Pattern_3() {
        GameObject ins;
        AudioSource lct = GameObject.Find("Sound_EnemyLazer").GetComponent<AudioSource>();
        AudioSource pct = GameObject.Find("Sound_EnemyPetal").GetComponent<AudioSource>();

        Image Back1Panel = GameObject.Find("Back1Panel").GetComponent<Image>();
        Image Back2Panel = GameObject.Find("Back2Panel").GetComponent<Image>();

        pct.volume = 0.35f; pct.Play();
        Back1Panel.color = Color.black; Back2Panel.color = Color.black;

        yield return new WaitForSeconds(0.5f);

        for (int j = 0; j < 20; j++) {
            lct.volume = 0.35f; lct.Play();
            for (float i = Random.Range(-60f, -50f); i < 55f; i += 10)
            {
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ALCADROLAZZOSHORT", transform.localPosition, 15f, ToPlayerAngle() + i,
                    0.3f, 0.3f, Color.white, Color.white);
                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("UNIVERN_0", transform.localPosition, 5f, ToPlayerAngle() + i,
                    0.5f, 0.5f, Color.black, Color.black);
            }

            yield return new WaitForSeconds(0.4f);        
        }
        pct.volume = 0.35f; pct.Play();
        Back1Panel.color = Color.white; Back2Panel.color = Color.white;
        yield return new WaitForSeconds(1f);
        StartCoroutine("Launch");
    }

    IEnumerator Pattern_4() {
        GameObject ins;
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();
        for (int i = 0; i < 50; i++)
        {
            cct.Play();
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("UNIVERN_0", transform.localPosition + new Vector3(-100f * Mathf.Cos((float)i / 3), 100f * Mathf.Sin((float)i / 3)), 3f, ToPlayerAngle() + Random.Range(-50f, 50f), 0.5f, 0.5f,
                    Color.white, new Color(Random.Range(0.7f, 1f), Random.Range(0.7f, 1f), Random.Range(0.7f, 1f)));
            }
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("UNIVERN_0", transform.localPosition + new Vector3(-100f * Mathf.Cos((float)i / 3), -100f * Mathf.Sin((float)i / 3)), 2f, ToPlayerAngle() + Random.Range(-50f, 50f), 0.5f, 0.5f,
                    Color.white, new Color(Random.Range(0.4f, 0.7f), Random.Range(0.4f, 0.7f), Random.Range(0.4f, 0.7f)));
            }
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("UNIVERN_0", transform.localPosition + new Vector3(200f * Mathf.Cos((float)i / 3), 200f * Mathf.Sin((float)i / 3)), 5f, ToPlayerAngle() + Random.Range(-50f, 50f), 0.5f, 0.5f,
                    Color.white, new Color(Random.Range(0.2f, 0.5f), Random.Range(0.2f, 0.5f), Random.Range(0.2f, 0.5f)));
            }
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("UNIVERN_0", transform.localPosition + new Vector3(200f * Mathf.Cos((float)i / 3), -200f * Mathf.Sin((float)i / 3)), 4f, ToPlayerAngle() + Random.Range(-50f, 50f), 0.5f, 0.5f,
                    Color.white, new Color(Random.Range(0f, 0.3f), Random.Range(0f, 0.3f), Random.Range(0f, 0.3f)));
            }
            yield return new WaitForSeconds(1 / 10f);
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(Launch());
    }

    bool flag_pat5 = false;
    IEnumerator Pattern_5()
    {
        if (!flag_pat5) {
            PatternSupport = Pattern_5_Support();
            StartCoroutine(PatternSupport);
            flag_pat5 = true;
        }

        GameObject ins;
        AudioSource lct = GameObject.Find("Sound_EnemyLazer").GetComponent<AudioSource>();
        Color[] colors = new Color[8] { Color.white, Color.red, new Color(1f, 0.5f, 0f), Color.yellow, Color.green, BulletCode.COLOR_SKY, BulletCode.COLOR_VIOLET, Color.black };
        int dens = 15;

        for (int k = 0; k < 3; k++) {
            for (int i = 0; i < 8; i++)
            {
                float randValue = Random.Range(0f, 360f);
                lct.volume = 0.35f; lct.Play();

                for (float j = 0; j < 360; j += 360f / dens)
                {
                    ins = GetBullet("PETAL");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ALCADROLAZZOSHORT",
                        transform.localPosition + new Vector3((40 * (i + 1)) * Mathf.Cos((randValue + j) * Mathf.PI / 180f), (40 * (i + i)) * Mathf.Sin((randValue + j) * Mathf.PI / 180f)),
                        17 - i, randValue + j + (60 - 8 * i), 0.3f, 0.3f, Color.white, colors[i]);
                    ins = GetBullet("PETAL");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ALCADROLAZZOSHORT",
                        transform.localPosition + new Vector3((40 * (i + 1)) * Mathf.Cos((randValue + j) * Mathf.PI / 180f), (40 * (i + i)) * Mathf.Sin((randValue + j) * Mathf.PI / 180f)),
                        17 - i, randValue + j - (60 - 8 * i), 0.3f, 0.3f, Color.white, colors[i]);
                }

                yield return new WaitForSeconds(0.2f);
            }
        }

        yield return new WaitForSeconds(1.5f);
        StartCoroutine("Launch");
    }

    bool flag_pat6 = false;
    IEnumerator Pattern_6()
    {
        if (!flag_pat6)
        {
            StopCoroutine(PatternSupport);
            GameObject.Find("TriggerPanel").transform.localRotation = Quaternion.Euler(Vector3.zero);
            flag_pat6 = true;
        }

        GameObject ins;
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();
        AudioSource pct = GameObject.Find("Sound_EnemyPetal").GetComponent<AudioSource>();
        Image Back1Panel = GameObject.Find("Back1Panel").GetComponent<Image>();
        Image Back2Panel = GameObject.Find("Back2Panel").GetComponent<Image>();

        for (int i = 0; i < 60; i++)
        {
            if (i % 10 == 0)
            {
                pct.volume = 0.35f; pct.Play();
                Time.timeScale += 0.6f;
                Back1Panel.color -= new Color(0f, 0.15f, 0.15f, 0f);
                Back2Panel.color -= new Color(0f, 0.15f, 0.15f, 0f);
            }

            cct.Play();
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("UNIVERN_0", transform.localPosition + new Vector3(-100f * Mathf.Cos((float)i / 3), 100f * Mathf.Sin((float)i / 3)), 3f, ToPlayerAngle() + Random.Range(-50f, 50f), 0.4f, 0.4f,
                    Color.white, new Color(Random.Range(0.7f, 1f), Random.Range(0.7f, 1f), Random.Range(0.7f, 1f)));
            }
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("UNIVERN_0", transform.localPosition + new Vector3(-100f * Mathf.Cos((float)i / 3), -100f * Mathf.Sin((float)i / 3)), 2f, ToPlayerAngle() + Random.Range(-50f, 50f), 0.4f, 0.4f,
                    Color.white, new Color(Random.Range(0.4f, 0.7f), Random.Range(0.4f, 0.7f), Random.Range(0.4f, 0.7f)));
            }
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("UNIVERN_0", transform.localPosition + new Vector3(200f * Mathf.Cos((float)i / 3), 200f * Mathf.Sin((float)i / 3)), 4f, ToPlayerAngle() + Random.Range(-50f, 50f), 0.4f, 0.4f,
                    Color.white, new Color(Random.Range(0.2f, 0.5f), Random.Range(0.2f, 0.5f), Random.Range(0.2f, 0.5f)));
            }
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("UNIVERN_0", transform.localPosition + new Vector3(200f * Mathf.Cos((float)i / 3), -200f * Mathf.Sin((float)i / 3)), 3f, ToPlayerAngle() + Random.Range(-50f, 50f), 0.4f, 0.4f,
                    Color.white, new Color(Random.Range(0f, 0.3f), Random.Range(0f, 0.3f), Random.Range(0f, 0.3f)));
            }
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("UNIVERN_0", transform.localPosition + new Vector3(300f * Mathf.Cos((float)i / 3), 300f * Mathf.Sin((float)i / 3)), 4f, ToPlayerAngle() + Random.Range(-50f, 50f), 0.4f, 0.4f,
                    Color.white, Color.white);
            }
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("UNIVERN_0", transform.localPosition + new Vector3(-300f * Mathf.Cos((float)i / 3), 300f * Mathf.Sin((float)i / 3)), 3f, ToPlayerAngle() + Random.Range(-50f, 50f), 0.4f, 0.4f,
                    Color.white, Color.black);
            }
            yield return new WaitForSeconds(1 / 5f);
        }
        Time.timeScale = 1;
        Back1Panel.color = Color.white;
        Back2Panel.color = Color.white;
        yield return new WaitForSeconds(1f);
        StartCoroutine(Launch());
    }

    bool flag_pat7 = false;
    IEnumerator Pattern_7() {
        if (!flag_pat7)
        {
            PatternSupport = Pattern_7_Support();
            StartCoroutine(PatternSupport);
            flag_pat7 = true;
        }

        GameObject ins;
        AudioSource pct = GameObject.Find("Sound_EnemyPetal").GetComponent<AudioSource>();
        pct.volume = 0.35f; pct.Play();
        for (float i = 400, count = 0; i >= -400; i -= 15, count++)
        {
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("UNIVERN_0", new Vector3(300 + (i / 2f), i),
                                 0.01f, ToPlayerAngle() + Random.Range(-60f, 60f), 0.35f, 0.35f, Color.white, Color.white);
            }
            if (count % 7 == 0) yield return new WaitForSeconds(1 / 120f);
        }
        yield return new WaitForSeconds(0.1f);

        pct.volume = 0.35f; pct.Play();
        for (float i = 400, count = 0; i >= -400; i -= 15, count++)
        {
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("UNIVERN_0", new Vector3(300 - (i / 2f), i),
                                 0.01f, ToPlayerAngle() + Random.Range(-60f, 60f), 0.35f, 0.35f, Color.black, Color.black);
            }
            if (count % 7 == 0) yield return new WaitForSeconds(1 / 120f);
        }
        yield return new WaitForSeconds(0.2f);
        yield return new WaitForSeconds(1f);
        StartCoroutine(Launch());
    }

    bool flag_pat8 = false;
    IEnumerator Pattern_8() {
        if (!flag_pat8) {
            if(PatternSupport != null) StopCoroutine(PatternSupport);
            GameObject.Find("TriggerPanel").transform.localScale = Vector3.one;
            maxHealth *= 2;
            health *= 2;
            flag_pat8 = true;
        }

        GameObject ins;
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();
        float vertical_screenSizeDelta = 0;
        float horizontal_screenSizeDelta = 0;
        while (true) {
            cct.Play();
            vertical_screenSizeDelta += 0.07f;
            horizontal_screenSizeDelta += 0.03f;
            Screen.SetResolution((int)(780 + 500 * Mathf.Cos(horizontal_screenSizeDelta)), (int)(580 + 300 * Mathf.Cos(vertical_screenSizeDelta)), false);
            Color randomColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("UNIVERN_0", new Vector3(700f, Random.Range(-250f - 150 * Mathf.Cos(vertical_screenSizeDelta), 250f + 150 * Mathf.Cos(vertical_screenSizeDelta))), Random.Range(4f, 9f), Random.Range(170f, 190f), 0.35f, 0.35f,
                    randomColor, randomColor);
            }
            randomColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("UNIVERN_0", new Vector3(700f, Random.Range(-250f - 150 * Mathf.Cos(vertical_screenSizeDelta), 250f + 150 * Mathf.Cos(vertical_screenSizeDelta))), Random.Range(4f, 9f), Random.Range(170f, 190f), 0.35f, 0.35f,
                    randomColor, randomColor);
            }
            yield return new WaitForSeconds(1 / 24f);
        }
    }

    private void GiveItem(string id) {
        ArrayList sample = new ArrayList();
        foreach (ArrayList ar in ItemController.Items)
        {
            if (((string)ar[0]).Contains(id))
            {
                sample = ar;
                break;
            }
        }
        GameObject.Find("InventoryPanel").GetComponent<ItemController>().GetItem(sample, false);
    }

    private IEnumerator Pattern_5_Support() {
        float i = 0;
        Transform TriggerPanel = GameObject.Find("TriggerPanel").transform;

        while (true) {
            i += 0.03f;
            TriggerPanel.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 30 * Mathf.Sin(i)));
            yield return new WaitForSeconds(1 / 60f);
        }
    }

    private IEnumerator Pattern_7_Support()
    {
        float i = 0;
        Transform TriggerPanel = GameObject.Find("TriggerPanel").transform;

        while (true)
        {
            i += 0.05f;
            TriggerPanel.localScale = new Vector3(Mathf.Cos(i), 1f, 1f);
            yield return new WaitForSeconds(1 / 60f);
        }
    }
}
