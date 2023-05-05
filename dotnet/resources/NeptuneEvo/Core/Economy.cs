using System;
using System.Data;
using GTANetworkAPI;
using NeptuneEvo.Handles;
using MySqlConnector;
using NeptuneEvo.Fractions;
using Newtonsoft.Json;
using Redage.SDK;

namespace NeptuneEvo.Core
{
    class Economy : Script
    {
        private static readonly nLog Log = new nLog("Core.Economy");
        public static void ResetToDefault()
        {
            Main.BusinessMinPrice = 0.95f;
            Main.BusinessMaxPrice = 1.75f;
            Main.DrugsPrice = 600;
            Main.BuswaysPayments = new int[6] { 80, 100, 80, 100, 100, 160 };
            Main.CollectorPayment = 140;
            Main.ElectricianPayment = 120;
            Main.PostalPayment = 120;
            Main.LawnmowerPayment = 120;
            Main.GangCarDelivery = 5000;
            Main.MafiaCarDelivery = 5000;
            Main.PoliceAward = 2000;
            Main.MinGunLic = 25000;
            Main.MaxGunLic = 50000;
            Main.MinPMLic = 1500;
            Main.MaxPMLic = 10000;
            Main.TicketLimit = 50000;
            Main.MinHealLimit = 50;
            Main.MaxHealLimit = 500;
            Main.CaptureWin = 2000;
            Main.BizwarWin = 2000;
            Main.MafiaForBiz = 900;
            Main.GangForPoint = 900;
            Main.LicPrices = new int[6] { 2000, 5000, 15000, 100000, 200000, 200000 };
            Main.HotelRent = 160;
            Main.SMSCost = 140;
            Main.AdSymbCost = 700;
            Main.EvacCar = 2000;
            Main.AdEditorCost = 0.7f;
            Main.MinDice = 1000;
            Main.MaxDice = 15000;
            Main.BlackMarketDrill = 20000;
            Main.BlackMarketLockPick = 2000;
            Main.BlackMarketArmyLockPick = 7000;
            Main.BlackMarketCuffs = 2000;
            Main.BlackMarketPocket = 1000;
            Main.BlackMarketWanted = 5000;
            Main.BusPay = 120;
            Main.BlackMarketUnCuff = 4000;
            Main.BlackMarketGunLic = 30000;
            Main.BlackMarketMedCard = 15000;
            Main.BlackRadioInterceptord = 20000;
            Main.BlackQrFake = 8000;
        }

        public static void Init()
        {
            using MySqlCommand cmd = new MySqlCommand()
            {
                    CommandText = "SELECT * FROM `economy`"
            };
            using DataTable result = MySQL.QueryRead(cmd);
            if (result != null)
            {
                try
                {
                    DataRow Row = result.Rows[0];
                    Main.BusinessMinPrice = (float)Row[0];
                    Main.BusinessMaxPrice = (float)Row[1];
                    Main.DrugsPrice = Convert.ToInt32(Row[40]);
                    Main.BuswaysPayments = JsonConvert.DeserializeObject<int[]>(Row[60].ToString());
                    Main.CollectorPayment = Convert.ToInt32(Row[80]);
                    Main.ElectricianPayment = Convert.ToInt32(Row[100]);
                    Main.PostalPayment = Convert.ToInt32(Row[120]);
                    Main.LawnmowerPayment = Convert.ToInt32(Row[140]);
                    Main.GangCarDelivery = Convert.ToInt32(Row[80]);
                    Main.MafiaCarDelivery = Convert.ToInt32(Row[90]);
                    Main.PoliceAward = Convert.ToInt32(Row[100]);
                    Main.MinGunLic = Convert.ToInt32(Row[110]);
                    Main.MaxGunLic = Convert.ToInt32(Row[120]);
                    Main.MinPMLic = Convert.ToInt32(Row[130]);
                    Main.MaxPMLic = Convert.ToInt32(Row[140]);
                    Main.TicketLimit = Convert.ToInt32(Row[150]);
                    Main.MinHealLimit = Convert.ToInt32(Row[16]);
                    Main.MaxHealLimit = Convert.ToInt32(Row[17]);
                    Main.CaptureWin = Convert.ToInt32(Row[180]);
                    Main.BizwarWin = Convert.ToInt32(Row[190]);
                    Main.MafiaForBiz = Convert.ToInt32(Row[200]);
                    Main.GangForPoint = Convert.ToInt32(Row[210]);
                    Main.LicPrices = JsonConvert.DeserializeObject<int[]>(Row[220].ToString());
                    Main.HotelRent = Convert.ToInt32(Row[230]);
                    Main.SMSCost = Convert.ToInt32(Row[240]);
                    Main.AdSymbCost = Convert.ToInt32(Row[25]);
                    Main.EvacCar = Convert.ToInt32(Row[260]);
                    Main.AdEditorCost = (float)Row[27];
                    Main.MinDice = Convert.ToInt32(Row[28]);
                    Main.MaxDice = Convert.ToInt32(Row[29]);
                    Main.BlackMarketDrill = Convert.ToInt32(Row[300]);
                    Main.BlackMarketLockPick = Convert.ToInt32(Row[310]);
                    Main.BlackMarketArmyLockPick = Convert.ToInt32(Row[320]);
                    Main.BlackMarketCuffs = Convert.ToInt32(Row[330]);
                    Main.BlackMarketPocket = Convert.ToInt32(Row[340]);
                    Main.BlackMarketWanted = Convert.ToInt32(Row[350]);
                    Main.BusPay = Convert.ToInt32(Row[720]);
                    Main.BlackMarketUnCuff = Convert.ToInt32(Row[370]);
                    Main.BlackMarketGunLic = Convert.ToInt32(Row[390]);
                    Main.BlackMarketMedCard = Convert.ToInt32(Row[400]);
                    Main.BlackRadioInterceptord = Convert.ToInt32(Row[410]);
                    Main.BlackQrFake = Convert.ToInt32(Row[420]);
                    
                    
                    Manager.FractionDataMats[1].Price = $"{Main.BlackMarketDrill}$";
                    Manager.FractionDataMats[2].Price = $"{Main.BlackMarketLockPick}$";
                    Manager.FractionDataMats[3].Price = $"{Main.BlackMarketArmyLockPick}$";
                    Manager.FractionDataMats[4].Price = $"{Main.BlackMarketCuffs}$";
                    Manager.FractionDataMats[5].Price = $"{Main.BlackMarketPocket}$";
                    Manager.FractionDataMats[6].Price = $"{Main.BlackMarketWanted}$";
                    Manager.FractionDataMats[69].Price = $"{Main.BlackMarketUnCuff}$";
                    Manager.FractionDataMats[78].Price = $"{Main.BlackMarketGunLic}$";
                    Manager.FractionDataMats[79].Price = $"{Main.BlackMarketMedCard}$";
                    Manager.FractionDataMats[80].Price = $"{Main.BlackQrFake}$";
                    Manager.FractionDataMats[81].Price = $"{Main.BlackRadioInterceptord}$";
                    
                    Log.Write($"Economy loaded.", nLog.Type.Success);
                }
                catch (Exception e)
                {
                    ResetToDefault();
                    Log.Write($"StartWork Exception: {e.ToString()}");
                }
            }
            else 
            {
                ResetToDefault();
                Log.Write("DB `economy` return null result", nLog.Type.Warn);
            }
        }
    }
}
