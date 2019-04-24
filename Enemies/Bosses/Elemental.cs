using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Elemental : Boss {
    public GameObject Soul;

    IEnumerator IPattern;
    GameObject Thyatera;

    int id;
    int rank;
    float _speed = 0;
    float direction = 0;
    float radDir;

    public void SetIdentity(int _id, GameObject par) {
        id = _id;
        Thyatera = par;
    }

    public void SetRank(int _rank) {
        if (_rank < 0) rank++;
        else rank = _rank;
    }

    public void InvokeMove(Vector3 pos) {
        StartCoroutine(ToPointMove(pos));
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
        switch (maxPattern - pattern) {
            case 0:
                IPattern = Pattern_0();
                break;
            case 1:
                IPattern = Pattern_1();
                break;
            case 2:
                IPattern = Pattern_2();
                break;
            case 3:
                IPattern = Pattern_3();
                break;
            default:
                IPattern = Pattern_3();
                break;
        }
        
        StartCoroutine(IPattern);
        immortal = false;
        yield return null;
    }

    IEnumerator Pattern_0()
    {
        GameObject ins;
        float[] divide = new float[4] { 1, 1.5f, 2.5f, 5f };
        if (id == 0) {
            float randValue = Random.Range(0f, 360f);
            for (float i = 0; i < 360; i += 360 / (GameController.rank * 3 + 6)) {
                ins = GetBullet("CIRCLE");
                if (ins != null) {
                    ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 10 + GameController.rank * 0.5f, randValue + i, 0.35f, 0.35f,
                        Color.white, Color.red);
                }
            }
            yield return new WaitForSeconds(2.5f / divide[rank]);
        }

        if (id == 1)
        {
            float angle = ToPlayerAngle();
            for (float i = 0; i < 2 + GameController.rank / 2; i++)
            {             
                ins = GetBullet("THORN");
                if (ins != null)
                {
                    ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 7 + 2 * i, angle, 0.4f, 0.4f,
                        Color.white, BulletCode.COLOR_SKY);
                }
                if (rank >= 2) {
                    ins = GetBullet("THORN");
                    if (ins != null)
                    {
                        ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 7 + 2 * i, angle + 20f, 0.4f, 0.4f,
                            Color.white, BulletCode.COLOR_SKY);
                    }
                    ins = GetBullet("THORN");
                    if (ins != null)
                    {
                        ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 7 + 2 * i, angle - 20f, 0.4f, 0.4f,
                            Color.white, BulletCode.COLOR_SKY);
                    }
                }                
            }
            yield return new WaitForSeconds(3f / divide[rank]);
        }

        if (id == 2)
        {
            float angle = ToPlayerAngle();

            for (float i = Random.Range(-40f, -50f); i < 45f; i += 170f / (GameController.rank * 2 + 3))
            {               
                ins = GetBullet("PETAL");
                if (ins != null)
                {
                    ins.GetComponent<Bullet>().SetIdentityEx("WIDDY", transform.localPosition, 3, angle + i, 0.4f, 0.4f,
                        Color.white, Color.green);
                }
                if (rank >= 2) {
                    ins = GetBullet("PETAL");
                    if (ins != null)
                    {
                        ins.GetComponent<Bullet>().SetIdentityEx("WIDDY", transform.localPosition, 5, angle + i, 0.3f, 0.3f,
                            Color.white, Color.green);
                    }
                }

                yield return new WaitForSeconds(1 / 30f);
            }
            yield return new WaitForSeconds(0.7f / divide[rank]);
        }

        if (id == 3)
        {
            for (float i = 0; i < divide[rank]; i += 1) {
                ins = GetBullet("CIRCLE");
                if (ins != null)
                {
                    ins.GetComponent<Bullet>().SetIdentityEx("GNOME", transform.localPosition, 0.01f, 180, 0.35f, 0.35f, Color.white, BulletCode.COLOR_BROWN);
                }
                
            }
            yield return new WaitForSeconds(0.7f - 0.045f * GameController.rank);
        }

        StartCoroutine(Launch());
    }

    IEnumerator Pattern_1()
    {
        GameObject ins;
        float[] divide = new float[4] { 1, 1.5f, 2.5f, 4f };
        Color[] colors = new Color[4] { Color.red, Color.blue, Color.green, BulletCode.COLOR_DARKBROWN };
        string[] strings = new string[4] { "red", "blue", "green", "brown" };
        float[] dens = new float[4] { 90, 90, 60, 45 };

        for (float i = 90; i < 450; i += dens[rank])
        {
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("thyatera_" + strings[id], transform.localPosition, 12 + GameController.rank, ((rank == 2)? i : i + 45), 0.35f, 0.35f,
                    Color.white, colors[id]);
            }
        }
        yield return new WaitForSeconds((1.2f - 0.1f * GameController.rank) / divide[rank]);

        StartCoroutine(Launch());
    }

    IEnumerator Pattern_2()
    {
        GameObject ins;
        float[] divide = new float[4] { 1, 1.5f, 3f, 6f };
        float[] baseDelay = new float[4] { 4f, 2f, 4.5f, 3f };

        yield return new WaitForSeconds((baseDelay[id]) / divide[rank]);
        ins = Instantiate(Soul, GameObject.Find("TriggerPanel").transform);
        ins.GetComponent<Soul>().SetIdentity(transform.localPosition + new Vector3(Random.Range(-50f, 50f), Random.Range(-50f, 50f)), Random.Range(0.25f, 2f), Random.Range(0f, 360f));
        StartCoroutine(Launch());
    }

    IEnumerator Pattern_3() {
        maxHealth *= 1.5f;
        health *= 1.5f;
        yield return null;
    }

    // overrided
    public override void CheckHealth()
    {
        if (pattern >= 0) {
            Image img = transform.GetChild(pattern).gameObject.GetComponent<Image>();
            img.fillAmount = health / maxHealth;
        }      

        if (health <= 0)
        {
            immortal = true;
            GameController.EraseBullet();
            ParticleController Par = GameObject.Find("ParticleController").GetComponent<ParticleController>();
            GameObject ins = Instantiate(Par.ParticleAscend, GameObject.Find("ParticlePanel").transform);
            ins.transform.localPosition = transform.localPosition;
            ins.GetComponent<Image>().color = Color.white;

            StopCoroutine(IPattern);
            GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
            Thyatera.GetComponent<Thyatera>().RankUp(id);
        }
    }

    public void Revive(bool GoNextPattern) {
        GetComponent<Image>().color = Color.white;
        if (GoNextPattern)
        {
            pattern -= 1;
            if (pattern < 0)
            {
                ParticleController Par = GameObject.Find("ParticleController").GetComponent<ParticleController>();
                GameObject ins = Instantiate(Par.ParticleAscend, GameObject.Find("ParticlePanel").transform);
                ins.transform.localPosition = transform.localPosition;
                ins.GetComponent<Image>().color = Color.black;
                //Destroy(gameObject);
            }
            else {
                ParticleController Par = GameObject.Find("ParticleController").GetComponent<ParticleController>();
                GameObject ins = Instantiate(Par.ParticleAscend, GameObject.Find("ParticlePanel").transform);
                ins.transform.localPosition = transform.localPosition;
                ins.GetComponent<Image>().color = Color.white;
            }
        }
        else {
            StopCoroutine(IPattern);
            // New Particle: Revive with no pattern skip
            ParticleController Par = GameObject.Find("ParticleController").GetComponent<ParticleController>();
            GameObject ins = Instantiate(Par.ParticleAscend, GameObject.Find("ParticlePanel").transform);
            ins.transform.localPosition = transform.localPosition;
            ins.GetComponent<Image>().color = Color.white;
        }

        health = maxHealth;
        CheckHealth();
        StartCoroutine(StartLaunch());
    }

    IEnumerator ToPointMove(Vector3 pos)
    {
        float mag = ToPointMagnitude(pos);
        float rad = ToPointAngleRAD(pos);
        GameObject ins;

        if (pattern == 0 && health > 0)
        {
            float randValue = Random.Range(0f, 360f);
            for (float j = 0; j < 360f; j += 360f / (GameController.rank / 2 + 3))
            {
                ins = GetBullet("CIRCLE");
                if (ins != null) {
                    ins.GetComponent<Bullet>().SetIdentityEx("thyatera", transform.localPosition, 1.5f, j + randValue, 0.35f, 0.35f,
                        Color.white, Color.white);
                }
            }
        }

        for (int i = 0; i < 15; i++)
        {
            transform.localPosition += new Vector3(mag * Mathf.Cos(rad) / 15, mag * Mathf.Sin(rad) / 15);
            if (pattern == 0 && i % (6 - GameController.rank / 6) == 0 && health > 0)
            {
                for (float j = 0; j < 1 + GameController.rank / 5; j++)
                {
                    ins = GetBullet("PETAL");
                    if (ins != null)
                    {
                        ins.GetComponent<Bullet>().SetIdentityEx("thyatera", transform.localPosition, 1.5f, Random.Range(0f, 360f), 0.4f, 0.4f,
                            Color.white, Color.white);
                    }
                }
            }
            yield return new WaitForSeconds(1 / 120f);
        }
        transform.localPosition = pos;
    }

    public void Elementalize() {
        GameObject ins;
        foreach (GameObject target in GameObject.FindGameObjectsWithTag("Bullet")) {
            if (Mathf.Abs(transform.localPosition.y - target.transform.localPosition.y) < 80) {
                Bullet data = target.GetComponent<Bullet>();
                if ("thyatera".Equals(data.moveCode)) {
                    if (id == 0) {
                        data.ChangeColor(Color.red);
                        data.speed = 7 + rank;
                        data.direction = data.ToPlayerAngle() + Random.Range(-90f, 90f);
                        if (rank >= 3) {
                            ins = GetBullet("THORN");
                            if (ins != null) {
                                ins.GetComponent<Bullet>().SetIdentity(target.transform.localPosition, 9 + rank, data.direction + 30f, 0.3f, 0.3f,
                                    Color.white, Color.red);
                            }
                            ins = GetBullet("THORN");
                            if (ins != null)
                            {
                                ins.GetComponent<Bullet>().SetIdentity(target.transform.localPosition, 9 + rank, data.direction - 30f, 0.3f, 0.3f,
                                    Color.white, Color.red);
                            }
                        }
                    }
                    if (id == 1)
                    {
                        data.ChangeColor(Color.blue);
                        data.speed = 12 + rank * 2;
                        data.direction = data.ToPlayerAngle() + Random.Range(-5f, 5f);
                        if (rank >= 3)
                        {
                            ins = GetBullet("THORN");
                            if (ins != null)
                            {
                                ins.GetComponent<Bullet>().SetIdentity(target.transform.localPosition, 10 + rank * 2, data.direction + 30f, 0.3f, 0.3f,
                                    Color.white, Color.blue);
                            }
                            ins = GetBullet("THORN");
                            if (ins != null)
                            {
                                ins.GetComponent<Bullet>().SetIdentity(target.transform.localPosition, 10 + rank * 2, data.direction - 30f, 0.3f, 0.3f,
                                    Color.white, Color.blue);
                            }
                        }
                    }
                    if (id == 2)
                    {
                        data.SetIdentityEx("WIDDY", target.transform.localPosition, 1, data.ToPlayerAngle() + Random.Range(-120f, 120f), target.transform.localScale.x, target.transform.localScale.y,
                            Color.white, Color.green);
                        if (rank >= 3)
                        {
                            ins = GetBullet("THORN");
                            if (ins != null)
                            {
                                ins.GetComponent<Bullet>().SetIdentityEx("WIDDY", target.transform.localPosition, 3, data.direction + 30f, 0.3f, 0.3f,
                                    Color.white, Color.green);
                            }
                            ins = GetBullet("THORN");
                            if (ins != null)
                            {
                                ins.GetComponent<Bullet>().SetIdentityEx("WIDDY", target.transform.localPosition, 3, data.direction - 30f, 0.3f, 0.3f,
                                    Color.white, Color.green);
                            }
                        }
                    }
                    if (id == 3)
                    {
                        data.ChangeColor(BulletCode.COLOR_DARKBROWN);
                        data.speed = 4 + rank;
                        target.transform.localScale *= (0.4f * (rank + 3));
                        if (rank >= 3)
                        {
                            ins = GetBullet("THORN");
                            if (ins != null)
                            {
                                ins.GetComponent<Bullet>().SetIdentity(target.transform.localPosition, 3, data.direction + 30f, 0.7f, 0.7f,
                                    Color.white, BulletCode.COLOR_DARKBROWN);
                            }
                            ins = GetBullet("THORN");
                            if (ins != null)
                            {
                                ins.GetComponent<Bullet>().SetIdentity(target.transform.localPosition, 3, data.direction - 30f, 0.7f, 0.7f,
                                    Color.white, BulletCode.COLOR_DARKBROWN);
                            }
                        }
                    }
                }
            }          
        }
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
