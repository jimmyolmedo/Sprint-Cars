using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class RacingManager : Singleton<RacingManager>
{
    //variables
    [Header("race")]
    public List<PlayerController> players = new List<PlayerController>();//lista de jugadores
    [SerializeField] ControlPoints[] points;//puntos por los que el jugador tendra que pasar a lo largo de la carrera
    [SerializeField] Color[] colorPlayers;//color designado de cada jugador
    [SerializeField] int MaxLap = 3;//numero de vueltas que tendra que dar el jugador

    [Header("UI")]
    [SerializeField] TextMeshProUGUI startCount;//cuenta regresiva para iniciar la carrera

    [Header("Score")]
    [SerializeField] int[] scorevalues;//lista de puntajes, dependiendo de los lugares de cada jugador ganaran mas o menos puntos
    int scoreIndex;//contador del puntaje para el jugador que termine la carrera
                   //cuando esto pasa el numero se aumenta el uno para entregar el proximo valor del arreglo anterior

    //properties
    protected override bool persistent => false;//no debe ser persistente, el singleton es solo para poder llamarlo desde otros scripts
    public ControlPoints[] Points { get => points; }

    //methods
    private void Start()
    {
        //toma los jugadores conectados
        players = new List<PlayerController>(PlayersManager.instance.players);
        //activa la funcion de iniciar carrera, la cual se usa para resetear las variables de los jugadores y dar inicio a la carrera 
        StartRacing();
    }

    private void Update()
    {
        players = new List<PlayerController>(PlayersManager.instance.players);
        //recorre la lista de jugadores, tomando uno por uno para ver si llegaron a los puntos de la pista
        for (int i = 0; i < players.Count; i++)
        {
            int position = players[i].CurrentControlPoints;
            //en este caso el jugador paso por todos los puntos, por lo cual retorna
            if (position >= points.Length) return;
            //se recorre la lista de puntos para ver si el jugador paso por su punto designado actualmente
            for (int j = 0; j < points[position].positions.Length; j++)
            {
               Collider2D[] coll = Physics2D.OverlapCircleAll(points[position].positions[j], points[position].ranges[j]);
                foreach(Collider2D coll2 in coll)
                {
                    if(coll2.transform == players[i].transform)
                    {
                        //si llego a su punto designado actualmente, se le asigna el siguiente punto
                        players[i].CurrentControlPoints++;
                    }

                    //if(coll2.TryGetComponent(out Proyectile proyectile))
                    //{
                    //    proyectile.PositionIndex++;
                    //    proyectile.ChooseTarget();
                    //}
                }
            }
        }
    }

    public void CheckGoal(PlayerController player)
    {
        if(player.CurrentControlPoints >= points.Length)//pregunta si el jugador paso por todos los puntos, si es asi, el jugador dio una vuelta entera
        {
            player.CurrentControlPoints = 0;//reinicia los puntos de control
            player.CurrentLap++;//le aumenta una vuelta
        }
        if (player.CurrentLap >= MaxLap) { CheckFinish(player); }//pregunta si el jugador dio todas las vueltas
    }

    public void StartRacing()
    {
        foreach(PlayerController player in players)
        {
            player.RestoreValues();
            player.SwitchState(PlayerState.enable);
        }
        StartCoroutine(StartRace());
    }

    IEnumerator StartRace()
    {
        startCount.gameObject.SetActive(true);
        GameManager.SwitchState(GameState.Menu);
        startCount.text = "3";
        yield return new WaitForSeconds(1);
        startCount.text = "2";
        yield return new WaitForSeconds(1);
        startCount.text = "1";
        yield return new WaitForSeconds(1);
        startCount.text = "GO!";
        GameManager.SwitchState(GameState.Gameplay);
        yield return new WaitForSeconds(.5f);
        startCount.gameObject.SetActive(false);

    }

    public void CheckFinish(PlayerController player)
    {
        player.Win = true;
        player.Score += scorevalues[scoreIndex];
        scoreIndex++;
        player.SwitchState(PlayerState.disable);

        for (int i = 0; i < players.Count; i++)
        {
            if (!players[i].Win) { return; }
        }
        FinishRacing();
    }

    void FinishRacing()
    {
        Debug.Log("La carrera ha terminado");
        scoreIndex = 0;
        SceneManager.instance.LoadScene("ScoreScene");
        GameManager.SwitchState(GameState.Menu);
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (i >= players.Count)
            {
                for (int j = 0; j < points[i].positions.Length; j++)
                {
                    Gizmos.color = points[i].colorPoint;
                    Gizmos.DrawWireSphere(points[i].positions[j], points[i].ranges[j]);
                }
            }
        }

    }
}
