using UnityEngine;
using UnityEngine.UI;

public class CarouselUI : MonoBehaviour
{
    public ScrollRect scrollView;
    public RectTransform content;

    [Header("Movimiento")]
    public float scrollSpeed = 0.2f;
    public int[] scrollTimeValues;
    public float autoScrollTime = 3f;

    [Header("Escala")]
    public float minScale = 0.8f;
    public float maxScale = 1.2f;

    float[] positions;
    Transform[] items;

    float timer;
    bool autoScrolling = false;
    bool centering = false;
    float targetPosition;

    void Start()
    {
        UpdateItems();
    }

    void Update()
    {
        if (autoScrolling)
        {
            timer += Time.deltaTime;

            scrollView.horizontalNormalizedPosition += scrollSpeed * Time.deltaTime;

            CheckReorder();

            if (timer >= autoScrollTime)
            {
                autoScrolling = false;
                FindClosestItem();
            }
        }
        else if (centering)
        {
            scrollView.horizontalNormalizedPosition = Mathf.Lerp(scrollView.horizontalNormalizedPosition, targetPosition, 5f * Time.deltaTime);

            if (Mathf.Abs(scrollView.horizontalNormalizedPosition - targetPosition) < 0.001f)
            {
                scrollView.horizontalNormalizedPosition = targetPosition;
                centering = false;
            }
        }

        ScaleItems();
    }

    void UpdateItems()
    {
        int count = content.childCount;

        positions = new float[count];
        items = new Transform[count];

        float distance = 1f / (count - 1f);

        for (int i = 0; i < count; i++)
        {
            positions[i] = distance * i;
            items[i] = content.GetChild(i);
        }
    }

    void CheckReorder()
    {
        if (scrollView.horizontalNormalizedPosition >= 0.99f)
        {
            Transform last = content.GetChild(content.childCount - 1);
            last.SetAsFirstSibling();

            scrollView.horizontalNormalizedPosition = 0;

            UpdateItems();
        }
    }

    void ScaleItems()
    {
        for (int i = 0; i < items.Length; i++)
        {
            float distance = Mathf.Abs(scrollView.horizontalNormalizedPosition - positions[i]);
            float scale = Mathf.Lerp(maxScale, minScale, distance * 5f);

            items[i].localScale = Vector3.one * scale;
        }
    }

    void FindClosestItem()
    {
        float closestDistance = float.MaxValue;
        int closestIndex = 0;

        for (int i = 0; i < positions.Length; i++)
        {
            float distance = Mathf.Abs(scrollView.horizontalNormalizedPosition - positions[i]);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        targetPosition = positions[closestIndex];
        centering = true;

        Debug.Log("Objeto seleccionado: " + items[closestIndex].name);
        LevelsManager manager = LevelsManager.instance;
        manager.currentNextLevel = items[closestIndex].GetComponent<Level>();
        manager.ChargeLevel();
    }

    public void StartScrolling()
    {
        scrollView.horizontalNormalizedPosition = 0;
        UpdateItems();
        if (items.Length > 1)
        {
            autoScrollTime = Random.Range(scrollTimeValues[0], scrollTimeValues[1]);
        }
        else { autoScrollTime = 0; }
        autoScrolling = true;
        timer = 0;
    }
}
