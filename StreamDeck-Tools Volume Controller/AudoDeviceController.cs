using NAudio.CoreAudioApi;
using System;
using System.Data;
using System.Linq;

public class AudioDeviceController
{
    private MMDeviceEnumerator deviceEnumerator;

    public AudioDeviceController()
    {
        deviceEnumerator = new MMDeviceEnumerator();
    }

    public void SetVolume(string deviceId, float volume)
    {
        if (volume < 0 || volume > 100)
        {
            throw new ArgumentException("Volume must be between 0 and 100.");
        }

        var device = GetDeviceById(deviceId);
        if (device == null)
        {
            throw new ArgumentException($"No device found with ID {deviceId}.");
        }

        device.AudioEndpointVolume.MasterVolumeLevelScalar = volume / 100;
    }

    public float GetVolume(string deviceId)
    {
        var device = GetDeviceById(deviceId);
        if (device == null)
        {
            throw new ArgumentException($"No device found with ID {deviceId}.");
        }

        return device.AudioEndpointVolume.MasterVolumeLevelScalar * 100;
    }

    public MMDeviceCollection GetAllDevices()
    {
        return deviceEnumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
    }

    private MMDevice GetDeviceById(string deviceId)
    {
        return GetAllDevices().FirstOrDefault(device => device.ID == deviceId);
    }
}
