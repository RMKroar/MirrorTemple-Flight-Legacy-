using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaodiceaSword : MonoBehaviour {

    public int moveCode;
    public float speed;
    public float direction;

    Image myImage;

    // Use this for initialization
    void Start()
    {
        myImage = GetComponent<Image>();
        myImage.color = new Color(1f, 1f, 1f, 0f);
        StartCoroutine("Launch");
        StartCoroutine("Enable");
        StartCoroutine("Refresh");
    }

    IEnumerator Refresh()
    {
        transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, direction));
        float radDir = direction * Mathf.PI / 180f;
        transform.localPosition += new Vector3(speed * Mathf.Cos(radDir), speed * Mathf.Sin(radDir), 0f);

        yield return new WaitForSeconds(1 / 60f);
        StartCoroutine("Refresh");
    }

    IEnumerator Launch()
    {
        GameObject ins;
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();
        AudioSource sct = GameObject.Find("Sound_EnemyAttack").GetComponent<AudioSource>();

        if (moveCode == 0 || moveCode == 2) {
            if (moveCode == 2) {
                myImage.color = Color.white;
                transform.localScale = new Vector3(0.5f, 0.5f, 1f);
            }

            for (int i = 0; speed > 0; speed -= 2f, i++) {
                if (moveCode == 0) {
                    if (i % (7 - GameController.rank / 3) == 0)
                    {
                        cct.Play();
                        ins = GetBullet("CIRCLE");
                        if (ins != null)
                        {
                            ins.GetComponent<Bullet>().SetIdentityEx("CRYSTAL",
                                transform.localPosition + new Vector3(50f * Mathf.Cos(direction * Mathf.PI / 180f), 50f * Mathf.Sin(direction * Mathf.PI / 180f)),
                                Random.Range(5f, 10f), direction + Random.Range(-12f, 12f), 0.3f, 0.3f,
                                new Color(1f, Random.Range(0.3f, 0.7f), 0f), Color.white);
                        }
                    }                   
                }
                yield return new WaitForSeconds(1 / 60f);
            }
            InvokeUnable();
        }

        if (moveCode == 1) {
            transform.localScale = new Vector3(0.6f, 0.6f, 1f);
            yield return new WaitForSeconds(1.5f - 0.05f * GameController.rank);
            speed = 50f;

            for (int i = 0; i < 60; i++)
            {
                if (i % (5 - GameController.rank / 4) == 0)
                {
                    cct.Play();
                    ins = GetBullet("THORN");
                    if (ins != null)
                    {
                        ins.GetComponent<Bullet>().SetIdentity(
                            transform.localPosition + new Vector3(50f * Mathf.Cos(direction * Mathf.PI / 180f), 50f * Mathf.Sin(direction * Mathf.PI / 180f)),
                            Random.Range(10f, 15f), direction + Random.Range(-GameController.rank * 0.5f, GameController.rank * 0.5f), 0.35f, 0.35f,
                            Color.white, new Color(1f, Random.Range(0.3f, 0.7f), 0f));
                    }
                }
                yield return new WaitForSeconds(1 / 60f);
            }
            InvokeUnable();
        }

        if (moveCode == 3 || moveCode == 4)
        {
            myImage.color = Color.white;
            transform.localScale = new Vector3(0.4f, 0.4f, 1f);

            for (int i = 0; i < 120; i++)
            {
                cct.Play();
                if (i % (23 - GameController.rank / 2) == 0)
                {
                    ins = GetBullet("THORN");
                    if (ins != null)
                    {
                        ins.GetComponent<Bullet>().SetIdentity(
                            transform.localPosition + new Vector3(50f * Mathf.Cos(direction * Mathf.PI / 180f), 50f * Mathf.Sin(direction * Mathf.PI / 180f)),
                            Random.Range(6f, 12f), direction + Random.Range(-GameController.rank * 0.5f, GameController.rank * 0.5f), 0.35f, 0.35f,
                            Color.white, new Color(1f, Random.Range(0.3f, 0.7f), 0f));
                    }
                }
                direction = (moveCode == 3) ? direction + (0.25f * (1 + GameController.rank / 3)) : direction - (0.25f * (1 + GameController.rank / 3));
                yield return new WaitForSeconds(1 / 60f);
            }
            InvokeUnable();
        }

        if (moveCode == 5 || moveCode == 6)
        {
            myImage.color = Color.white;
            transform.localScale = new Vector3(0.5f, 0.5f, 1f);

            for (int i = 0; i < 30; i++)
            {
                direction = (moveCode == 5) ? direction + 4f : direction - 4f;
                yield return new WaitForSeconds(1 / 60f);
            }
            InvokeUnable();
        }

        if (moveCode == 7 || moveCode == 8)
        {
            if (moveCode == 7) {
                transform.localScale = new Vector3(0.6f, 0.6f, 1f);
                yield return new WaitForSeconds(1.5f - 0.05f * GameController.rank);
                speed = 45f;
            }
            else
            {
                myImage.color = Color.white;
                transform.localScale = new Vector3(1f, 1f, 1f);
                speed = 50f;
            }

            for (int i = 0; i < 90; i++)
            {
                if (i % (15 - GameController.rank) == 0)
                {
                    cct.Play();
                    ins = GetBullet("THORN");
                    if (ins != null)
                    {
                        ins.GetComponent<Bullet>().SetIdentity(
                            transform.localPosition + new Vector3(50f * Mathf.Cos(direction * Mathf.PI / 180f), 50f * Mathf.Sin(direction * Mathf.PI / 180f)),
                            Random.Range(8f, 13f), direction + Random.Range(-GameController.rank * 2, GameController.rank * 2), transform.localScale.x * 0.8f, transform.localScale.y * 0.8f,
                            Color.white, new Color(0f, Random.Range(0f, 1f), 1f));
                    }
                }
                yield return new WaitForSeconds(1 / 60f);
            }
            InvokeUnable();
        }

        if (moveCode == 9 || moveCode == 10)
        {
            myImage.color = Color.white;         

            if (moveCode == 9)
            {
                transform.localScale = new Vector3(0.4f, 0.4f, 1f);
                yield return new WaitForSeconds(0.5f);
            }
            else {
                transform.localScale = new Vector3(1f, 1f, 1f);
                for (; speed > 0; speed -= 1f)
                {
                    yield return new WaitForSeconds(1 / 60f);
                }
            }
            InvokeUnable();
        }

        if (moveCode == 11)
        {
            yield return new WaitForSeconds(0.5f);
            speed = 70;
            if (direction == 0) {
                for (; transform.localPosition.x < 500;) {
                    yield return new WaitForSeconds(1 / 30f);
                }
                speed = 1;
            }
            if (direction == 180)
            {
                for (; transform.localPosition.x > -500;)
                {
                    yield return new WaitForSeconds(1 / 30f);
                }
                speed = 1;
            }
            if (direction == 90)
            {
                for (; transform.localPosition.y < 320;)
                {
                    yield return new WaitForSeconds(1 / 30f);
                }
                speed = 1;
            }
            if (direction == 270)
            {
                for (; transform.localPosition.y > -320;)
                {
                    yield return new WaitForSeconds(1 / 30f);
                }
                speed = 1;
            }

            yield return new WaitForSeconds(0.5f);

            sct.Play();
            for (int i = 0; i < GameController.rank * 3 / 2 + 4; i++) {
                ins = GetBullet("CIRCLE");
                if (ins != null)
                {
                    ins.GetComponent<Bullet>().SetIdentityEx(
                        "CRYSTAL", transform.localPosition + new Vector3(100f * Mathf.Cos(direction * Mathf.PI / 180f), 100f * Mathf.Sin(direction * Mathf.PI / 180f)),
                        Random.Range(4f, 9f), Random.Range(0f, 360f), 0.5f, 0.5f,
                        new Color(Random.Range(0.4f, 1f), 0f, 0f), Color.white);
                }
            }
            for (int i = 0; i < 20; i++) {
                cct.Play();
                ins = GetBullet("THORN");
                if (ins != null)
                {
                    ins.GetComponent<Bullet>().SetIdentity(
                        transform.localPosition + new Vector3(100f * Mathf.Cos(direction * Mathf.PI / 180f), 100f * Mathf.Sin(direction * Mathf.PI / 180f)),
                        Random.Range(35f, 45f + GameController.rank), direction + 180 + Random.Range(-GameController.rank, GameController.rank), 1f, 1f
                        , Color.white, new Color(Random.Range(0.4f, 1f), 0f, 0f));
                }
                yield return new WaitForSeconds(1 / 60f);
            }
            InvokeUnable();
        }

        if (moveCode == 12)
        {
            transform.localScale = new Vector3(0.8f, 0.8f, 1f);
            yield return new WaitForSeconds(0.5f);
            speed = 40f;

            for (int i = 0; i < 90; i++)
            {
                if (i % (8 - (GameController.rank / 2)) == 0)
                {
                    cct.Play();
                    ins = GetBullet("THORN");
                    if (ins != null)
                    {
                        ins.GetComponent<Bullet>().SetIdentity(
                            transform.localPosition + new Vector3(50f * Mathf.Cos(direction * Mathf.PI / 180f), 50f * Mathf.Sin(direction * Mathf.PI / 180f)),
                            Random.Range(8f, 13f), direction + Random.Range(-GameController.rank * 2, GameController.rank * 2), transform.localScale.x * 0.8f, transform.localScale.y * 0.8f,
                            Color.white, new Color(Random.Range(0.4f, 1f), 0f, 0f));
                    }
                }
                yield return new WaitForSeconds(1 / 60f);
            }
            InvokeUnable();
        }

        if (moveCode == 13) {
            transform.localScale = new Vector3(0.4f, 0.4f, 1f);
            myImage.color = Color.white;
            speed = 30f;
            yield return new WaitForSeconds(0.5f);
            InvokeUnable();
        }

        if (moveCode == 14) {
            transform.localScale = new Vector3(0.8f, 0.8f, 1f);
            yield return new WaitForSeconds(0.5f);
            speed = 30;

            for (int i = 0; speed > 0; speed -= 2f, i++) {
                if (i % (7 - GameController.rank / 4) == 0)
                {
                    cct.Play();
                    ins = GetBullet("CIRCLE");
                    if (ins != null)
                    {
                        ins.GetComponent<Bullet>().SetIdentityEx("CRYSTAL",
                            transform.localPosition + new Vector3(50f * Mathf.Cos(direction * Mathf.PI / 180f), 50f * Mathf.Sin(direction * Mathf.PI / 180f)),
                            Random.Range(5f, 10f), direction + Random.Range(-12f, 12f), 0.3f, 0.3f,
                            new Color(1f, Random.Range(0.3f, 0.7f), 0f), Color.white);
                    }
                }
                yield return new WaitForSeconds(1 / 60f);
            }
            InvokeUnable();
        }

        yield return null;
    }

    public void SetIdentity(int _moveCode, float _speed, float _direction) {
        moveCode = _moveCode;
        speed = _speed;
        direction = _direction;
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

    IEnumerator Enable()
    {
        for (; myImage.color.a < 1f; myImage.color += new Color(0f, 0f, 0f, 1 / 15f))
        {
            yield return new WaitForSeconds(1 / 60f);
        }
        yield return null;
    }

    public void InvokeUnable()
    {
        StartCoroutine("Unable");
    }

    IEnumerator Unable()
    {
        StopCoroutine("Launch");
        Image img = GetComponent<Image>();
        for (; img.color.a > 0; img.color -= new Color(0f, 0f, 0f, 1 / 12f))
        {
            yield return new WaitForSeconds(1 / 60f);
        }
        Destroy(gameObject);
    }
}
