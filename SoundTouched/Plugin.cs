using Audio;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace SoundTouched;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger { get; private set; }
    public static string SoundsPath { get; private set; }
    public static List<string> WavFiles { get; private set; } = [];
    private void Awake()
    {
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

        var pluginPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var SoundsPath = Path.Combine(pluginPath, "sounds");
        if (!Directory.Exists(SoundsPath))
            Directory.CreateDirectory(SoundsPath);
        WavFiles = Directory.EnumerateFiles(SoundsPath)
            .Where(q => q.ToLower().EndsWith(".wav")).ToList();

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
    }

    public static AudioClip GetAudioClip(string path, AudioType audioType = AudioType.UNKNOWN)
    {
        AudioClip audioClip = null;
        using var uwr = UnityWebRequestMultimedia.GetAudioClip(path, audioType);
        uwr.SendWebRequest();
        try
        {
            while (!uwr.isDone)
                Task.Delay(5).Wait();

            if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
                Logger.LogError($"{uwr.error}");
            else
                audioClip = DownloadHandlerAudioClip.GetContent(uwr);
        }
        catch (Exception err)
        {
            Logger.LogError($"{err.Message}, {err.StackTrace}");
        }


        return audioClip;
    }
}
