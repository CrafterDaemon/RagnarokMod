using System;
using System.Collections.Generic;

/*
    UnifiedRandom version 1.01
    by Michal Obara (mobaradev) mobaradev@yahoo.com http://mobaradev.com
    MIT License
*/

public class UnifiedRandom
{
    private string _seed;
    private int _index = 0;
    private Dictionary<char, double> _signValues = new Dictionary<char, double>() {
		{'0', (1f/16f)/2f},
		{'1', (1f/16f + 2f/16f)/2f},
		{'2', (2f/16f + 3f/16f)/2f},
		{'3', (3f/16f + 4f/16f)/2f},
		{'4', (4f/16f + 5f/16f)/2f},
		{'5', (5f/16f + 6f/16f)/2f},
		{'6', (6f/16f + 7f/16f)/2f},
		{'7', (7f/16f + 8f/16f)/2f},
		{'8', (8f/16f + 9f/16f)/2f},
		{'9', (9f/16f + 10f/16f)/2f},
		{'A', (10f/16f + 11f/16f)/2f},
		{'B', (11f/16f + 12f/16f)/2f},
		{'C', (12f/16f + 13f/16f)/2f},
		{'D', (13f/16f + 14f/16f)/2f},
		{'E', (14f/16f + 15f/16f)/2f},
		{'F', (15f/16f + 16f/16f)/2f}
    };

    public UnifiedRandom(int v)
    {
        this._seed = ((long)DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalSeconds).ToString();
    }

    public UnifiedRandom(string seed)
    {
        this._seed = seed;
    }

    private string _hash(string text)
    {
        using (var sha = new System.Security.Cryptography.SHA256Managed())
        {
            byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(text);
            byte[] hashBytes = sha.ComputeHash(textBytes);

            string hash = BitConverter
                .ToString(hashBytes)
                .Replace("-", String.Empty);

            return hash;
        }
    }

    public double GetValue()
    {
        string toHash = this._seed + "-" + this._index;
        string hash = this._hash(toHash);

        double value = 0;

        for (int i = 0; i < 8; i++)
        {
            char sign = hash[i];
            double signValue = this._signValues[sign];

            if (i == 0)
            {
                value = signValue;
            }
            else
            {
                float delta = 1;
                for (int j = 0; j < i; j++) delta = delta/16;

                if (this._signValues[hash[15 - i]] < 0.5)
                {
                    value -= signValue * delta;
                }
                else
                {
                    value += signValue * delta;
                }

                if (value < 0) value = 0;
                else if (value > 1) value = 1;
            }
        }

        this._index++;
        return value;
    }

    public int GetNumber(int min, int max)
    {
        double value = this.GetValue();

        int intervals = max - min + 1;
		return (int)Math.Floor((intervals * value)) + min;
    }
}