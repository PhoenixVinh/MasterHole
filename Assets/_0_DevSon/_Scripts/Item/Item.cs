using System;
using System.Collections;
using System.Threading.Tasks;
using _Scripts.Event;
using _Scripts.Hole;
using _Scripts.Map.MapSpawnItem;
using _Scripts.ObjectPooling;
using _Scripts.Sound;
using _Scripts.UI.MissionUI;
using _Scripts.Vibration;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;




public class Item : MonoBehaviour
{
    public int score = 1;
    public string type = "food";
    private Rigidbody rb;
    private bool isGetScore = false;

    private bool isPhysic = false;


    [SerializeField] private string nameLayerOn = "NoCollision";
    [SerializeField] private string nameLayerOff = "Collision";



    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void SetData(string foodName)
    {

        this.type = foodName;

        isGetScore = false;
    }


    public void SetPhysic()
    {

        if (!isPhysic)
        {
            rb.isKinematic = false;
            isPhysic = true;

            SetLayerOn();



        }
        SetLayerOn();
        rb.WakeUp();




        //rb.velocity = new Vector3(0, -0.1f, 0);
        // if (isPhysic) return;
        // StartCoroutine(FallSmoothly());
    }


    public void SetWakeUpPhysic()
    {
        transform.Translate(Vector3.down * 0.0001f);
        //        rb.WakeUp();
    }








    public void DestroyObject()
    {
        isGetScore = true;
        ItemEvent.OnAddScore?.Invoke(score);
        //SpawnItemMap.Instance.RemoveItem(gameObject);
        TextPooling.Instance.SpawnText(HoleController.Instance.transform.position + Vector3.up * 2, score);

        ManagerMission.Instance.CheckMinusItems(gameObject.name, gameObject);
        if (HoleController.Instance.isUseFindingSKill())
        {
            ItemEvent.OnItemMissionFinding?.Invoke(gameObject);
        }
        // rb.isKinematic = true;
        // rb.useGravity = false;
        //this.gameObject.SetActive(false);
        StartCoroutine(DestroyCoroutine());
    }


    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        if (gameObject != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            DestroyObject(gameObject);
        }


    }

    public void SetLayerOn()
    {

        gameObject.layer = LayerMask.NameToLayer(nameLayerOn);
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer(nameLayerOn);
        }
    }

    public void SetLayerOff()
    {
        gameObject.layer = LayerMask.NameToLayer(nameLayerOff);
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer(nameLayerOff);
        }
    }

    public void InstanceCollider()
    {
        string strLuna = "Luna_";
        Transform posChil0 = transform.GetChild(0);
        string nameobject = "_" + this.gameObject.name;
        if (posChil0 == null)
        {
            Debug.LogError("Item does not have a child to instantiate collider.");
            return;
        }

        // MeshRenderer meshRenderer = posChil0.GetComponent<MeshRenderer>();
        // if (meshRenderer == null)
        // {
        //     Debug.LogError("Item does not have a MeshRenderer.");
        //     return;
        // }
        // meshRenderer.shadowCastingMode = ShadowCastingMode.Off;

        GameObject collider = new GameObject("Collider");

        collider.transform.SetParent(transform);
        collider.transform.localPosition = posChil0.localPosition;
        collider.transform.localRotation = posChil0.localRotation;
        collider.transform.localScale = Vector3.one;

        Collider[] cols = transform.GetComponentsInChildren<Collider>();

        if (cols.Length > 0)
        {
            foreach (Collider col in cols)
            {
                if (col is BoxCollider)
                {
                    BoxCollider boxCollider = collider.AddComponent<BoxCollider>();
                    boxCollider.size = (col as BoxCollider).size;
                    boxCollider.center = (col as BoxCollider).center;

                    Vector3 localScale = col.gameObject.transform.localScale;

                    Vector3 newSize = new Vector3(
                       boxCollider.size.x * localScale.x,
                        boxCollider.size.y * localScale.y,
                        boxCollider.size.z * localScale.z
                    );

                    Vector3 newCenter = new Vector3(
                        boxCollider.center.x * localScale.x,
                        boxCollider.center.y * localScale.y,
                        boxCollider.center.z * localScale.z
                    );

                    Debug.Log("posChil0 sử dụng BoxCollider");

                    boxCollider.center = newCenter;
                    boxCollider.size = newSize;
                    collider.gameObject.name = strLuna + "BoxCollider" + nameobject;

                }
                else if (col is SphereCollider)
                {
                    Vector3 vector3Center = (col as SphereCollider).center;
                    float radius = (col as SphereCollider).radius;
                    Vector3 localScale = col.gameObject.transform.localScale;

                    SphereCollider sphereCollider = collider.AddComponent<SphereCollider>();
                    sphereCollider.radius = localScale.x * radius;
                    sphereCollider.center = localScale.x * vector3Center;

                    collider.gameObject.name = strLuna + "SphereCollider" + nameobject;

                    Debug.Log("posChil0 sử dụng SphereCollider");

                }
                else if (col is CapsuleCollider)
                {
                    Vector3 vector3Center = (col as CapsuleCollider).center;
                    float vector3Height = (col as CapsuleCollider).height;
                    float radius = (col as CapsuleCollider).radius;

                    Vector3 localScale = col.gameObject.transform.localScale;

                    CapsuleCollider capsuleCollider = collider.AddComponent<CapsuleCollider>();
                    capsuleCollider.radius = localScale.x * radius;
                    capsuleCollider.height = localScale.x * vector3Height;
                    capsuleCollider.center = localScale.x * vector3Center;

                    Debug.Log("posChil0 sử dụng CapsuleCollider");

                    collider.gameObject.name = strLuna + "CapsuleCollider" + nameobject;

                }
                else if (col is MeshCollider)
                {
                    CapsuleCollider meshCollider = collider.AddComponent<CapsuleCollider>();
                    meshCollider.center = new Vector3(0, 0.5f, 0);
                    Debug.Log("posChil0 sử dụng MeshCollider");

                    collider.gameObject.name = strLuna + "MeshCollider" + nameobject;


                }
                else
                {
                    Debug.LogWarning("posChil0 sử dụng Collider loại khác: " + col.GetType().Name);
                }
            }
        }
        else
        {

        }

        foreach (Collider col in cols)
        {
            if (col != null)
            {
                if (col is BoxCollider || col is SphereCollider || col is CapsuleCollider)
                {
                    Debug.LogWarning("Destroying collider: " + col.name);

                    DestroyImmediate(col);

                }
            }
        }
    }

    
}