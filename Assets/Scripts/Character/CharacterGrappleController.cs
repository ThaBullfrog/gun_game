using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CharacterGrappleController : MonoBehaviour, ICanDisableAirControl, IGrappleInfo
{

    public LayerMask grappleLayerMask;
    public LineRenderer line;

    private ICharacterInput input;
    public bool grappled { get; private set; }
    private DistanceJoint2D joint = null;

    public Vector2 direction
    {
        get
        {
            if(!grappled)
            {
                return Vector2.up;
            }
            else
            {
                return (joint.connectedAnchor - transform.position.Vector2()).normalized;
            }
        }
    }

    private void Start()
    {
        grappled = false;
        input = GetComponent<ICharacterInput>();
    }

    private void Update()
    {
        if (input != null && input.grapple)
        {
            if(!grappled)
            {
                Grapple();
            }
            else
            {
                UnGrapple();
            }
        }
        if (line != null)
        {
            if (grappled)
            {
                line.SetPosition(0, transform.position);
                line.SetPosition(1, joint.connectedAnchor);
            }
            else
            {
                line.SetPosition(0, Vector3.zero);
                line.SetPosition(1, Vector3.zero);
            }
        }
    }

    private void Grapple()
    {
        Vector2 direction = Vector3.right;
        if(input != null)
        {
            direction = (input.aimLocation - transform.position.Vector2()).normalized;
        }
        Vector2 origin = transform.position.Vector2();
        float distance = 5000f;
        int storedLayer = gameObject.layer;
        gameObject.layer = LayerMask.NameToLayer("NoCollide");
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, grappleLayerMask);
        gameObject.layer = storedLayer;
        if (hit.collider != null)
        {
            grappled = true;
            joint = gameObject.AddComponent<DistanceJoint2D>();
            joint.anchor = Vector2.zero;
            joint.connectedAnchor = hit.point;
            joint.maxDistanceOnly = true;
        }
    }

    private void UnGrapple()
    {
        grappled = false;
        Destroy(joint);
        joint = null;
    }

    public bool disableAirControl { get { return grappled; } }

}