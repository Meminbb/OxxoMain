using UnityEngine;
using UnityEngine.Networking;

public class Trivia_ForceAcceptAll:CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        return true; 
    }
}
