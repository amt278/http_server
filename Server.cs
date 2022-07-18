using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace HTTPServer
{
    class Server
    {
        Socket serverSocket;
        EndPoint iep;

        public Server(int portNumber, string redirectionMatrixPath)
        {
            //TODO: call this.LoadRedirectionRules passing redirectionMatrixPath to it
            LoadRedirectionRules(redirectionMatrixPath);

            //TODO: initialize this.serverSocket
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            iep = new IPEndPoint(IPAddress.Any, portNumber);
            serverSocket.Bind(iep);
        }

        public void StartServer()
        {
            // TODO: Listen to connections, with large backlog.
            serverSocket.Listen(1000);
            // TODO: Accept connections in while loop and start a thread for each connection on function "Handle Connection"
            while (true)
            {
                //TODO: accept connections and start thread for each accepted connection.
                Socket clinentSocket = serverSocket.Accept();
                Thread newThread = new Thread(new ParameterizedThreadStart(HandleConnection));
                newThread.Start(clinentSocket);
            }
            //Socket clinentSocket = serverSocket.Accept();
            //HandleConnection(clinentSocket);
        }

        public void HandleConnection(object obj)
        {
            // TODO: Create client socket 
            // set client socket ReceiveTimeout = 0 to indicate an infinite time-out period
            Socket clientSocket = (Socket)obj;
            clientSocket.ReceiveTimeout = 0;
            //string welcome = "Welcome to my test server";
            //byte[] sendData = Encoding.ASCII.GetBytes(welcome);
            //clientSocket.Send(sendData);
            //int receivedLength;

            // TODO: receive requests in while true until remote client closes the socket.
            while (true)
            {
                try
                {
                    // TODO: Receive request
                    int receivedLength = 0;
                    byte[] receivedData = new byte[1024 * 1024];
                    receivedLength = clientSocket.Receive(receivedData);

                    // TODO: break the while loop if receivedLen==0
                    if (receivedLength == 0)
                    {
                        break;
                    }

                    // TODO: Create a Request object using received request string
                    string requestStr = Encoding.ASCII.GetString(receivedData);
                    Request request = new Request(requestStr);
                    request = null;

                    // TODO: Call HandleRequest Method that returns the response
                    Response response = HandleRequest(request);

                    // TODO: Send Response back to client
                    clientSocket.Send(Encoding.ASCII.GetBytes(response.ResponseString));


                }
                catch (Exception ex)
                {
                    // TODO: log exception using Logger class
                    Logger.LogException(ex);

                }
            }

            // TODO: close client socket
            clientSocket.Close();
        }

        Response HandleRequest(Request request)
        {
            //throw new NotImplementedException();
            string content;
            try
            {
                //TODO: check for bad request 
                if (!request.ParseRequest())
                {
                    content = LoadDefaultPage(Configuration.BadRequestDefaultPageName);
                    return new Response(StatusCode.BadRequest, "html", content, string.Empty);
                }

                //TODO: map the relativeURI in request to get the physical path of the resource.
                string uri = Configuration.RootPath + request.relativeURI;
                string redirectionPath = GetRedirectionPagePathIFExist(request.relativeURI);
                //TODO: check for redirect
                //if (uri.Contains("aboutus.html"))
                //if (Configuration.RedirectionRules.ContainsKey(request.relativeURI))
                if (!String.IsNullOrEmpty(redirectionPath))
                {
                    uri = Configuration.RootPath + "/" + redirectionPath;
                    content = File.ReadAllText(uri);
                    return new Response(StatusCode.Redirect, "html", content, redirectionPath);
                }

                //TODO: check file exists
                //if (LoadDefaultPage(uri) == string.Empty)
                if (!File.Exists(uri))
                {
                    content = LoadDefaultPage(Configuration.NotFoundDefaultPageName);
                    return new Response(StatusCode.NotFound, "html", content, string.Empty);
                }

                //TODO: read the physical file
                //string fileData = LoadDefaultPage(uri);
                content = File.ReadAllText(uri);

                // Create OK response
                return new Response(StatusCode.OK, "html", content, string.Empty);
            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                Logger.LogException(ex);

                // TODO: in case of exception, return Internal Server Error.
                content = LoadDefaultPage(Configuration.InternalErrorDefaultPageName);
                return new Response(StatusCode.InternalServerError, "html", content, string.Empty);
            }
        }

        private string GetRedirectionPagePathIFExist(string relativePath)
        {
            // using Configuration.RedirectionRules return the redirected page path if exists else returns empty
            if (relativePath[0] == '/')
                relativePath = relativePath.Substring(1);
            string redirectPath;
            if (Configuration.RedirectionRules.TryGetValue(relativePath, out redirectPath))
                return redirectPath;

            return string.Empty;
        }

        private string LoadDefaultPage(string defaultPageName)
        {
            string filePath = Path.Combine(Configuration.RootPath, defaultPageName);
            // TODO: check if filepath not exist log exception using Logger class and return empty string
            if (!File.Exists(filePath))
            {
                Logger.LogException(new Exception("Default Page " + defaultPageName + " doesn't exist"));
                return string.Empty;
            }
            // else read file and return its content
            StreamReader reader = new StreamReader(filePath);
            string file = reader.ReadToEnd();
            reader.Close();
            return file;

        }

        private void LoadRedirectionRules(string filePath)
        {
            try
            {
                // TODO: using the filepath paramter read the redirection rules from file 
                StreamReader reader = new StreamReader(filePath);

                // then fill Configuration.RedirectionRules dictionary 
                Configuration.RedirectionRules = new Dictionary<string, string>();

                while (!reader.EndOfStream)
                {
                    string temp = reader.ReadLine();
                    string[] result = temp.Split(',');
                    Configuration.RedirectionRules.Add(result[0], result[1]);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                Logger.LogException(ex);

                Environment.Exit(1);
            }
        }
    }
}