using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace ClaymoreMonitorDesktop
{
    class Rig
    {
        private readonly string _ip;
        private readonly int _port;

        private int _currHashRate;
        private int _shares;
        private int _invalids;
        private string _miningPool;
        private int _upTime;
        private string _version;

        private readonly List<Gpu> _gpus;

        public Rig(string ip, int port)
        {
            _ip = ip;
            _port = port;
            _gpus = new List<Gpu>();
        }

        // Handle Communication to get Statistics
        public void GetStatistics1()
        {
            try
            {
                var res = JsonConvert.DeserializeObject<Response>(SendCommand("{\"id\":0,\"jsonrpc\":\"2.0\",\"method\":\"miner_getstat1\"}"));
                var temp = res.Result[2].Split(';');

                //Claymore Version
                _version = res.Result[0];

                //Current Hash Rate
                Int32.TryParse(temp[0], out _currHashRate); //In MHash/s

                //Shares
                Int32.TryParse(temp[1], out _shares);

                //Invalids
                Int32.TryParse(res.Result[8].Split(';')[0], out _invalids); 

                //UpTime
                Int32.TryParse(res.Result[1], out _upTime); //In minutes

                //Mining Pool
                _miningPool = res.Result[7];
                
                string[] hr = res.Result[3].Split(';');
                string[] tempFan = res.Result[6].Split(';');

                for (int i = 0; i < hr.Length; i++)
                {
                    Gpu tmpGpu = new Gpu(); 
                    int iHr;
                    Int32.TryParse(hr[i], out iHr);
                    int iTemp;
                    Int32.TryParse(tempFan[(i + 1) * 2 - 2], out iTemp);
                    int iFan;
                    Int32.TryParse(tempFan[(i + 1) * 2 - 1], out iFan);

                    tmpGpu.EthHash = iHr;
                    tmpGpu.Id = i;
                    tmpGpu.Temperature = iTemp;
                    tmpGpu.Fan = iFan;

                    _gpus.Add(tmpGpu);
                }
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Restart()
        {
            var res = JsonConvert.DeserializeObject<Response>(SendCommand("{\"id\":0,\"jsonrpc\":\"2.0\",\"method\":\"miner_restart\"}"));
        }

        private string SendCommand(string command)
        {
            TcpClient client = new TcpClient();

            client.Connect(_ip, _port);
            Stream stm = client.GetStream();

            ASCIIEncoding asen = new ASCIIEncoding();
            byte[] ba = asen.GetBytes(command);
            stm.Write(ba, 0, ba.Length);
            byte[] bb = new byte[1000];
            int k = stm.Read(bb, 0, 1000);

            StringBuilder Response = new StringBuilder();

            for (int i = 0; i < k; i++)
                Response.Append(Convert.ToChar(bb[i]));

            client.Close();

            return Response.ToString();
        }


        // Running time, in minutes.
        public string Uptime()
        {
            return timeConvert(_upTime);     
        }

        // Current Hash Rates = Total GPUs Hash Rates
        public int CurrentHashRates()
        {
            return _currHashRate;
        }

        // Shares = Total GPUs Shares
        public int Shares()
        {
            return _shares;
        }

        // Invalids = Shares invalids
        public int Invalids()
        {
            return _invalids;
        }

        // Current mining pool. For dual mode, there will be two pools here.
        public string MiningPool()
        {
            return _miningPool;
        }

    
        //GPUs List
        public List<Gpu> ListGPUs()
        {
            return _gpus;
        }

        public string Version()
        {
            return _version;
        }

        private string timeConvert(int n)
        {
            //CONVERT MINUTES TO DAY HOUR MIN 
            int num = n;
            int min = 0;
            int hours = 0;
            int days = 0;

            if (num < 60) return n + "m";

            if (num / 60 < 24)
            {
                min = num % 60;
                hours = num % 60;
                return hours + "h:" + min + "m";
            }

            min = num % 60;
            hours = num % 60;
            days = hours % 24;
            hours = hours % 24;

            return days + "d" + hours + " h: " + min + " m";
        }
    }
}
