using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    public static BallController instance;

    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private GameObject areaAffector;
    [SerializeField] private float maxForce, forceModifire;
    [SerializeField] private LayerMask rayLayer;

    private float force;
    private Rigidbody rgBody;
    private Vector3 startPos, endPos;
    private bool canShoot = false, isBallStatic = true;
    private Vector3 direction;
    private int numOfPunch = 0;
    private void Awake() {
        if(instance == null){
            instance = this;
        } else {
            Destroy(gameObject);
        }
        rgBody = GetComponent<Rigidbody>();
    }
    
    void Start()
    {
        CameraFollow.instance.SetTarget(gameObject);
    }

    public void MouseDownMethod(){
        startPos = ClickedPoint();
        lineRenderer.gameObject.SetActive(true);
        lineRenderer.SetPosition(0,lineRenderer.transform.localPosition);
    }
    public void MouseNormalMethod(){
        endPos = ClickedPoint();
        endPos.y = lineRenderer.transform.position.y;
        force = Mathf.Clamp(Vector3.Distance(endPos,startPos) * forceModifire, 0, maxForce);
        lineRenderer.SetPosition(1,transform.InverseTransformPoint(endPos));
    }
    public void MouseUpMethod(){
        canShoot = true;
        lineRenderer.gameObject.SetActive(false);
        numOfPunch++;
        Debug.Log(numOfPunch);
    }

    private void Update() {
        if(rgBody.velocity == Vector3.zero && !isBallStatic){
            isBallStatic = true;
            rgBody.angularVelocity = Vector3.zero;
            areaAffector.SetActive(true);
        }
    }

    private void FixedUpdate() {
        if(canShoot){
            canShoot = false;
            direction = startPos - endPos;
            rgBody.AddForce(direction * force, ForceMode.Impulse);
            areaAffector.SetActive(false);
            force = 0;
            startPos = endPos = Vector3.zero;
            isBallStatic = false;
        }
    }

    Vector3 ClickedPoint(){
        Vector3 position = Vector3.zero;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, rayLayer)){
            position = hit.point;
        }
        return position;
    }
    public Transform uiScreen;
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "DeathGround"){
            PlayerPrefs.SetInt("lvlWidth", 3);
            PlayerPrefs.SetInt("lvlHeight", 3);
            PlayerPrefs.SetInt("Currentlvl", 1);
            PlayerPrefs.SetInt("CurrentPoints", 0);
            var :Transform deathScreen = GameObject.Find("Canvas").GetComponent<Transform>().GetChild(0);
            deathScreen.gameObject.SetActive(true);
            uiScreen = GameObject.Find("Canvas").GetComponent<Transform>().GetChild(1);
            uiScreen.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
        if(other.gameObject.tag == "Hole"){
            PlayerPrefs.SetInt("lvlWidth", PlayerPrefs.GetInt("lvlWidth") + 1);
            PlayerPrefs.SetInt("lvlHeight", PlayerPrefs.GetInt("lvlHeight") + 1);
            if(PlayerPrefs.GetInt("lvlHeight") > 130){
                PlayerPrefs.SetInt("lvlHeight", 130);
            }
            if(PlayerPrefs.GetInt("lvlWidth") > 66){
                PlayerPrefs.SetInt("lvlHeight", 66);
            }

            PlayerPrefs.SetInt("CurrentPoints", (PlayerPrefs.GetInt("CurrentPoints")  + (int)(Mathf.Round(200/numOfPunch))));
            PlayerPrefs.SetInt("Currentlvl", PlayerPrefs.GetInt("Currentlvl") + 1);

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
