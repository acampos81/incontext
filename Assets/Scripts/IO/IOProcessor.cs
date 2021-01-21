using System;
using System.IO;
using SimpleJSON;

public static class IOProcessor
{
    public static void Save(string filePath, IWorldObjectModel[] models)
    {
        JSONNode modelArray = new JSONArray();
        for(int i=0; i<models.Length; i++)
        {
            modelArray.Add(models[i].ToJSON());
        }

        JSONNode json = new JSONObject();
        json["models"] = modelArray;

        File.WriteAllText(filePath, json.ToString());
    }

    public static JSONArray Load(string filePath)
    {
        string modelsText = File.ReadAllText(filePath);
        JSONNode json = JSON.Parse(modelsText);
        return json["models"].AsArray;
    }
}
