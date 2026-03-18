using UnityEngine;

public class FormationPreview : MonoBehaviour
{
    [SerializeField] private FormationDataSO formationData;
    [SerializeField] private float handleSize = 0.2f;

    public FormationDataSO FormationData => formationData;
    public float HandleSize => handleSize;
}

