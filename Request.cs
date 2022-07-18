using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTTPServer
{
    public enum RequestMethod
    {
        GET,
        POST,
        HEAD
    }

    public enum HTTPVersion
    {
        HTTP10,
        HTTP11,
        HTTP09
    }

    class Request
    {
        string[] requestLines;
        RequestMethod method;
        public string relativeURI;
        Dictionary<string, string> headerLines;

        public Dictionary<string, string> HeaderLines
        {
            get { return headerLines; }
        }

        HTTPVersion httpVersion;
        string requestString;
        string[] contentLines;

        public Request(string requestString)
        {
            this.requestString = requestString;
        }
        /// <summary>
        /// Parses the request string and loads the request line, header lines and content, returns false if there is a parsing error
        /// </summary>
        /// <returns>True if parsing succeeds, false otherwise.</returns>
        public bool ParseRequest()
        {
            //throw new NotImplementedException();

            //TODO: parse the receivedRequest using the \r\n delimeter   
            string[] split = { "\r\n" };
            requestLines = requestString.Split(split, StringSplitOptions.None);

            // check that there is atleast 3 lines: Request line, Host Header, Blank line (usually 4 lines with the last empty line for empty content)
            if (requestLines.Length < 3)
                return false;

            // Parse Request line
            if (!ParseRequestLine())
                return false;

            // Validate blank line exists
            if (!ValidateBlankLine())
                return false;

            // Load header lines into HeaderLines dictionary
            if (!LoadHeaderLines())
                return false;

            //akher el func
            return true;
        }

        private bool ParseRequestLine()
        {
            //throw new NotImplementedException();
            //split request line
            char[] split = { ' ' };
            string[] requestLine = requestLines[0].Split(split, StringSplitOptions.RemoveEmptyEntries);
            if (requestLine.Length < 2)
                return false;
            else if (requestLine.Length == 2)
            {
                httpVersion = HTTPVersion.HTTP09;
            }
            //get method
            if (requestLine[0].ToLower() == "get")
                method = RequestMethod.GET;
            else if (requestLine[0].ToLower() == "post")
                method = RequestMethod.POST;
            else if (requestLine[0].ToLower() == "head")
                method = RequestMethod.HEAD;
            else
                return false;

            //get uri
            if (ValidateIsURI(requestLine[1]))
                relativeURI = requestLine[1];
            else
                return false;

            //get http v
            if (requestLine[2].ToLower() == "http/1.0")
                httpVersion = HTTPVersion.HTTP10;
            else if (requestLine[2].ToLower() == "http/1.1")
                httpVersion = HTTPVersion.HTTP11;
            else if (requestLine[2].ToLower() == "http/0.9")
                httpVersion = HTTPVersion.HTTP09;
            else
                return false;

            //akher el func
            return true;
        }

        private bool ValidateIsURI(string uri)
        {
            return Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);
        }

        private bool LoadHeaderLines()
        {
            //throw new NotImplementedException();
            string[] split = { ": " };
            //bool returnValue = true;
            headerLines = new Dictionary<string, string>();
            for (int i = 1; i < requestLines.Length - 2; i++)
            {
                if (requestLines[i].Contains(":"))
                {
                    string[] header = requestLines[i].Split(split, StringSplitOptions.None);
                    headerLines.Add(header[0], header[1]);
                }
                else
                    //returnValue = false;
                    return false;
            }
            return true;
        }

        private bool ValidateBlankLine()
        {
            //throw new NotImplementedException();
            if (requestLines[requestLines.Length - 2] == string.Empty)
                return true;
            else
                return false;
        }

    }
}