using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class PlayerCamera : MonoBehaviour {

    [SerializeField]
    public Transform target;

    [SerializeField]
    public float followSpeed;

	void Start () {
        
	}
	
	
	void Update () {
	    if(target == null) {
            return;
        }

        transform.position = Vector3.Lerp(transform.position, target.position, followSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, -10.0f);
	}
}
