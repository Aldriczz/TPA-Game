using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheatController : MonoBehaviour
{
    [SerializeField] private IntEventChannel ZenUpdateEventChannel;
    [SerializeField] private VoidEventChannel UnlockAllLevelsEventChannel;
    [SerializeField] private InputField CheatText;
    public void CheckCheatInput()
    {
        if (CheatText.text == "tpagamegampang")
        {
            AudioManager.Instance.PlayCheatSound();
            ZenUpdateEventChannel.RaiseIntEvent(20000);
            CheatText.text = "";
        }else if (CheatText.text == "hesoyam")
        {
            AudioManager.Instance.PlayCheatSound();
            ExpManager.Instance.AddExp(10000);
            CheatText.text = "";
            
        }else if (CheatText.text == "opensesame")
        {
            AudioManager.Instance.PlayCheatSound();
            UnlockAllLevelsEventChannel.RaiseVoidEvent();
            CheatText.text = "";
        }
    }
}
