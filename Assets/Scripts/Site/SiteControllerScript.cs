using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiteControllerScript : MonoBehaviour {

    OldManScript oldMan;

    bool gameOver;

	// Use this for initialization
	void Start () {
        oldMan = FindObjectOfType<OldManScript>();

        BuildHome();
	}

    public void BuildHome()
    {
        int widthBlocks = 10;
        int heightBlocks = 10;
        float width = 5f;
        float height = 5f;
        float xScale = width/widthBlocks;
        float yScale = height / heightBlocks;
        for (int i = 0; i < widthBlocks; i++)
            for (int j = 0; j < heightBlocks; j++)
            {
                if (i == 0 || j == 0 || i == widthBlocks - 1 || j == heightBlocks - 1)
                {
                    if(i != widthBlocks/2 && j != heightBlocks/2)
                    {
                        BrickScript brick = BrickScript.CreateBrick(new Vector2(i * xScale, j * yScale) - new Vector2(width / 2f, height / 2f) + new Vector2(xScale/2f,yScale/2f) );
                        brick.transform.localScale = new Vector3(xScale, yScale, 1f);

                    }
                }
            }
    }

	// Update is called once per frame
	void Update () {

        if(gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                UnityEngine.SceneManagement.SceneManager.LoadScene("SiteScene");

            return; //AHAH wacky wacky optimize
        }

        float spawnProbSec = 0.5f * Mathf.Sqrt(Time.timeSinceLevelLoad * 0.2f)*0.4f; //persecond
        float wavePeriod = 20f / Mathf.Sqrt(Time.timeSinceLevelLoad * 0.02f);
        float waveFactor = Mathf.Cos(Time.timeSinceLevelLoad * Mathf.PI * 2 / wavePeriod) + 0.8f;
        spawnProbSec *= waveFactor;

        float truProb = spawnProbSec * (1f / 60f);

        float trySpawn = Random.Range(0f, 1f);

        //Debug.Log("PROB:" + truProb);

        if(trySpawn < truProb)
        {
            float spawnAngle = Random.Range(0, Mathf.PI * 2);
            float spawnRange = Random.Range(9f, 9f);

            float xPos = -1000f;
            float yPos = -1000f;

            do
            {
                xPos = Random.Range(-9, 9);
                yPos = Random.Range(-9, 9);
            }
            while (!((xPos > -10f && xPos < 10f && yPos > -10f && yPos < 10f) && !(xPos > -7f && xPos < 7f && yPos > -7f && yPos < 7f)));



            Vector2 spawnPosition = new Vector2(xPos,yPos);

            InsectScript.CreateInsect(spawnPosition);
        }

        UpdateCamera();
	}

    public void UpdateCamera()
    {
        Vector2 oldManPos = oldMan.transform.position;
        float terrainHeight = 20f;
        float terrainWidth = 20f;
        float cameraWidth = 10f;
        float cameraHeight = 10f;
        float xFactor = 2 * oldManPos.x / terrainWidth;
        float yFactor = 2 * oldManPos.y / terrainHeight;
        float dilateFactor = 1.5f;
        float maxCameraX = terrainWidth / 2f - cameraWidth / 2f;
        float maxCameraY = terrainHeight / 2f - cameraHeight / 2f;
        Vector2 cameraPos = new Vector2( dilateFactor * xFactor * maxCameraX, dilateFactor * yFactor * maxCameraY);
        cameraPos.x = Mathf.Clamp(cameraPos.x, -maxCameraX, maxCameraX);
        cameraPos.y = Mathf.Clamp(cameraPos.y, -maxCameraY, maxCameraY);
        Camera.main.transform.position = new Vector3(cameraPos.x, cameraPos.y, -10);
        //Debug.Log("CameraUpdate TO: " + cameraPos.x + " , " + cameraPos.y);
    }

    public void PlayerDead()
    {
        gameOver = true;
        Resources.FindObjectsOfTypeAll<GameOverPanelScript>()[0].gameObject.SetActive(true);
    }
}
