using MySqlConnector;
using GTANetworkAPI;
using NeptuneEvo.Character;
using NeptuneEvo.Handles;
using Redage.SDK;
using System;
using System.Collections.Generic;
using System.Data;
using System.Timers;
using System.Linq;
using NeptuneEvo.Core;
using NeptuneEvo.Accounts;

namespace NeptuneEvo.SystemsByAdminov
{
    class ContainerSystem : Script
    {
        #region Data
        private Random _rndm = new Random();
        private bool canRate = false;
        private Timer _checkDateTime;
        nLog log = new nLog("ContainerSystem by Adminov");

        private const string CONTAINER_MODEL = "prop_container_01d";
        private const int STARTED_RATE_MONEY = 1000000;
        private const int STARTED_RATE_DONATE = 10000;
        private const string COLSHAPE_ENTER_DATA = "container:colshape";
        private List<int> _rateHours = new List<int>
        {
            17,18,19
        };
        private List<int> _donateContainersIds = new List<int> // тут id, поэтому -1. Пример: 9 контейнер это 8 id
        {
            6,7,8
        };

        private List<ContainerPrize> _containerPrizes = new List<ContainerPrize>();

        private List<ContainerRate> _playersWhoRatesContainer1 = new List<ContainerRate>();
        private List<ContainerRate> _playersWhoRatesContainer2 = new List<ContainerRate>();
        private List<ContainerRate> _playersWhoRatesContainer3 = new List<ContainerRate>();
        private List<ContainerRate> _playersWhoRatesContainer4 = new List<ContainerRate>();
        private List<ContainerRate> _playersWhoRatesContainer5 = new List<ContainerRate>();
        private List<ContainerRate> _playersWhoRatesContainer6 = new List<ContainerRate>();
        private List<ContainerRate> _playersWhoRatesContainer7 = new List<ContainerRate>();
        private List<ContainerRate> _playersWhoRatesContainer8 = new List<ContainerRate>();
        private List<ContainerRate> _playersWhoRatesContainer9 = new List<ContainerRate>();

        private List<GTANetworkAPI.Object> _containers = new List<GTANetworkAPI.Object>();
        private List<TextLabel> _containersTexts = new List<TextLabel>();
        private List<ColShape> _containersColshapes = new List<ColShape>();

        private List<Vector3> _containersPositions = new List<Vector3> 
        { 
            new Vector3(1211.1191, -2969.806, 4.866059),
            new Vector3(1211.1191, -2974.806, 4.866059),
            new Vector3(1211.1191, -2979.806, 4.866059),
            new Vector3(1211.1191, -2984.806, 4.866059),
            new Vector3(1211.1191, -2989.806, 4.866059),
            new Vector3(1211.1191, -2994.806, 4.866059),
            new Vector3(1211.1191, -2999.806, 4.866059),
            new Vector3(1211.1191, -3004.806, 4.866059),
            new Vector3(1211.1191, -3009.806, 4.866059),
        };

        #endregion

        [ServerEvent(Event.PlayerEnterColshape)]
        private void OnPlayerEnterColshape(ColShape colShape, ExtPlayer player)
        {
            if (_containersColshapes.Contains(colShape))
            {
                player.SetSharedData(COLSHAPE_ENTER_DATA, _containersColshapes.IndexOf(colShape));
            }
        }

        [ServerEvent(Event.PlayerExitColshape)]
        private void OnPlayerExitColshape(ColShape colShape, ExtPlayer player)
        {
            if (_containersColshapes.Contains(colShape))
            {
                player.SetSharedData(COLSHAPE_ENTER_DATA, -1);
            }
        }

        [ServerEvent(Event.PlayerConnected)]
        private void OnPlayerConnected(Player player)
        {
            player.SetSharedData(COLSHAPE_ENTER_DATA, -1);
            player.TriggerEvent("containers:loadPositions", Newtonsoft.Json.JsonConvert.SerializeObject(_containersPositions));
        }

        [ServerEvent(0)]
        public void OnServerStarted()
        {
            try
            {

                for (int i = 0; i < _containersPositions.Count; i++)
                {
                    var pos = _containersPositions[i];
                    GTANetworkAPI.Object container = NAPI.Object.CreateObject(NAPI.Util.GetHashKey(CONTAINER_MODEL), pos, new Vector3(0, 0, -90));
                    _containers.Add(container);
                    var id = i + 1;

                    var textLabelText = $"Контейнер {id} \n Аукциона нет \n Начало в {_rateHours.First()}";
                    if (_donateContainersIds.Contains(i))
                    {
                        textLabelText = $"Донатный контейнер \n Контейнер {id} \n Аукциона нет \n Начало в {_rateHours.First()} ";
                    }
                    TextLabel textLabel = NAPI.TextLabel.CreateTextLabel($"Контейнер {id} \n Аукциона нет \n Начало в {_rateHours.First()}", new Vector3(1204.667f, pos.Y, pos.Z + 1), 15f, 1, 4, new Color(255,255,255));
                    if(_donateContainersIds.Contains(i)) 
                    {
                        textLabel.Text = $"Донатный контейнер \n {textLabel}";
                    }
                    _containersTexts.Add(textLabel);

                    ColShape colShape = NAPI.ColShape.CreateSphereColShape(new Vector3(1204.667f, pos.Y, pos.Z), 2);
                    _containersColshapes.Add(colShape);
                    NAPI.Marker.CreateMarker(1, new Vector3(1203.667f, pos.Y, pos.Z), new Vector3(), new Vector3(), 2, new Color(0, 255, 0));
                }

                var response = MySQL.QueryReadAsync("SELECT * FROM container_prizes").GetAwaiter().GetResult();
                
                foreach (DataRow dataRow in response.Rows)
                {
                    var model = (string)dataRow["model"];
                    var fullName = (string)dataRow["fullname"];
                    ContainerPrize containerPrize = new ContainerPrize(fullName, model);
                    _containerPrizes.Add(containerPrize);
                }

                _checkDateTime = new Timer(10000);
                _checkDateTime.AutoReset = true;
                _checkDateTime.Elapsed += CheckTimerElapsed;
                _checkDateTime.Enabled = true;

                log.Write("Container System Ready", nLog.Type.Success);

                //NAPI.Task.Run(() => // потом убрать
                //{
                //    EndRating();
                //}, 90000);
            }
            catch (Exception ex)
            {
                log.Write("Container System loaded with the error: " + ex, nLog.Type.Error);
                NAPI.Task.Run(() =>
                {
                    Environment.Exit(0);
                }, 5000);
            }
        }

        private void CheckTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if(_rateHours.Contains(DateTime.Now.Hour) && !canRate) // start
            {
                StartRating();
            }
            else if (DateTime.Now.Hour > _rateHours.Last() && canRate) // end
            {
                EndRating();
            }
        }

       
        private void EndRating()
        {
            if (canRate == false) return;
            canRate = false;
            var newsStr = "[Контейнеры]~g~ Торги завершены!";
            NAPI.Chat.SendChatMessageToAll(newsStr);
            log.Write(newsStr);

            NAPI.Task.Run(() =>
            {
                foreach (TextLabel textLabel in _containersTexts)
                {
                    var containerId = _containersTexts.IndexOf(textLabel);
                    if (_donateContainersIds.Contains(containerId))
                    {
                        textLabel.Text = $"Донатный контейнер \n Контейнер {containerId} \n Аукциона нет \n Начало в {_rateHours.First()}";
                    }
                    else textLabel.Text = $"Контейнер {containerId} \n Аукциона нет \n Начало в {_rateHours.First()}";
                }
            });

            for(int i = 0; i < 9; i++)
            {
                ContainerRate containerRate = new ContainerRate();
                switch (i)
                {
                    case 0:
                        if (_playersWhoRatesContainer1.Count == 0) continue;

                        containerRate = _playersWhoRatesContainer1.Last();
                        break;
                    case 1:
                        if (_playersWhoRatesContainer2.Count == 0) continue;
                        containerRate = _playersWhoRatesContainer2.Last();
                        break;
                    case 2:
                        if (_playersWhoRatesContainer3.Count == 0) continue;
                        containerRate = _playersWhoRatesContainer3.Last();
                        break;
                    case 3:
                        if (_playersWhoRatesContainer4.Count == 0) continue;
                        containerRate = _playersWhoRatesContainer4.Last();
                        break;
                    case 4:
                        if (_playersWhoRatesContainer5.Count == 0) continue;
                        containerRate = _playersWhoRatesContainer5.Last();
                        break;
                    case 5:
                        if (_playersWhoRatesContainer6.Count == 0) continue;
                        containerRate = _playersWhoRatesContainer6.Last();
                        break;
                    case 6:
                        if (_playersWhoRatesContainer7.Count == 0) continue;
                        containerRate = _playersWhoRatesContainer7.Last();
                        break;
                    case 7:
                        if (_playersWhoRatesContainer8.Count == 0) continue;
                        containerRate = _playersWhoRatesContainer8.Last();
                        break;
                    case 8:
                        if (_playersWhoRatesContainer9.Count == 0) continue;
                        containerRate = _playersWhoRatesContainer9.Last();
                        break;
                    default:
                        break;
                }

                var player = containerRate.Player;
                if (player == null) continue;
                var character = player.GetCharacterData();
                var account = player.GetAccountData();
                if (account == null) return;
                if (character == null) continue;
                var amount = containerRate.Amount;
                #region Checking
                if (character.Money < amount)
                {
                    player.SendNotification("~r~У вас не хватило средств на оплату приза");
                    continue;
                }

                var vehiclesCount = VehicleManager.GetVehiclesCarCountToPlayer(player.Name);
                if (vehiclesCount >= Houses.GarageManager.MaxGarageCars)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"У Вас максимальное кол-во машин", 3000);
                    continue;
                }
                var house = Houses.HouseManager.GetHouse(player, true);
                if (house != null)
                {
                    if (vehiclesCount >= Houses.GarageManager.GarageTypes[Houses.GarageManager.Garages[house.GarageID].Type].MaxCars)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"У Вас максимальное кол-во машин, которое поддерживает Ваше место жительства.", 3000);
                        return;
                    }
                }
                #endregion 
                if(!containerRate.IsDonate) MoneySystem.Wallet.Change(player, -amount);
                else
                {
                    account.RedBucks -= amount;
                    Database.Models.Money.AddDonateUpdate(account.Login, account.RedBucks);
                    Trigger.ClientEvent(player, "client.accountStore.Redbucks", account.RedBucks);
                }
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы выйграли лот в контейнере: -{amount}", 3000);
                #region RandomCar

                var randomCar = _rndm.Next(0, _containerPrizes.Count );
                var prize = _containerPrizes[randomCar];
                VehicleManager.Create(player, prize.ModelName, new Color(255,255,255), new Color(255, 255, 255), Logs: $"winContainer({prize.ModelName}");

                #endregion
                #region MySQLCommand_EndRating
                if (!containerRate.IsDonate)
                {
                    MySqlCommand mySqlCommand = new MySqlCommand("UPDATE characters SET money=@money WHERE uuid=@uuid");
                    mySqlCommand.Parameters.AddWithValue("@money", character.Money);
                    mySqlCommand.Parameters.AddWithValue("@uuid", character.UUID);
                    MySQL.QueryAsync(mySqlCommand).GetAwaiter();
                }
                else
                {
                    MySqlCommand mySqlCommand = new MySqlCommand("UPDATE accounts SET redbucks=@redbucks WHERE login=@login");
                    mySqlCommand.Parameters.AddWithValue("@redbucks", account.RedBucks);
                    mySqlCommand.Parameters.AddWithValue("@login", account.Login);
                    MySQL.QueryAsync(mySqlCommand).GetAwaiter();
                }
                #endregion
            }

            _playersWhoRatesContainer1.Clear();
            _playersWhoRatesContainer2.Clear();
            _playersWhoRatesContainer3.Clear();
            _playersWhoRatesContainer4.Clear();
            _playersWhoRatesContainer5.Clear();
            _playersWhoRatesContainer6.Clear();
            _playersWhoRatesContainer7.Clear();
            _playersWhoRatesContainer8.Clear();
            _playersWhoRatesContainer9.Clear();
        }

        private void StartRating()
        {
            if (canRate == true) return;

            canRate = true;
            var newsStr = "[Контейнеры]~g~ Торги стартовали!";
            NAPI.Chat.SendChatMessageToAll(newsStr);
            NAPI.Chat.SendChatMessageToAll($"[Контейнеры]~g~ Торги закончатся в {_rateHours.Last()}!");
            log.Write(newsStr);

            NAPI.Task.Run(() =>
            {
                foreach (TextLabel textLabel in _containersTexts)
                {
                    var containerId = _containersTexts.IndexOf(textLabel);
                    var money = GetRate(containerId);
                    var moneyText = money != -1 ? $"{money}" : "Отсутсвует";
                    if(moneyText != "Отсутсвует")
                    {
                        if (_donateContainersIds.Contains(containerId))
                        {
                            moneyText = $"{moneyText} RB";
                        }
                        else
                        {
                            moneyText = $"{moneyText}$";
                        }
                    }
                    if (_donateContainersIds.Contains(containerId))
                    {
                        textLabel.Text = $"Донатный контейнер \n Контейнер {containerId + 1} \n Аукцион идёт \n Ставка: {moneyText}";
                    }
                    else textLabel.Text = $"Контейнер {containerId + 1} \n Аукцион идёт \n Ставка: {moneyText} ";

                }
            });

        }

        private void AddRate(ContainerRate containerRate, int containerId)
        {
            switch (containerId)
            {
                case 0:
                    _playersWhoRatesContainer1.Add(containerRate);
                    break;
                case 1:
                    _playersWhoRatesContainer2.Add(containerRate);
                    break;
                case 2:
                    _playersWhoRatesContainer3.Add(containerRate);
                    break;
                case 3:
                    _playersWhoRatesContainer4.Add(containerRate);
                    break;
                case 4:
                    _playersWhoRatesContainer5.Add(containerRate);
                    break;
                case 5:
                    _playersWhoRatesContainer6.Add(containerRate);
                    break;
                case 6:
                    _playersWhoRatesContainer7.Add(containerRate);
                    break;
                case 7:
                    _playersWhoRatesContainer8.Add(containerRate);
                    break;
                case 8:
                    _playersWhoRatesContainer9.Add(containerRate);
                    break;
                default:
                    break;
            }
        }

        private int GetRate(int containerId)
        {
            var result = -1;
            switch (containerId)
            {
                case 0:
                    if (_playersWhoRatesContainer1.Count == 0) return -1;
                    result = _playersWhoRatesContainer1.Last().Amount;
                    break;
                case 1:
                    if (_playersWhoRatesContainer2.Count == 0) return -1;
                    result = _playersWhoRatesContainer2.Last().Amount;
                    break;
                case 2:
                    if (_playersWhoRatesContainer3.Count == 0) return -1;
                    result = _playersWhoRatesContainer3.Last().Amount;
                    break;
                case 3:
                    if (_playersWhoRatesContainer4.Count == 0) return -1;
                    result = _playersWhoRatesContainer4.Last().Amount;
                    break;
                case 4:
                    if (_playersWhoRatesContainer5.Count == 0) return -1;
                    result = _playersWhoRatesContainer5.Last().Amount;
                    break;
                case 5:
                    if (_playersWhoRatesContainer6.Count == 0) return -1;
                    result = _playersWhoRatesContainer6.Last().Amount;
                    break;
                case 6:
                    if (_playersWhoRatesContainer7.Count == 0) return -1;
                    result = _playersWhoRatesContainer7.Last().Amount;
                    break;
                case 7:
                    if (_playersWhoRatesContainer8.Count == 0) return -1;
                    result = _playersWhoRatesContainer8.Last().Amount;
                    break;
                case 8:
                    if (_playersWhoRatesContainer9.Count == 0) return -1;
                    result = _playersWhoRatesContainer9.Last().Amount;
                    break;
                default:
                    break;
            }

            return result;
        }

        [Command("container", "/container ~o~[ваша ставка]")]
        private void CMD_ContainerRate(ExtPlayer player, int rate)
        {
            try
            {
                if (player == null) return;

                var character = player.GetCharacterData();
                if (character == null) return;

                var account = player.GetAccountData();
                if (account == null) return;

                if (!canRate)
                {
                    player.SendNotification($"~r~Ставки можно делать только с {_rateHours.First()} по {_rateHours.Last()}!");
                    return;
                }

                if (player.GetSharedData<int>(COLSHAPE_ENTER_DATA) == -1)
                {
                    player.SendNotification("~r~Вы должны находиться рядом с контейнером!");
                    return;
                }

                var containerId = player.GetSharedData<int>(COLSHAPE_ENTER_DATA);
                var isDonate = _donateContainersIds.Contains(containerId);

                if (!isDonate)
                {
                    if (rate < STARTED_RATE_MONEY)
                    {
                        player.SendNotification("~r~Ставка должна быть больше начальной!");
                        return;
                    }

                    if (character.Money < rate)
                    {
                        player.SendNotification("~r~У вас недостаточно средств!");
                        return;
                    }
                }
                else
                {
                    if (rate < STARTED_RATE_DONATE)
                    {
                        player.SendNotification("~r~Ставка должна быть больше начальной!");
                        return;
                    }

                    if (account.RedBucks < rate)
                    {
                        player.SendNotification("~r~У вас недостаточно донат валюты!");
                        return;
                    }
                }

                var lastRate = GetRate(containerId);
                if (lastRate != -1)
                {
                    if (lastRate >= rate)
                    {
                        player.SendNotification("~r~Ваша ставка должна быть больше прошлой!");
                        return;
                    }
                }
                
                ContainerRate containerRate = new ContainerRate(player, rate, isDonate);

                AddRate(containerRate, containerId);

                #region MySQLCommand_CommandRate
                MySqlCommand mySqlCommand = new MySqlCommand("INSERT INTO container_rates (uuid, amount, containerId) VALUES (@uuid, @amount, @containerId)");
                mySqlCommand.Parameters.AddWithValue("@uuid", character.UUID);
                mySqlCommand.Parameters.AddWithValue("@amount", rate);
                mySqlCommand.Parameters.AddWithValue("@containerId", containerId);
                MySQL.QueryAsync(mySqlCommand).GetAwaiter();
                #endregion

                var textLabel = _containersTexts[containerId];
                var moneyText = $"Контейнер {containerId + 1} \n Аукцион идёт \n Ставка: {rate}";
                if (_donateContainersIds.Contains(containerId))
                {
                    moneyText = $"Донатный контейнер \n Контейнер {containerId + 1} \n Аукцион идёт \n Ставка: {rate}";
                }
                
                if (_donateContainersIds.Contains(containerId))
                {
                    moneyText = $"{moneyText} RB";
                }
                else
                {
                    moneyText = $"{moneyText}$";
                }
                textLabel.Text = moneyText;
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы успешно поставили ставку на контейнер {1 + containerId}", 5000);
            }
            catch(Exception ex)
            {
                log.Write("CMD_ContainerRate: " + ex, nLog.Type.Error);
            } 
        }
    }

    class ContainerPrize
    {
        public string CarFullName { get; set; }
        public string ModelName { get; set; }

        public ContainerPrize(string carFullName, string modelName)
        {
            CarFullName = carFullName;
            ModelName = modelName;
        }
    }

    class ContainerRate
    {
        public ExtPlayer Player { get; set; }
        public int Amount { get; set; }
        public bool IsDonate { get; set; }

        public ContainerRate(ExtPlayer player, int amount, bool isDonate)
        {
            Player = player;
            Amount = amount;
            IsDonate = isDonate;
        }

        public ContainerRate() { }
    }
}
