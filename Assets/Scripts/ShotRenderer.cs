

using UnityEngine;
[RequireComponent(typeof (LineRenderer))]
public class ShotRenderer : MonoBehaviour {

    [SerializeField] private float lifetime;
    private float elapsedTime;
    private bool show;
    public bool Show {
        get {
            return show;
        }
    }
    private LineRenderer lineRenderer;
    private Gradient originalColorGradient;
    [SerializeField] private Gradient endGradient;
    [SerializeField] private float endEndWith, endStartWidth;
    [SerializeField] private float shrinkAmount = 0.05f;
    private float startEndWidth, startStartWidth;

    [SerializeField] private Vector3 startOffset;

    void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
        startEndWidth = lineRenderer.endWidth;
        startStartWidth = lineRenderer.startWidth;
        Debug.Log("startStartWidth: " + startStartWidth);
    }

    void Start() {
        originalColorGradient = new Gradient();
        originalColorGradient.SetKeys(
            lineRenderer.colorGradient.colorKeys,
            lineRenderer.colorGradient.alphaKeys
        );
    }

    void Update() {
        if (show) {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= lifetime) {
                Hide();
                return;
            }
            float amount = elapsedTime / lifetime;
            var newGradient = Lerp(originalColorGradient, endGradient, amount);
            lineRenderer.colorGradient = newGradient;
            lineRenderer.widthCurve = new AnimationCurve(new Keyframe[]{
                new Keyframe(0f, Mathf.Lerp(startStartWidth, endStartWidth, amount)),
                new Keyframe(1f, Mathf.Lerp(startEndWidth, endEndWith, amount))
            });
            lineRenderer.SetPosition(0, Vector3.Lerp(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1), shrinkAmount));
        }
    }

    private Gradient Lerp(Gradient start, Gradient end, float amount) {
        var output = new Gradient();
        var length = Mathf.Min(start.alphaKeys.Length, end.alphaKeys.Length);
        var alphaKeys = new GradientAlphaKey[length];
        for (var i = 0; i < length; i++) {
            alphaKeys[i] = new GradientAlphaKey(
                Mathf.Lerp(start.alphaKeys[i].alpha, end.alphaKeys[i].alpha, amount),
                start.alphaKeys[i].time
            );
        }
        length = Mathf.Min(start.colorKeys.Length, end.colorKeys.Length);
        var colorKeys = new GradientColorKey[length];
        for (var i = 0; i < length; i++) {
            colorKeys[i] = new GradientColorKey(Color.Lerp(
                start.colorKeys[i].color,
                end.colorKeys[i].color,
                amount
            ), 
            start.colorKeys[i].time
            );
        }
        output.SetKeys(
            colorKeys,
            alphaKeys
        );
        return output;
    }

    public void ShowShot(Vector3 start, Vector3 end) {
        gameObject.SetActive(true);
        lineRenderer.enabled = true;
        show = true;
        elapsedTime = 0f;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions(new Vector3[]{
            start + startOffset, end
        });
    }

    private void print(Vector3[] arr) {
        string output = "";
        for (var i = 0; i < arr.Length; i++) {
            var v = arr[i];
            output += v.ToString() + ", ";
        }
        Debug.LogFormat(output);
    }

    public void Hide() {
        lineRenderer.enabled = false;
        show = false;
        gameObject.SetActive(false);
    }
}