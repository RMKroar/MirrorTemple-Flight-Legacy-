using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Laodicea : Boss {

    public GameObject[] weapons;
    public GameObject laodiceaAction;
    GameObject gauge;

    float _speed = 0;
    float direction = 0;
    float radDir;
    Transform trigTr;
    Image background;

    bool fightFlag = false;
    private IEnumerator IControlPlayer;

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
        StopCoroutine("Move");
        int nowPattern = maxPattern - pattern;
        trigTr = GameObject.Find("TriggerPanel").transform;
        StartCoroutine("Pattern_" + (nowPattern));
        immortal = false;
        yield return null;
    }

    IEnumerator Pattern_0_support() {
        GameObject ins;
        float rand = 8 + GameController.rank * 3f;
        float randValue = Random.Range(-rand, rand);

        ins = Instantiate(weapons[0], trigTr);
        if (ins != null)
        {
            ins.GetComponent<LaodiceaSword>().SetIdentity(0, 30, 180 + randValue);
            ins.transform.localPosition = transform.localPosition;
        }
        yield return new WaitForSeconds(1 / 20f);

        ins = Instantiate(weapons[0], trigTr);
        if (ins != null)
        {
            ins.GetComponent<LaodiceaSword>().SetIdentity(0, 30, 180 - randValue);
            ins.transform.localPosition = transform.localPosition;
        }
        yield return new WaitForSeconds(1 / 20f);

        ins = Instantiate(weapons[0], trigTr);
        if (ins != null)
        {
            ins.GetComponent<LaodiceaSword>().SetIdentity(0, 30, 180);
            ins.transform.localPosition = transform.localPosition;
        }
        ins = Instantiate(weapons[0], trigTr);
        if (ins != null)
        {
            ins.GetComponent<LaodiceaSword>().SetIdentity(0, 30, randValue);
            ins.transform.localPosition = transform.localPosition;
        }
        ins = Instantiate(weapons[0], trigTr);
        if (ins != null)
        {
            ins.GetComponent<LaodiceaSword>().SetIdentity(0, 30, -randValue);
            ins.transform.localPosition = transform.localPosition;
        }
        yield return new WaitForSeconds(1 / 20f);
    }

    IEnumerator Pattern_0()
    {
        fightFlag = false;
        GameObject ins;
        AudioSource pct = GameObject.Find("Sound_EnemyPetal").GetComponent<AudioSource>();
        AudioSource sct = GameObject.Find("Sound_EnemyAttack").GetComponent<AudioSource>();
        for (int i = 0; transform.localPosition.x > -450f; transform.localPosition -= new Vector3(8f, 0f), i++) {
            if (i % 20 == 0) {
                pct.volume = 0.35f; pct.Play();
                StartCoroutine(Pattern_0_support());
            }
            yield return new WaitForSeconds(1 / 60f);
        }

        float savedAngle = ToPlayerAngle();
        float tempRand = Random.Range(0f, 360f);
        Vector3 savedPos = transform.localPosition;
        sct.Play();
        for (float j = 0; j < 360; j += 360f / (4 + GameController.rank * 2)) {
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 6f + 0.6f * GameController.rank, tempRand + j,
                    0.4f, 0.4f, Color.white, Color.red);
            }
        }

        transform.localPosition = new Vector3(450f, Random.Range(-200, 200));
        pct.volume = 0.35f; pct.Play();
        for (float j = Random.Range(-40f, -50f); j < 45; j += 45 / (1 + GameController.rank / 4))
        {
            ins = Instantiate(weapons[0], trigTr);
            if (ins != null) {
                float tempDir = savedAngle + j;
                ins.GetComponent<LaodiceaSword>().SetIdentity(1, 0, tempDir);
                ins.transform.localPosition = savedPos +
                    new Vector3(50 * Mathf.Cos(tempDir * Mathf.PI / 180f), 50 * Mathf.Sin(tempDir * Mathf.PI / 180f));
            }
            yield return new WaitForSeconds(1 / 10f);
        }

        yield return new WaitForSeconds(5f - 0.1f * GameController.rank);
        StartCoroutine("Launch");
    }

    IEnumerator Pattern_1() {
        GameObject ins;
        if (!fightFlag)
        {
            FightInitialize();
            fightFlag = true;
        }
        else {
            ParticleController Par = GameObject.Find("ParticleController").GetComponent<ParticleController>();
            ins = Instantiate(Par.ParticleRecovery, GameObject.Find("ParticlePanel").transform);
            ins.transform.localPosition = GetPlayer().transform.localPosition;

            GetPlayer().GetComponent<Player>().health += 1;
            GameObject.Find("CurrentHealth").GetComponent<Text>().text = GetPlayer().GetComponent<Player>().health.ToString();
        }
        yield return new WaitForSeconds(1.2f);       

        for (int i = 0; i < 2 + GameController.rank / 4; i++) {
            ins = Instantiate(laodiceaAction, trigTr);
            if (ins != null)
            {
                ins.GetComponent<LaodiceaAction>().SetIdentity(gameObject, 0, Random.Range(1, 5), 1f / (30f - GameController.rank * 1.3f));
                ins.transform.localPosition = new Vector3(0f, 150f);
            }
            yield return new WaitForSeconds((30f - GameController.rank * 1.3f) / 30f);
        }

        StartCoroutine("Launch");
    }

    IEnumerator Pattern_2()
    {
        GameObject ins;
        AudioSource pct = GameObject.Find("Sound_EnemyPetal").GetComponent<AudioSource>();
        AudioSource sct = GameObject.Find("Sound_EnemyAttack").GetComponent<AudioSource>();

        if (fightFlag)
        {
            FightEnd();
            fightFlag = false;
        }

        yield return new WaitForSeconds(3f - 0.2f * GameController.rank);

        pct.volume = 0.35f; pct.Play();
        for (int i = 0; i < 2 + GameController.rank / 3; i++) {
            float randValue = Random.Range(10f, 50f);
            ins = Instantiate(weapons[1], trigTr);
            if (ins != null)
            {
                ins.GetComponent<LaodiceaSword>().SetIdentity(3, Random.Range(12f, 24f), 180 - randValue);
                ins.transform.localPosition = transform.localPosition;
                yield return new WaitForSeconds(1 / 15f);
                
            }
            ins = Instantiate(weapons[1], trigTr);
            if (ins != null)
            {
                ins.GetComponent<LaodiceaSword>().SetIdentity(4, Random.Range(12f, 24f), 180 + randValue);
                ins.transform.localPosition = transform.localPosition;
                yield return new WaitForSeconds(1 / 15f);
            }
        }

        float savedAngle = ToPlayerAngle();
        float tempRand = Random.Range(0f, 360f);
        Vector3 savedPos = transform.localPosition;
        sct.Play();
        for (float j = 0; j < 360; j += 360f / (5 + GameController.rank))
        {
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 6f + 0.2f * GameController.rank, tempRand + j,
                    0.4f, 0.4f, Color.white, Color.red);
            }

            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 9f + 0.3f * GameController.rank, tempRand + j,
                    0.4f, 0.4f, Color.white, Color.yellow);
            }
        }

        transform.localPosition = new Vector3(450f, Random.Range(-200, 200));
        pct.volume = 0.35f; pct.Play();
        for (float j = Random.Range(-50f, -60f); j < 55; j += 45 / (1 + GameController.rank / 4))
        {
            ins = Instantiate(weapons[1], trigTr);
            if (ins != null)
            {
                float tempDir = savedAngle + j;
                ins.GetComponent<LaodiceaSword>().SetIdentity(1, 0, tempDir);
                ins.transform.localPosition = savedPos +
                    new Vector3(50 * Mathf.Cos(tempDir * Mathf.PI / 180f), 50 * Mathf.Sin(tempDir * Mathf.PI / 180f));
            }
            yield return new WaitForSeconds(1 / 10f);
        }
        StartCoroutine("Launch");
    }

    IEnumerator Pattern_3() {
        GameObject ins;
        if (!fightFlag)
        {
            FightInitialize();
            fightFlag = true;
        }
        else
        {
            ParticleController Par = GameObject.Find("ParticleController").GetComponent<ParticleController>();
            ins = Instantiate(Par.ParticleRecovery, GameObject.Find("ParticlePanel").transform);
            ins.transform.localPosition = GetPlayer().transform.localPosition;

            GetPlayer().GetComponent<Player>().health += 1;
            GameObject.Find("CurrentHealth").GetComponent<Text>().text = GetPlayer().GetComponent<Player>().health.ToString();
        }
        yield return new WaitForSeconds(1.2f);

        for (int i = 0; i < 2 + GameController.rank / 6; i++)
        {
            ins = Instantiate(laodiceaAction, trigTr);
            if (ins != null)
            {
                ins.GetComponent<LaodiceaAction>().SetIdentity(gameObject, 2, Random.Range(1, 5), 1f / (30f - GameController.rank * 1.3f));
                ins.transform.localPosition = new Vector3(-100f, 150f);
            }
            yield return new WaitForSeconds((30f - GameController.rank * 1.3f) / 10f);
        }

        StartCoroutine("Launch");
    }

    IEnumerator Pattern_4()
    {
        GameObject ins;
        AudioSource pct = GameObject.Find("Sound_EnemyPetal").GetComponent<AudioSource>();
        AudioSource sct = GameObject.Find("Sound_EnemyAttack").GetComponent<AudioSource>();

        if (fightFlag)
        {
            FightEnd();
            fightFlag = false;
        }

        yield return new WaitForSeconds(2f - 0.1f * GameController.rank);

        pct.volume = 0.35f; pct.Play();
        for (float i = 0; i < 70; i += 20f - GameController.rank)
        {          
            ins = Instantiate(weapons[2], trigTr);
            if (ins != null)
            {
                ins.GetComponent<LaodiceaSword>().SetIdentity(7, -0.5f, 180 + i);
                ins.transform.localPosition = transform.localPosition;
                yield return new WaitForSeconds(1 / 5f);
            }
            ins = Instantiate(weapons[2], trigTr);
            if (ins != null)
            {
                ins.GetComponent<LaodiceaSword>().SetIdentity(7, -0.5f, 180 - i);
                ins.transform.localPosition = transform.localPosition;
                yield return new WaitForSeconds(1 / 5f);
            }
        }

        float tempRand = Random.Range(0f, 360f);
        Vector3 savedPos = transform.localPosition;
        sct.Play();
        float tempDensity = 360f / (5 + GameController.rank);
        for (float j = 0; j < 360; j += tempDensity)
        {
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("CRYSTAL", transform.localPosition, 6f + 0.2f * GameController.rank, tempRand + j,
                    0.4f, 0.4f, Color.blue, Color.white);
            }

            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("CRYSTAL", transform.localPosition, 8f + 0.3f * GameController.rank, tempRand + j + (tempDensity / 3f),
                    0.4f, 0.4f, new Color(0f, 0.5f, 1f), Color.white);
            }

            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("CRYSTAL", transform.localPosition, 10f + 0.4f * GameController.rank, tempRand + j + tempDensity * 2 / 3f,
                    0.4f, 0.4f, BulletCode.COLOR_SKY, Color.white);
            }
        }

        transform.localPosition = new Vector3(450f, Random.Range(-200, 200));

        for (float i = 0; i < 5; i++)
        {
            sct.Play();
            ins = Instantiate(weapons[2], trigTr);
            if (ins != null)
            {
                ins.GetComponent<LaodiceaSword>().SetIdentity(8, 50f, 180 + Random.Range(-3f, 3f));
                ins.transform.localPosition = savedPos;
                yield return new WaitForSeconds(1 / 10f);
            }
        }
        
        StartCoroutine("Launch");
    }

    IEnumerator Pattern_5()
    {
        GameObject ins;
        if (!fightFlag)
        {
            FightInitialize();
            fightFlag = true;
        }
        else
        {
            ParticleController Par = GameObject.Find("ParticleController").GetComponent<ParticleController>();
            ins = Instantiate(Par.ParticleRecovery, GameObject.Find("ParticlePanel").transform);
            ins.transform.localPosition = GetPlayer().transform.localPosition;

            GetPlayer().GetComponent<Player>().health += 1;
            GameObject.Find("CurrentHealth").GetComponent<Text>().text = GetPlayer().GetComponent<Player>().health.ToString();
        }
        yield return new WaitForSeconds(1.2f);

        for (int i = 0; i < 1 + GameController.rank / 6; i++)
        {
            ins = Instantiate(laodiceaAction, trigTr);
            if (ins != null)
            {
                ins.GetComponent<LaodiceaAction>().SetIdentity(gameObject, 4, Random.Range(1, 5), 1f / (30f - GameController.rank * 1.3f));
                ins.transform.localPosition = new Vector3(-200f, 150f);
            }
            yield return new WaitForSeconds((30f - GameController.rank * 1.3f) / 6f);
        }

        StartCoroutine("Launch");
    }

    IEnumerator Pattern_6()
    {
        GameObject ins;
        AudioSource pct = GameObject.Find("Sound_EnemyPetal").GetComponent<AudioSource>();
        AudioSource sct = GameObject.Find("Sound_EnemyAttack").GetComponent<AudioSource>();
        int randPath = Random.Range(0, 2);

        if (fightFlag)
        {
            FightEnd();
            fightFlag = false;
        }
        yield return new WaitForSeconds(3f - 0.05f * GameController.rank);

        pct.volume = 0.35f; pct.Play(); sct.Play();
        ins = Instantiate(weapons[3], trigTr);
        if (ins != null)
        {
            ins.GetComponent<LaodiceaSword>().SetIdentity(11, 0f, 180);
            ins.transform.localPosition = transform.localPosition;
        }
        yield return new WaitForSeconds(0.8f);

        StartCoroutine(ToPointMove(new Vector3(-450f, transform.localPosition.y)));
        yield return new WaitForSeconds(0.2f);

        pct.volume = 0.35f; pct.Play(); sct.Play();
        ins = Instantiate(weapons[3], trigTr);
        if (ins != null)
        {
            ins.GetComponent<LaodiceaSword>().SetIdentity(11, 0f, 0);
            ins.transform.localPosition = transform.localPosition;
        }
        yield return new WaitForSeconds(0.8f);

        if (randPath == 0) {
            StartCoroutine(ToPointMove(new Vector3(-300f, -250f)));
            yield return new WaitForSeconds(0.2f);

            pct.volume = 0.35f; pct.Play(); sct.Play();
            ins = Instantiate(weapons[3], trigTr);
            if (ins != null)
            {
                ins.GetComponent<LaodiceaSword>().SetIdentity(11, 0f, 90);
                ins.transform.localPosition = transform.localPosition;
            }
            yield return new WaitForSeconds(0.3f);

            StartCoroutine(ToPointMove(new Vector3(0f, 250f)));
            yield return new WaitForSeconds(0.2f);

            pct.volume = 0.35f; pct.Play(); sct.Play();
            ins = Instantiate(weapons[3], trigTr);
            if (ins != null)
            {
                ins.GetComponent<LaodiceaSword>().SetIdentity(11, 0f, 270);
                ins.transform.localPosition = transform.localPosition;
            }
            yield return new WaitForSeconds(0.3f);

            StartCoroutine(ToPointMove(new Vector3(300f, -250f)));
            yield return new WaitForSeconds(0.2f);

            pct.volume = 0.35f; pct.Play(); sct.Play();
            ins = Instantiate(weapons[3], trigTr);
            if (ins != null)
            {
                ins.GetComponent<LaodiceaSword>().SetIdentity(11, 0f, 90);
                ins.transform.localPosition = transform.localPosition;
            }
            yield return new WaitForSeconds(0.3f);

            StartCoroutine(ToPointMove(new Vector3(450f, Random.Range(-200f, 200f))));
        }
        else
        {
            StartCoroutine(ToPointMove(new Vector3(-300f, 250f)));
            yield return new WaitForSeconds(0.2f);

            pct.volume = 0.35f; pct.Play(); sct.Play();
            ins = Instantiate(weapons[3], trigTr);
            if (ins != null)
            {
                ins.GetComponent<LaodiceaSword>().SetIdentity(11, 0f, 270);
                ins.transform.localPosition = transform.localPosition;
            }
            yield return new WaitForSeconds(0.3f);

            StartCoroutine(ToPointMove(new Vector3(0f, -250f)));
            yield return new WaitForSeconds(0.2f);

            pct.volume = 0.35f; pct.Play(); sct.Play();
            ins = Instantiate(weapons[3], trigTr);
            if (ins != null)
            {
                ins.GetComponent<LaodiceaSword>().SetIdentity(11, 0f, 90);
                ins.transform.localPosition = transform.localPosition;
            }
            yield return new WaitForSeconds(0.3f);

            StartCoroutine(ToPointMove(new Vector3(300f, 250f)));
            yield return new WaitForSeconds(0.2f);

            pct.volume = 0.35f; pct.Play(); sct.Play();
            ins = Instantiate(weapons[3], trigTr);
            if (ins != null)
            {
                ins.GetComponent<LaodiceaSword>().SetIdentity(11, 0f, 270);
                ins.transform.localPosition = transform.localPosition;
            }
            yield return new WaitForSeconds(0.3f);

            StartCoroutine(ToPointMove(new Vector3(450f, Random.Range(-200f, 200f))));
        }

        pct.volume = 0.35f; pct.Play();
        float savedAngle = ToPlayerAngle();
        for (float j = Random.Range(-60f, -70f); j < 65; j += 55 / (1 + GameController.rank / 4))
        {
            ins = Instantiate(weapons[3], trigTr);
            if (ins != null)
            {
                float tempDir = savedAngle + j;
                ins.GetComponent<LaodiceaSword>().SetIdentity(12, 0, tempDir);
                ins.transform.localPosition = transform.localPosition +
                    new Vector3(50 * Mathf.Cos(tempDir * Mathf.PI / 180f), 50 * Mathf.Sin(tempDir * Mathf.PI / 180f));
            }
            yield return new WaitForSeconds(1 / 10f);
        }

        StartCoroutine("Launch");
    }

    IEnumerator Pattern_7()
    {
        GameObject ins;
        if (!fightFlag)
        {
            FightInitialize();
            fightFlag = true;
        }
        else
        {
            ParticleController Par = GameObject.Find("ParticleController").GetComponent<ParticleController>();
            ins = Instantiate(Par.ParticleRecovery, GameObject.Find("ParticlePanel").transform);
            ins.transform.localPosition = GetPlayer().transform.localPosition;

            GetPlayer().GetComponent<Player>().health += 1;
            GameObject.Find("CurrentHealth").GetComponent<Text>().text = GetPlayer().GetComponent<Player>().health.ToString();
        }

        
        ins = Instantiate(laodiceaAction, trigTr);
        if (ins != null)
        {
            ins.GetComponent<LaodiceaAction>().SetIdentity(gameObject, 7, Random.Range(1, 5), 1f / (30f - GameController.rank * 1.3f));
            ins.transform.localPosition = new Vector3(-350f, 150f);
        }
        yield return new WaitForSeconds((30f - GameController.rank * 1.3f) / 3f);
        StartCoroutine("Launch");
    }

    bool lastFlag = false;
    IEnumerator Pattern_8() {
        GameObject ins;
        AudioSource pct = GameObject.Find("Sound_EnemyPetal").GetComponent<AudioSource>();
        AudioSource sct = GameObject.Find("Sound_EnemyAttack").GetComponent<AudioSource>();
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();

        if (fightFlag)
        {
            FightEnd();
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("SpecialBullet")) {
                if (obj.name.Contains("Action")) Destroy(obj);
            }
            transform.localScale = Vector3.one;
            fightFlag = false;
        }

        if (!lastFlag)
        {
            maxHealth *= 2;
            health *= 2;
            lastFlag = true;
        }

        yield return new WaitForSeconds(0.7f);
        pct.volume = 0.35f; pct.Play();
        for (int i = 0; i < 2 + GameController.rank / 4; i++)
        {
            float randValue = Random.Range(10f, 70f);
            ins = Instantiate(weapons[1], trigTr);
            if (ins != null)
            {
                ins.GetComponent<LaodiceaSword>().SetIdentity(3, Random.Range(12f, 24f), 180 - randValue);
                ins.transform.localPosition = transform.localPosition;
                yield return new WaitForSeconds(1 / 10f);

            }
            ins = Instantiate(weapons[1], trigTr);
            if (ins != null)
            {
                ins.GetComponent<LaodiceaSword>().SetIdentity(4, Random.Range(12f, 24f), 180 + randValue);
                ins.transform.localPosition = transform.localPosition;
                yield return new WaitForSeconds(1 / 10f);
            }
        }

        pct.volume = 0.35f; pct.Play();
        for (float i = 10; i < 70; i += 30f - GameController.rank)
        {
            ins = Instantiate(weapons[2], trigTr);
            if (ins != null)
            {
                ins.GetComponent<LaodiceaSword>().SetIdentity(7, -0.5f, 180 + i);
                ins.transform.localPosition = transform.localPosition;
            }
            ins = Instantiate(weapons[2], trigTr);
            if (ins != null)
            {
                ins.GetComponent<LaodiceaSword>().SetIdentity(7, -0.5f, 180 - i);
                ins.transform.localPosition = transform.localPosition;
                yield return new WaitForSeconds(1 / 15f);
            }
        }
        StartCoroutine(ToPointMove(GetPlayer().transform.localPosition));
        yield return new WaitForSeconds(1f);

        float defValue = Random.Range(0f, 360f);
        for (float j = 0; j < 360; j += 360f / (5 + GameController.rank)) {
            cct.Play();
            ins = Instantiate(weapons[0], trigTr);
            if (ins != null)
            {
                ins.GetComponent<LaodiceaSword>().SetIdentity(14, 0, j + defValue);
                ins.transform.localPosition = transform.localPosition -
                    new Vector3(150f * Mathf.Cos((defValue + j) * Mathf.PI / 180), 150f * Mathf.Sin((defValue + j) * Mathf.PI / 180));
            }
            yield return new WaitForSeconds(1 / 15f);
        }
        StartCoroutine(ToPointMove(GetPlayer().transform.localPosition));
        yield return new WaitForSeconds(1 / 10f);
        pct.volume = 0.35f; pct.Play();
        sct.Play();
        ins = Instantiate(weapons[3], trigTr);
        if (ins != null)
        {
            ins.GetComponent<LaodiceaSword>().SetIdentity(11, 0f, 180);
            ins.transform.localPosition = transform.localPosition;
        }
        ins = Instantiate(weapons[3], trigTr);
        if (ins != null)
        {
            ins.GetComponent<LaodiceaSword>().SetIdentity(11, 0f, 0);
            ins.transform.localPosition = transform.localPosition;
        }
        yield return new WaitForSeconds(0.5f);
        pct.volume = 0.35f; pct.Play();
        sct.Play();
        ins = Instantiate(weapons[3], trigTr);
        if (ins != null)
        {
            ins.GetComponent<LaodiceaSword>().SetIdentity(11, 0f, 90);
            ins.transform.localPosition = transform.localPosition;
        }
        ins = Instantiate(weapons[3], trigTr);
        if (ins != null)
        {
            ins.GetComponent<LaodiceaSword>().SetIdentity(11, 0f, 270);
            ins.transform.localPosition = transform.localPosition;
        }

        StartCoroutine(ToPointMove(new Vector3(450f, Random.Range(-200f, 200f))));
        yield return new WaitForSeconds(2f);

        float savedAngle = ToPlayerAngle();
        pct.volume = 0.35f; pct.Play();
        for (float j = Random.Range(-50f, -60f); j < 55; j += 55 / (1 + GameController.rank / 4))
        {
            ins = Instantiate(weapons[1], trigTr);
            if (ins != null)
            {
                float tempDir = savedAngle + j;
                ins.GetComponent<LaodiceaSword>().SetIdentity(1, 0, tempDir);
                ins.transform.localPosition = transform.localPosition +
                    new Vector3(50 * Mathf.Cos(tempDir * Mathf.PI / 180f), 50 * Mathf.Sin(tempDir * Mathf.PI / 180f));
            }
            yield return new WaitForSeconds(1 / 10f);
        }

        yield return new WaitForSeconds(1f);

        pct.volume = 0.35f; pct.Play();
        savedAngle = ToPlayerAngle();
        for (float j = Random.Range(-60f, -70f); j < 65; j += 65 / (1 + GameController.rank / 4))
        {
            ins = Instantiate(weapons[3], trigTr);
            if (ins != null)
            {
                float tempDir = savedAngle + j;
                ins.GetComponent<LaodiceaSword>().SetIdentity(12, 0, tempDir);
                ins.transform.localPosition = transform.localPosition +
                    new Vector3(50 * Mathf.Cos(tempDir * Mathf.PI / 180f), 50 * Mathf.Sin(tempDir * Mathf.PI / 180f));
            }
            yield return new WaitForSeconds(1 / 10f);
        }
        for (float i = 0; i < 5; i++)
        {
            sct.Play();
            ins = Instantiate(weapons[2], trigTr);
            if (ins != null)
            {
                ins.GetComponent<LaodiceaSword>().SetIdentity(8, 50f, 180 + Random.Range(-3f, 3f));
                ins.transform.localPosition = transform.localPosition;
                yield return new WaitForSeconds(1 / 10f);
            }
        }
        StartCoroutine("Launch");
    }

    public void FightInitialize() {
        GameController.EraseBullet();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("SpecialBullet")) {
            Destroy(obj);
        }
        background = GameObject.Find("Back1Panel").GetComponent<Image>();
        background.color = Color.black;
        immortal = true;
        IControlPlayer = ControlPlayer();
        StartCoroutine(IControlPlayer);
    }

    public void FightEnd()
    {
        transform.localScale = Vector3.one;
        background = GameObject.Find("Back1Panel").GetComponent<Image>();
        background.color = new Color(1f, 0.5f, 0.5f);
        StopCoroutine(IControlPlayer);

        immortal = false;
        transform.localPosition = new Vector3(450f, 0f);
        health = maxHealth;
        CheckHealth();
    }

    IEnumerator ControlPlayer() {
        GameObject target = GetPlayer();
        Vector3 pos = new Vector3(0f, -100f);
        while (true) {
            target.transform.localPosition = pos;
            immortal = true;
            yield return new WaitForSeconds(1 / 60f);
        }
    }

    IEnumerator ToPointMove(Vector3 pos) {
        float mag = ToPointMagnitude(pos);
        float rad = ToPointAngleRAD(pos);
        for (int i = 0; i < 5; i++)
        {
            transform.localPosition += new Vector3(mag * Mathf.Cos(rad) / 5, mag * Mathf.Sin(rad) / 5);
            yield return new WaitForSeconds(1 / 120f);
        }
        transform.localPosition = pos;
    }

    public float ToPointAngleRAD(Vector3 targetPosition)
    {
        Vector3 moveDirection = targetPosition - transform.localPosition;
        if (moveDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x);
            return angle;
        }
        else return 0;
    }

    public float ToPointMagnitude(Vector3 targetPosition)
    {
        Vector3 moveDirection = transform.localPosition - targetPosition;
        return moveDirection.magnitude;
    }
}
