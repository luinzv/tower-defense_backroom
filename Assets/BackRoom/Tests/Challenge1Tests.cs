using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Challenge1Tests : MonoBehaviour
{
    [SerializeField]
    TMPro.TextMeshProUGUI text;
    [SerializeField]
    GameObject[] gameObjects;
    bool[] tests = new bool[7];
    int[] points = { 10, 10, 10, 10, 20, 20, 20 };
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(-4f, 7.5f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Test7();
        PrintScore();
        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(gameObject.scene.name);
    }

    void PrintScore()
    {
        var sb = new System.Text.StringBuilder("Score: " + Score() + "%\n");
        for (int i = 0; i < tests.Length; i++)
            sb.AppendLine("Test " + (i + 1) + ":" + (tests[i] ? "Passed" : ""));
        text.SetText(sb.ToString());
    }
    void Test7()
    {
        tests[6] = gameObjects[1] == null && gameObjects[0] == null;
    }

    int Score()
    {
        int score = 0;
        for (int i = 0; i < tests.Length; i++)
        {
            if (tests[i]) score += points[i];
        }
        return score;
    }

    IEnumerator SpeedCheck(Transform t)
    {
        float time = .5f;
        Vector3 start = t.position;
        yield return new WaitForSeconds(time);
        Vector3 end = t.position;
        tests[1] = (int)((end - start).magnitude / time) == 7;
    }

    int state = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (state)
        {
            case 0:
                {
                    tests[state] = true;
                    state += 2;
                    transform.position = new Vector3(-6.3f, -1.5f, 0);
                    StartCoroutine(SpeedCheck(collision.transform));
                    break;
                }
            case 2:
                {
                    tests[state] = (collision.transform.right.normalized - Vector3.up).magnitude < .1f;
                    state++;
                    transform.position = new Vector3(-4.19f, -0.3f, 0);
                    break;
                }
            case 3:
                {
                    tests[state] = (collision.transform.right.normalized - Vector3.right).magnitude < .1f;
                    transform.position = new Vector3(9.3f, 7.5f, 0);
                    state++;
                    break;
                }
            case 4:
                {
                    tests[state] = true;
                    transform.position = new Vector3(8.5f, -2.5f, 0);
                    state++;
                    break;
                }
            case 5:
                {
                    tests[state] = true;
                    state++;
                    break;
                }
        }
    }

}
