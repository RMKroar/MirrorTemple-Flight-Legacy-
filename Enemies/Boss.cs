using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : Enemy {
    public Sprite MoveImage;
    public Sprite CastImage;

    public float moveFrequency;
    public float moveSpeed;
    public float moveFriction;
    
    // Pattern Index: Start from 0
    public int maxPattern = 0;
    public int pattern;
    public float maxHealth;

    public bool isSemi = false;
    public Sprite back1;
    public Sprite back2;
    public GameObject SemiTriggerObject;

    void Start()
    {
        immortal = true;
        pattern = maxPattern;
        health += GameController.rank * 5;

        float[] healthDelta = new float[] { 1f, 1.1f, 1.3f, 1.6f, 2f, 2.5f, 2.5f};
        if(GameController.stageNum >= 0) health *= healthDelta[GameController.stageNum];
        maxHealth = health;

        ParticleController Par = GameObject.Find("ParticleController").GetComponent<ParticleController>();
        GameObject ins = Instantiate(Par.ParticleAscend, GameObject.Find("ParticlePanel").transform);
        ins.transform.localPosition = transform.localPosition;
        ins.GetComponent<Image>().color = Color.white;

        StartCoroutine("Animate");
        StartCoroutine("StartLaunch");
        StartCoroutine("Move");
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Shot" && !immortal)
        {
            health -= collision.GetComponent<Shot>().damage;
            if ("KUNAI".Equals(collision.gameObject.GetComponent<Shot>().moveCode))
            {
                GameObject tmp = GameController.ShotPool.GetComponent<ShotPool>().GetChildMin("SUPPORT");
                if (tmp != null) tmp.GetComponent<Shot>().SetIdentity(transform.localPosition, 30f, Random.Range(175f, 185f), 1f, 1f, 0);
            }
            if (!collision.GetComponent<Shot>().moveCode.Contains("#")) collision.GetComponent<Shot>().Unable();

            CheckHealth();
        }
    }

    public new void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Shot" && !immortal && collision.GetComponent<Shot>().moveCode.Contains("#")) {
            health -= collision.GetComponent<Shot>().damage;
            CheckHealth();
        }
    }

    // override
    public override void CheckHealth() {
        Image img = transform.GetChild(pattern).gameObject.GetComponent<Image>();
        img.fillAmount = health / maxHealth;

        if (health <= 0)
        {
            pattern -= 1;
            immortal = true;
            if (pattern < 0)
            {
                if (!isSemi) GameObject.Find("InventoryPanel").GetComponent<ItemController>().Reward();
                else
                {
                    GameObject stage = null;
                    foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Stage"))
                    {
                        stage = obj;
                        break;
                    }
                    if (stage != null)
                    {
                        if (SemiTriggerObject != null) stage.GetComponent<Stage>().InvokeChangeBackground(back1, back2, SemiTriggerObject);
                        else stage.GetComponent<Stage>().InvokeChangeBackground(back1, back2);
                    }
                }

                if (!name.Contains("Pasen"))
                {
                    foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
                    {
                        if (obj != gameObject) Destroy(obj);
                    }
                }
                ParticleController Par = GameObject.Find("ParticleController").GetComponent<ParticleController>();
                GameObject ins = Instantiate(Par.ParticleAscend, GameObject.Find("ParticlePanel").transform);
                ins.transform.localPosition = transform.localPosition;
                ins.GetComponent<Image>().color = Color.black;
                Destroy(gameObject);
            }
            else {
                ParticleController Par = GameObject.Find("ParticleController").GetComponent<ParticleController>();
                GameObject ins = Instantiate(Par.ParticleAscend, GameObject.Find("ParticlePanel").transform);
                ins.transform.localPosition = transform.localPosition;
                ins.GetComponent<Image>().color = Color.white;
            }

            health = maxHealth;

            GameController.EraseBullet();
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("SpecialBullet")) {
                if (obj.name.Contains("Revilla")) obj.GetComponent<RevillaSword>().InvokeUnable();
                if (obj.name.Contains("Hex")) obj.GetComponent<Hexagram>().InvokeUnable();
                if (obj.name.Contains("Falcifer") || obj.name.Contains("Rekirekon") || obj.name.Contains("Cretapere")
                    || obj.name.Contains("Nicheraria")) obj.GetComponent<LaodiceaSword>().InvokeUnable();
                if (obj.name.Contains("Limit")) Destroy(obj);
                if (obj.name.Contains("Gold")) obj.GetComponent<GoldCoin>().InvokeUnable();
            }
        }
    }
}
