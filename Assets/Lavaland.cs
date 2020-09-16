using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lavaland : MonoBehaviour
{
    private int zLayer = 3;
    
    private class Location
    {
        public string Tag { get; set; }
        public string Area { get; set; }
        public Vector3Int Position { get; set; }

        public Location(string inputString)
        {
            int positionIndexStart = inputString.LastIndexOf('(');
            int tagEndIndex = inputString.IndexOf(':');

            Tag = inputString.Substring(0, tagEndIndex).Trim();
            Area = inputString.Substring(tagEndIndex + 1, positionIndexStart - tagEndIndex - 1).Trim();
            
            string positionString = inputString.Substring(inputString.LastIndexOf('('));
            positionString = positionString.Substring(1, positionString.Length - 3);
            positionString = positionString.Replace(" ", "");
            string[] numbers = positionString.Split(',');
            
            Position = new Vector3Int(
                int.Parse(numbers[0]), 
                int.Parse(numbers[1]), 
                int.Parse(numbers[2]));
        }
    }

    public Canvas Canvas;
    public GameObject ImagePrefab;

    void Update()
    {
        if ((Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl)) 
            && Input.GetKeyDown(KeyCode.V))
        {
            for (int i = Canvas.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(Canvas.transform.GetChild(i).gameObject);
            }
            
            string clipBoard = GUIUtility.systemCopyBuffer;

            string[] lines = clipBoard.Split('\n');

            foreach (var line in lines)
            {
                if(line.Length <= 0)
                    continue;
            
                var location = new Location(line);

                if (location.Position.z != zLayer) // Not on the z layer
                {
                    continue;
                }
                
                var image = Instantiate(ImagePrefab, Canvas.transform);
                
                image.GetComponent<RectTransform>().anchoredPosition 
                    = new Vector2(location.Position.x, location.Position.y);

                image.GetComponentInChildren<Text>().text = location.Tag;
                image.GetComponentInChildren<Text>().color = ColorFromTag(location.Tag);
            }
        }
    }

    private Color ColorFromTag(string tag)
    {
        if (tag.Contains("Eerie"))
            return Color.yellow;
        
        if(tag.Contains("Fiery"))
            return Color.red;
        
        if(tag.Contains("Bloody"))
            return Color.magenta;
        
        if(tag.Contains("Resonant"))
            return Color.cyan;
        
        if(tag.Contains("Echoing"))
            return Color.white;
        
        return Color.green;
    }

    void OnGUI()
    {
        float x = 10;
        float y = 10;
        float width = 1000;
        float height = 15;
        
        var plusButtonRect = new Rect(x + 20, y, 20, 20);
        var minusButtonRect = new Rect(x, y, 20, 20);
        var textRect = new Rect(x + 40, y, 60, 20);
        
        if (GUI.Button(plusButtonRect, "+"))
        {
            zLayer++;
            if (zLayer > 20)
                zLayer = 20;
        }

        if (GUI.Button(minusButtonRect, "-"))
        {
            zLayer--;
            if (zLayer <= 1)
                zLayer = 1;
        }

        var color = GUI.color;
        GUI.color = Color.green;
        GUI.Label(textRect, "Z = " + zLayer.ToString());
        GUI.color = color;
    }
}
