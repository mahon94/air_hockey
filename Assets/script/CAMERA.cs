using UnityEngine;
using System.Collections;

public class CAMERA : MonoBehaviour
{
    private float DesignOrthographicSize;
    private float DesignAspect;
    private float DesignWidth;

    public float DesignAspectHeight;
    public float DesignAspectWidth;
    public void Start()
    {
        this.DesignOrthographicSize = this.camera.orthographicSize;
        this.DesignAspect = this.DesignAspectHeight / this.DesignAspectWidth;
        this.DesignWidth = this.DesignOrthographicSize * this.DesignAspect;

        this.Resize();
    }

    public void Resize()
    {
        Debug.Log("ss");
        float wantedSize = this.DesignWidth / this.camera.aspect;
        this.camera.orthographicSize = Mathf.Max(wantedSize,
            this.DesignOrthographicSize);
    }
}