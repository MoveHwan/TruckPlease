using Google.Play.Review;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviewInGame : MonoBehaviour
{
    public void GooglePlayReview()
    {
        PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold", 0) + 300);
        
        StartCoroutine(StartGooglePlayReview());
    }


    IEnumerator StartGooglePlayReview()
    {
        var reviewManager = new ReviewManager();

        var requestFlowOperation = reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;

        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            Debug.LogError("requestFlowOperation Error ::" + requestFlowOperation.Error.ToString());
            yield break;
        }

        var playReviewInfo = requestFlowOperation.GetResult();

        var launchFlowOperation = reviewManager.LaunchReviewFlow(playReviewInfo);
        yield return launchFlowOperation;

        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
            Debug.LogError("launchFlowOperation Error ::" + launchFlowOperation.Error.ToString());
            yield break;
        }

    }
}
