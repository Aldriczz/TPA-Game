    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public static PlayerStateMachine Instance { get; private set; }
    
    [SerializeField] private Transform camera;
    [SerializeField] private ParticleSystem particleVFX;

    [HideInInspector] public State currentState;
    [HideInInspector] public IdleState idleState;
    [HideInInspector] public MovingState movingState;
     public TurnChangeEventChannel turnChangeEventChannel;
     public IntEventChannel SkillReduceCooldownEventChannel;
     public GameObjectEventChannel SkillUseEventChannel;

     private Player player;
     
    private void Awake()
    {
        if (Instance == null || Instance != this)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    

    private void Start()
    {
        player = GetComponent<Player>();
        idleState = new IdleState(this, player, camera);
        movingState = new MovingState(this, player, camera);
        ChangeState(idleState);
    }

    private void Update()
    {
        DrawPathDebug();
        currentState?.HandleInput();
        currentState?.LogicUpdate();
    }

    private void FixedUpdate()
    {
        currentState?.PhysicsUpdate();
    }
    
    

    public void ChangeState(State newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }
    private void DrawPathDebug()
    {
        if (player.resultMap != null && player.resultMap.Count > 1)
        {
            for (int i = 0; i < player.resultMap.Count - 1; i++)
            {
                Vector3 start = new Vector3(player.resultMap[i].x, 0.75f, player.resultMap[i].y);
                Vector3 end = new Vector3(player.resultMap[i + 1].x, 0.75f, player.resultMap[i + 1].y);
                Debug.DrawLine(start, end, Color.red);
            }
        }
    }
}
