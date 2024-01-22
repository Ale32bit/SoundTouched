using Audio;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Networking;

namespace SoundTouched;

[HarmonyPatch(typeof(AudioController))]
class AudioControllerPatch
{
    internal static Dictionary<string, string> AudioFiles = [];

    [HarmonyPatch("Awake")]
    [HarmonyPostfix]
    static void AwakePatch(AudioController __instance, ref AudioComponent[] ____audioComponents)
    {

        Plugin.Logger.LogInfo("This scene has the following audio tracks:");
        foreach (var audio in ____audioComponents)
        {
            Plugin.Logger.LogInfo($" - {audio.Name}");
        }
    }

    internal static void PatchAudioClip(AudioComponent component, AudioClip audioClip)
    {
        Plugin.Logger.LogInfo($"Patching sound: {component.Name}");
        var originalAudioObjectTraverse = Traverse.Create<AudioComponent>().Field<AudioSource>("Audio");
        UnityEngine.Object.Destroy(originalAudioObjectTraverse.Value, 0f);
        var audioSource = component.gameObject.AddComponent<AudioSource>();
        originalAudioObjectTraverse.Value = audioSource;
        component.GetType().GetField("Audio", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(component, audioSource);
        audioSource.clip = audioClip;
        component.GetType().GetField("PitchMinModifier", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(component, 1f);
        component.GetType().GetField("PitchMaxModifier", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(component, 1f);
    }
}

[HarmonyPatch(typeof(SfxComponent))]
class SfxComponentPatch
{
    [HarmonyPatch("Awake")]
    [HarmonyPostfix]
    static void AwakePatch(SfxComponent __instance, ref AudioSource ___Audio)
    {
        var wavFile = Plugin.WavFiles.FirstOrDefault(p => Path.GetFileNameWithoutExtension(p) == __instance.Name);
        if (wavFile == null)
            return;

        var volume = ___Audio.volume;
        Plugin.Logger.LogInfo($"Loading audio file {wavFile}");

        var audioClip = Plugin.GetAudioClip(wavFile, AudioType.WAV);
        AudioControllerPatch.PatchAudioClip(__instance, audioClip);
        ___Audio.volume = volume;
    }
}

[HarmonyPatch(typeof(BgmComponent))]
class BgmComponentPatch
{
    [HarmonyPatch("Awake")]
    [HarmonyPostfix]
    static void AwakePatch(SfxComponent __instance, ref AudioSource ___Audio, ref bool ____hasIntro, AudioSource ____introAudioSource)
    {
        var wavFile = Plugin.WavFiles.FirstOrDefault(p => Path.GetFileNameWithoutExtension(p) == __instance.Name);
        if (wavFile == null)
            return;


        var volume = ___Audio.volume;
        Plugin.Logger.LogInfo($"Loading audio file {wavFile}");

        var audioClip = Plugin.GetAudioClip(wavFile, AudioType.WAV);
        AudioControllerPatch.PatchAudioClip(__instance, audioClip);
        ___Audio.volume = volume;

        ____hasIntro = false;

        if (____hasIntro)
        {
            /*var dirPath = Path.GetDirectoryName(wavFile);
            var extension = Path.GetExtension(wavFile);
            var introFile = $"{__instance.Name}_Intro{extension}";

            ____introAudioSource.volume = volume;*/
        }

        // TODO: add logic for _introAudioSource
    }
}