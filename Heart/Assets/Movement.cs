using UnityEngine;

public class Movement : MonoBehaviour
{
    public GameObject heartShape;
    GameObject[] hearts;
    static int numSphere = 30; 
    float time = 0f;
    Vector3[] startPosition, endPosition;

    // Start is called before the first frame update
    void Start()
    {
        // Assign proper types and sizes to the variables.
        hearts = new GameObject[numSphere];
        startPosition = new Vector3[numSphere]; 
        endPosition = new Vector3[numSphere]; 
        
        // Define target positions. Start = random, End = heart 
        for (int i =0; i < numSphere; i++){
            // Random start positions
            // float sin = Mathf.Sin(i * 2 * Mathf.PI / numSphere);
            // float cos = Mathf.Cos(i * 2 * Mathf.PI / numSphere);
            // startPosition[i]= new Vector3( 1 *
            //                         20 * (Mathf.Sqrt(2f) * sin * sin * sin),
            //                         20 * (-1 * cos * cos * cos - cos * cos + 2 * cos),
            //                         20f);   
            
            // //r = 3f; // radius of the circle
            // // Circular end position
            // endPosition[i] = new Vector3( 1 *
            //                         10 * (Mathf.Sqrt(2f) * sin * sin * sin),
            //                         10 * (-1 * cos * cos * cos - cos * cos + 2 * cos),
            //                         20f);
            float theta = i * 2 * Mathf.PI / numSphere;

            // star
            float R = 30f;     // size
            float k = 0.3f;    // spike sharpness 
            float rStar = R * (1f + k * Mathf.Cos(5f * theta)); // 5 sided star
            startPosition[i] = new Vector3(
                rStar * Mathf.Cos(theta),
                rStar * Mathf.Sin(theta),
                20f
            );

            // heart
            float sin = Mathf.Sin(theta);
            float cos = Mathf.Cos(theta);
            endPosition[i] = new Vector3( 1 *
                                    20 * (Mathf.Sqrt(2f) * sin * sin * sin),
                                    20 * (-1 * cos * cos * cos - cos * cos + 2 * cos) + 10,
                                    20f);
        }
        // Let there be spheres..
        for (int i =0; i < numSphere; i++){
            // Draw primitive elements:
            // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/GameObject.CreatePrimitive.html
            hearts[i] = Instantiate(heartShape);

            hearts[i].transform.position = startPosition[i];


            // Color. Get the renderer of the spheres and assign colors.
            Renderer[] rends = hearts[i].GetComponentsInChildren<Renderer>();
            // HSV color space: https://en.wikipedia.org/wiki/HSL_and_HSV
            float hue = (float)i / numSphere; 
            Color color = Color.HSVToRGB(hue, 1f, 1f);
            
            foreach (Renderer r in rends)
                r.material.color = color;
        }

        heartShape.SetActive(false);
    }

    void Update()
    {
        // Measure Time 
        time += Time.deltaTime; //
        float interval = 1f; //seconds
        int activeCount = (int)(time / interval);

        // what to update over time?
        for (int i =0; i < numSphere; i++){
            // Lerp : Linearly interpolates between two points.
            // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Vector3.Lerp.html
            // Vector3.Lerp(startPosition, endPosition, lerpFraction)
            
            // lerpFraction variable defines the point between startPosition and endPosition (0~1)
            // let it oscillate over time using sin function
            float lerpFraction = Mathf.Sin(time) * 0.5f + 0.5f;
            // Lerp logic. Update position
            hearts[i].transform.position = Vector3.Lerp(startPosition[i], endPosition[i], lerpFraction);
            // For now, start positions and end positions are fixed. But what if you change it over time?
            // startPosition[i]; endPosition[i];

            // Color Update over time
            Color targetColor;

            float hueStart = 0.16f; // yellow
            float hueEnd = 0f;      // red
            float hue = Mathf.Lerp(hueStart, hueEnd, lerpFraction);
            targetColor = Color.HSVToRGB(hue, 1f, 1f);
            
            Renderer[] rends = hearts[i].GetComponentsInChildren<Renderer>();
            //float hue = (float)i / numSphere; // Hue cycles through 0 to 1
            //Color color = Color.HSVToRGB(Mathf.Abs(hue * Mathf.Sin(time)), Mathf.Cos(time), 2f + Mathf.Cos(time)); // Full saturation and brightness
            foreach(Renderer r in rends)
            {
                r.material.color = targetColor;
            }
        }
    }



}
