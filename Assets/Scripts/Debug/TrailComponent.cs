using System.Collections;
using UnityEngine;

public class TrailComponent : MonoBehaviour
{
    [SerializeField]
    private TrailRenderer trailRenderer;
    IEnumerator Start()
    {
        yield return new WaitForSeconds(trailRenderer.time);
        Destroy(this.gameObject);
    }
}