using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

public class HttpHelper : MonoBehaviour
{
    public static string HttpGet(string Url)
    {
        try
        {
            string retString = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            Stream myResponseStrea = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(myResponseStrea);
            retString = streamReader.ReadToEnd();
            streamReader.Close();
            myResponseStrea.Close();
            return retString;
        }
        catch(Exception ex)
        {
            throw (ex);
        }
    }

    public static string HttpPostJson(string Url, string content)
    {
        string result = "";
        try
        {
            HttpWebRequest request = WebRequest.Create(Url) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/json";

            byte[] data = Encoding.UTF8.GetBytes(content);
            request.ContentLength = data.Length;
            using(Stream reqStream = request.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }

            HttpWebResponse resp = request.GetResponse() as HttpWebResponse;
            Stream stream = resp.GetResponseStream();
            using(StreamReader streamReader = new StreamReader(stream,Encoding.UTF8))
            {
                result = streamReader.ReadToEnd();
                streamReader.Close();
            }
            stream.Close();
        }
        catch (Exception ex)
        {
            if (ex.GetType() == typeof(WebException))
            {
                var response = ((WebException)ex).Response;
                Stream stream = response.GetResponseStream();
                StreamReader streamReader = new StreamReader(stream);
                result = streamReader.ReadToEnd();
                streamReader.Close();
                stream.Close();
            }
            else
            {
                result = ex.ToString();
            }
        }
        return result;
    }
}
