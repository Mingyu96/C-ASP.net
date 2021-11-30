using System;
using System.Collections.Generic;
using CA_Project.Models;
using System.Linq;
using System.Threading.Tasks;

namespace CA_Project.Data
{
    public class SessionDictionary
    {
        private Dictionary<string, long> sessionDict;
        private List<string> guestList;
        private int Count;

        public SessionDictionary()
        {
            sessionDict = new Dictionary<string, long>();
            guestList = new List<string>();
            Count = 1;
        }

        public void InitialiseGuestList(string session)//add session into guest list
        {
            guestList.Add(session);
        }
        public void RemoveGuestList(string session)//remove session from guestlist
        {
            guestList.Remove(session);
        }


        public bool CheckGuestListContainsGuestId(string sessionid)//check if guest list contain specific session id
        {
            if (guestList.Contains(sessionid))
            {
                return true;
            }
            return false;
        }

        public void InputStoredSessions(List<Session> s)//input stored session
        {
            if(Count == 1)
            {
                foreach(Session sess in s)
                {
                    //add only when session time is not expired
                    if((sess.TimeStamp + Session.timeout) > DateTimeOffset.Now.ToUnixTimeSeconds())
                    {
                        sessionDict.Add(sess.Id.ToString(), sess.TimeStamp);
                    }
                }
                Count++;
            }
        }

        public long? CheckSessionPresence(string sessionid)//return dictionary with specific session id
        {
            if (sessionDict.ContainsKey(sessionid))
            {
                return sessionDict[sessionid];
            }
            return null;
        }

        public bool AddSession(string sessionid, long timestamp)//Add session into dictionary
        {
            if (!sessionDict.ContainsKey(sessionid))
            {
                sessionDict[sessionid] = timestamp;
                return true;
            }
            return false;
        }


        public bool RemoveSession(string sessionid)//REmove session from dictionary
        {
            if (sessionDict.ContainsKey(sessionid))
            {
                sessionDict.Remove(sessionid);
                return true;
            }
            return false;
        }


    }
}
