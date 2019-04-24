using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaodiceaAction : MonoBehaviour {

    GameObject Laodicea;
    int value;
    int amount;
    bool inputKey = false;

    void Update()
    {
        if (!inputKey && Time.timeScale != 0) {
            if (value == 1) {
                if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
                {
                    inputKey = true;
                    StartCoroutine(Action(true));
                }
                else if (Input.anyKeyDown) {
                    inputKey = true;
                    StartCoroutine(Action(false));
                }
            }
            if (value == 2)
            {
                if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
                {
                    inputKey = true;
                    StartCoroutine(Action(true));
                }
                else if (Input.anyKeyDown)
                {
                    inputKey = true;
                    StartCoroutine(Action(false));
                }
            }
            if (value == 3)
            {
                if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
                {
                    inputKey = true;
                    StartCoroutine(Action(true));
                }
                else if (Input.anyKeyDown)
                {
                    inputKey = true;
                    StartCoroutine(Action(false));
                }
            }
            if (value == 4)
            {
                if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
                {
                    inputKey = true;
                    StartCoroutine(Action(true));
                }
                else if (Input.anyKeyDown)
                {
                    inputKey = true;
                    StartCoroutine(Action(false));
                }
            }
        }
    }

    public void SetIdentity(GameObject boss, int _amount, int _value, float speed) {
        Laodicea = boss;
        amount = _amount;
        value = _value;
        transform.GetChild(0).gameObject.GetComponent<Text>().text = "" + value;

        inputKey = false;
        StartCoroutine(Disappear(speed));
    }

    IEnumerator Disappear(float speed) {
        CanvasGroup myImage = GetComponent<CanvasGroup>();
        for (myImage.alpha = 1; myImage.alpha > 0; myImage.alpha -= speed) {
            yield return new WaitForSeconds(1 / 60f);
        }

        if (!inputKey)
        {
            StartCoroutine(Action(false));
            inputKey = true;
        }
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

    IEnumerator Action(bool attack) {
        GameObject sword = Laodicea.GetComponent<Laodicea>().weapons[amount / 2];
        GameObject ins;
        float mag = 0;
        float rad = 0;
        if (value == 1) {
            mag = ToPointMagnitude(new Vector3(150f, -100f));
            rad = ToPointAngleRAD(new Vector3(150f, -100f));
            for (int i = 0; i < 5; i++) {
                Laodicea.transform.localPosition += new Vector3(mag * Mathf.Cos(rad) / 5, mag * Mathf.Sin(rad) / 5);
                yield return new WaitForSeconds(1 / 120f);
            }
            Laodicea.transform.localPosition = new Vector3(150f, -100f);
            Laodicea.transform.localScale = Vector3.one;

            if (amount / 2 == 0) {
                ins = Instantiate(sword, GameObject.Find("TriggerPanel").transform);
                if (ins != null)
                {
                    ins.GetComponent<LaodiceaSword>().SetIdentity(2, 25, 180);
                    ins.transform.localPosition = Laodicea.transform.localPosition + new Vector3(-50f, 0f);
                }
            }
            if (amount / 2 == 1) {
                ins = Instantiate(sword, GameObject.Find("TriggerPanel").transform);
                if (ins != null)
                {
                    ins.GetComponent<LaodiceaSword>().SetIdentity(5, 15, 135);
                    ins.transform.localPosition = Laodicea.transform.localPosition + new Vector3(-10f, 0f);
                }
                ins = Instantiate(sword, GameObject.Find("TriggerPanel").transform);
                if (ins != null)
                {
                    ins.GetComponent<LaodiceaSword>().SetIdentity(6, 15, 225);
                    ins.transform.localPosition = Laodicea.transform.localPosition + new Vector3(-10f, 0f);
                }
            }
            if (amount / 2 == 2)
            {
                for (int i = 0; i < 3; i++) {
                    ins = Instantiate(sword, GameObject.Find("TriggerPanel").transform);
                    if (ins != null)
                    {
                        ins.GetComponent<LaodiceaSword>().SetIdentity(9, 20, 170 + 10 * i);
                        ins.transform.localPosition = Laodicea.transform.localPosition + new Vector3(-50f, 0f);
                    }
                    yield return new WaitForSeconds(1 / 10f);
                }              
            }
            if (amount / 2 == 3)
            {
                ins = Instantiate(sword, GameObject.Find("TriggerPanel").transform);
                if (ins != null)
                {
                    ins.GetComponent<LaodiceaSword>().SetIdentity(13, 30, 225);
                    ins.transform.localPosition = Laodicea.transform.localPosition + new Vector3(0f, 100f);
                }
                yield return new WaitForSeconds(1 / 10f);
                ins = Instantiate(sword, GameObject.Find("TriggerPanel").transform);
                if (ins != null)
                {
                    ins.GetComponent<LaodiceaSword>().SetIdentity(13, 30, 135);
                    ins.transform.localPosition = Laodicea.transform.localPosition + new Vector3(0f, -100f);
                }
                yield return new WaitForSeconds(1 / 3f);
                ins = Instantiate(sword, GameObject.Find("TriggerPanel").transform);
                if (ins != null)
                {
                    ins.GetComponent<LaodiceaSword>().SetIdentity(13, 30, 200);
                    ins.transform.localPosition = Laodicea.transform.localPosition + new Vector3(0f, 50f);
                }
                ins = Instantiate(sword, GameObject.Find("TriggerPanel").transform);
                if (ins != null)
                {
                    ins.GetComponent<LaodiceaSword>().SetIdentity(13, 30, 160);
                    ins.transform.localPosition = Laodicea.transform.localPosition + new Vector3(0f, -50f);
                }
                mag = ToPointMagnitude(new Vector3(-150f, -100f));
                rad = ToPointAngleRAD(new Vector3(-150f, -100f));
                for (int i = 0; i < 5; i++)
                {
                    Laodicea.transform.localPosition += new Vector3(mag * Mathf.Cos(rad) / 5, mag * Mathf.Sin(rad) / 5);
                    yield return new WaitForSeconds(1 / 120f);
                }
            }
        }

        else if (value == 2)
        {
            mag = ToPointMagnitude(new Vector3(-150f, -100f));
            rad = ToPointAngleRAD(new Vector3(-150f, -100f));
            for (int i = 0; i < 5; i++)
            {
                Laodicea.transform.localPosition += new Vector3(mag * Mathf.Cos(rad) / 5, mag * Mathf.Sin(rad) / 5);
                yield return new WaitForSeconds(1 / 120f);
            }
            Laodicea.transform.localPosition = new Vector3(-150f, -100f);
            Laodicea.transform.localScale = new Vector3(-1, 1, 1);

            if (amount / 2 == 0) {
                ins = Instantiate(sword, GameObject.Find("TriggerPanel").transform);
                if (ins != null)
                {
                    ins.GetComponent<LaodiceaSword>().SetIdentity(2, 25, 0);
                    ins.transform.localPosition = Laodicea.transform.localPosition + new Vector3(50f, 0f);
                }
            }
            if (amount / 2 == 1)
            {
                ins = Instantiate(sword, GameObject.Find("TriggerPanel").transform);
                if (ins != null)
                {
                    ins.GetComponent<LaodiceaSword>().SetIdentity(5, 15, -45);
                    ins.transform.localPosition = Laodicea.transform.localPosition + new Vector3(10f, 0f);
                }
                ins = Instantiate(sword, GameObject.Find("TriggerPanel").transform);
                if (ins != null)
                {
                    ins.GetComponent<LaodiceaSword>().SetIdentity(6, 15, 45);
                    ins.transform.localPosition = Laodicea.transform.localPosition + new Vector3(10f, 0f);
                }
            }
            if (amount / 2 == 2)
            {
                for (int i = 0; i < 3; i++)
                {
                    ins = Instantiate(sword, GameObject.Find("TriggerPanel").transform);
                    if (ins != null)
                    {
                        ins.GetComponent<LaodiceaSword>().SetIdentity(9, 20, 10 - 10 * i);
                        ins.transform.localPosition = Laodicea.transform.localPosition + new Vector3(50f, 0f);
                    }
                    yield return new WaitForSeconds(1 / 10f);
                }
            }
            if (amount / 2 == 3)
            {
                ins = Instantiate(sword, GameObject.Find("TriggerPanel").transform);
                if (ins != null)
                {
                    ins.GetComponent<LaodiceaSword>().SetIdentity(13, 30, 315);
                    ins.transform.localPosition = Laodicea.transform.localPosition + new Vector3(0f, 100f);
                }
                yield return new WaitForSeconds(1 / 10f);
                ins = Instantiate(sword, GameObject.Find("TriggerPanel").transform);
                if (ins != null)
                {
                    ins.GetComponent<LaodiceaSword>().SetIdentity(13, 30, 45);
                    ins.transform.localPosition = Laodicea.transform.localPosition + new Vector3(0f, -100f);
                }
                yield return new WaitForSeconds(1 / 3f);
                ins = Instantiate(sword, GameObject.Find("TriggerPanel").transform);
                if (ins != null)
                {
                    ins.GetComponent<LaodiceaSword>().SetIdentity(13, 30, 340);
                    ins.transform.localPosition = Laodicea.transform.localPosition + new Vector3(0f, 50f);
                }
                ins = Instantiate(sword, GameObject.Find("TriggerPanel").transform);
                if (ins != null)
                {
                    ins.GetComponent<LaodiceaSword>().SetIdentity(13, 30, 20);
                    ins.transform.localPosition = Laodicea.transform.localPosition + new Vector3(0f, -50f);
                }
                mag = ToPointMagnitude(new Vector3(150f, -100f));
                rad = ToPointAngleRAD(new Vector3(150f, -100f));
                for (int i = 0; i < 5; i++)
                {
                    Laodicea.transform.localPosition += new Vector3(mag * Mathf.Cos(rad) / 5, mag * Mathf.Sin(rad) / 5);
                    yield return new WaitForSeconds(1 / 120f);
                }
            }
        }

        else if (value == 3)
        {
            mag = ToPointMagnitude(new Vector3(150f, 0f));
            rad = ToPointAngleRAD(new Vector3(150f, 0f));
            for (int i = 0; i < 5; i++)
            {
                Laodicea.transform.localPosition += new Vector3(mag * Mathf.Cos(rad) / 5, mag * Mathf.Sin(rad) / 5);
                yield return new WaitForSeconds(1 / 120f);
            }
            Laodicea.transform.localPosition = new Vector3(150f, 0f);
            Laodicea.transform.localScale = new Vector3(1, 1, 1);
            if (amount / 2 == 0) {
                ins = Instantiate(sword, GameObject.Find("TriggerPanel").transform);
                if (ins != null)
                {
                    ins.GetComponent<LaodiceaSword>().SetIdentity(2, 28, 225);
                    ins.transform.localPosition = Laodicea.transform.localPosition + new Vector3(0f, 0f);
                }
                ins = Instantiate(sword, GameObject.Find("TriggerPanel").transform);
                if (ins != null)
                {
                    ins.GetComponent<LaodiceaSword>().SetIdentity(2, 28, 315);
                    ins.transform.localPosition = Laodicea.transform.localPosition + new Vector3(-300f, 0f);
                }
            }
            if (amount / 2 == 1) {
                ins = Instantiate(sword, GameObject.Find("TriggerPanel").transform);
                if (ins != null)
                {
                    ins.GetComponent<LaodiceaSword>().SetIdentity(2, 28, 225);
                    ins.transform.localPosition = Laodicea.transform.localPosition + new Vector3(0f, 0f);
                }
            }
            if (amount / 2 == 2)
            {
                ins = Instantiate(sword, GameObject.Find("TriggerPanel").transform);
                if (ins != null)
                {
                    ins.GetComponent<LaodiceaSword>().SetIdentity(10, 20, 270);
                    ins.transform.localPosition = Laodicea.transform.localPosition + new Vector3(-150f, 100f);
                }
            }
            if (amount / 2 == 3)
            {
                ins = Instantiate(sword, GameObject.Find("TriggerPanel").transform);
                if (ins != null)
                {
                    ins.GetComponent<LaodiceaSword>().SetIdentity(13, 30, 180);
                    ins.transform.localPosition = Laodicea.transform.localPosition + new Vector3(0f, -100f);
                }
                ins = Instantiate(sword, GameObject.Find("TriggerPanel").transform);
                if (ins != null)
                {
                    ins.GetComponent<LaodiceaSword>().SetIdentity(13, 30, 270);
                    ins.transform.localPosition = Laodicea.transform.localPosition + new Vector3(-150f, 50f);
                }
                mag = ToPointMagnitude(new Vector3(-150f, -200f));
                rad = ToPointAngleRAD(new Vector3(-150f, -200f));
                for (int i = 0; i < 5; i++)
                {
                    Laodicea.transform.localPosition += new Vector3(mag * Mathf.Cos(rad) / 5, mag * Mathf.Sin(rad) / 5);
                    yield return new WaitForSeconds(1 / 120f);
                }
            }
        }

        else if (value == 4)
        {
            mag = ToPointMagnitude(new Vector3(-150f, 0f));
            rad = ToPointAngleRAD(new Vector3(-150f, 0f));
            for (int i = 0; i < 5; i++)
            {
                Laodicea.transform.localPosition += new Vector3(mag * Mathf.Cos(rad) / 5, mag * Mathf.Sin(rad) / 5);
                yield return new WaitForSeconds(1 / 120f);
            }
            Laodicea.transform.localPosition = new Vector3(-150f, 0f);
            Laodicea.transform.localScale = new Vector3(-1, 1, 1);

            if (amount / 2 == 0) {
                ins = Instantiate(sword, GameObject.Find("TriggerPanel").transform);
                if (ins != null)
                {
                    ins.GetComponent<LaodiceaSword>().SetIdentity(2, 28, 315);
                    ins.transform.localPosition = Laodicea.transform.localPosition + new Vector3(0f, 0f);
                }
                ins = Instantiate(sword, GameObject.Find("TriggerPanel").transform);
                if (ins != null)
                {
                    ins.GetComponent<LaodiceaSword>().SetIdentity(2, 28, 225);
                    ins.transform.localPosition = Laodicea.transform.localPosition + new Vector3(300f, 0f);
                }
            }
            if (amount / 2 == 1)
            {
                ins = Instantiate(sword, GameObject.Find("TriggerPanel").transform);
                if (ins != null)
                {
                    ins.GetComponent<LaodiceaSword>().SetIdentity(2, 28, 315);
                    ins.transform.localPosition = Laodicea.transform.localPosition + new Vector3(0f, 0f);
                }
            }
            if (amount / 2 == 2)
            {
                ins = Instantiate(sword, GameObject.Find("TriggerPanel").transform);
                if (ins != null)
                {
                    ins.GetComponent<LaodiceaSword>().SetIdentity(10, 20, 270);
                    ins.transform.localPosition = Laodicea.transform.localPosition + new Vector3(150f, 100f);
                }
            }
            if (amount / 2 == 3)
            {
                ins = Instantiate(sword, GameObject.Find("TriggerPanel").transform);
                if (ins != null)
                {
                    ins.GetComponent<LaodiceaSword>().SetIdentity(13, 30, 0);
                    ins.transform.localPosition = Laodicea.transform.localPosition + new Vector3(0f, -100f);
                }
                ins = Instantiate(sword, GameObject.Find("TriggerPanel").transform);
                if (ins != null)
                {
                    ins.GetComponent<LaodiceaSword>().SetIdentity(13, 30, 270);
                    ins.transform.localPosition = Laodicea.transform.localPosition + new Vector3(150f, 50f);
                }
                mag = ToPointMagnitude(new Vector3(150f, -200f));
                rad = ToPointAngleRAD(new Vector3(150f, -200f));
                for (int i = 0; i < 5; i++)
                {
                    Laodicea.transform.localPosition += new Vector3(mag * Mathf.Cos(rad) / 5, mag * Mathf.Sin(rad) / 5);
                    yield return new WaitForSeconds(1 / 120f);
                }
            }
        }

        AudioSource aud = transform.GetChild(0).gameObject.GetComponent<AudioSource>();
        aud.Play();

        if (attack)
        {
            ParticleController Par = GameObject.Find("ParticleController").GetComponent<ParticleController>();
            ins = Instantiate(Par.ParticleDefend, GameObject.Find("ParticlePanel").transform);
            ins.transform.localPosition = GetPlayer().transform.localPosition;

            Laodicea data = Laodicea.GetComponent<Laodicea>();
            data.health -= data.maxHealth * (1 / 18f);
            data.CheckHealth();

            if (amount > 0) {
                GameObject laodiceaAction = Laodicea.GetComponent<Laodicea>().laodiceaAction;
                ins = Instantiate(laodiceaAction, GameObject.Find("TriggerPanel").transform);
                if (ins != null)
                {
                    ins.GetComponent<LaodiceaAction>().SetIdentity(Laodicea, amount - 1, Random.Range(1, 5), 1f / (30f - GameController.rank * 1.3f));
                    ins.transform.localPosition = transform.localPosition + new Vector3(100f, 0f);
                }
            }
            
            Destroy(gameObject);
        }
        else {
            HitPlayer();
        }
    }

    public void HitPlayer() {
        ParticleController Par = GameObject.Find("ParticleController").GetComponent<ParticleController>();
        GameObject ins = Instantiate(Par.ParticleHit, GameObject.Find("ParticlePanel").transform);
        ins.transform.localPosition = GetPlayer().transform.localPosition;
        ins.GetComponent<Image>().color = Color.red;
        GetPlayer().GetComponent<Player>().health -= 1;

        GameObject.Find("CurrentHealth").GetComponent<Text>().text = GetPlayer().GetComponent<Player>().health.ToString();
        Destroy(gameObject);
    }

    public float ToPointAngleRAD(Vector3 targetPosition)
    {
        Vector3 moveDirection = targetPosition - Laodicea.transform.localPosition;
        if (moveDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x);
            return angle;
        }
        else return 0;
    }

    public float ToPointMagnitude(Vector3 targetPosition)
    {
        Vector3 moveDirection = Laodicea.transform.localPosition - targetPosition;
        return moveDirection.magnitude;
    }
}
