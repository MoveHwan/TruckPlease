using GoogleMobileAds.Api;
using GoogleMobileAds.Ump.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GDPRSettingButton : MonoBehaviour
{

    string url = "https://sites.google.com/view/lbstudio-privacy-policy";

    void Start()
    {
        ConsentRequestParameters requestParameters = new ConsentRequestParameters
        {
            TagForUnderAgeOfConsent = false
        };

        ConsentInformation.Update(requestParameters, (formError) =>
        {
            if (formError != null)
            {
                Debug.Log("Consent form load error: " + formError.Message);
                return;
            }

            // ���� GDPR ���� ���¸� �α׷� ���
            ConsentStatus status = ConsentInformation.ConsentStatus;

            switch (status)
            {
                case ConsentStatus.Obtained:
                    Debug.Log("[GDPR]  ����ڰ� ���� **����**�� (����ȭ ���� ����)");
                    break;

                case ConsentStatus.Required:
                    Debug.Log("[GDPR]  ����ڰ� ���� **����**�� �Ǵ� ���� �������� ���� (����ȭ ���� ����)");
                    break;

                case ConsentStatus.NotRequired:
                    Debug.Log("[GDPR]  GDPR ���� ����� �ƴ� (��: ���� ���� �ƴ�)");
                    break;

                case ConsentStatus.Unknown:
                default:
                    Debug.Log("[GDPR]  ���� ���� �� �� ���� (ConsentInfo ������Ʈ ���̰ų� ����)");
                    break;
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PrivacyButton()
    {
        if (ConsentInformation.ConsentStatus == ConsentStatus.NotRequired)
        {
            OpenLink();

            Debug.Log("Consent already not required.");
            return;
        }
        else if (ConsentInformation.IsConsentFormAvailable())
        {
            LoadAndShowConsentForm();
        }
    }

    public void OpenLink()
    {
        Application.OpenURL(url);
    }

    void LoadAndShowConsentForm()
    {
        ConsentForm.Load((consentForm, loadError) =>
        {
            if (loadError != null)
            {

                Debug.Log("Consent form failed to load: " + loadError.Message);
                return;
            }

            // ���� �� ǥ��
            consentForm.Show((formError) =>
            {
                if (formError != null)
                {
                    Debug.Log("Consent form failed to show: " + formError.Message);
                    return;
                }

                if (ConsentInformation.CanRequestAds())
                {
                    MobileAds.Initialize((InitializationStatus initstatus) =>
                    {
                        Debug.Log("���� �ε� ����");
                    });
                }

                // ���� ���� �����. �ʿ� �� ���� �ε� ���� ����
                Debug.Log("Consent form shown successfully");
            });
        });
    }
}
