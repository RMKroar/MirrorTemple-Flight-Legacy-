using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thyatera : Boss {

    public GameObject[] ElementalsForSummon;
    public GameObject[] Elementals;

    public GameObject ThyateraPanelForSummon;
    GameObject ThyateraPanel;

    public GameObject ElementalImage;
    ArrayList ElementalOrder;

    float _speed = 0;
    float direction = 0;
    float radDir;
    int mode = 0;
    bool flag = false;

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
        StartCoroutine(Pattern_0());
        // Always Immortal
        if (flag && Elementals[0] == null) {
            health = 0;
            CheckHealth();
        }
        yield return null;
    }

    int ea = 0;
    bool mode3flag = false;
    float elementalDir = 0;

    IEnumerator Pattern_0()
    {
        GameObject ins;
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();

        if (!flag) {
            ThyateraPanel = Instantiate(ThyateraPanelForSummon, GameObject.Find("TriggerPanel").transform);
            SetOrder();

            transform.localPosition += new Vector3(100, 0);
            Transform trigTr = GameObject.Find("TriggerPanel").transform;
            for (int i = 0; i < 4; i++) {
                ins = Instantiate(ElementalsForSummon[i], trigTr);
                ins.GetComponent<Elemental>().SetIdentity(i, gameObject);
                ins.transform.localPosition = new Vector3(350f, 240f - 160 * i);
                Elementals[i] = ins;

                yield return new WaitForSeconds(1 / 4f);
            }
            flag = true;
        }

        if (mode == 0) {  
            cct.Play();

            float tempAngle = ToPlayerAngle() + Random.Range(-10f, 10f);
            ins = GetBullet("THORN");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("DOLL", transform.localPosition, 15 + GameController.rank, tempAngle,
                    0.3f, 0.3f, Color.white, Color.black);
            }

            if (GameController.rank >= 5)
            {
                ins = GetBullet("THORN");
                if (ins != null)
                {
                    ins.GetComponent<Bullet>().SetIdentityEx("DOLL", transform.localPosition, 15 + GameController.rank, tempAngle - 2f,
                        0.3f, 0.3f, Color.white, Color.black);
                }
                ins = GetBullet("THORN");
                if (ins != null)
                {
                    ins.GetComponent<Bullet>().SetIdentityEx("DOLL", transform.localPosition, 15 + GameController.rank, tempAngle + 2f,
                        0.3f, 0.3f, Color.white, Color.black);
                }
            }

            yield return new WaitForSeconds(3f);
        }

        if (mode == 1) {
            if (ea++ % 3 == 0) {
                Vector3[] vectors = new Vector3[4] { new Vector3(350, -150), new Vector3(50, -150), new Vector3(350, 150), new Vector3(50, 150) };
                ElementalMove(vectors);
            }

            int value = Random.Range(0, 4);
            Color[] colors = new Color[4] { Color.red, Color.blue, Color.green, BulletCode.COLOR_DARKBROWN };
            string[] strings = new string[4] { "red", "blue", "green", "brown" };
            Vector3 playerPos = GetPlayer().transform.localPosition;

            for (int i = 0; i < 360; i += 45)
            {
                ins = GetBullet("CIRCLE");
                if (ins != null)
                {
                    ins.GetComponent<Bullet>().SetIdentityEx("DEBRIS", playerPos + new Vector3(100f * Mathf.Cos(i * Mathf.PI / 180f), 100f * Mathf.Sin(i * Mathf.PI / 180f))
                        , 0.01f, i, 0.35f, 0.35f,
                        colors[value], colors[value]);
                }
            }

            foreach (GameObject target in GameObject.FindGameObjectsWithTag("Bullet")) {
                Bullet tarData = target.GetComponent<Bullet>();
                if (("thyatera_" + strings[value]).Equals(tarData.moveCode)) {
                    tarData.direction = Random.Range(0f, 360f);
                    tarData.speed = 3 + GameController.rank * 0.5f;
                }
            }

            yield return new WaitForSeconds(1f - 0.05f * GameController.rank);
        }

        if (mode == 2)
        {
            if (ea++ % 3 == 0)
            {
                Vector3[] vectors = new Vector3[4] { new Vector3(250, 240), new Vector3(350, 80), new Vector3(350, -80), new Vector3(250, -240) };
                ElementalMove(vectors);
            }
            cct.Play();

            float tempAngle = ToPlayerAngle() + Random.Range(-10f, 10f);
            ins = GetBullet("THORN");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("DOLL", transform.localPosition, 15 + GameController.rank, tempAngle,
                    0.3f, 0.3f, Color.white, Color.black);
            }

            if (GameController.rank >= 5)
            {
                ins = GetBullet("THORN");
                if (ins != null)
                {
                    ins.GetComponent<Bullet>().SetIdentityEx("DOLL", transform.localPosition, 15 + GameController.rank, tempAngle - 2f,
                        0.3f, 0.3f, Color.white, Color.black);
                }
                ins = GetBullet("THORN");
                if (ins != null)
                {
                    ins.GetComponent<Bullet>().SetIdentityEx("DOLL", transform.localPosition, 15 + GameController.rank, tempAngle + 2f,
                        0.3f, 0.3f, Color.white, Color.black);
                }
            }

            yield return new WaitForSeconds(2f);
        }
        if (mode == 3) {
            if (!mode3flag) {
                Vector3[] vectors = new Vector3[4] { new Vector3(300, 0), new Vector3(0, 300), new Vector3(-300, 0), new Vector3(0, -300) };
                for (int i = 0; i < 4; i++)
                {
                    if (Elementals[i] != null) Elementals[i].GetComponent<Elemental>().InvokeMove(vectors[i]);
                }
                mode3flag = true;
                yield return new WaitForSeconds(1f);
            }

            elementalDir += 0.04f * (count + 1); cct.Play();
            float distMag = 300 * Mathf.Cos(elementalDir / 4f);
            Color[] colors = new Color[4] { Color.red, Color.blue, Color.green, Color.yellow };
            for (int i = 0; i < 4; i++) {
                Elementals[i].transform.localPosition = new Vector3(distMag * Mathf.Cos(elementalDir + 0.5f * Mathf.PI * i), distMag * Mathf.Sin(elementalDir + 0.5f * Mathf.PI * i));
                if ((int)(elementalDir * 25f / (count + 1)) % (6 - GameController.rank / 2) == 0) {
                    ins = GetBullet("CIRCLE");
                    if (ins != null)
                    {
                        ins.GetComponent<Bullet>().SetIdentity(Elementals[i].transform.localPosition, 4 + GameController.rank / 2,
                            elementalDir * 180f / Mathf.PI + 90 * Random.Range(0, 4), 0.35f, 0.35f, Color.white, colors[i]);
                    }
                }      
            }
            yield return new WaitForSeconds(1 / 30f);
        }

        if (mode == 4) {
            health = 0;
            CheckHealth();
            yield return new WaitForSeconds(3f);
        }

        StartCoroutine(Launch());
    }

    public void ElementalMove(Vector3[] positions) {
        ArrayList ar = new ArrayList() { 0, 1, 2, 3 };

        for (int i = 0; i < 4; i++) {
            int randValue = Random.Range(0, ar.Count);
            if(Elementals[i] != null) Elementals[i].GetComponent<Elemental>().InvokeMove(positions[(int)ar[randValue]]);
            ar.RemoveAt(randValue);
        }
    }

    int count = 0;
    public void RankUp(int id) {
        if (id == (int)ElementalOrder[0])
        {
            count++;
            for (int i = 0; i < 4; i++)
            {
                if (count <= 3) Elementals[i].GetComponent<Elemental>().SetRank(-1); // Increment
                else
                {
                    Elementals[i].GetComponent<Elemental>().SetRank(0);
                    Elementals[i].GetComponent<Elemental>().Revive(true);
                }
            }
            ElementalOrder.RemoveAt(0);
            Destroy(ThyateraPanel.transform.GetChild(0).gameObject);
            if (count > 3)
            {
                mode++;
                if (mode > 3)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Destroy(Elementals[i]);
                    }
                    Destroy(ThyateraPanel);
                }
                else {
                    ea = 0; count = 0;
                    SetOrder();
                }              
            }
        }
        else {
            for (int i = 0; i < 4; i++)
            {
                Elementals[i].GetComponent<Elemental>().SetRank(0);
                Elementals[i].GetComponent<Elemental>().Revive(false);
            }
            ea = 0; count = 0;
            SetOrder();
        }
    }

    private void SetOrder() {
        GameObject ins;

        // Panel Initialize
        Transform tr = ThyateraPanel.transform;
        for (int i = tr.childCount - 1; i >= 0; i--) {
            if(i >= 0) Destroy(tr.GetChild(i).gameObject);
        }

        ElementalOrder = new ArrayList();
        ElementalOrder.Clear();
        ArrayList ar = new ArrayList() { 0, 1, 2, 3 };

        for(int i = 0; i < 4; i++)
        {
            int randValue = Random.Range(0, ar.Count);
            ElementalOrder.Add(ar[randValue]);
            ins = Instantiate(ElementalImage, tr);
            ins.GetComponent<ElementalImage>().SetIdentity((int)ar[randValue]);

            ar.RemoveAt(randValue);
        }
    }
}
