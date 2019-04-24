using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour {

    public float speed;
    public float direction;
    public string bulletCode;
    public string moveCode = null;

    public GameObject BulletAddon;
    GameObject player;

	// Use this for initialization
	void Awake () {
        BulletAddon = transform.GetChild(0).gameObject;
	}

    IEnumerator Move() {
        float radDir = direction * Mathf.PI / 180f;
        transform.localPosition += new Vector3(speed * Mathf.Cos(radDir), speed * Mathf.Sin(radDir), 0f);
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, direction));

        yield return new WaitForSeconds(1 / 60f);
        StartCoroutine("Move");
    }

    IEnumerator SpecialMove() {
        GameObject ins;
        AudioSource sct = GameObject.Find("Sound_EnemyAttack").GetComponent<AudioSource>();
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();
        AudioSource pct = GameObject.Find("Sound_EnemyPetal").GetComponent<AudioSource>();

        Image myImage = GetComponent<Image>();
        Image addonImage = BulletAddon.GetComponent<Image>();
        switch (moveCode) {
            case "DEBRIS":
                myImage.color -= new Color(0f, 0f, 0f, 1 / 5f);
                addonImage.color -= new Color(0f, 0f, 0f, 1 / 5f);

                if (myImage.color.a <= 0f) Unable();
                else
                {
                    yield return new WaitForSeconds(1 / 60f);
                    StartCoroutine("SpecialMove");
                }
                break;
            case "CODE_SPLIT":
                yield return new WaitForSeconds(1f);
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 6f, direction + 60f,
                    0.4f, 0.4f, Color.white, Color.magenta);
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 6f, direction - 60f,
                    0.4f, 0.4f, Color.white, Color.magenta);
                Unable();
                break;
            case "CODE_SEED":
                while (true) {
                    if (Vector2.Distance(transform.localPosition, player.transform.localPosition) <= 200)
                    {
                        Color temp_color = (int)Random.Range(0, 2) == 1 ? Color.red : Color.magenta;
                        for (float i = Random.Range(0f, 72f); i < 360f; i += 72)
                        {
                            ins = GetBullet("PETAL");
                            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CODE_FRICTION", transform.localPosition, 7f, i, 0.4f, 0.4f,
                                Color.white, temp_color);
                        }
                        UnableWithDebris();
                    }
                    yield return new WaitForSeconds(1 / 60f);
                }
            case "CODE_FRICTION":
                speed -= 0.1f;
                if (speed > 0)
                {
                    yield return new WaitForSeconds(1 / 60f);
                    StartCoroutine("SpecialMove");
                }
                else
                {
                    ChangeColor(BulletCode.COLOR_TEAL);
                    if (GameController.rank <= 4)
                    {
                        speed = 0;
                        yield return new WaitForSeconds(2f);
                        UnableWithDebris();
                    }
                    else
                    {
                        speed = 2f + (GameController.rank - 5);
                        direction += 180;
                        yield return new WaitForSeconds(2f);
                        UnableWithDebris();
                    }
                }
                break;
            case "CODE_HORNET":
                yield return new WaitForSeconds(0.5f);
                pct.volume = 0.2f; pct.Play();
                for (int i = 0; i <= GameController.rank; i++) {
                    ins = GetBullet("THORN");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 8 + 1.5f * i, direction,
                        transform.localScale.x, transform.localScale.y, myImage.color, Color.black);
                }
                Unable();
                break;
            case "CODE_ARIDUM1":
                yield return new WaitForSeconds(1 / 5f);
                cct.Play();
                speed += 1f;
                if (transform.localScale.x >= 0.4f) transform.localScale -= new Vector3(0.06f, 0.06f);
                for (int i = 0; i <= GameController.rank; i++) {
                    ins = GetBullet("CIRCLE");
                    float randDir = Random.Range(0f, 360f);
                    float randScale = Random.Range(0.2f, 0.6f);
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition + new Vector3(25f * transform.localScale.x * Mathf.Cos(randDir * Mathf.PI / 180f), 25f * transform.localScale.y * Mathf.Sin(randDir * Mathf.PI / 180f)),
                         Random.Range(2f, 5f + 0.6f * GameController.rank), randDir, randScale, randScale, BulletCode.COLOR_BROWN, BulletCode.COLOR_DARKBROWN);
                }
                StartCoroutine("SpecialMove");
                break;
            case "CODE_ARIDUM2":
                yield return new WaitForSeconds(1 / 3f);
                cct.Play();
                speed += 1.5f;
                if (transform.localScale.x >= 0.4f) transform.localScale -= new Vector3(0.06f, 0.06f);
                for (int i = 0; i <= GameController.rank; i++)
                {
                    ins = GetBullet("CIRCLE");
                    float randDir = Random.Range(0f, 360f);
                    float randScale = Random.Range(0.15f, 0.4f);
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition + new Vector3(25f * transform.localScale.x * Mathf.Cos(randDir * Mathf.PI / 180f), 25f * transform.localScale.y * Mathf.Sin(randDir * Mathf.PI / 180f)),
                         Random.Range(2f, 4f + 0.6f * GameController.rank), randDir, randScale, randScale, BulletCode.COLOR_BROWN, BulletCode.COLOR_DARKBROWN);
                }
                StartCoroutine("SpecialMove");
                break;
            case "CODE_ARIDUM3":
                yield return new WaitForSeconds(1 / 60f);
                cct.Play();
                if (speed > 0) speed -= 0.3f;
                else speed = 0;

                if (transform.localScale.x <= 4f + 0.6f * GameController.rank)
                {
                    transform.localScale += new Vector3(0.04f + 0.01f * GameController.rank, 0.04f + 0.01f * GameController.rank);
                }
                else UnableWithDebris();
                for (int i = 0; i <= GameController.rank; i++)
                {
                    ins = GetBullet("CIRCLE");
                    float randDir = Random.Range(0f, 360f);
                    float randScale = Random.Range(0.15f, 0.4f);
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition + new Vector3(50f * transform.localScale.x * Mathf.Cos(randDir * Mathf.PI / 180f), 50f * transform.localScale.y * Mathf.Sin(randDir * Mathf.PI / 180f)),
                         Random.Range(2f, 4f + 0.6f * GameController.rank), randDir, randScale, randScale, Color.yellow, Color.red);
                }
                StartCoroutine("SpecialMove");
                break;
            case "CODE_CACTUS":
                if (speed > 0)
                {
                    speed -= 1 / 10f;
                    if (speed <= 0)
                    {
                        speed = 0;
                        float randSp = Random.Range(0f, 15f);
                        for (float i = randSp; i <= 360; i += 30)
                        {
                            ins = GetBullet("CIRCLE");
                            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CODE_CACTUS", transform.localPosition + new Vector3(80 * Mathf.Cos(i * Mathf.PI / 180f), 80 * Mathf.Sin(i * Mathf.PI / 180f)),
                                0f, i, 0.6f, 0.6f, Color.green, BulletCode.COLOR_TEAL);
                        }
                        for (float i = randSp + 15; i <= 360; i += 30)
                        {
                            ins = GetBullet("CIRCLE");
                            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CODE_CACTUS", transform.localPosition + new Vector3(120 * Mathf.Cos(i * Mathf.PI / 180f), 120 * Mathf.Sin(i * Mathf.PI / 180f)),
                                0f, i, 0.4f, 0.4f, Color.green, BulletCode.COLOR_TEAL);
                        }
                        yield return new WaitForSeconds(1f);
                        UnableWithDebris();
                    }
                    else
                    {
                        yield return new WaitForSeconds(1 / 60f);
                        StartCoroutine("SpecialMove");
                    }
                }
                else {
                    yield return new WaitForSeconds(1f);
                    cct.Play();
                    ins = GetBullet("THORN");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 4 + GameController.rank, direction, 0.3f, 0.2f, Color.green, BulletCode.COLOR_TEAL);
                    ins = GetBullet("THORN");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 3 + GameController.rank, direction, 0.3f, 0.2f, Color.green, BulletCode.COLOR_TEAL);
                    UnableWithDebris();
                }
                break;
            case "CODE_MIRAGE":
                if (Vector2.Distance(transform.localPosition, player.transform.localPosition) <= 210) {
                    ins = GetBullet("CIRCLE");
                    if (GameController.rank <= 5)
                    {
                        if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition + new Vector3(Random.Range(-40f, 40f), Random.Range(-40f, 40f)), speed, direction, transform.localScale.x,
                            transform.localScale.y, myImage.color, addonImage.color);
                    }
                    else {
                        if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition + new Vector3(Random.Range(-50f, 50f), Random.Range(-50f, 50f)), speed + Random.Range(-2f, 3f),
                             direction + Random.Range(-5f, 5f), transform.localScale.x, transform.localScale.y, myImage.color, addonImage.color);
                    }
                    UnableWithDebris();
                }
                yield return new WaitForSeconds(1 / 60f);
                StartCoroutine("SpecialMove");
                break;
            case "CODE_TRIA1":
                speed += (0.2f + 0.02f * GameController.rank);
                if (speed >= 10 + 1.2f * GameController.rank) {
                    ins = GetBullet("CIRCLE");
                    if (Random.Range(0f, 2f) <= 1)
                    {
                        ins.GetComponent<Bullet>().SetIdentityEx("CODE_TRIA_L", transform.localPosition, 5 + GameController.rank * 0.5f, direction, transform.localScale.x, transform.localScale.y,
                        Color.white, Color.blue);
                    }
                    else {
                        ins.GetComponent<Bullet>().SetIdentityEx("CODE_TRIA_R", transform.localPosition, 5 + GameController.rank * 0.5f, direction, transform.localScale.x, transform.localScale.y,
                        Color.white, Color.blue);
                    }
                    UnableWithDebris();
                }
                yield return new WaitForSeconds(1 / 60f);
                StartCoroutine("SpecialMove");
                break;
            case "CODE_TRIA_L":
                direction += (0.3f + GameController.rank * 0.03f);
                yield return new WaitForSeconds(1 / 45f);
                StartCoroutine("SpecialMove");
                break;
            case "CODE_TRIA_R":
                direction -= (0.3f + GameController.rank * 0.03f);
                yield return new WaitForSeconds(1 / 45f);
                StartCoroutine("SpecialMove");
                break;
            case "CODE_TRIA2":
                yield return new WaitForSeconds(0.2f - 0.01f * GameController.rank);
                ins = GetBullet("CIRCLE");
                ins.GetComponent<Bullet>().SetIdentityEx("CODE_TRIA2", transform.localPosition, speed + 2f, direction + Random.Range(-3f, 3f), transform.localScale.x,
                    transform.localScale.y, BulletCode.COLOR_TEAL, BulletCode.COLOR_TEAL);
                break;
            case "CODE_TRIA3":
                speed += (0.2f + 0.02f * GameController.rank);
                if (speed >= 10 + 0.8f * GameController.rank)
                {
                    ins = GetBullet("CIRCLE");
                    ins.GetComponent<Bullet>().SetIdentityEx("CODE_TRIA3", transform.localPosition, 1, direction + Random.Range(-10 - 1f * GameController.rank, 10f + 1f * GameController.rank), transform.localScale.x, transform.localScale.y,
                        Color.white, Color.blue);
                    UnableWithDebris();
                }
                yield return new WaitForSeconds(1 / 60f);
                StartCoroutine("SpecialMove");
                break;
            case "CODE_TRIA4":
                speed += (0.2f + 0.02f * GameController.rank);
                yield return new WaitForSeconds(1 / 60f);
                StartCoroutine("SpecialMove");
                break;
            case "CODE_TRIA5":
                for (; myImage.color.a > 0;) {
                    myImage.color -= new Color(0, 0, 0, 0.1f);
                    addonImage.color -= new Color(0, 0, 0, 0.1f);

                    yield return new WaitForSeconds(1 / 10f);
                }
                for (; ; )
                {
                    if (Vector3.Distance(GetPlayer().transform.localPosition, transform.localPosition) < 350f - 15f * GameController.rank)
                    {
                        if (myImage.color.a < 1)
                        {
                            myImage.color += new Color(0, 0, 0, 0.2f);
                            addonImage.color += new Color(0, 0, 0, 0.2f);
                            if (GameController.rank < 5) speed -= 0.2f;
                            else speed -= 0.7f + 0.05f * (GameController.rank - 4);
                        }
                    }
                    yield return new WaitForSeconds(1 / 10f);
                    if (myImage.color.a >= 1) break;
                }
                break;
            case "CANNON":
                for (float del = 0; del <= 40; del += 0.7f) {
                    transform.localPosition -= new Vector3(0f, del);
                    yield return new WaitForSeconds(1 / 60f);
                }
                sct.Play();
                if (GameController.rank <= 2)
                {
                    for (float i = Random.Range(0f, 60f); i < 360; i += 60)
                    {
                        ins = GetBullet("CIRCLE");
                        ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 8, i, 0.45f, 0.45f, Color.yellow, Color.red);
                    }
                }
                else if (GameController.rank <= 4) {
                    for (float i = Random.Range(0f, 30f); i < 360; i += 30)
                    {
                        ins = GetBullet("CIRCLE");
                        ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 8, i, 0.4f, 0.4f, Color.yellow, Color.red);
                    }
                }
                else if (GameController.rank <= 6)
                {
                    for (float i = Random.Range(0f, 30f); i < 360; i += 30)
                    {
                        ins = GetBullet("CIRCLE");
                        ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 6, i, 0.45f, 0.45f, Color.yellow, Color.red);
                        ins = GetBullet("CIRCLE");
                        ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 10, i + 15, 0.4f, 0.4f, Color.yellow, Color.red);
                    }
                }
                else if (GameController.rank <= 8)
                {
                    for (float i = Random.Range(0f, 30f); i < 360; i += 30)
                    {
                        ins = GetBullet("CIRCLE");
                        ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 6, i, 0.45f, 0.45f, Color.yellow, Color.red);
                        ins = GetBullet("CIRCLE");
                        ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 10, i + 15, 0.4f, 0.4f, Color.yellow, Color.red);
                    }
                }
                else
                {
                    for (float i = Random.Range(0f, 30f); i < 360; i += 30)
                    {
                        ins = GetBullet("CIRCLE");
                        ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 4, i + 15, 0.3f, 0.3f, Color.yellow, Color.red);
                        ins = GetBullet("CIRCLE");
                        ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 4, i, 0.3f, 0.3f, Color.yellow, Color.red);
                        ins = GetBullet("CIRCLE");
                        ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 8, i, 0.4f, 0.4f, Color.yellow, Color.red);
                        ins = GetBullet("CIRCLE");
                        ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 12, i + 15, 0.45f, 0.45f, Color.yellow, Color.red);
                    }
                }
                UnableWithDebris();
                break;
            case "CANNON_GENERAL":
                for (int i = 0; i <= 60; i++)
                {
                    speed = (speed > 0) ? speed - 0.2f : 0;
                    yield return new WaitForSeconds(1 / 60f);
                }
                sct.Play();
                if (GameController.rank <= 2)
                {
                    for (float i = Random.Range(0f, 60f); i < 360; i += 60)
                    {
                        ins = GetBullet("CIRCLE");
                        ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 8, i, 0.45f, 0.45f, Color.yellow, Color.red);
                    }
                }
                else if (GameController.rank <= 4)
                {
                    for (float i = Random.Range(0f, 30f); i < 360; i += 30)
                    {
                        ins = GetBullet("CIRCLE");
                        ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 8, i, 0.4f, 0.4f, Color.yellow, Color.red);
                    }
                }
                else if (GameController.rank <= 6)
                {
                    for (float i = Random.Range(0f, 30f); i < 360; i += 30)
                    {
                        ins = GetBullet("CIRCLE");
                        ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 6, i, 0.45f, 0.45f, Color.yellow, Color.red);
                        ins = GetBullet("CIRCLE");
                        ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 10, i + 15, 0.4f, 0.4f, Color.yellow, Color.red);
                    }
                }
                else if (GameController.rank <= 8)
                {
                    for (float i = Random.Range(0f, 30f); i < 360; i += 30)
                    {
                        ins = GetBullet("CIRCLE");
                        ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 6, i, 0.45f, 0.45f, Color.yellow, Color.red);
                        ins = GetBullet("CIRCLE");
                        ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 10, i + 15, 0.4f, 0.4f, Color.yellow, Color.red);
                    }
                }
                else
                {
                    for (float i = Random.Range(0f, 30f); i < 360; i += 30)
                    {
                        ins = GetBullet("CIRCLE");
                        ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 4, i + 15, 0.3f, 0.3f, Color.yellow, Color.red);
                        ins = GetBullet("CIRCLE");
                        ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 4, i, 0.3f, 0.3f, Color.yellow, Color.red);
                        ins = GetBullet("CIRCLE");
                        ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 8, i, 0.4f, 0.4f, Color.yellow, Color.red);
                        ins = GetBullet("CIRCLE");
                        ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 12, i + 15, 0.45f, 0.45f, Color.yellow, Color.red);
                    }
                }
                UnableWithDebris();
                break;
            case "REV":
                if (speed < 10) speed += 0.2f;
                yield return new WaitForSeconds(1 / 30f);
                StartCoroutine("SpecialMove");
                break;
            case "ILLUSION":
                if (Vector3.Distance(GetPlayer().transform.localPosition, transform.localPosition) < 250f - 10f * GameController.rank) {
                    UnableWithDebris();
                }
                yield return new WaitForSeconds(1 / 30f);
                StartCoroutine("SpecialMove");
                break;
            case "ALCADROLAZZO":
                for (; transform.localScale.x < 4 * (GameController.rank + 1); transform.localScale += new Vector3((GameController.rank + 1) / 6f, 0)) {
                    yield return new WaitForSeconds(1 / 30f);
                }
                for (; transform.localScale.x > 1; transform.localScale -= new Vector3((GameController.rank + 1) / 4f, 0))
                {
                    yield return new WaitForSeconds(1 / 24f);
                }
                break;
            case "ALCADROCHASE":
                direction += ((ToPlayerAngle() + 360) % 360 > direction) ? 0.4f * (GameController.rank + 1) : -0.4f * (GameController.rank + 1);
                yield return new WaitForSeconds(1 / 30f);
                StartCoroutine("SpecialMove");
                break;
            case "ALCADROLAZZOSHORT":
                for (; transform.localScale.x < 0.8f + 0.2f * (GameController.rank + 1); transform.localScale += new Vector3((GameController.rank + 1) / 30f, 0))
                {
                    yield return new WaitForSeconds(1 / 60f);
                }
                yield return new WaitForSeconds(0.5f);
                break;
            case "SNOWFLOWER":
                yield return new WaitForSeconds(1.5f);
                speed = Random.Range(5 + GameController.rank * 0.3f, 10 + GameController.rank * 0.7f);
                break;
            case "SNOWFLOWER2":
                yield return new WaitForSeconds(1.5f);
                speed = Random.Range(1f, 1.5f + GameController.rank * 0.4f);
                break;
            case "SNOWSOUL":
                for (float del = 0; ; del += 0.7f)
                {
                    transform.localPosition -= new Vector3(0f, del);
                    if (Vector2.Distance(transform.localPosition, player.transform.localPosition) <= 320) break;
                    else yield return new WaitForSeconds(1 / 60f);
                }
                if (Vector2.Distance(transform.localPosition, player.transform.localPosition) <= 320)
                {
                    ins = GetBullet("PETAL");
                    if (GameController.rank <= 6)
                    {
                        if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 3f + GameController.rank * 0.5f, ToPlayerAngle(), 0.4f,
                            0.4f, Color.white, Color.blue);
                    }
                    else
                    {
                        if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 2f + (GameController.rank - 6) * 1.2f, ToPlayerAngle(), 0.4f,
                            0.4f, Color.white, Color.blue);
                        ins = GetBullet("PETAL");
                        if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 2f + (GameController.rank - 6) * 1.2f, ToPlayerAngle() + 60f, 0.4f,
                            0.4f, Color.white, Color.blue);
                        ins = GetBullet("PETAL");
                        if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 2f + (GameController.rank - 6) * 1.2f, ToPlayerAngle() - 60f, 0.4f,
                            0.4f, Color.white, Color.blue);
                    }
                    UnableWithDebris();
                }
                yield return new WaitForSeconds(1 / 60f);
                break;
            case "SNOWPETAL":
                float sc = 80 + GameController.rank * 10;
                float randRad = 0;
                while (true) {
                    randRad = Random.Range(0f, Mathf.PI * 2);
                    ins = GetBullet("CIRCLE");
                    ins.GetComponent<Bullet>().SetIdentity(transform.localPosition + new Vector3(Random.Range(0f, sc) * Mathf.Cos(randRad), Random.Range(0f, sc) * Mathf.Sin(randRad)), 
                        speed, direction, 0.35f, 0.35f, Color.white, new Color(0f, Random.Range(0f, 1f), 1f));

                    yield return new WaitForSeconds(0.2f - GameController.rank * 0.016f);
                }            
            case "LAVABALL":
                while (true) {
                    speed -= 0.4f;
                    ins = GetBullet("CIRCLE");
                    if (ins != null)
                    {
                        ins.GetComponent<Bullet>().SetIdentity(transform.localPosition + new Vector3(Random.Range(-30f, 30f), Random.Range(-30f, 30f)), Random.Range(4f, 5f + GameController.rank),
                            direction + Random.Range(-40f, 40f), 0.3f, 0.3f, Color.white, Color.red);
                    }
                    yield return new WaitForSeconds(1 / 60f);

                    if (speed <= 0) UnableWithDebris();
                }
            case "BIGGINGLAVA":
                for (; transform.localScale.x <= 1f + GameController.rank * 0.4f; transform.localScale += new Vector3(1 / 60f + GameController.rank * (1 / 120f), 1 / 60f + GameController.rank * (1 / 120f))) {
                    yield return new WaitForSeconds(1 / 60f);
                }
                break;
            case "MAGMA":
                for (float del = 0; ; del += 0.4f)
                {
                    int deltaDel = (direction > 180) ? 1 : -1;

                    transform.localPosition += new Vector3(0f, del * deltaDel);
                    yield return new WaitForSeconds(1 / 60f);
                }
            case "MANEATERDRAKE":
                ins = GetBullet("CIRCLE");
                if (ins != null) {
                    ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, Random.Range(4f, 7f), ToPlayerAngle() + Random.Range(-30f, 30f),
                        0.25f, 0.25f, Color.white, Color.yellow);
                }
                yield return new WaitForSeconds(0.7f - 0.06f * GameController.rank);
                if(transform.localPosition.x >= 0) StartCoroutine(SpecialMove());
                break;
            case "BLOB":
                yield return new WaitForSeconds(1f);
                speed = Random.Range(3f + GameController.rank * 0.3f, 11f + GameController.rank * 0.5f);
                direction = ToPlayerAngle() + Random.Range(-30f, 30f);
                myImage.color = BulletCode.COLOR_SKY;
                addonImage.color = Color.blue;
                break;
            case "MOTHFLY":
                yield return new WaitForSeconds(1f);
                float crazyScale = 1 + GameController.rank * 0.3f;
                float crazyDir = 0;
                speed = Random.Range(5f, 9f);
                direction = ToPlayerAngle() + Random.Range(-50f, 50f);
                myImage.color = Color.magenta;
                addonImage.color = BulletCode.COLOR_VIOLET;
                while (true) {
                    transform.localPosition += new Vector3(crazyScale * Mathf.Cos(crazyDir * Mathf.PI / 180f), crazyScale * Mathf.Sin(crazyDir * Mathf.PI / 180f));
                    crazyDir += 10;
                    yield return new WaitForSeconds(1 / 30f);
                }
            case "ORACLE_1":
                float o1dir = Random.Range(0f, 2f) * Mathf.PI;
                float o1Speed = 1f + GameController.rank * 0.2f;
                while (true)
                {
                    speed += o1Speed * Mathf.Sin(o1dir);
                    o1dir += 0.1f + 0.02f * GameController.rank;
                    yield return new WaitForSeconds(1 / 30f);
                }
            case "ORACLE_2":
                float o2Scale = 1 + GameController.rank * 0.3f;
                float o2Dir = Random.Range(0f, 360f);
                while (true)
                {
                    transform.localPosition += new Vector3(o2Scale * Mathf.Cos(o2Dir * Mathf.PI / 180f), o2Scale * Mathf.Sin(o2Dir * Mathf.PI / 180f));
                    o2Dir += 10;
                    yield return new WaitForSeconds(1 / 30f);
                }
            case "EPHESUSLAZZO":
                for (; transform.localScale.x < 30; transform.localScale += new Vector3(1 / 3f + 1 / 15f * GameController.rank, 0f))
                {
                    yield return new WaitForSeconds(1 / 30f);
                }
                break;
            case "DOLL":
                for (; speed > 0; speed -= 0.5f) {
                    yield return new WaitForSeconds(1 / 30f);
                }
                direction = ToPlayerAngle();
                speed = 4f + GameController.rank;
                break;
            case "WIDDY":
                while (true) {
                    speed += 0.3f;
                    yield return new WaitForSeconds(1 / 30f);
                }
            case "GNOME":
                float placeYmag = (Random.Range(320f, -320f) - transform.localPosition.y) / 15f;
                for (int i = 0; i < 15; i++) {
                    transform.localPosition += Vector3.up * placeYmag;
                    yield return new WaitForSeconds(1 / 30f);
                }
                direction = 180f;
                speed = 3 + 0.2f * GameController.rank;
                break;
            case "DOLL2":
                for (; speed > 0; speed -= 0.9f) {
                    yield return new WaitForSeconds(1 / 30f);
                }
                for (float j = Random.Range(40f, 50f); j > -45f; j -= 120f / (3 + GameController.rank / 2))
                {
                    cct.Play();
                    ins = GetBullet("CIRCLE");
                    if (ins != null)
                    {
                        ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 7 + GameController.rank * 0.5f, ToPlayerAngle() + j, 0.3f, 0.3f, Color.white, Color.black);
                    }
                }
                UnableWithDebris();
                break;
            case "TAR":
                while (true) {
                    yield return new WaitForSeconds(1 / 60f);

                    if (Vector2.Distance(transform.localPosition, player.transform.localPosition) <= 210)
                    {
                        speed = 2 + GameController.rank * 0.3f;
                        break;
                    }
                }
                break;
            case "AVARI":
                while (true)
                {
                    speed += 0.2f;
                    yield return new WaitForSeconds(1 / 30f);
                }
            case "VOID":
                yield return new WaitForSeconds(1f);
                speed = 12f + 0.4f * GameController.rank;
                float randAngle = Random.Range(0f, 360f);
                while (true) {
                    for (float i = 0; i < 360; i += 360 / (2 + GameController.rank / 3)) {
                        ins = GetBullet("CIRCLE");
                        if (ins != null) {
                            ins.GetComponent<Bullet>().SetIdentityEx("WIDDY", transform.localPosition, 1f,
                                randAngle + i, 0.3f, 0.3f, Color.white, Color.white);
                        }
                    }              
                    yield return new WaitForSeconds(0.5f - 0.025f * GameController.rank);
                }
            case "INFINITE":
                yield return new WaitForSeconds(1f);
                speed = 3f;
                while (true)
                {
                    speed += 0.3f;
                    yield return new WaitForSeconds(1 / 30f);
                }
            case "UNIVERN_0":
                while (true)
                {
                    speed += 0.8f;
                    yield return new WaitForSeconds(1 / 30f);
                }
            default:
                break;
        }
    }

    // Direction uses degree (not radian)
    public void SetIdentity(Vector3 pos, float speed, float direction, float scaleX, float scaleY, Color myColor, Color addonColor)
    {
        gameObject.SetActive(true);

        Image myImage = GetComponent<Image>();
        Image addonImage = BulletAddon.GetComponent<Image>();
        myImage.color = myColor;
        addonImage.color = addonColor;
        this.speed = speed;
        this.direction = direction;

        transform.localPosition = pos;
        transform.localScale = new Vector3(scaleX, scaleY, 1f);

        player = GetPlayer();
        if (speed != 0) {
            StartCoroutine("Move");
        }
        if (moveCode != null) {
            StartCoroutine("SpecialMove");
        }              
        
    }

    public void SetIdentityEx(string code, Vector3 pos, float speed, float direction, float scaleX, float scaleY, Color myColor, Color addonColor) {
        moveCode = code;
        SetIdentity(pos, speed, direction, scaleX, scaleY, myColor, addonColor);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Main")
        {
            if (moveCode != null) {
                if (moveCode == "UNDINE" && transform.localPosition.x >= -250f)
                {
                    GameObject ins = GetBullet("CIRCLE");
                    ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 4, ToPlayerAngle() + Random.Range(-30f, 30f),
                        transform.localScale.x, transform.localScale.y, Color.white, BulletCode.COLOR_EMERALD);
                }
                else if (moveCode == "SALAMANDER" && Mathf.Abs(GetPlayer().transform.localPosition.x - transform.localPosition.x) <= 400)
                {
                    GameObject ins = GetBullet("CIRCLE");
                    float dir = (transform.localPosition.y >= 0) ? 270 : 90;
                    ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 3.5f, dir,
                        transform.localScale.x, transform.localScale.y, Color.white, Color.yellow);
                }
                else if (moveCode == "CODE_TRIA4" && GameController.rank >= 5 && transform.localPosition.y <= -250f)
                {
                    GameObject ins = GetBullet("CIRCLE");
                    float sc = Random.Range(0.2f, 0.3f);
                    ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, Random.Range(2f, 4f), direction + Random.Range(160f, 200f),
                        sc, sc, Color.white, BulletCode.COLOR_SKY);
                }
                else if (moveCode == "CRYSTAL") {
                    GameObject ins = GetBullet("CIRCLE");
                    float dir = direction;
                    Image myImage = GetComponent<Image>();
                    Image addonImage = BulletAddon.GetComponent<Image>();

                    if (Mathf.Abs(transform.localPosition.x) > 660) {
                        dir = 180 - dir;
                    }
                    if (Mathf.Abs(transform.localPosition.y) > 365) {
                        dir = -dir;
                    }

                    ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, speed, dir,
                        transform.localScale.x, transform.localScale.y, addonImage.color, myImage.color);
                }
            }

            if(moveCode != "NOT_UNABLE") Unable();
        }
    }

    private void Update()
    {
        if (Mathf.Abs(transform.localPosition.x) > 1000f || Mathf.Abs(transform.localPosition.y) > 1000f) {
            moveCode = null;
            gameObject.SetActive(false);
        }
    }

    public GameObject GetBullet(string code)
    {
        return GameController.BulletPool.GetComponent<BulletPool>().GetChildMin(code);
    }

    public void Unable() {
        moveCode = null;
        gameObject.SetActive(false);
    }

    public void UnableWithDebris()
    {
        GameObject ins;
        Image myImage = GetComponent<Image>();
        Image addonImage = BulletAddon.GetComponent<Image>();
        moveCode = null;
        ins = GetBullet(name.Split('_')[1].Split('(')[0].ToUpper());
        if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("DEBRIS", transform.localPosition, 0.001f, direction, transform.localScale.x, transform.localScale.y, myImage.color, addonImage.color);
        gameObject.SetActive(false);
    }

    public void ChangeColor(Color col) {
        Image addonImage = BulletAddon.GetComponent<Image>();
        addonImage.color = col;
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
}
