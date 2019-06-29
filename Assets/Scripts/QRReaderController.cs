using System.Collections;
using UnityEngine;
using System;
using ZXing;
using Vuforia;
using UnityEngine.SceneManagement;

public class QRReaderController : MonoBehaviour {
    private bool cameraInitialized;
    private IBarcodeReader barCodeReader;

    void Start() {
        // TODO: The Firebase Unity SDK for Android requires Google Play services, which must be up-to-date before the SDK can be used.
        barCodeReader = new BarcodeReader();
        StartCoroutine(InitializeCamera());
    }

    private IEnumerator InitializeCamera() {
        // Waiting a little seem to avoid the Vuforia's crashes.
        yield return new WaitForSeconds(1.25f);
        var isFrameFormatSet = CameraDevice.Instance.SetFrameFormat(PIXEL_FORMAT.GRAYSCALE, true);
        Debug.Log(String.Format("FormatSet : {0}", isFrameFormatSet));

        // Force autofocus.
        var isAutoFocus = CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        if (!isAutoFocus) {
            CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_NORMAL);
        }
        Debug.Log(String.Format("AutoFocus : {0}", isAutoFocus));
        cameraInitialized = true;
    }

    private void Update() {
        if (cameraInitialized) {
            try {
                var cameraFeed = CameraDevice.Instance.GetCameraImage(PIXEL_FORMAT.GRAYSCALE);
                if (cameraFeed == null) {
                    return;
                }
                var data = barCodeReader.Decode(cameraFeed.Pixels, cameraFeed.BufferWidth, cameraFeed.BufferHeight, RGBLuminanceSource.BitmapFormat.Gray8);
                if (data != null) {
                    GlobalData.projectId = data.Text;
                    SceneManager.LoadScene("ProjectScene");
                }

            } catch (Exception e) {
                Debug.Log("Error: " + e.Message);
            }

        }
    }
}