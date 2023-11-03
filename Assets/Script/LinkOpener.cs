using UnityEngine;

public class LinkOpener : MonoBehaviour
{
    public void OpenLink(string linkToOpen)
    {
        Application.OpenURL(linkToOpen);
    }
}
