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

        try
        {
            // Create file if it does not exist.
            if (!File.Exists(fileName))
            {
                File.Create(fileName).Dispose();
            }
        }
        catch (Exception)
        {
        }
    }

    public void SetString(string config, string value)
    {
        try
        {
            // Check for existing config.
            bool found = false;
            string contents = "";
            foreach (string line in File.ReadAllLines(fileName))
            {
                if (line.StartsWith(config + "=") || line.StartsWith(config + " ="))
                {
                    // Existing config found.
                    found = true;
                    contents += config + " = " + value + Environment.NewLine;
                }
                else
                {
                    contents += line + Environment.NewLine;
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
