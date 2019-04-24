using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RevillaSword : MonoBehaviour {
    public int moveCode;
    public float direction;

    // Update is called once per frame
    void Start() {
        GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        StartCoroutine("Launch");
        StartCoroutine("Enable");
        StartCoroutine("Refresh");
    }

    IEnumerator Refresh() {
        transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, direction));
        yield return new WaitForSeconds(1 / 60f);
        StartCoroutine("Refresh");
    }

    IEnumerator Launch() {
        GameObject ins;
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();

        if (moveCode == 0)
        {
            direction = 100f;
            for (float i = 3; i >= 0; i -= 0.2f)
            {
                direction -= i;
                yield return new WaitForSeconds(1 / 30f);
            }
            for (; direction <= 220; direction += 6)
            {
                if (GameController.rank <= 0)
                {
                    cct.Play();
                    ins = GetBullet("CIRCLE");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.parent.localPosition + new Vector3(250f * Mathf.Cos(direction * Mathf.PI / 180f), 250f * Mathf.Sin(direction * Mathf.PI / 180f)),
                        Random.Range(GameController.rank * 2 + 3, GameController.rank * 3 + 8), direction, 0.25f, 0.25f, Color.white, Color.red);
                    ins = GetBullet("CIRCLE");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.parent.localPosition + new Vector3(250f * Mathf.Cos(direction * Mathf.PI / 180f), 250f * Mathf.Sin(direction * Mathf.PI / 180f)),
                        Random.Range(GameController.rank * 2 + 3, GameController.rank * 3 + 8), direction + 180, 0.25f, 0.25f, Color.white, Color.blue);
                    yield return new WaitForSeconds(1 / 80f);
                }
                else if (GameController.rank <= 3)
                {
                    cct.Play();
                    for (int i = -1; i <= 1; i++)
                    {
                        ins = GetBullet("CIRCLE");
                        if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.parent.localPosition + new Vector3(250f * Mathf.Cos(direction * Mathf.PI / 180f), 250f * Mathf.Sin(direction * Mathf.PI / 180f)),
                            Random.Range((GameController.rank - 1) + 3, (GameController.rank - 1) * 2 + 8), direction + i * 9.7f, 0.25f, 0.25f, Color.white, Color.red);
                        ins = GetBullet("CIRCLE");
                        if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.parent.localPosition + new Vector3(250f * Mathf.Cos(direction * Mathf.PI / 180f), 250f * Mathf.Sin(direction * Mathf.PI / 180f)),
                            Random.Range((GameController.rank - 1) + 3, (GameController.rank - 1) * 2 + 8), (direction + i * 9.7f) + 180, 0.25f, 0.25f, Color.white, Color.blue);
                    }
                    yield return new WaitForSeconds(1 / 90f);
                }
                else if (GameController.rank <= 5)
                {
                    cct.Play();
                    for (float i = -1.5f; i <= 1.5; i += 1)
                    {
                        ins = GetBullet("CIRCLE");
                        if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.parent.localPosition + new Vector3(250f * Mathf.Cos(direction * Mathf.PI / 180f), 250f * Mathf.Sin(direction * Mathf.PI / 180f)),
                            Random.Range((GameController.rank - 3) + 2, (GameController.rank - 3) * 2 + 8), direction + i * 8.7f, 0.25f, 0.25f, Color.white, Color.red);
                        ins = GetBullet("CIRCLE");
                        if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.parent.localPosition + new Vector3(250f * Mathf.Cos(direction * Mathf.PI / 180f), 250f * Mathf.Sin(direction * Mathf.PI / 180f)),
                            Random.Range((GameController.rank - 3) + 2, (GameController.rank - 3) * 2 + 8), (direction + i * 8.7f) + 180, 0.25f, 0.25f, Color.white, Color.blue);
                    }
                    yield return new WaitForSeconds(1 / 100f);
                }
                else if (GameController.rank <= 7)
                {
                    cct.Play();
                    for (int i = -2; i <= 2; i++)
                    {
                        ins = GetBullet("CIRCLE");
                        if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.parent.localPosition + new Vector3(250f * Mathf.Cos(direction * Mathf.PI / 180f), 250f * Mathf.Sin(direction * Mathf.PI / 180f)),
                            Random.Range((GameController.rank - 5) + 2, (GameController.rank - 5) * 2 + 7), direction + i * 7.7f, 0.25f, 0.25f, Color.white, Color.red);
                        ins = GetBullet("CIRCLE");
                        if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.parent.localPosition + new Vector3(250f * Mathf.Cos(direction * Mathf.PI / 180f), 250f * Mathf.Sin(direction * Mathf.PI / 180f)),
                            Random.Range((GameController.rank - 5) + 2, (GameController.rank - 5) * 2 + 7), (direction + i * 7.7f) + 180, 0.25f, 0.25f, Color.white, Color.blue);
                    }
                    ins = GetBullet("THORN");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.parent.localPosition + new Vector3(250f * Mathf.Cos(direction * Mathf.PI / 180f), 250f * Mathf.Sin(direction * Mathf.PI / 180f)),
                        Random.Range(5f, 9f), direction, 0.3f, 0.3f, Color.white, Color.green);
                    yield return new WaitForSeconds(1 / 100f);
                }
                else
                {
                    cct.Play();
                    for (float i = -2.5f; i <= 2.5f; i++)
                    {
                        ins = GetBullet("CIRCLE");
                        if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.parent.localPosition + new Vector3(250f * Mathf.Cos(direction * Mathf.PI / 180f), 250f * Mathf.Sin(direction * Mathf.PI / 180f)),
                            Random.Range((GameController.rank - 6) + 2, (GameController.rank - 6) * 2 + 7), direction + i * 6.7f, 0.25f, 0.25f, Color.white, Color.red);
                        ins = GetBullet("CIRCLE");
                        if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.parent.localPosition + new Vector3(250f * Mathf.Cos(direction * Mathf.PI / 180f), 250f * Mathf.Sin(direction * Mathf.PI / 180f)),
                            Random.Range((GameController.rank - 6) + 2, (GameController.rank - 6) * 2 + 7), (direction + i * 6.7f) + 180, 0.25f, 0.25f, Color.white, Color.blue);
                    }
                    ins = GetBullet("THORN");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.parent.localPosition + new Vector3(250f * Mathf.Cos(direction * Mathf.PI / 180f), 250f * Mathf.Sin(direction * Mathf.PI / 180f)),
                        Random.Range(3f, 7f), direction, 0.4f, 0.4f, Color.white, Color.green);
                    yield return new WaitForSeconds(1 / 100f);
                }
                
            }
            StartCoroutine("Unable");
        }
        else if (moveCode == 1) {
            transform.localScale = new Vector3(0.5f, 0.5f, 1f);
            Vector3 originPos = transform.localPosition;
            float dist = 100;

            for (float i = 8; i > 0; i -= 0.4f) {
                dist += i;
                transform.localPosition = originPos - new Vector3(dist * Mathf.Cos(direction * Mathf.PI / 180f), dist * Mathf.Sin(direction * Mathf.PI / 180f));
                yield return new WaitForSeconds(1 / 45f);
            }
            for (; dist > -45; dist -= 40) {
                transform.localPosition = originPos - new Vector3(dist * Mathf.Cos(direction * Mathf.PI / 180f), dist * Mathf.Sin(direction * Mathf.PI / 180f));
                yield return new WaitForSeconds(1 / 60f);
            }
            StartCoroutine("Unable");
        }
        if (moveCode == 2)
        {
            direction = 100f;
            for (float i = 3; i >= 0; i -= 0.2f)
            {
                direction -= i;
                yield return new WaitForSeconds(1 / 30f);
            }
            for (; direction <= 220; direction += 12)
            {
                if (GameController.rank <= 0)
                {
                    cct.Play();
                    ins = GetBullet("CIRCLE");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.parent.localPosition + new Vector3(250f * Mathf.Cos(direction * Mathf.PI / 180f), 250f * Mathf.Sin(direction * Mathf.PI / 180f)),
                        Random.Range(GameController.rank * 2 + 3, GameController.rank * 3 + 8), direction, 0.25f, 0.25f, Color.white, Color.red);
                    ins = GetBullet("CIRCLE");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.parent.localPosition + new Vector3(250f * Mathf.Cos(direction * Mathf.PI / 180f), 250f * Mathf.Sin(direction * Mathf.PI / 180f)),
                        Random.Range(GameController.rank * 2 + 3, GameController.rank * 3 + 8), direction + 180, 0.25f, 0.25f, Color.white, Color.blue);
                }
                else if (GameController.rank <= 3)
                {
                    cct.Play();
                    for (int i = -1; i <= 1; i++)
                    {
                        ins = GetBullet("CIRCLE");
                        if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.parent.localPosition + new Vector3(250f * Mathf.Cos(direction * Mathf.PI / 180f), 250f * Mathf.Sin(direction * Mathf.PI / 180f)),
                            Random.Range((GameController.rank - 1) + 3, (GameController.rank - 1) * 2 + 8), direction + i * 9.7f, 0.25f, 0.25f, Color.white, Color.red);
                        ins = GetBullet("CIRCLE");
                        if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.parent.localPosition + new Vector3(250f * Mathf.Cos(direction * Mathf.PI / 180f), 250f * Mathf.Sin(direction * Mathf.PI / 180f)),
                            Random.Range((GameController.rank - 1) + 3, (GameController.rank - 1) * 2 + 8), (direction + i * 9.7f) + 180, 0.25f, 0.25f, Color.white, Color.blue);
                    }
                }
                else if (GameController.rank <= 5)
                {
                    cct.Play();
                    for (float i = -1.5f; i <= 1.5; i += 1)
                    {
                        ins = GetBullet("CIRCLE");
                        if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.parent.localPosition + new Vector3(250f * Mathf.Cos(direction * Mathf.PI / 180f), 250f * Mathf.Sin(direction * Mathf.PI / 180f)),
                            Random.Range((GameController.rank - 3) + 2, (GameController.rank - 3) * 2 + 8), direction + i * 8.7f, 0.25f, 0.25f, Color.white, Color.red);
                        ins = GetBullet("CIRCLE");
                        if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.parent.localPosition + new Vector3(250f * Mathf.Cos(direction * Mathf.PI / 180f), 250f * Mathf.Sin(direction * Mathf.PI / 180f)),
                            Random.Range((GameController.rank - 3) + 2, (GameController.rank - 3) * 2 + 8), (direction + i * 8.7f) + 180, 0.25f, 0.25f, Color.white, Color.blue);
                    }
                }
                else if (GameController.rank <= 7)
                {
                    cct.Play();
                    for (int i = -2; i <= 2; i++)
                    {
                        ins = GetBullet("CIRCLE");
                        if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.parent.localPosition + new Vector3(250f * Mathf.Cos(direction * Mathf.PI / 180f), 250f * Mathf.Sin(direction * Mathf.PI / 180f)),
                            Random.Range((GameController.rank - 5) + 2, (GameController.rank - 5) * 2 + 7), direction + i * 7.7f, 0.25f, 0.25f, Color.white, Color.red);
                        ins = GetBullet("CIRCLE");
                        if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.parent.localPosition + new Vector3(250f * Mathf.Cos(direction * Mathf.PI / 180f), 250f * Mathf.Sin(direction * Mathf.PI / 180f)),
                            Random.Range((GameController.rank - 5) + 2, (GameController.rank - 5) * 2 + 7), (direction + i * 7.7f) + 180, 0.25f, 0.25f, Color.white, Color.blue);
                    }
                }
                else
                {
                    cct.Play();
                    for (int i = -3; i <= 3; i++)
                    {
                        ins = GetBullet("CIRCLE");
                        if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.parent.localPosition + new Vector3(250f * Mathf.Cos(direction * Mathf.PI / 180f), 250f * Mathf.Sin(direction * Mathf.PI / 180f)),
                            Random.Range((GameController.rank - 6) + 2, (GameController.rank - 6) * 2 + 7), direction + i * 6.7f, 0.25f, 0.25f, Color.white, Color.red);
                        ins = GetBullet("CIRCLE");
                        if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.parent.localPosition + new Vector3(250f * Mathf.Cos(direction * Mathf.PI / 180f), 250f * Mathf.Sin(direction * Mathf.PI / 180f)),
                            Random.Range((GameController.rank - 6) + 2, (GameController.rank - 6) * 2 + 7), (direction + i * 6.7f) + 180, 0.25f, 0.25f, Color.white, Color.blue);
                    }

                }
                yield return new WaitForSeconds(1 / 150f);
            }
            StartCoroutine("Unable");
        }
        else if (moveCode == 3)
        {
            transform.localScale = new Vector3(0.3f, 0.3f, 1f);
            Vector3 originPos = transform.localPosition;
            float dist = 100;

            for (float i = 6; i > 0; i -= 0.2f)
            {
                dist += i;
                transform.localPosition = originPos - new Vector3(dist * Mathf.Cos(direction * Mathf.PI / 180f), dist * Mathf.Sin(direction * Mathf.PI / 180f));
                yield return new WaitForSeconds(1 / 45f);
            }
            yield return new WaitForSeconds(1 / 10f);
            for (; dist > -45; dist -= 40)
            {
                cct.Play();
                transform.localPosition = originPos - new Vector3(dist * Mathf.Cos(direction * Mathf.PI / 180f), dist * Mathf.Sin(direction * Mathf.PI / 180f));

                Vector3 TempPos = transform.localPosition + new Vector3(30 * Mathf.Cos(direction * Mathf.PI / 180f), 30 * Mathf.Sin(direction * Mathf.PI / 180f));

                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("REV", TempPos, 1, direction + Random.Range(-15f, 15f), 0.25f, 0.25f, Color.white, Color.red);
                if (GameController.rank >= 3)
                {
                    ins = GetBullet("CIRCLE");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("REV", TempPos, 1, direction + Random.Range(-30f, 30f), 0.22f, 0.22f, Color.white, Color.blue);
                }
                if (GameController.rank >= 6)
                {
                    ins = GetBullet("CIRCLE");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("REV", TempPos, 1, direction + Random.Range(-45f, 45f), 0.2f, 0.2f, Color.white, Color.green);
                }
                if (GameController.rank >= 9)
                {
                    ins = GetBullet("CIRCLE");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("REV", TempPos, 1, direction + Random.Range(-60f, 60f), 0.18f, 0.18f, Color.white, Color.yellow);
                }
                yield return new WaitForSeconds(1 / 60f);
            }
            StartCoroutine("Unable");
        }
        else if (moveCode == 4) {
            transform.localScale = new Vector3(1.2f, 1.2f, 1f);
            Vector3 originPos = transform.localPosition;
            float dist = 0;

            for (float i = 40; i > 0; i -= 1f)
            {
                dist += i;
                transform.localPosition = originPos - new Vector3(dist * Mathf.Cos(direction * Mathf.PI / 180f), dist * Mathf.Sin(direction * Mathf.PI / 180f));
                yield return new WaitForSeconds(1 / 45f);
            }
            yield return new WaitForSeconds(1 / 10f);
            for (; dist > -450; dist -= 60)
            {
                cct.Play();
                transform.localPosition = originPos - new Vector3(dist * Mathf.Cos(direction * Mathf.PI / 180f), dist * Mathf.Sin(direction * Mathf.PI / 180f));

                Vector3 TempPos = transform.localPosition + new Vector3(200 * Mathf.Cos(direction * Mathf.PI / 180f), 200 * Mathf.Sin(direction * Mathf.PI / 180f));

                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("REV", TempPos, 1, direction + Random.Range(-15f, 15f), 0.25f, 0.25f, Color.white, Color.red);
                if (GameController.rank >= 3)
                {
                    ins = GetBullet("CIRCLE");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("REV", TempPos, 1, direction + Random.Range(-30f, 30f), 0.22f, 0.22f, Color.white, Color.blue);
                }
                if (GameController.rank >= 6)
                {
                    ins = GetBullet("CIRCLE");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("REV", TempPos, 1, direction + Random.Range(-45f, 45f), 0.2f, 0.2f, Color.white, Color.green);
                }
                if (GameController.rank >= 9)
                {
                    ins = GetBullet("CIRCLE");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("REV", TempPos, 1, direction + Random.Range(-60f, 60f), 0.18f, 0.18f, Color.white, Color.yellow);
                }
                yield return new WaitForSeconds(1 / 60f);
            }
            StartCoroutine("Unable");
        }
        yield return null;
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
        Vector3 moveDirection = player.transform.localPosition - transform.parent.localPosition;
        if (moveDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            return angle;
        }
        else return 0;
    }

    IEnumerator Enable()
    {
        Image img = GetComponent<Image>();
        for (; img.color.a < 1; img.color += new Color(0f, 0f, 0f, 1 / 15f))
        {
            yield return new WaitForSeconds(1 / 60f);
        }
        yield return null;
    }

    public void InvokeUnable() {
        StartCoroutine("Unable");
    }

    IEnumerator Unable() {
        StopCoroutine("Launch");
        Image img = GetComponent<Image>();
        for (; img.color.a > 0; img.color -= new Color(0f, 0f, 0f, 1 / 30f)) {
            yield return new WaitForSeconds(1 / 60f);
        }
        Destroy(gameObject);
    }
}
