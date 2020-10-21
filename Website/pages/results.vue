<template>
    <section class="section is-fullheight">
      <div class="container column is-mobile is-centered is-8">
          <article class="tile is-child notification is-black">
            <p class="title titlefont is-centered" id="playerPosition">----</p>
            <p class="subtitle subfont is-centered" id="playerScore">----</p>
          </article>
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
      });
      console.info("update player table !!!");
      console.info(response);
    },

    async updatePlayerScoreAndPos() {
      this.tableData = await fetch(`/quizApi/Players/inlobby/${this.lobbyInfo.id}`).then((res) => res.json());
      this.score = this.findCurrentPlayer(false, this.tableData);
      this.tableData.sort((a,b) => b.score - a.score);                                                          //  sort the table
      let playerPos = this.findCurrentPlayer(true, this.tableData) + 1;                                         //  find the player's position on the leaderboard
      document.getElementById("playerScore").innerHTML = `Score: ${this.score}`;                                //  update the player's score on screen
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

    /*
     *  Find Current Player in the Table function
     *  This function finds the player's data in the latest version of the data pulled from the database.
     *  It can be used to get the player's score or their position in the leaderboard
     */
    findCurrentPlayer(position, tempTable) {
      for (let playerPos=0; playerPos < tempTable.length; playerPos++) {
        console.info("in player loop!");
        if (tempTable[playerPos].name === this.nickname) {
            if (position) {
              return playerPos;
            }
            else {
              return tempTable[playerPos].score;
            }
        }
      }
    },

    async refreshLeaderboard() {
      this.tableData = await fetch(`/quizApi/Players/inlobby/${this.lobbyInfo.id}`).then((res) => res.json());
      console.info(this.tableData);
    }
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
    document.getElementById("lobbyCode").innerHTML = this.lobbyInfo.id;
    document.getElementById("userNickname").innerHTML = this.nickname;
    this.updatePlayerScoreAndPos();
    this.refreshLeaderboard();
  },
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
