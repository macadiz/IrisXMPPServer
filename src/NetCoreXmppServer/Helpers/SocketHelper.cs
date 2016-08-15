using NetCoreXmppServer.CoreClasses;
using NetCoreXmppServer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NetCoreXmppServer.Helpers
{
    public class SocketHelper
    {
        TcpClient mscClient;
        string mstrMessage;
        string mstrResponse;
        byte[] bytesSent;
        private Client client;

        public SocketHelper(Client client)
        {
            this.client = client;
        }

        public void processMsg()
        {
            try
            {
                // Handle the message received and 
                // send a response back to the client.}
                Console.WriteLine("TCPCLIENT " + client.InternalId);
                NetworkStream stream = client.TcpClient.GetStream();
                byte[] bytes = new byte[4096];
                stream.Read(bytes, 0, bytes.Length);
                mstrMessage = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                mscClient = client.TcpClient;
                mstrMessage = mstrMessage.TrimEnd(new char[] { '\0' });
                Console.Write("Mensaje entrante: ");
                mstrResponse = XmlMessageProccessing(mstrMessage);
                Console.WriteLine("Mensaje saliente: " + mstrResponse);
                bytesSent = Encoding.UTF8.GetBytes(mstrResponse);
                stream.Write(bytesSent, 0, bytesSent.Length);
            }
            catch(Exception ex)
            {
                client.Connected = false;
            }
        }

        public string XmlMessageProccessing(string msg)
        {
            try
            {
                List<string> xmlElements = getXmlElements(msg, new List<string>());
                string internalStanza = createInternalStanza(xmlElements);
                Console.WriteLine(internalStanza);
                XmlDocument xm = new XmlDocument();
                xm.LoadXml(internalStanza);
                XmlNode intStanza = xm.FirstChild;
                XmlNode root = intStanza.ChildNodes[0];
                MessageType type = getMessageType(root.Name);
                switch(type){
                    case MessageType.STREAM:
                        {
                            string salida = "";
                            if (intStanza.Attributes["closeConnection"] != null)
                            {
                                if (Int32.Parse(intStanza.Attributes["closeConnection"].Value) == 1)
                                { 
                                    salida = "</stream:stream>";
                                    client.Connected = false;
                                }
                            }
                            else
                            {
                                salida = "<?xml version='1.0'?><stream:stream from='192.168.0.6' id='" + client.InternalId + "' xmlns='jabber:client' xmlns:stream='http://etherx.jabber.org/streams' version='1.0'>";
                                //salida += "<stream:features><mechanisms xmlns='urn:ietf:params:xml:ns:xmpp-sasl'><mechanism>PLAIN</mechanism></mechanisms></stream:features>";
                            }
                            return salida;
                        }
                    case MessageType.UNKNOWN:
                        {
                            return "";
                        }
                    case MessageType.IQ:
                        {
                            string salida = "";
                            string msgId = "";
                            string from = "";
                            IQContext context = resolveIQContext(root, out msgId, out from);
                            switch(context)
                            {
                                case IQContext.AUTH:
                                    {
                                        salida = "<iq type='result' id='" + msgId + "'><query xmlns='jabber:iq:auth'><username/><password/><resource/></query></iq>";
                                        break;
                                    }
                                case IQContext.PING:
                                    {
                                        salida = "<iq from='capulet.lit' to='juliet@capulet.lit/balcony' id='c2s1' type='result'/>";
                                        break;
                                    }
                                default:
                                    {
                                        salida = "";
                                        break;
                                    }
                            }
                            return salida;
                        }
                    default:
                        {
                            return "";
                        }
                }
                //if (root.HasChildNodes)
                //{
                //    for (int i = 0; i < root.ChildNodes.Count; i++)
                //    {
                //        Console.WriteLine(root.ChildNodes[i].InnerText);
                //    }
                //}
            }
            catch (Exception ex)
            {
                return "Problems! " + ex.Message + " - " + msg;
            }
        }

        private MessageType getMessageType(string nodeName)
        {
            if (nodeName.Contains("stream"))
            {
                return MessageType.STREAM;
            }
            else if (nodeName.Equals("iq"))
            {
                return MessageType.IQ;
            }
            else if (nodeName.Equals("message"))
            {
                return MessageType.MESSAGE;
            }
            else
            {
                return MessageType.UNKNOWN;
            }
        }

        private string createInternalStanza(List<string> xmlStrings)
        {
            string stanzaAttributes = "";
            string stanzaTemplate = "<intStanza{0}>{1}</intStanza>";
            string finalXml = "";
            foreach (string xmlString in xmlStrings)
            {
                if (xmlString.Equals("<stream:stream></stream:stream>"))
                {
                    stanzaAttributes += " closeConnection='1'";
                }
                finalXml += xmlString;
            }
            return string.Format(stanzaTemplate, stanzaAttributes, finalXml);
        }

        private List<String> getXmlElements(string xmlString, List<string> elements)
        {
            if (xmlString.Length > 0)
            {
                xmlString = xmlString.Replace("<?xml version='1.0' ?>", "").Replace("<?xml version='1.0'?>", "");
                string nodeName = "";
                string toAdd = "";
                string original = "";
                if (xmlString[1] == '/')
                {
                    nodeName = xmlString.Substring(2, xmlString.IndexOf('>') - 1);
                    original = xmlString.Substring(0, xmlString.IndexOf('>'));
                    toAdd = "<" + nodeName + ">" + original;
                }
                else
                {
                    nodeName = xmlString.Substring(1, ((xmlString.IndexOf(' ') > -1) ? xmlString.IndexOf(' ') : xmlString.IndexOf('>')) - 1);
                    int firstNodeOcurrence = xmlString.IndexOf(nodeName);
                    int lastNodeOcurrence = xmlString.LastIndexOf(nodeName);
                    if (firstNodeOcurrence == lastNodeOcurrence)
                    {
                        original = xmlString.Substring(0, xmlString.IndexOf('>') + 1);
                        toAdd = xmlString.Substring(0, xmlString.IndexOf('>') + 1) + "</" + nodeName + ">";
                    }
                    else
                    {
                        toAdd = xmlString.Substring(0, lastNodeOcurrence + nodeName.Length + 1);
                        original = toAdd;
                    }
                }
                elements.Add(toAdd);
                string newXml = xmlString.Replace(original, "");
                return getXmlElements(newXml, elements);
            }
            else
            {
                return elements;
            }
        }

        private void StreamStack(string str)
        {
            string spliced = str.Substring(1, (str.Contains(" ") ? str.IndexOf(' ') : str.Length) - 1);
            if (spliced.IndexOf('/') == 1 && spliced.Contains("stream"))
            {
                client.StreamStack.Clear();
            }
            else
            {
                client.StreamStack.Add(str);
            }
        }

        private IQContext resolveIQContext(XmlNode IQNode, out string msgId, out string from)
        {
            IQContext context = IQContext.UNKNOWN;
            from = "";
            msgId = IQNode.Attributes[0].Value;
            if (IQNode.HasChildNodes)
            {
                foreach (XmlNode child in IQNode.ChildNodes)
                {
                    if (child.Name.Equals("query"))
                    {
                        if (child.Attributes.Count > 0)
                        {
                            for (int i = 0; i < child.Attributes.Count; i++)
                            {
                                if (child.Attributes[i].Name == "xmlns")
                                {
                                    if (child.Attributes[i].Value.Equals("jabber:iq:auth"))
                                    {
                                        context = IQContext.AUTH;
                                    }
                                }
                            }
                        }
                    }
                    if (child.Name.Equals("ping"))
                    {
                        if (child.Attributes.Count > 0)
                        {
                            for (int i = 0; i < child.Attributes.Count; i++)
                            {
                                if (child.Attributes[i].Name == "xmlns")
                                {
                                    if (child.Attributes[i].Value.Equals("urn:xmpp:ping"))
                                    {
                                        context = IQContext.PING;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return context;
        }

        //public dynamic stanzaToObject(string stanza)
        //{
        //    stanza = stanza.Replace("<", "").Replace(">", "");
        //    if(stanza[0] == '/')
        //    {
        //        return new { isEndTag = false,  }
        //    }
        //}
    }
}
