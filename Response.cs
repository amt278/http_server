using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{

    public enum StatusCode
    {
        OK = 200,
        InternalServerError = 500,
        NotFound = 404,
        BadRequest = 400,
        Redirect = 301
    }

    class Response
    {
        string responseString;
        public string ResponseString
        {
            get
            {
                return responseString;
            }
        }
        StatusCode code;
        List<string> headerLines = new List<string>();
        public Response(StatusCode code, string contentType, string content, string redirectoinPath)
        {
            //throw new NotImplementedException();
            //assign to code
            this.code = code;
            //string statusLine = GetStatusLine(code);

            // TODO: Add headlines (Content-Type, Content-Length,Date, [location if there is redirection])
            headerLines.Add("Content-type: " + contentType);
            headerLines.Add("Content-Length: " + content.Length);
            string date = DateTime.Now.ToString("ddd, dd MMM yyy HH':'mm':'ss 'GMT'");
            headerLines.Add("Date: " + date);
            if (redirectoinPath != string.Empty || code == StatusCode.Redirect)
                headerLines.Add("Redirection-Path: " + redirectoinPath);

            // TODO: Create the request string
            responseString = GetStatusLine(code);
            for (int i = 0; i < headerLines.Count(); i++)
            {
                responseString += headerLines[i] + "\r\n";
            }
            responseString += "\r\n";
            responseString += content;
        }

        private string GetStatusLine(StatusCode code)
        {
            // TODO: Create the response status line and return it
            //string statusLine = string.Empty;
            string statusLine = Configuration.ServerHTTPVersion + " " + ((int)code).ToString() + " " + code.ToString() + "\r\n";

            return statusLine;
        }
    }
}