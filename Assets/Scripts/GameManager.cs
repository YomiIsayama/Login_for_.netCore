using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public static List<ActionAct> actionlist;
    delegate void alertClickOk();

    InputField nameField;
    InputField pwdField;
    Button registerBtn;
    Button loginBtn;
    InputField rNameField;
    InputField rPwdField;
    InputField rConfirmField;
    Button rRegisterBtn;
    Button rBackBtn;
    Image loading;
    GameObject registerPanel;
    GameObject mainPanel;
    GameObject alert;
    AlertBox alertBox;


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        GameObject uicanvas = GameObject.Find("Canvas");
        foreach(Transform t in uicanvas.GetComponentsInChildren<Transform>())
        {
            if(t.name.CompareTo("name")==0)
            {
                nameField = t.GetComponent<InputField>();
            }
            else if (t.name.CompareTo("password")==0)
            {
                pwdField = t.GetComponent<InputField>();
            }
            else if (t.name.CompareTo("register")==0)
            {
                registerBtn = t.GetComponent<Button>();
                registerBtn.onClick.AddListener(delegate ()
                {
                    ShowRegisterPanel(true);
                });
            }
            
            if (t.name.CompareTo("rName") == 0)
            {
                rNameField = t.GetComponent<InputField>();
            }
            else if(t.name.CompareTo("rPwd")==0)
            {
                rPwdField = t.GetComponent<InputField>();
            }
            else if (t.name.CompareTo("rConfirm") == 0)
            {
                rConfirmField = t.GetComponent<InputField>();
            }
            else if (t.name.CompareTo("rRegister") == 0)
            {
                rRegisterBtn = t.GetComponent<Button>();
                rRegisterBtn.onClick.AddListener(RegisterGo);
            }
            else if (t.name.CompareTo("rBack") == 0)
            {
                rBackBtn = t.GetComponent<Button>();
                rBackBtn.onClick.AddListener(delegate() 
                {
                    ShowRegisterPanel(false);
                });
            }
            else if(t.name.CompareTo("loading")==0)
            {
                loading = t.GetComponent<Image>();
                loading.enabled = false;
            }
            else if(t.name.CompareTo("alert")==0)
            {
                alertBox = t.GetComponent<AlertBox>();
            }

        }

        actionlist = new List<ActionAct>();

        mainPanel = GameObject.Find("MainPanel");
        registerPanel = GameObject.Find("RegisterPanel");
        ShowRegisterPanel(false);

        alert = GameObject.Find("alert");
        alert.SetActive(false);
    }

    void Update()
    {
        if(actionlist.Count>0)
        {
            foreach(ActionAct act in actionlist)
            {
                loading.enabled = false;
                switch(act.code)
                {
                    case ActionCode.Login:
                        {
                            if(dealReturnCode(act.message))
                            {
                                ShowDialog("Login Successful", null);
                            }
                            break;
                        }
                    case ActionCode.Register:
                        {
                            if(dealReturnCode(act.message))
                            {
                                ShowDialog("Register Successful", RegisterDone);
                            }
                            break;
                        }
                }
            }
            actionlist.Clear();
        }
    }

    private void RegisterGo()
    {
        if (string.IsNullOrEmpty(rNameField.text))
        {
            ShowDialog("please input name", null);
            return;
        }
        if (string.IsNullOrEmpty(rPwdField.text))
        {
            ShowDialog("please input password", null);
            return;
        }
        if (string.IsNullOrEmpty(rConfirmField.text))
        {
            ShowDialog("please input password again", null);
            return;
        }
        loading.enabled = true;

        Thread r = new Thread(RegisterRequest);
        r.Start();


    }
    private void RegisterDone()
    {
        ShowRegisterPanel(false);
    }

    public void RegisterRequest()
    {
        string requestData = "{\"name\":\"" + rNameField.text + "\",\"pwd\":\"" + rPwdField.text + "\"}";
        string ret = HttpHelper.HttpPostJson("https://localhost:44335/api/registerlogin/registerlogin/register", requestData);

        ActionAct actionAct = new ActionAct();
        actionAct.code = ActionCode.Register;
        actionAct.message = ret;
        actionlist.Add(actionAct);
    }

    private void Login()
    {
        if(string.IsNullOrEmpty(nameField.text))
        {
            ShowDialog("please input name", null);
            return;
        }
        if(string.IsNullOrEmpty(pwdField.text))
        {
            ShowDialog("please input password", null);
            return;
        }

        loading.enabled = true;
        Thread r = new Thread(LoginRequest);
        r.Start();
    }

    public void LoginRequest()
    {
        string requestData = "{\"name\":\"" + nameField.text + "\",\"pwd\":\"" + pwdField.text + "\"}";
        string ret = HttpHelper.HttpPostJson("https://localhost:44335/api/registerlogin/login", requestData);
        ActionAct actionAct = new ActionAct();
        actionAct.code = ActionCode.Login;
        actionAct.message = ret;
        actionlist.Add(actionAct);
    }

    private void ShowDialog(string message,alertClickOk alertClickOk)
    {
        alert.SetActive(true);
        alertBox.aText.text = message;
        alertBox.aBtn.onClick.AddListener(delegate ()
        {
            alert.SetActive(false);
            alertClickOk?.Invoke();
        });
    }

    private void ShowRegisterPanel(bool show)
    {
        if(show)
        {
            rNameField.text = "";
            rConfirmField.text = "";
            rPwdField.text = "";
            registerPanel.SetActive(true);
            mainPanel.SetActive(false);
        }
        else
        {
            nameField.text = "";
            pwdField.text = "";
            registerPanel.SetActive(false);
            mainPanel.SetActive(true);
        }
    }

    private bool dealReturnCode(string ret)
    {
        //ReturnCode rc = JsonUtility.FromJson<ReturnCode>(ret);
        //if(rc.code!=0)
        //{
        //    ShowDialog(rc.message, null);
        //    return false;
        //}
        return true;
    }
}
