using Unity.Mathematics;
using UnityEngine;

public class Proyectile : Item
{
    [Header("Grupos en orden")]
    ControlPoints[] wayPoints;

    [Header("Damage")]
    [SerializeField] float damage = 10;

    [Header("Target")]
    Transform target;
    float detectionRadius = 5f;

    [Header("Movement")]
    [SerializeField] float speed = 10f;
    float arriveDistance = 0.1f;

    private bool isChasing = false;

    // Índice del grupo actual
    int currentGroupIndex = 0;

    // Destino actual dentro del grupo
    Vector3 currentDestination;
    bool hasDestination = false;

    public override void SetPlayerUser(PlayerController _playerUser)
    {
        base.SetPlayerUser(_playerUser);
        InitialTarget(_playerUser.gameObject);
    }
    public override void InitialTarget(GameObject originObj)//funcion para entregar el objetivo que va a seguir el proyectil
    {
        wayPoints = RacingManager.instance.Points;//toma los puntos de la pista actual
        Transform origin = originObj.transform;//toma el transform del objeto que lanzo el proyectil
        float oldDistance = 0;//se guardara la distancia de los objetos mientras se recorre el arreglo, para saber si la distancias posteriores son mayores o menores
        for (int i = 0; i < PlayersManager.instance.players.Count; i++)//recorre la lista de jugadores para elegir al objetivo
        {
            PlayerController playerUser = originObj.GetComponent<PlayerController>();//jugador que lanzo el objeto
            PlayerController playerEnemy = PlayersManager.instance.players[i];//posible target
            //si se esta comparando con el mismo o con un jugador en una o mas vueltas mas atras, retorna
            if (PlayersManager.instance.players[i].gameObject == originObj || playerEnemy.CurrentLap < playerUser.CurrentLap) { continue; }

            //direccion entre el jugador que lanzo el objeto y el target actual
            Vector3 directionToPlayer = (PlayersManager.instance.players[i].transform.position - origin.position).normalized;

            float dot = Vector3.Dot(origin.up, directionToPlayer);//numero que undica si el target esta adelante o atras

            if (oldDistance == 0 && dot > 0)//compara si el jugador esta en su valor inicial y si esta adelante, si es asi se define como target
            {
                oldDistance = Vector3.Distance(PlayersManager.instance.players[i].transform.position, origin.position);
                target = PlayersManager.instance.players[i].transform;
            }
            else//si no es el primer objeto del arreglo
            {
                if (dot > 0) // compara si esta adelante
                {
                    float newDistance = Vector3.Distance(PlayersManager.instance.players[i].transform.position, origin.position);

                    if (newDistance < oldDistance)//si la nueva distancia es menor que la anterior el objetivo pasa a ser este objeto del arreglo de jugadores
                    {
                        oldDistance = newDistance;
                        target = PlayersManager.instance.players[i].transform;
                    }
                }
            }
        }
        if (target == null) { currentDestination = (transform.position - origin.transform.position).normalized; }//si no encontro objetivo, siplemente ira hacia adelante
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerCollision player))
        {
            if(player.Controller != playerUser) { player.Controller.GetDamage(damage); }
        }
        if(playerUser.gameObject != collision.gameObject) { Destroy(gameObject); }
    }

    void Update()
    {
        if (target == null) { transform.position += currentDestination * speed * Time.deltaTime; return;}

        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        // Si entra en rango → persecución directa
        if (!isChasing && distanceToTarget <= detectionRadius)
            isChasing = true;

        if (isChasing)
        {
            MoveTo(target.position);
        }
        else
        {
            HandleSequentialWaypoints();
        }
    }

    // ============================================================
    // LÓGICA SECUENCIAL POR GRUPOS
    // ============================================================

    void HandleSequentialWaypoints()
    {
        // si llego al ultimo punto, que vuelva a empezar
        if (currentGroupIndex >= wayPoints.Length)
            currentGroupIndex = 0;

        // Si todavía no tenemos destino dentro del grupo actual
        if (!hasDestination)
        {
            currentDestination = GetClosestPointInGroup(currentGroupIndex);
            hasDestination = true;
        }

        // Movimiento hacia el punto seleccionado
        MoveTo(currentDestination);

        // Si llegó al punto → pasar al siguiente grupo
        float distance = Vector2.Distance(transform.position, currentDestination);

        if (distance <= arriveDistance)
        {
            currentGroupIndex++;   // avanzar al siguiente grupo
            hasDestination = false; // forzar nuevo cálculo
        }
    }

    // ============================================================
    // BUSCA EL PUNTO MÁS CERCANO AL JUGADOR
    // PERO SOLO DENTRO DEL GRUPO INDICADO
    // ============================================================

    Vector2 GetClosestPointInGroup(int groupIndex)
    {
        ControlPoints group = wayPoints[groupIndex];

        Vector2 closestPoint = group.positions[0];
        float minDistance = Vector2.Distance(closestPoint, target.position);

        foreach (var point in group.positions)
        {
            float distance = Vector2.Distance(point, target.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestPoint = point;
            }
        }

        return closestPoint;
    }

    // ============================================================
    // MOVIMIENTO CON MoveTowards
    // ============================================================

    void MoveTo(Vector2 destination)
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
    }

}
