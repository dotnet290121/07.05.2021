using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MS_webapi_demi
{
    public static class RabbitClient
    {
        public class Request
        {
            public object Key { get; set; }
            public string Result { get; set; }
        }
        private static ConcurrentDictionary<string, Request> m_requests = new ConcurrentDictionary<string, Request>();

        static RabbitClient()
        {
            // connect to rabbit
            // attach listener
        }

        private static void Listener(string queue_name, string message)
        {
            // runs on its own thread
            string corr_id = message; // fix this

            // free correlated thread
            lock (m_requests[corr_id].Key)
            {
                Monitor.Pulse(m_requests[corr_id].Key);
            }
        }

        private static void Enqueue(string queue_name, string json)
        {
            // push message into rabbit
        }

        public static string StartRead(string sp_name, params string[] args)
        {
            // convert sp_name + args to json
            string json = "";
            string corr_id = Guid.NewGuid().ToString();
            json = $"{{ corr_id: '{corr_id}', sp_name: '{sp_name}', args: '' }}";
            Enqueue("mq_read", json);

            object current_key = new object();
            // start wait
            while (!m_requests.TryAdd(corr_id, new Request { Key = current_key, Result = null }))
            {
                Thread.Sleep(50);
            }

            lock(current_key)
            {
                // check maybe response already here before wait...
                Monitor.Wait(current_key);

                // reponse is here
                string result = m_requests[corr_id].Result; // parse the json into list<dic<string, obj>>
                m_requests.TryRemove(corr_id, out Request req);

                return result;

            }
        }
    }
}
