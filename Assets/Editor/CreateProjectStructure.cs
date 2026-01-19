using UnityEditor;
using UnityEngine;

public static class CreateProjectStructure
{
    [MenuItem("Tools/Create Project Structure")]
    public static void CreateFolders()
    {
        string[] folders =
        {
            // Scripts
            "Scripts/Managers",
            "Scripts/Player",
            "Scripts/Enemies",
            "Scripts/UI",
            "Scripts/Systems",
            "Scripts/Utilities",
            "Scripts/Data",

            // Scenes
            "Scenes",

            // Prefabs
            "Prefabs/Player",
            "Prefabs/Enemies",
            "Prefabs/UI",

            // Art
            "Art/Sprites/Player",
            "Art/Sprites/Enemies",
            "Art/Sprites/Environment",
            "Art/UI",

            // Audio
            "Audio/Music",
            "Audio/SFX",

            // Resources
            "Resources/Prefabs"
        };

        foreach (string folder in folders)
        {
            CreateFolder("Assets", folder);
        }

        AssetDatabase.Refresh();
        Debug.Log("✅ Project structure created successfully!");
    }

    private static void CreateFolder(string root, string path)
    {
        string[] parts = path.Split('/');
        string currentPath = root;

        foreach (string part in parts)
        {
            if (!AssetDatabase.IsValidFolder($"{currentPath}/{part}"))
            {
                AssetDatabase.CreateFolder(currentPath, part);
            }
            currentPath += "/" + part;
        }
    }
}
