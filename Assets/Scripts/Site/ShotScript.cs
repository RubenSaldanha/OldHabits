using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotScript : MonoBehaviour {

    float startTime;
    float angleDeg;
    float trailLife;

    OldManScript oldMan;

    LineRenderer[] trails;
    float[] trailAlphas;

    public void Initialize(Vector2 position, float angleDeg)
    {
        oldMan = FindObjectOfType<OldManScript>();

        trailLife = 2f;
        this.angleDeg = angleDeg;
        transform.position = position;
        transform.rotation = Quaternion.AngleAxis(angleDeg, Vector3.forward);
    }

	// Use this for initialization
	void Start () {
        startTime = Time.time;
        GameObject smoke = (GameObject)Instantiate(Resources.Load("SmokePrefab"));
        smoke.transform.SetParent(transform, false);


        float range = 20f;
        int bulletCount = oldMan.shotBullets;

        trails = new LineRenderer[bulletCount];
        trailAlphas = new float[bulletCount];
        for(int i=0;i<bulletCount;i++)
        {
            trailAlphas[i] = Random.Range(0.5f, 2f);

            //SPREAD
            float deviation;
            deviation = Random.Range(-oldMan.shotSpreadDeg, oldMan.shotSpreadDeg);
            if (i == 0)
                deviation *= 0.1f;

            float bulletAngle = angleDeg + deviation;
            Vector2 bulletDirection = new Vector2(Mathf.Cos(Mathf.Deg2Rad*bulletAngle), Mathf.Sin(Mathf.Deg2Rad*bulletAngle));

            Vector2 lineEnd = (Vector2)transform.position + bulletDirection*range;
            //Raycast
            RaycastHit2D ray = Physics2D.Raycast(transform.position, bulletDirection, range);
            if (ray.collider != null)
            {
                lineEnd = ray.point;

                //TODO HIT MECHANICS DMG etc
                InsectScript insect = ray.collider.GetComponent<InsectScript>();
                if (insect != null)
                    insect.Hit();
            }

            //Create Bullets
            GameObject bulletPrefab = (GameObject)Instantiate(Resources.Load("BulletPrefab"));
            LineRenderer lr = bulletPrefab.GetComponent<LineRenderer>();
            trails[i] = lr;
            Vector3[] lp = new Vector3[2];
            lp[0] = transform.position;
            lp[1] = lineEnd;
            lr.SetPositions(lp);
            bulletPrefab.transform.SetParent(transform, false);
        }

	}
	
	// Update is called once per frame
	void Update () {
        float elapsed = Time.time - startTime;
        if (elapsed > 10)
            Destroy(gameObject);

        float alphas = 1f - elapsed / trailLife;
        for(int i=0;i<trails.Length;i++)
        {
            trails[i].startColor = new Color(1f, 1f, 1f, (alphas - 0.25f) * trailAlphas[i]);
            trails[i].endColor = new Color(1f, 1f, 1f, alphas * trailAlphas[i]);
        }
	}

    public static ShotScript CreateShot(Vector2 position, float angleDeg)
    {
        GameObject gobj = new GameObject("Shot");
        ShotScript shotScript = gobj.AddComponent<ShotScript>();
        shotScript.Initialize(position, angleDeg);

        return shotScript;
    }
}
