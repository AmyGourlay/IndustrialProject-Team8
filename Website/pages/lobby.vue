<template>


    <section class="section is-fullheight">
      <div class="column is-mobile is-centered">
          <!--<b-button type="is-primary" @click="createLobby">
            <b-icon class="icon" pack="mdi" icon="refresh">
            </b-icon>
          </b-button>--> <!--////////////////FOR HOMEPAGE/////////////////-->
          <LobbyCodeLbl title="Players"></LobbyCodeLbl>
      </div>
      <div class="column is-mobile is-centered">
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
export default {
  name: 'Game',
  metaInfo: {
    meta: [
      { charset: 'utf-8' },
      { name: 'viewport', content: 'width=device-width, initial-scale=1' }
    ]
  },
  data() {
    return {
        tableData: [],
        currQuestion: 1,
        currQuestionJSON: null,
        lifeline5050: true,
        lifelineSkip: true,
        nickname: "",
        score: 0,
        tableColumns: [
            {
                field: 'name',
                label: 'Nickname',
                width: '40',
            },
            {
                field: 'score',
                label: 'Score',
                numeric: true,
                sortable: true,
                centered: true
            }
        ],
      lobbyInfo: []
    }
  },
  methods: {
    getGameDetails() {
      let allCookies = document.cookie;
      let cookieArr = allCookies.split('; ');
      let nickname;
      let lobbyId;
      let cookie;
      for (cookie of cookieArr) {
        if (cookie.includes("nickname")) {
          nickname = cookie.split("nickname=")[1];
        }
        if (cookie.includes("lobbyId")) {
          lobbyId = cookie.split("lobbyId=")[1];
        }
      }
      this.nickname = nickname;
      return lobbyId;
    },
    async updateLobbyTable() {
      let tempLobbyInfo = {
        id: this.lobbyInfo.id,
        easyQuestions: JSON.stringify(this.lobbyInfo.easyQuestions),
        mediumQuestions: JSON.stringify(this.lobbyInfo.mediumQuestions),
        hardQuestions: JSON.stringify(this.lobbyInfo.hardQuestions),
      };
      const response = await fetch(`/quizApi/Lobbies/${this.lobbyInfo.id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(this.lobbyInfo)
      }).then((res) => res.json());
      console.info("IN UPDATE LOBBY");
      console.info(response);
    },
    async updatePlayerTable(){
      let playerInfo = {
        name: this.nickname,
        Score: this.score,
        lobbyId: this.lobbyInfo.id,
        questionIndex: this.currQuestion,
        Lifeline5050: this.lifeline5050,
        LifelineSkip: this.lifelineSkip
      };
      console.info(playerInfo);
      const response = await fetch('/quizApi/Players', {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(playerInfo)
      }).then((res) => res.json());
      console.info(response);
    },
    updatePlayerScoreAndPos(adjustment) {
      this.score += adjustment;
      this.refreshLeaderboard();
      if (adjustment != 0) { this.updatePlayerTable(); }
      document.getElementById("playerScore").innerHTML = `Score: ${this.score}`;
      // this.tableData.push({id: 5050, lifeline5050: true, lifelineSkip: true, lobbyId: 90909090, name: "bethany", score: this.score, questionIndex: this.currQuestion}); // TEMPORARY FIX !! TODO: REMOVE THIS LATER after full API integration
      this.tableData.sort((a,b) => b.score - a.score);
      let playerPos = 0;
      for (playerPos=0; playerPos < this.tableData.length; playerPos++) {
        console.info("in player loop!");
        if (this.tableData[playerPos].name === this.nickname) {
            break;
        }
      }
      playerPos++;
      let positionElem = document.getElementById("playerPosition");
      console.info(`Player position: ${playerPos}`);
      switch (playerPos) {
        case 1:
          positionElem.innerHTML = "1st";
          break;
        case 2:
          positionElem.innerHTML = "2nd";
          break;
        case 3:
          positionElem.innerHTML = "3rd";
          break;
        default:
          positionElem.innerHTML = `${playerPos}th`;
          break;
      }
    },
    async refreshLeaderboard() {
      this.tableData = await fetch(`/quizApi/Players/inlobby/${this.lobbyInfo.id}`).then((res) => res.json());
      console.info(this.tableData);
    },
    decodeJsonData() {  // had issues with HTML encoding so this converts the Base64 encoded data back into ASCII
      let tempVar = this.currQuestionJSON;
      this.currQuestionJSON.category = decodeURIComponent(escape(window.atob(tempVar.category)));
      this.currQuestionJSON.correct_answer = decodeURIComponent(escape(window.atob(tempVar.correct_answer)));
      this.currQuestionJSON.difficulty = decodeURIComponent(escape(window.atob(tempVar.difficulty)));
      this.currQuestionJSON.question = decodeURIComponent(escape(window.atob(tempVar.question)));
      this.currQuestionJSON.type = decodeURIComponent(escape(window.atob(tempVar.type)));
      let incorrectAnswers = [];
      let answer;
      for (answer of tempVar.incorrect_answers) {
        incorrectAnswers.push(decodeURIComponent(escape(window.atob(answer))));
      }
      this.currQuestionJSON.incorrect_answers = incorrectAnswers;
      console.info(this.currQuestionJSON);
    },
  },
  async fetch() {
    let lobbyId = this.getGameDetails();
    this.lobbyInfo = await fetch(`/quizApi/Lobbies/${lobbyId}`).then((res) => res.json());
    console.info(this.lobbyInfo);

    document.getElementById("lobbyCode").innerHTML = this.lobbyInfo.id;
    document.getElementById("userNickname").innerHTML = this.nickname;
    this.updatePlayerScoreAndPos(0);
    this.startGame();
  },
}

</script>
