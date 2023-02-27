using UnityEngine;
using TMPro;

public class ObjectiveController : MonoBehaviour
{
    public TextMeshProUGUI objectiveDescription;
    public GameObject objective;
    public RectTransform arrow;

    private Transform player;

    private bool pointPosition = false;
    private float x;
    private float y;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();    
    }

    public void ShowObjective(Objective o)
    {
        objectiveDescription.text = TranslationsManager.GetTranslation("objectives", o.translationKey);
        objective.SetActive(true);

        pointPosition = o.pointPosition;
        if (o.pointPosition)
        {
            arrow.gameObject.SetActive(true);
            x = o.position[0];
            y = o.position[1];
        }
        else 
            arrow.gameObject.SetActive(false);
    }

    public void HideObjective()
    {
        objective.SetActive(false);
    }

    void Update()
    {
        if (pointPosition)
        {
            //float rotation = Vector2.Angle(player.position, new Vector2(x, y));
            ////rotation*=Mathf.Rad2Deg;
            ////arrow.transform.SetPositionAndRotation(arrow.transform.position, new Quaternion(0f,0f,rotation,0f));
            //var imageRectEuler = arrow.eulerAngles;
            //imageRectEuler.z = rotation;
            //arrow.eulerAngles = imageRectEuler;

            //Vector3 direction = Vector3.Normalize(new Vector3(x,y,0f) - player.transform.position);
            //float angle = Vector3.Angle(transform.forward, direction);
            //Vector3 cross = Vector3.Cross(transform.forward, direction);
            //if (cross.y < 0)
            //{
            //    angle = -angle;
            //}
            //arrow.Rotate(0, angle, 0);

            Vector3 direction = new Vector3(x,y,0f) - player.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            arrow.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        }
    }
}
