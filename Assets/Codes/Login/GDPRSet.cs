using GoogleMobileAds.Api;
using GoogleMobileAds.Ump.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GDPRSet : MonoBehaviour
{
    public bool set;

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
                set = true;
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

            if (ConsentInformation.ConsentStatus == ConsentStatus.Obtained ||
                ConsentInformation.ConsentStatus == ConsentStatus.NotRequired)
            {
                if (ConsentInformation.CanRequestAds())
                {
                    MobileAds.Initialize((InitializationStatus initstatus) =>
                    {
                        Debug.Log("���� �ε� ����");
                    });
                }

                set = true; // ������ �ƴ� ��쵵 true�� ����

                Debug.Log("Consent already obtained or not required.");
                return;
            }

            if (ConsentInformation.IsConsentFormAvailable())
            {
                LoadAndShowConsentForm();
            }
        });
    }

    void LoadAndShowConsentForm()
    {
        ConsentForm.Load((consentForm, loadError) =>
        {
            if (loadError != null)
            {
                set = true;

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
                set = true;

                // ���� ���� �����. �ʿ� �� ���� �ε� ���� ����
                Debug.Log("Consent form shown successfully");
            });
        });
    }
}
