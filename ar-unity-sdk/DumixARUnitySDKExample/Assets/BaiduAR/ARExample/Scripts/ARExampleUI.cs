﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BaiduARInternal;

public class ARExampleUI : MonoBehaviour
{

    public GameObject loading;
    public GameObject _infoUI;
    public Button _sureButton;

    private Button _resetButton;

    private BaiduARWebCamera _cameraDevice;
    private BaiduARObjectTrackable _arObjectTrackle;


    //private RectTransform returnBtn;
    private RectTransform resetBtn;

	private RectTransform view;
	private RectTransform pictureBtn;


    // Use this for initialization
    void Start()
    {
        //ARLogin.Instance.OnStopCamera.AddListener (ErrorInfo);
        loading.SetActive(true);

        _sureButton.onClick.AddListener(ErrorOnClick);

        _cameraDevice =  FindObjectOfType<BaiduARWebCamera>();
        Transform bg = transform.Find("ResetButton");

		Transform scrollView = transform.Find ("ScrollView");

        if (bg != null)
        {
            resetBtn = bg.GetComponent<RectTransform>();
            _resetButton = bg.GetComponent<Button>();
            _resetButton.onClick.AddListener(ResetOnClick);
            _arObjectTrackle = GameObject.FindObjectOfType<BaiduARObjectTrackable>();
        }

        int adjustHeight = 0;
        ARDebug.Log("SystemInfo.deviceModel = " + SystemInfo.deviceModel);

        ARDebug.Log("SystemInfo.nam = " + SystemInfo.deviceName);
        if (ARUtils.IsIPhoneX())
        {
            adjustHeight = 100;
        }

        if (null != resetBtn)
			resetBtn.anchoredPosition3D = new Vector3(resetBtn.anchoredPosition3D.x, resetBtn.anchoredPosition3D.y + 2 * adjustHeight, resetBtn.anchoredPosition3D.z);

		if (null != scrollView) {
			view = scrollView.GetComponent<RectTransform> ();
			pictureBtn = transform.Find ("TakePicBtn").GetComponent<RectTransform> ();
			view.anchoredPosition3D =  new Vector3(view.anchoredPosition3D.x, view.anchoredPosition3D.y + 2 * adjustHeight, view.anchoredPosition3D.z);
			pictureBtn.anchoredPosition3D =  new Vector3(pictureBtn.anchoredPosition3D.x, pictureBtn.anchoredPosition3D.y + 2 * adjustHeight, pictureBtn.anchoredPosition3D.z);
		}
    }

    public void ErrorInfo(string num, string msg)
    {
        _infoUI.SetActive(true);

        //string num = ARErrorInfo.Instance.errorNum;
        // string msg = ARErrorInfo.Instance.errorMsg;
        ARDebug.LogWarning("msg =" + msg + "  error=" + num);
        if (num != null && msg != null)
        {
            _infoUI.transform.Find("Error").GetComponent<Text>().text = ShowInfo(num, msg);

        }
        else
        {
            _infoUI.transform.Find("Error").GetComponent<Text>().text = "出错了！";
        }

    }

    public void ErrorOnClick()
    {
        Application.Quit();
    }

    public void ResetOnClick()
    {

		BaiduARObjectTracker.Instance.StopAR();

        Vector3 pos = new Vector3(0.5f * _cameraDevice.width, 0.5f * _cameraDevice.height, 0);

        _arObjectTrackle.x = _cameraDevice.width - pos.x;
        _arObjectTrackle.y = _cameraDevice.height - pos.y;

		BaiduARObjectTracker.Instance.StartAR();
    }

    private void Update()
    {
        if (_cameraDevice.isLoad)
        {
            loading.SetActive(false);
        }
    }

    //信息显示
    private string ShowInfo(string num, string msg)
    {
        string error;
        switch (num)
        {
            case "1007":
                {
                    error = msg + ":" + "渠道不存在";
                    break;
                }
            case "1046":
                {
                    error = msg + ":" + "签名失败";
                    break;
                }
            case "1058":
                {
                    error = msg + ":" + "签名过期";
                    break;
                }
            case "1051":
                {
                    error = msg + ":" + "渠道停用";
                    break;
                }
            default:
                {

                    error = "未知错误";
                    break;
                }
        }
        return error;
    }
}
