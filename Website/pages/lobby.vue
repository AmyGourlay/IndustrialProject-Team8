<template>
    <div class="section is-fullheight">
      <div class="container column is-mobile is-centered is-8">
          <!--<b-button type="is-primary" @click="createLobby">
            <b-icon class="icon" pack="mdi" icon="refresh">
            </b-icon>
          </b-button>--> <!--////////////////FOR HOMEPAGE/////////////////-->
          <LobbyCodeLbl title="Players"></LobbyCodeLbl>
      </div>
      <div class="container column is-mobile is-centered is-8">
        <b-table
                :data="tableData"
                :columns="tableColumns"
                :perPage="5"
                paginated
                default-sort="score"
                default-sort-direction="desc"
                :paginationSimple="false"
              ></b-table>
      </div>
      <div class="buttons is-mobile is-centered">
        <div class="is-3">
            <BlackButton @click.native="startGame" title="Start Game"></BlackButton>
        </div>
        <div class="is-3">
          <router-link to="/"><BlackButton title="Leave Game"></BlackButton></router-link>
        </div>
      </div>
    </div>
</template>

<script>
export default {
  name: 'lobby',
  metaInfo: {
    meta: [
      { charset: 'utf-8' },
      { name: 'viewport', content: 'width=device-width, initial-scale=1' }
    ]
  },
  data() {
    return {
        tableData: [],
        nickname: "",
        tableColumns: [
            {
                field: 'name',
                label: 'Nickname',
                width: '150',
            },
            {
                field: 'score',
                label: 'Score',
                numeric: true,
                sortable: true,
                centered: true,
                width: '100'
            }
        ],
      lobbyInfo: []
    }
  },
  created() {
    let playerInfo = this.$route.params.playerInfo;
    console.log(playerInfo);
    let tempPlayerInfo = playerInfo.split(";");
    this.nickname = tempPlayerInfo[0].split("nickname=")[1];
    let lobbyId = tempPlayerInfo[1].split("lobbyId=")[1];
    let playerExistsAlready = tempPlayerInfo[2].split("playerExists=")[1];
    console.log(typeof playerExistsAlready);
    document.getElementById("userNickname").innerHTML = this.nickname;
    if (lobbyId == 0) {
      this.makeLobby();
    }
    else {
      if (playerExistsAlready == "true") {
        console.log("PLAYER EXISTS !");
        this.refreshLeaderboard(lobbyId);
      }
      else {
        this.addPlayerToLobby(lobbyId);
      }
      this.getLobbyInfo(lobbyId);
    }
    document.getElementById("lobbyCode").innerHTML = lobbyId;
  },
  methods: {
     startGame() {
       let playerString = `nickname=${this.nickname};lobbyId=${this.lobbyInfo.id}`;
       this.$router.push({ name: 'game', params: { playerInfo: playerString} });
     },

     async getLobbyInfo(id) {
       console.log(`get lobby info has ${id}`)
       this.lobbyInfo = await fetch(`/quizApi/Lobbies/${id}`).then((res) => res.json());
       console.log(this.lobbyInfo);
       return true;
     },

     async makeLobby() {
       let request = {
         requestURL: "DEPRECATED",
         date: new Date().toISOString()
       };
       this.lobbyInfo = await fetch(`/quizApi/Lobbies`, {                 // the POST request to generate a new lobby and get the ID for it.
         method: 'POST',
         headers: {
           'Content-Type': 'application/json;charset=utf-8'
         },
         body: JSON.stringify(request)
       }).then((res) => res.json());
       console.info(this.lobbyInfo);
       document.getElementById("lobbyCode").innerHTML = this.lobbyInfo.id;
       this.addPlayerToLobby(this.lobbyInfo.id);
     },

     async addPlayerToLobby(lobbyId) {
       this.lobbyInfo = await fetch(`/quizApi/Lobbies/${lobbyId}`).then((res) => res.json());
       console.info("IN ADD");
       console.info(this.lobbyInfo);
       let request = {
         name: this.nickname,
         score: 0,
         lobbyId: parseInt(lobbyId),
         questionIndex: 0,
         lifeline5050: true,
         lifelineSkip: true
       };
       console.info(request);
       let response = await fetch(`/quizApi/Players`, {                 // the POST request to generate a new lobby and get the ID for it.
         method: 'POST',
         headers: {
           'Content-Type': 'application/json;charset=utf-8'
         },
         body: JSON.stringify(request)
       });
       console.log(response);
       this.refreshLeaderboard(lobbyId);
     },

      async refreshLeaderboard(lobbyId) {
        this.tableData = await fetch(`/quizApi/Players/inlobby/${lobbyId}`).then((res) => res.json());
        console.info(this.tableData);
      }
  },
}
</script>
