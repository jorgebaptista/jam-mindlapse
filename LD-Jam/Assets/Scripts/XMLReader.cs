using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

internal class XMLReader : MonoBehaviour
{
    /// <summary>
    /// Gets a string to display as character speech.
    /// Example usage: string text = XMLReader.GetCharacterDialogue("Test_Location", -3);
    /// </summary>
    /// <param name="location">The current location ID</param>
    /// <param name="locationSinLevel">The current sin level for that location</param>
    /// <returns>The piece of text to display.</returns>
    internal string GetCharacterDialogue(string location, int? locationSinLevel)
    {
        string dialogue = "";

        try
        {
            TextAsset fileAsset = Resources.Load<TextAsset>("Dialogue");
            XDocument resourceFile = XDocument.Parse(fileAsset.text);
            List<String> allowedStrings = GetAllowedStrings(resourceFile, location, locationSinLevel);
            dialogue = GetNextString(allowedStrings);
        }
        catch
        {
            Debug.Log("ERROR: Could not find resource.");
            return "";
        }

        if (String.IsNullOrEmpty(dialogue))
        {
            Debug.Log("Could not find text for location " + location + " with sin level " + locationSinLevel + ".");
        }

        return dialogue;
    }

    /// <summary>
    /// Gets all strings matching the location's sin level.
    /// </summary>
    /// <param name="file">XML file</param>
    /// <param name="location">The current location ID</param>
    /// <param name="sinLevel">The current sin level for that location</param>
    /// <returns>Allowed strings.</returns>
    internal List<String> GetAllowedStrings(XDocument file, string location, int? sinLevel)
    {
        List<String> allowedStrings = new List<string>();

        var locationResources = from entry in file.Descendants("dialogue").Elements("location")
                                where (string)entry.Attribute("name") == location
                                select entry;

        IEnumerable<String> allowedText;

        if (sinLevel != null)
        {
            allowedText = from element in locationResources.Elements("string")
                          where
                          (int)element.Attribute("minSinLevel") <= sinLevel &&
                          ((int)element.Attribute("maxSinLevel") >= sinLevel || (int)element.Attribute("maxSinLevel") == -1)
                          select element.Value;
        }
        else
        {
            allowedText = from element in locationResources.Elements("string")
                          select element.Value;
        }

        allowedStrings = allowedText.ToList();

        return allowedStrings;
    }

    /// <summary>
    /// Gets whichever string wasn't shown previously, unless there is no other string.
    /// </summary>
    /// <param name="strings">List of allowed strings.</param>
    /// <returns>The next string to display.</returns>
    internal string GetNextString(List<String> strings)
    {
        string text = "";
        
        System.Random random = new System.Random();
        int index = random.Next(0, strings.Count);
        text = strings[index];

        if (String.IsNullOrEmpty(text))
        {
            return strings.Last();
        }
        else
        {
            return text;
        }
    }

}
