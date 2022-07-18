using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace HTTPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: Call CreateRedirectionRulesFile() function to create the rules of redirection 
            CreateRedirectionRulesFile();

            //Start server
            // 1) Make server object on port 1000
            // 2) Start Server
            Server se = new Server(1000, "redirectionRules.txt");
            se.StartServer();

            //Thread newThread = new Thread(new ThreadStart(CreateRedirectionRulesFile));
            //newThread.Start();

        }

        static void CreateRedirectionRulesFile()
        {
            // TODO: Create file named redirectionRules.txt
            // each line in the file specify a redirection rule
            // example: "aboutus.html,aboutus2.html"
            // means that when making request to aboustus.html,, it redirects me to aboutus2
            StreamWriter writer = new StreamWriter("redirectionRules.txt");
            writer.WriteLine("aboutus.html,aboutus2.html"); //redirect to another website.
            writer.Close();
        }
         
    }
}
