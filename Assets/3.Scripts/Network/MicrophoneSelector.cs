using System.Collections.Generic;
using Photon.Voice;
using Photon.Voice.Unity;
using TMPro;
using UnityEngine.UI;

namespace _3.Scripts.Network
{
    public class MicrophoneSelector : VoiceComponent
    {
        public TMP_Dropdown Dropdown;
        public DeviceInfo CurrentDevice
        {
            get
            {
                return availableDevices.Count == 0 ? 
                    default : availableDevices[Dropdown.value];
            }
        }
        
        private readonly List<DeviceInfo> availableDevices = new List<DeviceInfo>();
        private bool isInitialized;

        protected override void Awake()
        {
            Init();
        }

        public void Init()
        {
            if (isInitialized)
                return;
            
            FillDropdown();
            
            // 드롭다운 변경시 AssignMicrophone 호출
            Dropdown.onValueChanged.AddListener(AssignMicrophone);
            
            isInitialized = true;
        }
        
        private void AssignMicrophone(int index)
        {
            VoiceManager.Instance.VoiceRec.MicrophoneDevice = availableDevices[index];
        }
        
        private void FillDropdown()
        {
            availableDevices.Clear();
            Dropdown.ClearOptions();

            List<string> opts = new List<string>();
            
            foreach(DeviceInfo item in Platform.CreateAudioInEnumerator(this.Logger))
            {
                availableDevices.Add(item);
                opts.Add(item.Name);
            }

            Dropdown.AddOptions(opts);
        }
    }
}