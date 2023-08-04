using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AStarPathfinding))]
[RequireComponent (typeof(GridMovement))]
public abstract class Ghost : MonoBehaviour
{
    [SerializeField]
    protected PacMan target;

    [SerializeField]
    protected Transform scatterSpot;

    [SerializeField]
    protected Transform respawnSpot;

    [SerializeField]
    protected Transform resetSpot;

    protected AStarPathfinding aStarPathfinding;

    protected GridMovement gridMovement;

    protected GhostState currentState = GhostState.Idle;

    protected Color normalColor;

    protected Color frightenColor = Color.white;

    protected SpriteRenderer spriteRenderer;

    protected AstarNode frightenTargetNode;

    protected float chaseTime = 20f;

    protected float scatterTime = 7f;

    protected float frightenTime = 8f;

    protected float timeRemaining;

    protected float normalTimeToMove = 0.3f;

    protected float frightenTimeToMove = 0.5f;

    protected float eatenTimeToMove = 0.1f;

    protected int score = 200;

    [SerializeField]
    protected bool isReadyExitBox;


    protected virtual void Awake()
    {
        aStarPathfinding = GetComponent<AStarPathfinding>();
        gridMovement = GetComponent<GridMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        normalColor = spriteRenderer.color;
        EventCenter.GetInstance().AddEventListener<int>("PowerUpDotEaten", 
            (i) => 
            {
                EnterFrightenedState();
            });
    }

    protected void Update()
    {
        Debug.Log($"{gameObject.name}: {currentState}");
        switch (currentState)
        {
            case GhostState.Chase:
                InChaseState();
                break;
            case GhostState.Scatter:
                InScatterState();
                break;
            case GhostState.Frightened:
                InFrightenState();
                break;
            case GhostState.Eaten:
                InEatenState();
                break;
            case GhostState.Idle:
                break;
            default:
                break;
        }
    }

    protected void MoveTo(Vector3 targetPositon)
    {
        if (!gridMovement.IsMoving)
        {
            List<AstarNode> nodes = aStarPathfinding.FindPath(targetPositon);
            if (nodes != null && nodes.Count > 0)
            {
                gridMovement.MoveToNode(nodes[0]);
            }
        }
    }

    protected void MoveTo(AstarNode targetNode)
    {
        MoveTo(AStarGrid.GetInstance().AStarNodeToWorld(targetNode));
    }

    protected void EnterChaseState()
    {
        spriteRenderer.color = normalColor;

        if (isReadyExitBox)
        {
            gridMovement.timeToMove = normalTimeToMove;

            currentState = GhostState.Chase;
            timeRemaining = chaseTime;

        }
        
    }

    protected void EnterScatterState()
    {
        spriteRenderer.color = normalColor;

        if (isReadyExitBox) 
        {
            gridMovement.timeToMove = normalTimeToMove;

            currentState = GhostState.Scatter;
            timeRemaining = scatterTime;

        }
        
    }

    protected  void EnterFrightenedState()
    {
        spriteRenderer.color = frightenColor;

        if (isReadyExitBox)
        {
            gridMovement.timeToMove = frightenTimeToMove;

            currentState = GhostState.Frightened;
            timeRemaining = frightenTime;

            GenerateFrightenTargetNode();
        }
        
    }

    protected void EnterEatenState()
    {
        spriteRenderer.color = new Color(Color.white.r, Color.white.g, Color.white.b, 0.5f);

        if (isReadyExitBox)
        {
            gridMovement.timeToMove = eatenTimeToMove;

            currentState = GhostState.Eaten;

        }
        
    }

    protected virtual void InChaseState()
    {

    }

    protected virtual void InScatterState()
    {
        MoveTo(scatterSpot.transform.position);

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            EnterChaseState();
        }
    }

    protected void InFrightenState()
    {
        AstarNode currentNode = AStarGrid.GetInstance().WorldToAStarNode(transform.position);
        if (currentNode.IsCross)
        {
            GenerateFrightenTargetNode();
        }
        MoveTo(frightenTargetNode);

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            EnterChaseState();
        }
    }

    protected void InEatenState()
    {
        if (AStarGrid.GetInstance().WorldToAStarNode(transform.position) 
            == AStarGrid.GetInstance().WorldToAStarNode(respawnSpot.transform.position))
        {
            EnterChaseState();
        }
        MoveTo(respawnSpot.transform.position);
    }

    protected void GenerateFrightenTargetNode()
    {
        int x = Random.Range(0, AStarGrid.GetInstance().GridWidth);
        int y = Random.Range(0, AStarGrid.GetInstance().GridHeight);

        while (AStarGrid.GetInstance().NodeGrid[x, y].IsObstacle)
        {
            x = Random.Range(0, AStarGrid.GetInstance().GridWidth);
            y = Random.Range(0, AStarGrid.GetInstance().GridHeight);
        }
        frightenTargetNode = AStarGrid.GetInstance().NodeGrid[x, y];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (currentState == GhostState.Frightened)
            {
                EnterEatenState();
                EventCenter.GetInstance().EventTrigger("GhostEaten", score);
            }
            else if (currentState == GhostState.Chase || currentState == GhostState.Scatter)
            {
                EventCenter.GetInstance().EventTrigger("PlayerDie");
            }
        }
    }

    public void Reset()
    {
        gridMovement.TeleportTo(resetSpot.transform.position);
        if (currentState != GhostState.Idle)
        {
            EnterChaseState();
        }
    }

    protected virtual void OnDestroy()
    {
        EventCenter.GetInstance().RemoveEventListener<int>("PowerUpDotEaten",
            (i) =>
            {
                EnterFrightenedState();
            });
    }

}
