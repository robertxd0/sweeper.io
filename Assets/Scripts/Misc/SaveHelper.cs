using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveHelper
{
    const string fileName = "player_progress";
    const string directory = "gameData";

    public static void SaveToFile(PlayerProperties properties)
    {
        // To save in a directory, it must be created first
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        // The formatter will convert our unity data type into a binary file
        BinaryFormatter formatter = new BinaryFormatter();

        // Choose the save location
        FileStream saveFile = File.Create(directory + "/" + fileName + ".bin");

        // Write our C# Unity game data type to a binary file
        formatter.Serialize(saveFile, properties);

        saveFile.Close();
    }

    public static PlayerProperties LoadFromFile()
    {
         // Converts binary file back into readable data for Unity game
        BinaryFormatter formatter = new BinaryFormatter();

        // Choosing the saved file to open
        FileStream saveFile = File.Open(directory + "/" + fileName + ".bin", FileMode.Open);

        // Convert the file data into SaveGameData format for use in game
        PlayerProperties props = (PlayerProperties) formatter.Deserialize(saveFile);

        saveFile.Close();

        return props;
    }

    public static bool ProgressAvailableInStorage()
    {
        return File.Exists(directory + "/" + fileName + ".bin");
    }

}