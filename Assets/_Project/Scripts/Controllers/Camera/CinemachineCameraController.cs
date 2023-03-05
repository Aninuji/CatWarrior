using Cinemachine;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CinemachineCameraController : MonoBehaviour
{

    public float zoomDistance, normalDistance, zoomInSpeed, zoomOutSpeed;
    public AnimationCurve ZoomInCurve, ZoomOutCurve;
    public CinemachineFreeLook vCam;
    public CharacterActionController playerController;
    private float zoomTime;

    private void Start()
    {
        vCam = GetComponent<CinemachineFreeLook>();
        playerController = GameManager.Instance._player.GetComponent<CharacterActionController>();
        playerController.OnChangeCombatMode += Zoom;
    }

    private void OnDestroy()
    {
        playerController.OnChangeCombatMode -= Zoom;
    }

    public void Zoom(bool isZoomed)
    {
        if (isZoomed) StartCoroutine(ZoomIn(zoomDistance));
        else StartCoroutine(ZoomOut(normalDistance));
    }

    public IEnumerator ZoomIn(float end)
    {
        while (vCam.m_Lens.FieldOfView > end)
        {
            vCam.m_Lens.FieldOfView -= ZoomInCurve.Evaluate(zoomTime) * (Time.fixedDeltaTime * zoomInSpeed);
            zoomTime += Time.fixedDeltaTime;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

        vCam.m_Lens.FieldOfView = end;
        zoomTime = 0;
    }

    public IEnumerator ZoomOut(float end)
    {
        while (vCam.m_Lens.FieldOfView < end)
        {
            vCam.m_Lens.FieldOfView += ZoomInCurve.Evaluate(zoomTime) * (Time.fixedDeltaTime * zoomOutSpeed);
            zoomTime += Time.fixedDeltaTime;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

        vCam.m_Lens.FieldOfView = end;
        zoomTime = 0;

    }

}
