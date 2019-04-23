using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

/**
 * Author: Pantelis Andrianakis
 * Date: November 7th 2018
 */
class ConfigReader
{
    private readonly Dictionary<string, string> configs = new Dictionary<string, string>();

    public ConfigReader(string fileName)
    {
        try
        {
            foreach (string row in File.ReadAllLines(fileName))
            {
                if (!row.StartsWith("#") && row.Trim().Length > 0)
                {
                    string[] rowSplit = row.Split('=');
                    if (rowSplit.Length > 1)
                    {
                        configs.Add(rowSplit[0].Trim(), string.Join("=", rowSplit.Skip(1).ToArray()).Trim());
                    }
                }
            }
        }
        catch (Exception)
        {
        }
    }

    public string GetString(string config, string defaultValue)
    {
        if (!configs.ContainsKey(config))
        {
            return defaultValue;
        }
        return configs[config];
    }

    public bool GetBool(string config, bool defaultValue)
    {
        if (!configs.ContainsKey(config))
        {
            return defaultValue;
        }

        try
        {
            return bool.Parse(configs[config]);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    public int GetInt(string config, int defaultValue)
    {
        if (!configs.ContainsKey(config))
        {
            return defaultValue;
        }

        try
        {
            return int.Parse(configs[config]);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    public long GetLong(string config, long defaultValue)
    {
        if (!configs.ContainsKey(config))
        {
            return defaultValue;
        }

        try
        {
            return long.Parse(configs[config]);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    public float GetFloat(string config, float defaultValue)
    {
        if (!configs.ContainsKey(config))
        {
            return defaultValue;
        }

        try
        {
            return float.Parse(configs[config], CultureInfo.InvariantCulture);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    public double GetDouble(string config, double defaultValue)
    {
        if (!configs.ContainsKey(config))
        {
            return defaultValue;
        }

        try
        {
            return double.Parse(configs[config], CultureInfo.InvariantCulture);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }
}
