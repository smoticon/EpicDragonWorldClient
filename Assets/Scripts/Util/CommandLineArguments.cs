using System;

/**
 * Author: Pantelis Andrianakis
 * Date: December 25th 2018
 */
public class CommandLineArguments
{
    public static string Get(string value)
    {
        string[] arguments = Environment.GetCommandLineArgs();
        for (int i = 0; i < arguments.Length; i++)
        {
            if (arguments[i] == value && arguments.Length > i + 1)
            {
                return arguments[i + 1];
            }
        }
        return null;
    }
}
