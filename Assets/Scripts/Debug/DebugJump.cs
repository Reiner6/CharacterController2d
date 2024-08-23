using UnityEngine;

public class DebugJump : MonoBehaviour
{
    [SerializeField] private Jump jump;
    [SerializeField] private TrailRenderer trailPrefab;
    [SerializeField] private Color onGround;
    [SerializeField] private Color onJump;
    [SerializeField] private Color onApexTime;
    [SerializeField] private Color onCoyoteTime;
    private TrailRenderer trailPrevious;

    void Update()
    {
        if (jump.ApexTimeCheck)
        {
            ChangeColor(onApexTime);
            return;
        }
        if (jump.OnGroundOverride)
        {
            ChangeColor(onCoyoteTime);
            return;
        }
        if (jump.OnGround)
        {
            ChangeColor(onGround);
        }
        else
        {
            ChangeColor(onJump);
        }
        void ChangeColor(Color value)
        {
            if (trailPrevious != null && trailPrevious.material.color == value)
                return;
            var trail = Instantiate(trailPrefab, this.transform).GetComponent<TrailRenderer>();
            trail.enabled = true;
            trail.material.color = value;
            if (trailPrevious != null)
                trailPrevious.transform.parent = null;
            trailPrevious = trail;
        }
    }
}