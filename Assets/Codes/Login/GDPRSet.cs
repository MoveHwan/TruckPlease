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

            // 현재 GDPR 동의 상태를 로그로 출력
            ConsentStatus status = ConsentInformation.ConsentStatus;

            switch (status)
            {
                case ConsentStatus.Obtained:
                    Debug.Log("[GDPR]  사용자가 광고 **동의**함 (개인화 광고 가능)");
                    break;

                case ConsentStatus.Required:
                    Debug.Log("[GDPR]  사용자가 광고 **거절**함 또는 아직 동의하지 않음 (비개인화 광고만 가능)");
                    break;

                case ConsentStatus.NotRequired:
                    Debug.Log("[GDPR]  GDPR 적용 대상이 아님 (예: 유럽 지역 아님)");
                    break;

                case ConsentStatus.Unknown:
                default:
                    Debug.Log("[GDPR]  동의 상태 알 수 없음 (ConsentInfo 업데이트 전이거나 오류)");
                    break;
            }

            if (ConsentInformation.ConsentStatus == ConsentStatus.Obtained ||
                ConsentInformation.ConsentStatus == ConsentStatus.NotRequired)
            {
                if (ConsentInformation.CanRequestAds())
                {
                    MobileAds.Initialize((InitializationStatus initstatus) =>
                    {
                        Debug.Log("광고 로드 가능");
                    });
                }

                set = true; // 유럽이 아닌 경우도 true로 설정

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

            // 동의 폼 표시
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
                        Debug.Log("광고 로드 가능");
                    });
                }
                set = true;

                // 동의 상태 저장됨. 필요 시 광고 로드 시작 가능
                Debug.Log("Consent form shown successfully");
            });
        });
    }
}
