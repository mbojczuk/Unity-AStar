using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public List<Node> newPath;
    float speed = 5.0f;
    List<Vector3> points = new List<Vector3>();
    private IEnumerator coroutine;

    //grid script
    CreateNewGrid gridScript;
    AStar path;

    //reference to the game manager object
    public GameObject obj;

    //awake is done before any calls this is just so we know
    //things get instatiatiated before anything else
    void Awake()
    {
        gridScript = obj.GetComponent<CreateNewGrid>();
        path = obj.GetComponent<AStar>();
    }

    void Update()
    {
        //right click returns point hit 
        //note: tag the main camera as mainCamera
        if(Input.GetMouseButtonDown(1))
        {
            Vector3 mouse = Input.mousePosition;
            Ray castPoint = Camera.main.ScreenPointToRay(mouse);
            RaycastHit hit;
            if(Physics.Raycast(castPoint, out hit, Mathf.Infinity))
            {
                //ceil wont work here so round to int then add 1 for cases like 10.2 and so on * 2 cause we are the origin 
                gridScript.initialGridCreation((Mathf.RoundToInt(Distance(transform.position, hit.point) + 1)*2), transform.position);
                //start A* algorithm
                AstarPather(transform.position, hit.point);
            }
        }
    }

    float Distance(Vector3 curPos, Vector3 moveToPos)
    {
        return (Vector2.Distance(new Vector2(curPos.x, curPos.z), new Vector2(moveToPos.x, moveToPos.z)));
    }


    #region Astar

    /*
     * so everything here is fixed that it clears if not running resets values
     * and can redo a new path if the old one isnt running anymore
     */
    void AstarPather(Vector3 currentPos, Vector3 targetPos)
    {
        points.Clear();
        newPath = path.findPath(currentPos, targetPos);
        foreach (Node n in newPath)
        {
            Vector3 newDirection = n.worldPosition;
            points.Add(newDirection);
        }
        coroutine = runPath(points);
        StopAllCoroutines();
        StartCoroutine(coroutine);
    }

    IEnumerator runPath(List<Vector3> pathPoints)
    {
        for (int i = 0; i < pathPoints.Count; i++)
        {
            while (Vector3.Distance(pathPoints[i], this.transform.position) > 0.01)
            {
                transform.LookAt(pathPoints[i]);
                transform.position = Vector3.MoveTowards(transform.position, pathPoints[i], speed * Time.deltaTime);
                yield return null;
            }

        }
    }

    public void OnDrawGizmos()
    {
        if (newPath != null)
        {
            foreach (Node n in newPath)
            {
                if (newPath.Contains(n))
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (.5f));
                }
            }
        }
    }

    #endregion
}
