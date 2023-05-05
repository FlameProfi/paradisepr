export default {
    job: {
        get: {//Когда подошёл к боту Emma Smith, у которого можно устроиться на работу или уволиться
            desc: `Выберите любую понравившуюся работу, нажмите "Устроиться", а потом доберитесь до места начала работы, которое автоматически будет установлено на карте. Одновременно можно работать только на одной работе!`,
        },
        dismissal: {//Когда уволился с работы
            desc: `Вы в любой момент можете выбрать другую работу или вступить в одну из фракций. Информацию о всех фракциях можно найти в меню помощи, нажав F10, далее пункты "Криминальные фракции" и "Государственные фракции". Учтите, невозможно одновременно находиться во фракции и на работе.`,
        },
        electric: {//Когда подошёл к боту Ryan Nelson
            desc: "Нажмите E чтобы начать рабочий день. На карте будут появляться красные метки. Подбегайте к ним для выполнения задания, за которые Вы будете получать деньги. Для того, чтобы закончиться рабочий день, снова подойдите к боту и нажмите Е.",
        },
        lawn: {//Когда подошёл к работе газонокосильщика
            desc: "Сядьте на газонокосилку для начала работы. Далее следуйте за чекпоинтами. Вы будете получать деньги за каждый пройденный чекпоинт.",
        },
        post: {//Когда подошёл к Отделению почты
            desc: `Нажмите Е, далее "Начать работу". Вам необходимо доставлять почту по адресам, которые будут появляться на карте автоматически. Вы можете получить рабочий транспорт, для этого встаньте на белый чекпоинт и нажмите Е.`,
        },
        post: {//Когда закончились посылки
            desc: "У вас не осталось посылок для доставки. Чтобы получить новые, вернитесь в отделение почты. Метка на карте была установлена автоматически.",
        },
        taxi: {//Когда подошёл к таксопарку
            desc: `Сядьте в такси для аренды транспорта. Существует 4 класса такси, которые отличаются тарифами и ценой аренды. Для начала, рекомендуем выбрать "Econom" класс.`,
        },
        taxi: {//Когда арендовал транспорт
            desc: "Транспорт арендован, осталось дождаться первого заказа!",
        },
        taxi: {//Когда появился заказ
            desc: "В системе появился заказ. Чтобы принять его, воспользуйтесь командой /ta [ID]. Будьте внимательны, заказы делают другие игроки.",
        },
        taxi: {//Когда клиент сел в транспорт
            desc: "Доставьте клиента по нужному адресу. Чтобы выставить чек на оплату, воспользуйтесь командой /tprice [ID] [Сумма].",
        },
        bus: {//Когда подошел к автопарку автобусов
            desc: "Найдите свободный автобус и арендуйте его. Далее следуйте за чекпоинтами. Вы будете получать деньги за каждый пройденный чекпоинт.",
        },
        mechanik: {//Когда подошел к автопарку механиков
            desc: "Найдите свободный пикап и арендуйте его.",
        },
        mechanik: {//Когда арендовал транспорт
            desc: "Транспорт арендован, осталось дождаться первого заказа!",
        },
        mechanik: {//Когда появился заказ
            desc: "В системе появился заказ. Чтобы принять его, воспользуйтесь командой /ma [ID]. Будьте внимательны, заказы делают другие игроки.",
        },
        mechanik: {//Когда приехал на заказ
            desc: "Для предложения ремонта, воспользуйтесь командой /repair [ID] [Цена]. Помимо ремонта, Вы можете заправлять транспорт клиентов. Более подробная информация находится в меню помощи (F10)>Работы>Механик.",
        },
        truck: {//Когда подошел к автопарку дальнобойщиков
            desc: "Найдите свободный грузовик и арендуйте его.",
        },
        truck: {//Когда арендовал транспорт
            desc: "Воспользуйтесь командой /orders для выбора заказа.",
        },
        truck: {//Когда взял заказ
            desc: "Заберите заказ с места загрузки. Метка на карте была установлена автоматически.",
        },
        truck: {//Когда забрал прицеп
            desc: "Доставьте заказ в бизнес. Метка на карте была установлена автоматически.",
        },
        col: {//Когда подошел к автопарку инкассаторов
            desc: "Найдите свободный грузовик и арендуйте его.",
        },
        col: {//Когда арендовал транспорт
            desc: "Вам необходимо доставлять мешки по адресам, которые будут появляться на карте автоматически.",
        },
        col: {//Когда закончились мешки
            desc: `У вас не осталось мешков для доставки. Чтобы получить новые, вернитесь на парковку, встаньте на маркер "Взять мешки с деньгами" и нажмите Е. Метка на карте была установлена автоматически.`,
        },
    },
    other: {
        death: {//Игрока убивают
            desc: `Если Вы заметили нарушение правил сервера, то обратитесь к администрации. Для этого используйте команду /report. 
                    Правила сервера можно узнать на форуме, который можно открыть по ссылке: https://discord.gg/gejDFbARdE
                    Помимо правил, на форуме можно оставить жалобу на игрока или администратора. Для этого перейдите в раздел "Жалобы".`,
        },
        money: {//Игрок имеет на руках больше 15.000$
            desc: "Вы держите большую сумму денег на руках. Будьте осторожны, Вас могут ограбить другие игроки. Для сохранения денег, вы можете положить их на банковский счет. Чтобы найти банк, нажмите М>GPS>Ближайшие места>Ближайший банкомат.",
        },
        money: {//Игрок имеет больше 50.000$ и не владеет домом
            desc: "Вы накопили достаточно денег для покупки дома. Свободные дома отображены зеленым домиком на карте. Если их нет, то вы можете купить дом у другого игрока. В чате часто появляются объявления о продаже дома.",
        },
        money: {//Игрок имеет дом и больше 10.000
            desc: "Вы можете купить первый транспорт. Для этого отправляйтесь в Autoroom. Его можно найти через GPS. Для этого нажмите М>GPS>Бизнесы>Low Autoroom.",
        },
        simcard: {//Игрок открыл телефон
            desc: "Чтобы принимать и совершать звонки, необходимо купить сим-карту. Это можно сделать в магазине 24/7. Нажмите М>GPS>Бизнесы>24/7.",
        },
        transfer: {//Игроку пришли деньги на банковский счет
            desc: "Кто-то отправил Вам деньги на банковский счёт. Воспользуйтесь банкоматом для снятия денег. Для этого нажмите М>GPS>Ближайшие места>Ближайший банкомат.",
        },
        stock: {//Когда игрок получил предмет на склад
            desc: "На ваш склад была доставлена посылка. Склад можно найти на карте, он выглядит как голубой ящик.",
        },
        pickup: {//Когда игрок поднял предмет с земли
            desc: "Вы подобрали предмет и он был успешно перемещен в Ваш инвентарь. Открыть инвентарь - I.",
        },
        bag: {//Когда в инвентаре игрока находится более 14 предметов
            desc: "У вас уже много предметов в инвентаре. Вы можете купить сумку в магазине 24/7, в которой можно хранить вещи. Магазин 24/7 можно найти, нажав М>GPS>Бизнесы>24/7.",
        },
        bag: {//Когда игрок купил сумку
            desc: "Чтобы складывать или доставать предметы из сумки, её необходимо надеть. Будьте осторожны, если вы умрете в тот момент, когда сумка будет надета, то потеряете её.",
        },
        heal: {//Когда у игрока мало хп
            desc: "У Вас мало hp. Самый простой способ восполнить hp - использовать еду, которую можно купить в магазине 24/7 или Burger-Shot. Найти их можно с помощью GPS в телефоне.",
        },
        demorgan: {//Когда игрок получил деморган
            desc: "Вы получили наказание за нарушение правил. Используйте команду /time, чтобы узнать причину и срок наказания. Если Вы не согласны с наказанием, то можете оставить жалобу на администратора на форуме https://discord.gg/gejDFbARdE",
        },
        cuff: {//На игрока надели наручники или стяжки
            desc: "Вы в наручниках. Ни в коем случае не выходите из игры, пока они не будут сняты. Это приведет к наказание на 240 минут. Снять наручники можно на черном рынке.",
        },
    },
    location: {
        ems: {//Игрока появился в EMS после смерти
            desc: `Для восстановления здоровья, встаньте на белый маркер "Начать курс лечения" и нажмите Е. Чтобы восстановить здоровья моментально, воспользуйтесь услугами врача. Помните, врачи - это реальные игроки.`,
        },
        ghetto: {//Игрок заехал в гетто
            desc: "Вы находитесь в опасном районе гетто. Будьте осторожны, вас могут ограбить, похитить или убить!",
        },
        voice: {//Игрок вошел в зону войс-чата и слышит разговоры
            desc: "Для общения в voice-чате зажмите клавишу N. Клавишу можно заменить в меню биндера (Tab). Не прибегайте к оскорблению родителей, религии и нации. На сервере это запрещено и наказуемо!",
        },
        voicemute: {//Игрок вошел в зону c игроком, который установил решим "Я не слышу"
            desc: `Если Вы увидите надпись "Не слышит" над головой игрока, то он может видеть только текстовый чат. Установить такой решим можно в настройках. Нажмите I>Настройки>Я не слышу.`,
        },
        autoroom: {//Игрок подошёл к авто или мото руму и не имеет дома
            desc: "В салоне можно купить личный транспорт, но для начала необходимо купить дом!",
        },
        house: {//Игрок подошёл к свободному или чужому дому
            desc: "Вы можете купить личный дом, который можно улучшать (включая гаражные места). Их можно продавать другим игрокам. В доме можно хранить вещи и транспорт, установить аптечку, а также украсить интерьер.",
        },
        autoschool: {//Игрок подошёл к автошколе
            desc: "В автошколе можно получить лицензии на управление транспортом. Они необходимы для трудоустройства на некоторые работы и фракции. Полиция может выдать штраф за управление транспортом без лицензий.",
        },
        greenzone: {//Игрок вошёл в ЗЗ
            desc: "Вы находитесь в Зеленой зоне (мирной зоне). В ЗЗ невозможно наносить урон и запрещены любые криминальные действия.",
        },
    },
    vehicles: {
        seat: {//Игрок сел в машину
            desc: "Чтобы завести двигатель, воспользуйтесь клавишей В.",
        },
        key: {//Игрок зашёл в гараж, в котором стоит купленная машина
            desc: "Чтобы воспользоваться личным транспортом, необходимы ключи. Нажмите М>Машины>Мои машины>Выберите транспорт>Получить дубликат ключа.",
        },
        drive: {//Спустя минуту поездки на транспорте
            desc: "На транспорте можно перемещаться с помощью автопилота. Для этого необходимо поставить метку на карте и нажать Z.",
        },
        belt: {//Игрок сел на пассажирское место
            desc: "Чтобы пристегнуть ремень безопасности, нажми G>Пристегнуть ремень.",
        },
        key: {//Игрок попытался сесть в чужой закрытый транспорт
            desc: "Если машина закрыта, то открыть её можно только с помощью ключа. Ключ может дать владелец транспорта.",
        },
        lsc: {//Если игрок купил машину, прошёл час, а он не заходил в LSC
            desc: "Личный транспорт можно тюнинговать. Это можно сделать в LS Customs. Нажмите М>GPS>Бизнесы> LS Customs.",
        },
        seat: {//Когда игрок пытается сесть на водительское место фракционного транспорта
            desc: "Фракционным транспортом могут управлять только члены фракции.",
        },
    },
    licenses: {
        gun: {//Игрок подошёл к gun-shop
            desc: "Для покупки оружия необходима лицензия. Её можно получить в полицейском участке. Получите более подробную информацию у офицеров.",
        },
        gun: {//Игрок получил лицензию на оружие
            desc: "Теперь Вы можете покупать оружие в магазине. Чтобы найти нужный магазин, нажмите М>GPS>Бизнесы>Gun shop. Будьте осторожны, за нарушение законодательства, полиция может забрать лицензию.",
        },
        licB: {//Игрок получил лицензию категории В
            desc: "Вы получили лицензию категории B. Теперь Вы можете выбрать одну из следующих работ: Почтальон, Водитель такси, Механик. Утроиться на работу можно в Мэрии, которая отображена на карте в виде флага.",
        },
        licC: {//Игрок получил лицензию категории С
            desc: "Вы получили лицензию категории C. Теперь Вы можете выбрать одну из следующих работ: Водитель автобуса, Дальнобойщик, Инкассатор. Утроиться на работу можно в Мэрии, которая отображена на карте в виде флага.",
        },
    },
}