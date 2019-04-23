using System;
using System.IO;

/**
 * Author: Pantelis Andrianakis
 * Date: April 23rd 2019
 */
public class ConfigWriter
{
    private readonly string fileName;

    public ConfigWriter(string fileName)
    {
        this.fileName = fileName;
    }

    public void SetString(string config, string value)
    {
        try
        {
            // Create file if it does not exist.
            if (!File.Exists(fileName))
            {
                File.Create(fileName).Dispose();
            }

            // Check for existing config.
            bool found = false;
            string contents = "";
            foreach (string row in File.ReadAllLines(fileName))
            {
                if (row.StartsWith(config + "=") || row.StartsWith(config + " ="))
                {
                    // Existing config found.
                    found = true;
                    contents += config + " = " + value + Environment.NewLine;
                }
                else
                {
                    contents += row + Environment.NewLine;
                }
            }

            // If not found create a new entry.
            if (!found)
            {
                contents += config + " = " + value + Environment.NewLine;
            }

            // Write new file contents.
            File.WriteAllText(fileName, contents);
        }
        catch (Exception)
        {
        }
    }

    public void SetBool(string config, bool value)
    {
        SetString(config, value.ToString());
    }

    public void SetInt(string config, int value)
    {
        SetString(config, value.ToString());
    }

    public void SetLong(string config, long value)
    {
        SetString(config, value.ToString());
    }

    public void SetFloat(string config, float value)
    {
        SetString(config, value.ToString());
    }

    public void SetDouble(string config, double value)
    {
        SetString(config, value.ToString());
    }
}
