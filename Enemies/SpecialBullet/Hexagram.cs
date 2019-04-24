using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hexagram : MonoBehaviour {

    public int moveCode;
    public float speed;
    public float direction;
    public float duration;

    public GameObject[] summonTargets;
    public GameObject lord;

    int rotDir = 0;
    float dir;
    float dix;

    Image myImage;

    public void SetIdentity(float _speed, float _direction) {
        speed = _speed; direction = _direction;
    }

    // Update is called once per frame
    void Start()
    {
        myImage = GetComponent<Image>();
        myImage.color = new Color(1f, 1f, 1f, 0f);
        transform.localScale = new Vector3(3f, 3f, 1f);
        StartCoroutine("Enable");
        StartCoroutine("Refresh");

        dir = Random.Range(0f, 360f);
        dix = dir;
    }

    IEnumerator Enable() {
        for (; myImage.color.a < 0.75f; myImage.color += new Color(0f, 0f, 0f, 1 / 20f)) {
            yield return new WaitForSeconds(1 / 60f);
            transform.localScale -= new Vector3(2 / 15f, 2 / 15f);
            if (moveCode == 9) {
                GameObject.Find("BackgroundPanel").GetComponent<RectTransform>().localScale -= new Vector3(2 / 15f, 0f);
                GameObject.Find("TriggerPanel").GetComponent<RectTransform>().localScale -= new Vector3(2 / 15f, 0f);
                GameObject.Find("UIPanel").GetComponent<RectTransform>().localScale -= new Vector3(2 / 15f, 0f);
            }
            if (moveCode == 11)
            {
                GameObject.Find("BackgroundPanel").GetComponent<RectTransform>().localScale -= new Vector3(0f, 2 / 15f);
                GameObject.Find("TriggerPanel").GetComponent<RectTransform>().localScale -= new Vector3(0f, 2 / 15f);
                GameObject.Find("UIPanel").GetComponent<RectTransform>().localScale -= new Vector3(0f, 2 / 15f);
            }
        }

        if (moveCode == 9)
        {
            GameObject.Find("BackgroundPanel").GetComponent<RectTransform>().localScale = new Vector3(-1, 1, 1);
            GameObject.Find("TriggerPanel").GetComponent<RectTransform>().localScale = new Vector3(-1, 1, 1);
            GameObject.Find("UIPanel").GetComponent<RectTransform>().localScale = new Vector3(-1, 1, 1);
            myImage.color = new Color(1f, 1f, 1f, 0.5f);
        }
        if (moveCode == 11)
        {
            GameObject.Find("BackgroundPanel").GetComponent<RectTransform>().localScale = new Vector3(1, -1, 1);
            GameObject.Find("TriggerPanel").GetComponent<RectTransform>().localScale = new Vector3(1, -1, 1);
            GameObject.Find("UIPanel").GetComponent<RectTransform>().localScale = new Vector3(1, -1, 1);
            myImage.color = new Color(1f, 1f, 1f, 0.5f);
        }
        int rand = Random.Range(0, 2);
        rotDir = (rand == 0) ? 1 : -1;
        StartCoroutine("Launch");

        if (moveCode != 4) yield return new WaitForSeconds(duration);
        else yield return new WaitForSeconds(duration - GameController.rank * 0.2f);
        StartCoroutine("Unable");
    }

    float it = 0;
    IEnumerator Refresh()
    {
        it += rotDir * 1.5f;
        transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, it));

        if (moveCode <= 4 || moveCode >= 9)
        {
            transform.localPosition += new Vector3(speed * Mathf.Cos(direction * Mathf.PI / 180f), speed * Mathf.Sin(direction * Mathf.PI / 180f));
        }
        else if (moveCode % 2 == 1)
        {
            direction += 1.5f;
            if (lord != null) transform.localPosition = lord.transform.localPosition + new Vector3(speed * Mathf.Cos(direction * Mathf.PI / 180f), speed * Mathf.Sin(direction * Mathf.PI / 180f));
        }
        else {
            direction -= 1.5f;
            if (lord != null) transform.localPosition = lord.transform.localPosition + new Vector3(speed * Mathf.Cos(direction * Mathf.PI / 180f), speed * Mathf.Sin(direction * Mathf.PI / 180f));
        }
        yield return new WaitForSeconds(1 / 60f);
        StartCoroutine("Refresh");
    }

    IEnumerator Launch() {
        GameObject ins;
        AudioSource sct = GameObject.Find("Sound_EnemyAttack").GetComponent<AudioSource>();
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();

        if (moveCode == 0 || moveCode == 5)
        {
            sct.Play();

            float density = 360f / (5 + GameController.rank);
            for (float i = Random.Range(0f, density); i < 360; i += density)
            {
                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 7 - moveCode * 0.8f, i, 0.25f, 0.25f, Color.white, Color.green);
            }
            if (moveCode == 0) yield return new WaitForSeconds(1.8f - GameController.rank * 0.12f);
            else yield return new WaitForSeconds(2.4f - GameController.rank * 0.16f);
        }

        else if (moveCode == 1 || moveCode == 6)
        {
            sct.Play();

            for (int i = 0; i <= GameController.rank / 2; i++)
            {
                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 10 + i - (moveCode-1), ToPlayerAngle(), 0.25f, 0.25f, Color.white, Color.magenta);
            }
            if (moveCode == 1) yield return new WaitForSeconds(2f - GameController.rank * 0.1f);
            else yield return new WaitForSeconds(3f - GameController.rank * 0.15f);
        }

        else if (moveCode == 2 || moveCode == 7)
        {
            cct.Play();

            dir += 15 - GameController.rank * 0.5f;
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 9 - (moveCode - 2) * 0.6f, dir, 0.25f, 0.25f, Color.white, Color.yellow);
            dix -= 15 - GameController.rank * 0.5f;

            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 9 - (moveCode - 2) * 0.6f, dix, 0.25f, 0.25f, Color.white, Color.yellow);

            if (moveCode == 2) yield return new WaitForSeconds(0.5f - 0.038f * GameController.rank);
            else yield return new WaitForSeconds(0.8f - 0.05f * GameController.rank);
        }

        else if (moveCode == 3 || moveCode == 8)
        {
            cct.Play();

            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, Random.Range(4f, 8f + GameController.rank * 0.5f - (moveCode - 3)), Random.Range(0f, 360f), 0.25f, 0.25f, Color.white, Color.blue);

            yield return new WaitForSeconds(0.5f - 0.038f * GameController.rank);
        }

        else if (moveCode == 10) {
            float density = 360f / (8 + GameController.rank * 4);
            sct.Play();

            for (float i = Random.Range(0f, density); i < 90; i += density)
            {
                ins = GetBullet("CIRCLE");
                if(ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 5, i + (ToPlayerAngle() - 45), 0.25f, 0.25f, BulletCode.COLOR_SKY, Color.blue);
                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ILLUSION", transform.localPosition, 5, i + density/2f + (ToPlayerAngle() - 45), 0.25f, 0.25f, BulletCode.COLOR_SKY, Color.blue);
            }
            yield return new WaitForSeconds(1.5f - GameController.rank * 0.075f);
        }

        else if (moveCode == 12)
        {
            cct.Play();

            float borderY = transform.localPosition.y / Mathf.Abs(transform.localPosition.y);
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, Random.Range(3, 5 + GameController.rank * 0.5f), 
                (180 + borderY * 90) + Random.Range(-1f, 1f) * GameController.rank, 0.25f, 0.25f, Color.magenta, BulletCode.COLOR_VIOLET);
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ILLUSION", transform.localPosition, Random.Range(3, 5 + GameController.rank * 0.5f),
                (180 + borderY * 90) + Random.Range(-1f, 1f) * GameController.rank, 0.25f, 0.25f, Color.magenta, BulletCode.COLOR_VIOLET);
            yield return new WaitForSeconds(0.6f - GameController.rank * 0.046f);
        }
        else yield return new WaitForSeconds(100f);

        StartCoroutine("Launch");
    }

    public void InvokeUnable()
    {
        StopCoroutine("Enable");
        StartCoroutine("UnableEntirely");
    }

    IEnumerator Unable() {
        StopCoroutine("Launch");
        if (moveCode == 4) {
            GameObject ins = Instantiate(summonTargets[Random.Range(0, 2)], GameObject.Find("TriggerPanel").transform);
            ins.transform.localPosition = transform.localPosition;
        }

        for (; myImage.color.a > 0; myImage.color -= new Color(0f, 0f, 0f, 1 / 60f))
        {
            if (moveCode != 9 && moveCode != 11) yield return new WaitForSeconds(1 / 60f);
            else yield return new WaitForSeconds(1 / 180f);
            if (moveCode == 9)
            {
                GameObject.Find("BackgroundPanel").GetComponent<RectTransform>().localScale += new Vector3(2 / 45f, 0f);
                GameObject.Find("TriggerPanel").GetComponent<RectTransform>().localScale += new Vector3(2 / 45f, 0f);
                GameObject.Find("UIPanel").GetComponent<RectTransform>().localScale += new Vector3(2 / 45f, 0f);
            }
            if (moveCode == 11)
            {
                GameObject.Find("BackgroundPanel").GetComponent<RectTransform>().localScale += new Vector3(0f, 2 / 45f);
                GameObject.Find("TriggerPanel").GetComponent<RectTransform>().localScale += new Vector3(0f, 2 / 45f);
                GameObject.Find("UIPanel").GetComponent<RectTransform>().localScale += new Vector3(0f, 2 / 45f);
            }
        }
        if (moveCode == 9)
        {
            GameObject.Find("BackgroundPanel").GetComponent<RectTransform>().localScale = Vector3.one;
            GameObject.Find("TriggerPanel").GetComponent<RectTransform>().localScale = Vector3.one;
            GameObject.Find("UIPanel").GetComponent<RectTransform>().localScale = Vector3.one;
        }
        if (moveCode == 11)
        {
            GameObject.Find("BackgroundPanel").GetComponent<RectTransform>().localScale = Vector3.one;
            GameObject.Find("TriggerPanel").GetComponent<RectTransform>().localScale = Vector3.one;
            GameObject.Find("UIPanel").GetComponent<RectTransform>().localScale = Vector3.one;
        }
        Destroy(gameObject);
    }

    IEnumerator UnableEntirely()
    {
        StopCoroutine("Launch");

        if (moveCode == 9) myImage.color = new Color(1f, 1f, 1f, 0.75f);

        for (; myImage.color.a > 0; myImage.color -= new Color(0f, 0f, 0f, 1 / 60f))
        {
            if (moveCode != 9 && moveCode != 11) yield return new WaitForSeconds(1 / 60f);
            else yield return new WaitForSeconds(1 / 180f);
            if (moveCode == 9)
            {
                GameObject.Find("BackgroundPanel").GetComponent<RectTransform>().localScale += new Vector3(2 / 45f, 0f);
                GameObject.Find("TriggerPanel").GetComponent<RectTransform>().localScale += new Vector3(2 / 45f, 0f);
                GameObject.Find("UIPanel").GetComponent<RectTransform>().localScale += new Vector3(2 / 45f, 0f);
            }
            if (moveCode == 11)
            {
                GameObject.Find("BackgroundPanel").GetComponent<RectTransform>().localScale += new Vector3(0f, 2 / 45f);
                GameObject.Find("TriggerPanel").GetComponent<RectTransform>().localScale += new Vector3(0f, 2 / 45f);
                GameObject.Find("UIPanel").GetComponent<RectTransform>().localScale += new Vector3(0f, 2 / 45f);
            }
        }
        if (moveCode == 9)
        {
            GameObject.Find("BackgroundPanel").GetComponent<RectTransform>().localScale = Vector3.one;
            GameObject.Find("TriggerPanel").GetComponent<RectTransform>().localScale = Vector3.one;
            GameObject.Find("UIPanel").GetComponent<RectTransform>().localScale = Vector3.one;
        }
        if (moveCode == 11)
        {
            GameObject.Find("BackgroundPanel").GetComponent<RectTransform>().localScale = Vector3.one;
            GameObject.Find("TriggerPanel").GetComponent<RectTransform>().localScale = Vector3.one;
            GameObject.Find("UIPanel").GetComponent<RectTransform>().localScale = Vector3.one;
        }
        Destroy(gameObject);
    }

    public GameObject GetBullet(string code)
    {
        return GameController.BulletPool.GetComponent<BulletPool>().GetChildMin(code);
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

    public float ToPlayerAngle()
    {
        GameObject player = GetPlayer();
        Vector3 moveDirection = player.transform.localPosition - transform.localPosition;
        if (moveDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            return angle;
        }
        else return 0;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Main" && moveCode <= 4)
        {
            Destroy(gameObject);
        }
    }
}