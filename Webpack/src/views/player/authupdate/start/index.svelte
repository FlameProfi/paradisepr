<script>
    import { translateText } from 'lang'
	import Authentication from './authentication/index.svelte';
	import Registration from './registration/index.svelte';
    import Restore from './restore/index.svelte';
    import { addListernEvent } from 'api/functions'

    import { isInput, isSend } from '@/views/player/newauthentication/store.js';
    import {accountLogin} from "store/account";

    let PagesSorted = ["Authentication", "Registration", "Restore"];

    const Views = {
        Authentication,
        Registration,
        Restore
    }
    let SelectViews = "Authentication";

    const OnSelectViews = (view) => {
        if (SelectViews === view)
            return;
        
        SelectViews = view;
    }

    addListernEvent ("isSendEmailMessage", (toggled) => {
        isSend.set (toggled)
    })

    accountLogin.subscribe((value) => {
        if (value == -99) return;
        if (value == -1 || value == "-1") {
            SelectViews = "Registration";
            PagesSorted = ["Registration"];
        }
        else {
            SelectViews = "Authentication";
            PagesSorted = ["Authentication", "Restore"];
        }
    });
</script>

<div id="newauthentication">
    {#if $isSend}
    <div class="newauthentication__popup">
        <div class="newauthentication__popup_title">{translateText('player2', 'Мы отправили письмо вам на почту!')}</div>
        <div class="newauthentication__popup_subtitle">{translateText('player2', 'Подтвердите адрес электронной почты, чтобы перейти к созданию персонажа.')}</div>
        <!-- svelte-ignore a11y-click-events-have-key-events -->
        <div class="main__button main__button_mail" on:click={() => isSend.set (false)}>
            {translateText('player2', 'Я перепутал почту..')}
        </div>
        <div class="auth-mail newauthentication__popup_iconmail"></div>
    </div>
    {/if}

    <div class="header">
        <div class="header__logo"/>
        <div class="header__new"/>
        <div class="header__money" />
    </div>
    <svelte:component this={Views[SelectViews]} OnSelectViews={OnSelectViews}/>
</div>