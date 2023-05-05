gm.discord = (detailedStatus) => {
    let state = "на Revolution";

    if (global.localplayer && typeof global.localplayer.remoteId !== "undefined")
        state = translateText('на Revolution под ID {0}', global.localplayer.remoteId);

    mp.discord.update(detailedStatus, state);
}

global.discordDefault = () => {
    gm.discord(translateText('Наслаждается жизнью'))
};

discordDefault ();