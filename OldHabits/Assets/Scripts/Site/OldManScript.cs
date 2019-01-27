using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldManScript : MonoBehaviour
{

    SpriteRenderer sprite;

    float speed;

    public float health;
    public float fireState;
    public float fireCooldown;

    Vector2 walk;
    Vector2 target;
    float angle;
    bool fire;
    int ammo;
    int maxAmmo;
    public int killCount;
    int shotgunLevel;

    public float shotSpreadDeg;
    public int shotBullets;
    public int healLevel;
    public float healRange;

    UnityEngine.UI.Text magazineText;
    UnityEngine.UI.Text tooltipText;
    GameObject shotgunStation;

    // Use this for initialization
    void Start()
    {
        maxAmmo = 3;
        ammo = maxAmmo;
        health = 1f;
        shotBullets = 1;
        fireCooldown = 1f;
        sprite = gameObject.AddComponent<SpriteRenderer>();
        sprite.sprite = Resources.Load<Sprite>("OldManUpperBody");
        magazineText = GameObject.FindGameObjectWithTag("Ammo").GetComponent<UnityEngine.UI.Text>();
        tooltipText = GameObject.FindGameObjectWithTag("Tooltip").GetComponent<UnityEngine.UI.Text>();
        shotgunStation = GameObject.FindGameObjectWithTag("ShotgunStation");
        healRange = 1;

        shotSpreadDeg = 15;
    }

    // Update is called once per frame
    void Update()
    {
        if(health < 0f)
        {
            health = 0f;
            SiteControllerScript controller = FindObjectOfType<SiteControllerScript>();
            controller.PlayerDead();
        }

        float walkFactor = 0.1f;

        walk *= 0.9f;

        if (Input.GetKey(KeyCode.W))
            walk.y += walkFactor;
        if (Input.GetKey(KeyCode.S))
            walk.y -= walkFactor;
        if (Input.GetKey(KeyCode.A))
            walk.x -= walkFactor;
        if (Input.GetKey(KeyCode.D))
            walk.x += walkFactor;

        if (walk.magnitude > 1f)
            walk.Normalize();

        target = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        angle = Mathf.Atan2(target.y, target.x) * 360f / (2 * Mathf.PI);

        fire = Input.GetMouseButtonDown(0);


        tooltipText.text = "";


        float healthDist = (FindObjectOfType<HealthStationScript>().transform.position - transform.position).magnitude;
        if (healthDist < healLevel)
        {
            ///HEALING
            health += 0.001f;
            tooltipText.text = "-- Healing --";

            if(healthDist < 1f)
            {
                int cost = healLevel * 2;
                tooltipText.text = "Press 'E' to increase health range. (Cost" + cost + " )";

                if (Input.GetKeyDown(KeyCode.E) && killCount >= cost)
                {
                    healLevel++;
                    healRange = 1 + Mathf.Sqrt(healLevel);
                }
            }
        }

        float magazineDist = (FindObjectOfType<MagazineScript>().transform.position - transform.position).magnitude;
        if(magazineDist < 1f)
        {
            //GETTING AMMO
            ammo++;
            if (ammo >= maxAmmo)
                ammo = maxAmmo;

            if (maxAmmo < 40)
            {
                int cost = maxAmmo;
                tooltipText.text = "Press 'E' to increase ammo capacity. (Cost" + cost + " )";

                if (Input.GetKeyDown(KeyCode.E) && killCount >= cost)
                {
                    maxAmmo++;
                    killCount -= cost;
                }
            }
        }

        float shotgunStationDist = (shotgunStation.transform.position - transform.position).magnitude;
        if(shotgunStationDist < 1f)
        {
            //UPGRADING SHOTGUN
            int cost = (shotgunLevel + 1) * 5;
            tooltipText.text = "Press 'E' to upgrade your shotgun. (Cost " + cost + " )";

            if (Input.GetKeyDown(KeyCode.E) && killCount >= cost)
            {
                shotgunLevel++;
                killCount -= cost;
                shotSpreadDeg += 0.1f*(1f - (30 - shotSpreadDeg)/30f);
                shotSpreadDeg = Mathf.Clamp(shotSpreadDeg, 0, 30);
                Debug.Log("Spread at: " + shotSpreadDeg);
                shotBullets = shotgunLevel + 1;

                fireCooldown = 2 / (2 + (shotgunLevel / 4 ));
            }
        }


        //Update UI AMMO
        string ammoString = "";
        for (int i = 0; i < maxAmmo; i++)
            if (i < ammo)
                ammoString += "i";
            else
                ammoString += "'";
        magazineText.text = ammoString;

        GameObject.Find("KillText").GetComponent<UnityEngine.UI.Text>().text = "" + killCount;
    }

    void FixedUpdate()
    {
        speed = 0.075f;

        Vector3 displacement = new Vector3(walk.x, walk.y, 0f) * speed;
        transform.position = transform.position + displacement;

        transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        fireState -= Time.deltaTime;
        if(fireState < 0 && fire && ammo > 0)
        {
            ammo--;
            Fire();
            fireState = fireCooldown;
        }

    }

    public void Damage(float dmg)
    {
        health -= dmg;
    }

    void Fire()
    {
        ShotScript.CreateShot(transform.position + transform.right.normalized*0.2f, angle);
    }
}
