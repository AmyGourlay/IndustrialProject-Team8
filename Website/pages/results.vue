<template>
    <section class="section is-fullheight">
      <div class="column is-mobile is-centered">
          <article class="tile is-child notification is-black">
            <p class="title titlefont is-centered" id="playerPosition">----</p>
            <p class="subtitle subfont is-centered" id="playerScore">----</p>
          </article>
          <h1>{{lobbyCode}}</h1>
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
          <router-link to="/lobby"><BlackButton title="Play Again"></BlackButton></router-link>
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

    /*
     *  Get Game Details function
     *  This function gets the player's info so that their score and details can be pulled from the database
     */
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

    /*
     *  Update Lobby Table function
     *  Updates the database with the questions generated in the front end so players can pull that data when they want to play
     *  TODO: the PUT requests need to be fixed API side, so this function will need amending at some point
     */
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

    /*
     *  Update Player Table function
     *  Updates the database with the Player's info, such as their score, what question they're up to etc.
     *  Currently, it sends everything back to the database, rather than just what needs updating.
     */
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

    /*
     *  Update Player Score and Position function
     *
     *  This function has two purposes - adjusting the player's score and updating the score, position and leaderboard elements.
     *  If it is run at the start of the game, it will query the database for the current player's score and save that as their existing score.
     *  otherwise, it will adjust their score locally and then update the database with the change and pull the latest changes from the database
     *  It also updates the position shown on screen so the player knows where they place on the leaderboard
     */
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

  /*
   *  The starting function!
   *  This function is what is called when the page loads.
   *  The lobby information is loaded locally from the database and the player's nickname and lobby ID field are updated on screen.
   *  This function also gets the score and player info from the DB via update player score and position function.
   */
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
}

</script>

<style>
  .is-centered{
    text-align: center;
  }

  .titlefont{
    font-size: 60px;
  }

  .subfont{
    font-size: 40px;
  }
</style>
