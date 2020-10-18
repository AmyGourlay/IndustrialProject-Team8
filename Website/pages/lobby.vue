<template>


    <section class="section is-fullheight">
      <div class="column is-mobile is-centered">
          <!--<b-button type="is-primary" @click="createLobby">
            <b-icon class="icon" pack="mdi" icon="refresh">
            </b-icon>
          </b-button>--> <!--////////////////FOR HOMEPAGE/////////////////-->
          <LobbyCodeLbl title="Lobby Code Goes Here (ಥ﹏ಥ)"></LobbyCodeLbl>
      </div>
      <div class="column is-mobile is-centered">
        <LobbyTable></LobbyTable>
      </div>
      <div class="columns is-mobile is-centered is-gapless">
        <div class="column is-3">
          <router-link to="/game"><BlackButton title="Start Game" to="./game.vue"></BlackButton></router-link>
        </div>
        <div class="column is-3">
          <router-link to="/"><BlackButton title="Leave Game"></BlackButton></router-link>
        </div>
      </div>
    </section>


</template>

<script>
import Card from '~/components/Card';
import BlackButton from '~/components/BlackButton';
import LobbyTable from '~/components/LobbyTable';
import LobbyCodeLbl from '~/components/LobbyCodeLbl';
export default {

  name: 'lobby',
  components: {
    Card
  },
  data() {
    return {
      lobbyInfo: []
    }
  },
  methods: {
    async createLobby() {
      console.info("In lobby vue!");
      const date = new Date();
      const newLobby = {
        easyQs: "",
        mediumQs: "",
        hardQs: "",
        date: date.toISOString(),
        requestURL: "amount=10&category=9&type=multiple&encode=base64"
      };
      this.lobbyInfo = await fetch('/quizApi/Lobbies', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(newLobby)
      }).then((res) => res.json());
      console.info(this.lobbyInfo);
      document.getElementById("lobbyCode").innerHTML = this.lobbyInfo.id;
    }
  }
}
</script>
