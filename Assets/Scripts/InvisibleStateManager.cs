using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;
using UnityEngine.Rendering;

public class InvisibleStateManager : MonoBehaviour
{
    // Start is called before the first frame update

    public int imageBatchSize = 1;

    public GameObject[] textureGetter;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            CreateInvisible(SwitchModel.GetActiveCharacter());
        }
    }

    public void CreateInvisible(GameObject activeCharacter)
    {
        GameObject activeInvisible = activeCharacter.transform.GetChild(2).gameObject;

        Transform[] transforms = activeInvisible.GetComponentsInChildren<Transform>();

        Debug.Log(transforms.Length);

        foreach (Transform obj in transforms)
        {
            Debug.Log(obj.transform.name);
            AssignTexture(obj.gameObject);
        }
    }

    private void AssignTexture(GameObject obj)
    {
        Material newMaterial = new Material(Shader.Find("HDRP/Lit"));
        newMaterial.mainTexture = GetTexture();
        Renderer renderer = obj.GetComponent<Renderer>();
        renderer.material = newMaterial;
    }

    private Texture2D GetTexture()
    {
        //load image[s]
        string imagePath = null;
        if (Directory.Exists(Path.Combine(Application.persistentDataPath, "Photos")))
        {
            string photosDirectory = Path.Combine(Application.persistentDataPath, "Photos");
            //Get List of Images
            List<string> imageNames = new List<string>();
            DirectoryInfo directoryInfo = new DirectoryInfo(photosDirectory);
            foreach (FileInfo imageName in directoryInfo.GetFiles("*.jpg"))
            {
                imageNames.Add(imageName.Name);
            }
            //Select an image
            string imageFileName = imageNames[(int)Random.Range(0, imageNames.Count)];


            imagePath = Path.Combine(photosDirectory, imageFileName);
        }
        if (imagePath == null)
        {
            Debug.Log("No Image Found");
        }
        byte[] bytes = File.ReadAllBytes(imagePath);
        Texture2D texture = new Texture2D(900, 900, TextureFormat.RGB24, false);
        texture.LoadImage(bytes);

        //get interesting part
        //Rect cropArea = FindMostInterestingRectByVariance(texture, 64, 64);
        //Rect cropArea = FindMostInterestingRectByEdgeDetection(texture, 64, 64);

        Rect cropArea = GetRandomPartOfTexture(texture, 300, 300);
        //return texture
        texture = CropTexture(texture, cropArea);
        return texture;
    }

    private Rect GetRandomPartOfTexture(Texture texture, int width, int height)
    {
        int x = Random.Range(0, texture.width);
        int y = Random.Range(0, texture.height);

        if ((x + width > texture.width))
        {
            x = x - width;
        }
        if ((y + height > texture.height))
        {
            y = y - height;
        }
        return new Rect(x, y, width, height);
    }

    private Rect FindMostInterestingRectByEdgeDetection(Texture2D texture, int width, int height)
    {
        int texWidth = texture.width;
        int texHeight = texture.height;
        float maxEdgeDensity = 0;
        Rect bestRect = new Rect(0, 0, width, height);

        // Apply edge detection and find the region with the highest edge density
        Texture2D edgesTexture = ApplyEdgeDetection(texture);

        for (int x = 0; x <= texWidth - width; x += 10)
        {
            for (int y = 0; y <= texHeight - height; y += 10)
            {
                Rect rect = new Rect(x, y, width, height);
                float edgeDensity = CalculateEdgeDensity(edgesTexture, rect);

                if (edgeDensity > maxEdgeDensity)
                {
                    maxEdgeDensity = edgeDensity;
                    bestRect = rect;
                }
            }
        }

        return bestRect;
    }

    private Texture2D ApplyEdgeDetection(Texture2D texture)
    {
        Texture2D edgesTexture = new Texture2D(texture.width, texture.height);
        Color[] pixels = texture.GetPixels();
        Color[] edgePixels = new Color[pixels.Length];

        for (int y = 1; y < texture.height - 1; y++)
        {
            for (int x = 1; x < texture.width - 1; x++)
            {
                float gx = GetGrayScale(texture.GetPixel(x + 1, y - 1)) + 2 * GetGrayScale(texture.GetPixel(x + 1, y)) + GetGrayScale(texture.GetPixel(x + 1, y + 1))
                         - GetGrayScale(texture.GetPixel(x - 1, y - 1)) - 2 * GetGrayScale(texture.GetPixel(x - 1, y)) - GetGrayScale(texture.GetPixel(x - 1, y + 1));

                float gy = GetGrayScale(texture.GetPixel(x - 1, y + 1)) + 2 * GetGrayScale(texture.GetPixel(x, y + 1)) + GetGrayScale(texture.GetPixel(x + 1, y + 1))
                         - GetGrayScale(texture.GetPixel(x - 1, y - 1)) - 2 * GetGrayScale(texture.GetPixel(x, y - 1)) - GetGrayScale(texture.GetPixel(x + 1, y - 1));

                float g = Mathf.Sqrt(gx * gx + gy * gy);
                edgePixels[y * texture.width + x] = new Color(g, g, g);
            }
        }

        edgesTexture.SetPixels(edgePixels);
        edgesTexture.Apply();
        return edgesTexture;
    }

    private float GetGrayScale(Color color)
    {
        return (color.r + color.g + color.b) / 3f;
    }

    private float CalculateEdgeDensity(Texture2D texture, Rect rect)
    {
        Color[] pixels = texture.GetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
        float density = 0;

        foreach (Color pixel in pixels)
        {
            density += pixel.r; // since the edge detection result is grayscale, r = g = b
        }

        return density / pixels.Length;
    }

    private Rect FindMostInterestingRectByVariance(Texture2D texture, int width, int height)
    {
        int texWidth = texture.width;
        int texHeight = texture.height;
        float maxVariance = 0;
        Rect bestRect = new Rect(0, 0, width, height);

        for (int x = 0; x <= texWidth - width; x += 10)
        {
            for (int y = 0; y <= texHeight - height; y += 10)
            {
                Rect rect = new Rect(x, y, width, height);
                float variance = CalculateRegionVariance(texture, rect);

                if (variance > maxVariance)
                {
                    maxVariance = variance;
                    bestRect = rect;
                }
            }
        }

        return bestRect;
    }

    private float CalculateRegionVariance(Texture2D texture, Rect rect)
    {
        Color[] pixels = texture.GetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
        float mean = 0;
        foreach (Color pixel in pixels)
        {
            mean += GetGrayScale(pixel);
        }
        mean /= pixels.Length;

        float variance = 0;
        foreach (Color pixel in pixels)
        {
            variance += Mathf.Pow(GetGrayScale(pixel) - mean, 2);
        }
        variance /= pixels.Length;

        return variance;
    }

    private Texture2D CropTexture(Texture2D texture, Rect rect)
    {
        Texture2D croppedTexture = new Texture2D((int)rect.width, (int)rect.height);
        Color[] pixels = texture.GetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
        croppedTexture.SetPixels(pixels);
        croppedTexture.Apply();
        return croppedTexture;
    }

}
